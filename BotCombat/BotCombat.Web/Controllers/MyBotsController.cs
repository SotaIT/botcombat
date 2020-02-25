using System;
using System.Linq;
using System.Threading.Tasks;
using BotCombat.Engine.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BotCombat.Web.Data;
using BotCombat.Web.Data.Domain;
using BotCombat.Web.Models;
using BotCombat.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BotCombat.Web.Controllers
{
    [Authorize]
    public class MyBotsController : Controller
    {
        // todo: move all data methods to service classes
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly GameService _gameService;
        private Author _author;
        public MyBotsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, GameService gameService)
        {
            _context = context;
            _userManager = userManager;
            _gameService = gameService;
        }

        public Author Author => _author ??= GetAuthor();

        private Author GetAuthor()
        {
            var userId = _userManager.GetUserId(User);
            var author = _context.Authors.FirstOrDefault(a => a.UserId.Equals(userId));
            if (author != null) return author;
            author = new Author { UserId = userId };
            _context.Authors.Add(author);
            _context.SaveChanges();
            return author;
        }

        // GET: MyBots
        public async Task<IActionResult> Index()
        {
            var authorBots = await _context.AuthorBots.Where(b => b.AuthorId == Author.Id && b.Status != (int)BotStatus.Deleted).ToListAsync();
            var rootAuthorBotIds = authorBots.Select(ab => ab.GetRootId()).Distinct().ToList();
            var latestAuthorBots = rootAuthorBotIds.Select(r =>
            {
                return authorBots
                    .Where(ab => ab.GetRootId() == r)
                    .OrderByDescending(ab => ab.Version)
                    .FirstOrDefault();
            }).ToList();

            var botIds = latestAuthorBots.Select(ab => ab.BotId).Distinct().ToList();
            var bots = await _context.Bots.Where(b => botIds.Contains(b.Id)).ToListAsync();
            var models = latestAuthorBots.Select(ab =>
            {
                var bot = bots.FirstOrDefault(b => b.Id == ab.BotId);
                if (bot == null)
                    return null;

                return new BotListItemViewModel
                {
                    Id = ab.RootId ?? ab.Id,
                    Version = ab.Version,
                    Created = ab.Created,
                    Status = ab.Status.ToBotStatus(),
                    Name = bot.Name
                };
            });
            return View(models);
        }

        // GET: MyBots/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MyBots/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBotViewModel model)
        {
            if (ModelState.IsValid)
            {
                var authorBot = await CreateBot(model.Name);

                return RedirectToAction(nameof(Edit), new { id = authorBot.Id, version = authorBot.Version });
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<AuthorBot> CreateBot(string name, string code = null, int? rootId = null, int? parentId = null, int version = 1)
        {
            var bot = new Bot
            {
                Type = (int)BotTypes.Javascript,
                Name = name,
                Code = code ?? JsBots.JsBot.DefaultSourceCode
            };
            _context.Bots.Add(bot);
            await _context.SaveChangesAsync();

            var authorBot = new AuthorBot
            {
                RootId = rootId,
                ParentId = parentId,
                BotId = bot.Id,
                AuthorId = Author.Id,
                Created = DateTime.UtcNow,
                Version = version,
                Status = (int)BotStatus.Draft
            };
            _context.Add(authorBot);
            await _context.SaveChangesAsync();

            return authorBot;
        }

        // GET: MyBots/Edit/5
        public async Task<IActionResult> Edit(int? id, int version, bool run = false)
        {
            var authorBot = await GetAuthorBot(id, version);
            if (authorBot == null)
                return NotFound();

            var rootId = authorBot.GetRootId();
            var versions = await _context.AuthorBots
                .Where(b => b.AuthorId == Author.Id && (b.RootId == rootId || b.Id == rootId))
                .ToListAsync();

            var bot = await _context.Bots.FirstOrDefaultAsync(b => b.Id == authorBot.BotId);
            if (bot == null)
                return NotFound();
            var status = authorBot.Status.ToBotStatus();
            var model = new EditBotViewModel
            {
                Id = rootId,
                Version = authorBot.Version,
                Created = authorBot.Created,
                Status = status,
                Name = bot.Name,
                Code = bot.Code,
                ContinueEdit = true,
                Run = run,
                Versions = versions
                    .OrderBy(b => b.Version)
                    .ToArray(),
                Statuses = status.GetAllowedStatuses()
                    .Select(s => new SelectListItem(s.ToString(), s.ToString()))
                    .ToList()
            };

            if (run)
            {
                model.Game = _gameService.PlayEditModeGame(bot.Id).Json;
            }

            return View(model);
        }

        private async Task<AuthorBot> GetAuthorBot(int? rootId, int version)
        {
            if (rootId == null) return null;

            var authorBot = await _context.AuthorBots
                .FirstOrDefaultAsync(b => b.AuthorId == Author.Id 
                                          && (b.Id == rootId.Value || b.RootId == rootId.Value)
                                          && b.Version == version
                                          && b.Status != (int)BotStatus.Deleted);
            return authorBot;
        }

        // POST: MyBots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int version, [Bind("Id,Version,Status,Code,ContinueEdit,Run")] EditBotViewModel model)
        {
            if (id != model.Id || model.Version != version)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var authorBot = await GetAuthorBot(model.Id, model.Version);
                if (authorBot == null)
                    return NotFound();
                var bot = await _context.Bots.FirstOrDefaultAsync(b => b.Id == authorBot.BotId);
                if (bot == null)
                    return NotFound();

                var rootId = authorBot.GetRootId();
                var status = authorBot.Status.ToBotStatus();

                // if the code has changed
                if (bot.Code != model.Code)             
                {
                    // if the bot is in Draft status = change current bot
                    if (status == BotStatus.Draft)
                    {
                        bot.Code = model.Code;
                        _context.Update(bot);
                        await _context.SaveChangesAsync();
                    }
                    else // For other statuses - create new version
                    {
                        var versions = await _context.AuthorBots
                            .Where(b => b.AuthorId == Author.Id && (b.RootId == rootId || b.Id == rootId))
                            .ToListAsync();

                        var newBotVersion = await CreateBot(bot.Name,
                            model.Code,
                            rootId,
                            authorBot.Id,
                            versions.Max(v => v.Version) + 1);

                        return RedirectToAction(nameof(Edit), new { id = model.Id, version = newBotVersion.Version, run = model.Run });
                    }
                }

                if (status.IsAllowedStatus(model.Status))
                {
                    authorBot.Status = (int)model.Status;
                    _context.Update(authorBot);

                    // if it is a public status - make all other versions private
                    if (model.Status.IsPublicStatus())
                    {
                        var versions = await _context.AuthorBots
                            .Where(b => b.AuthorId == Author.Id && (b.RootId == rootId || b.Id == rootId) && b.Id != authorBot.Id)
                            .ToListAsync();
                        foreach (var botVersion in versions.Where(v => v.Status.ToBotStatus().IsPublicStatus()))
                        {
                            botVersion.Status = (int)BotStatus.Private;
                            _context.Update(version);
                        }
                    }

                    await _context.SaveChangesAsync();
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorBotExists(model.Id))
                    return NotFound();
                throw;
            }

            if (model.ContinueEdit)
                return RedirectToAction(nameof(Edit), new { id = model.Id, version = model.Version, run = model.Run });

            return RedirectToAction(nameof(Index));
        }

        // GET: MyBots/Delete/5
        public async Task<IActionResult> Delete(int? id, int version = 1)
        {
            var authorBot = await GetAuthorBot(id, version);
            if (authorBot == null)
                return NotFound();

            return View(authorBot);
        }

        // POST: MyBots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int version = 1)
        {
            var authorBot = await GetAuthorBot(id, version);
            authorBot.Status = (int)BotStatus.Deleted;
            _context.Update(authorBot);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorBotExists(int id)
        {
            return _context.AuthorBots.Any(e => e.Id == id);
        }
    }
}

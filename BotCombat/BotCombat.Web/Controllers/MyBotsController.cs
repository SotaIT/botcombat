using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BotCombat.Web.Data;
using BotCombat.Web.Data.Domain;
using BotCombat.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BotCombat.Web.Controllers
{
    [Authorize]
    public class MyBotsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private Author _author;
        public MyBotsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
            var bots = await _context.AuthorBots.Where(b => b.AuthorId == Author.Id && b.Status != (int)BotStatus.Deleted).ToListAsync();
            return View(bots);
        }

        // GET: MyBots/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MyBots/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBotViewModel model)
        {
            if (ModelState.IsValid)
            {
                var authorBot = await CreateBot(model.Name);

                return RedirectToAction(nameof(Edit), new { id = authorBot.Id });
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
        public async Task<IActionResult> Edit(int? id)
        {
            var authorBot = await GetAuthorBot(id);
            if (authorBot == null)
                return NotFound();

            var rootId = authorBot.RootId ?? authorBot.Id;
            var versions = await _context.AuthorBots
                .Where(b => b.AuthorId == Author.Id && (b.RootId == rootId || b.Id == rootId))
                .ToListAsync();

            var bot = await _context.Bots.FirstOrDefaultAsync(b => b.Id == authorBot.BotId);
            if (bot == null)
                return NotFound();

            return View(new EditBotViewModel
            {
                Id = authorBot.Id,
                Version = authorBot.Version,
                Created = authorBot.Created,
                Status = (BotStatus)(authorBot.Status ?? 0),
                Name = bot.Name,
                Code = bot.Code,
                Versions = versions.OrderBy(b => b.Version).ToArray(),
                ContinueEdit = true
            });
        }

        private async Task<AuthorBot> GetAuthorBot(int? id)
        {
            if (id == null) return null;

            var authorBot = await _context.AuthorBots
                .FirstOrDefaultAsync(b => b.AuthorId == Author.Id && b.Id == id.Value && b.Status != (int)BotStatus.Deleted);
            return authorBot;
        }

        // POST: MyBots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Version,Created,Status,Name,Code,ContinueEdit")] EditBotViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var authorBot = await GetAuthorBot(id);
                if (authorBot == null)
                    return NotFound();
                var bot = await _context.Bots.FirstOrDefaultAsync(b => b.Id == authorBot.BotId);
                if (bot == null)
                    return NotFound();

                // if bot is in Draft status = change the current bot
                if ((authorBot.Status ?? 0) == (int)BotStatus.Draft)
                {
                    bot.Name = model.Name;
                    bot.Code = model.Code;
                    _context.Update(bot);
                    await _context.SaveChangesAsync();
                }
                else // if the status is not draft
                {
                    // if the code changed - create new version
                    if (bot.Code != model.Code)
                    {
                        var rootId = authorBot.RootId ?? authorBot.Id;
                        var versions = await _context.AuthorBots
                            .Where(b => b.AuthorId == Author.Id && (b.RootId == rootId || b.Id == rootId))
                            .ToListAsync();

                        var newBotVersion = await CreateBot(model.Name, 
                            model.Code, 
                            authorBot.RootId ?? authorBot.Id, 
                            authorBot.Id, 
                            versions.Max(v=>v.Version) + 1);

                        return RedirectToAction(nameof(Edit), new { id = newBotVersion.Id });
                    }
                }
                // if the status has changed
                if (authorBot.Status != (int) model.Status)
                {
                    authorBot.Status = (int) model.Status;
                    _context.Update(authorBot);
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
                return RedirectToAction(nameof(Edit), new { id });

            return RedirectToAction(nameof(Index));
        }

        // GET: MyBots/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var authorBot = await GetAuthorBot(id);
            if (authorBot == null)
                return NotFound();

            return View(authorBot);
        }

        // POST: MyBots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var authorBot = await GetAuthorBot(id);
            authorBot.Status = (int) BotStatus.Deleted;
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

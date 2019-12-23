using System;
using System.Collections.Generic;
using System.Diagnostics;
using BotCombat.Web.Data.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BotCombat.Web.Models;
using BotCombat.Web.Services;

namespace BotCombat.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GameService _gameService;
        private readonly GameDataService _gameDataService;

        public HomeController(ILogger<HomeController> logger, GameService gameService, GameDataService gameDataService)
        {
            _logger = logger;
            _gameService = gameService;
            _gameDataService = gameDataService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            var game = _gameService.CreateGame(1, new List<int> { 1, 2, 3, 4, 5 });
            _gameService.QueueGame(game);
            _gameService.PlayGame(game);

            return Content(game.Json);
        }

        [HttpGet]
        public IActionResult Game(int id)
        {
            var game = _gameDataService.Get(id);
            if (game == null)
                return NotFound();

            return (GameStates) game.State switch
            {
                GameStates.Created => View("~/Views/Home/Game.Created.cshtml", game),
                GameStates.Queued => View("~/Views/Home/Game.Queued.cshtml", game),
                GameStates.Played => View(game),
                GameStates.Error => View("~/Views/Home/Game.Error.cshtml", game),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

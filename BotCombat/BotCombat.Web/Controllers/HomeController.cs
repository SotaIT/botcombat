using System.Collections.Generic;
using System.Diagnostics;
using BotCombat.Web.Models;
using BotCombat.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace BotCombat.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly GameService _gameService;

        public HomeController(GameService gameService)
        {
            _gameService = gameService;
        }

        public IActionResult Index()
        {
            var mapId = 1;
            var botIds = new List<int> {2, 3, 5, 6};

            var game = _gameService.GetGame(mapId, botIds);

            return View(game);
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
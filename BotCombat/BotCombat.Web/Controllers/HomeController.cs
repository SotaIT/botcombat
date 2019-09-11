using System.Collections.Generic;
using System.Diagnostics;
using BotCombat.Core.Models;
using BotCombat.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Map = BotCombat.Core.Models.Map;

namespace BotCombat.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var walls = new List<Wall> { new Wall(3, 3), new Wall(3, 4), new Wall(3, 5), new Wall(7, 2), new Wall(8, 2) };

            var bonuses = new List<Bonus>
                {new Bonus(0, 0, 5), new Bonus(2, 1, 3), new Bonus(3, 4, 15), new Bonus(13, 11, 8)};

            var traps = new List<Trap>
                {new Trap(4, 4, 10)};

            var map = new Map(1, 20, 15, 32, 10, 1, 1, walls, bonuses, traps);

            ViewData["Map"] = map;


            return View();
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
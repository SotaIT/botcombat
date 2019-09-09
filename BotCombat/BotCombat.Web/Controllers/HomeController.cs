﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BotCombat.Abstractions;
using BotCombat.Core;
using Microsoft.AspNetCore.Mvc;
using BotCombat.Web.Models;
using BotCombat.Js;
using BotCombat.Cs;

namespace BotCombat.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            
            var walls = new List<Wall> { new Wall(3, 3), new Wall(3, 4), new Wall(3, 5), new Wall(7, 2), new Wall(8, 2)};
            
            var bonuses = new List<Bonus> { new Bonus(0, 0, 5), new Bonus(2, 1, 3), new Bonus(3, 4, 15), new Bonus(13, 11, 8) };

            var bots = new List<IBot>
            {
                new CsBot(1, 100000, CsBot.DefaultSourceCode),
                new CsBot(2, 100000, CsBot.DefaultSourceCode),
                new JsBot(3, 100000, JsBot.DefaultInitPowerScript, JsBot.DefaultDistributePowerScript,
                    JsBot.DefaultChooseDirectionScript),
                new CsBot(4, 100000, CsBot.DefaultSourceCode)
            };
            var mapSettings = new MapSettings(1, 20, 20, 32, 10, 1, 1, walls, bonuses);
            ViewData["MapSettings"] = mapSettings;

            var map = new Map(mapSettings, bots);

            var steps = new List<Step>();
            while(true)
            {
                var step = map.Step();
                steps.Add(step);

                if (step.Bots.Count < 2)
                {
                    break;
                }
            }

            ViewData["Steps"] = steps;

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
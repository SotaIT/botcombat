using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BotCombat.Abstractions;
using BotCombat.Abstractions.Models;
using BotCombat.Core;
using BotCombat.Cs;
using BotCombat.Js;
using Map = BotCombat.Core.Map;

namespace BotCombat.TestApp
{
    internal class Program
    {
        private static MapManager _mapManager;

        private static void Main(string[] args)
        {
            var walls = new List<Wall> { new Wall(3, 3) };
            var bonuses = new List<Bonus> { new Bonus(0, 0, 5), new Bonus(2, 1, 3), new Bonus(3, 4, 15) };
            var bots = new List<IBot>
            {
                new TestBot(),
                new TestBot2(),
                new JsBot(3, 100000, JsBot.DefaultInitPowerScript, JsBot.DefaultDistributePowerScript,
                    JsBot.DefaultChooseDirectionScript),
                new CsBot(4, 100000, CsBot.DefaultSourceCode)
            };
            var map = new Map(1, 5, 5, 32, 10, 1, 1, walls, bonuses);

            _mapManager = new MapManager(map, bots);

            for (var i = 0; i < 100; i++)
            {
                var game = _mapManager.MakeStep();
                var step = game.LastStep;
                DrawStep(game, step);

                if (step.Bots.Count < 2)
                {
                    Console.WriteLine("Game Over in {0} steps.", i);

                    if (step.Bots.Count == 0)
                        Console.WriteLine("All bots dead!");
                    else
                        Console.WriteLine("The winner is: {0}!", step.Bots.Values.First().Id);
                    break;
                }

                Thread.Sleep(100);
            }

            Console.WriteLine("Time Over.");
        }


        private static void DrawStep(Game game, Step step)
        {
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.Write(new string('=', game.Map.Width * 10));
            Console.WriteLine(" ");
            Console.WriteLine(" ");

            HLine(game);

            for (var y = 0; y < game.Map.Height; y++)
            {
                Console.Write(" | ");
                for (var x = 0; x < game.Map.Width; x++)
                {
                    DrawPoint(game, step, x, y);
                    Console.Write(" | ");
                }

                HLine(game);
            }

            foreach (var dl in step.Logs)
                Console.WriteLine(
                    $"Bot {dl.SourceId} made {dl.Value} damage to bot {dl.TargetId} at ({dl.X}, {dl.Y}).");
        }

        private static void HLine(Game game)
        {
            Console.WriteLine(" ");
            Console.Write(" |");
            Console.Write(new string('-', game.Map.Width * 10 - 1));
            Console.Write("| ");
            Console.WriteLine(" ");
        }

        private static void DrawPoint(Game game, Step step, int x, int y)
        {
            var wall = game.Map.Walls.FirstOrDefault(w => w.X == x && w.Y == y);
            if (wall != null)
            {
                Console.Write(GetSingleCharCell('W'));
                return;
            }

            var bonus = step.Bonuses.FirstOrDefault(b => b.X == x && b.Y == y);
            if (bonus != null)
            {
                Console.Write(GetSingleCharCell('B'));
                return;
            }

            var bots = step.Bots.Values.Where(o => o.X == x && o.Y == y).ToList();
            if (bots.Count > 0)
            {
                if (bots.Count > 1)
                {
                    var str = string.Join(",", bots.Select(b => b.Id));
                    while (str.Length < 7)
                        if (str.Length % 2 == 0)
                            str = " " + str;
                        else
                            str += " ";
                    Console.Write(str);
                }
                else
                {
                    Console.Write(GetSingleCharCell(bots[0].Id.ToString()[0]));
                }

                return;
            }

            Console.Write(GetSingleCharCell('-'), bots.Count);
        }

        private static string GetSingleCharCell(char c)
        {
            return $"   {c}   ";
        }
    }
}
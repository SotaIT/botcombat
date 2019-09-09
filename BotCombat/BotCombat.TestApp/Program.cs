using BotCombat.Abstractions;
using BotCombat.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BotCombat.TestApp
{
    class Program
    {
        static Map map;
        static void Main(string[] args)
        {
            var walls = new List<Wall>() { new Wall(3, 3, null) };
            var bonuses = new List<Bonus>() { new Bonus(0, 0, 5, null), new Bonus(2, 1, 3, null), new Bonus(3, 4, 15, null) };
            var bots = new List<IBot> { 
                new TestBot(), 
                new TestBot2(), 
                new Js.JsBot(3, 10000, null, Js.JsBot.DefaultInitPowerScript, Js.JsBot.DefaultDistributePowerScript, Js.JsBot.DefaultChooseDirectionScript),
                new Cs.CsBot(4, 10000, null, Cs.CsBot.DefaultSourceCode)};
            var mapSettings = new MapSettings(1, 5, 5, 32, 10, 1, 1, walls, bonuses);

            map = new Map(mapSettings, bots);

            for (int i = 0; i < 1000; i++)
            {
                var step = map.Step();
                DrawStep(step);

                if (step.Bots.Count < 2)
                {
                    Console.WriteLine("Game Over in {0} steps.", i);

                    if (step.Bots.Count == 0)
                        Console.WriteLine("All bots dead!");
                    else
                        Console.WriteLine("The winner is: {0}!", step.Bots.Values.First().Id);
                    break;
                }

                Thread.Sleep(10);
            }

            Console.WriteLine("Time Over.");
        }


        private static void DrawStep(Step step)
        {
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.Write(new string('=', step.Map.Width * 10));
            Console.WriteLine(" ");
            Console.WriteLine(" ");

            HLine(step);

            for (var y = 0; y < step.Map.Height; y++)
            {
                Console.Write(" | ");
                for (var x = 0; x < step.Map.Width; x++)
                {
                    DrawPoint(step, x, y);
                    Console.Write(" | ");
                }
                HLine(step);
            }

            foreach (var dl in step.DamageLogs)
                Console.WriteLine($"Bot {dl.SourceId} made {dl.Damage} damage to bot {dl.TargetId} at ({dl.X}, {dl.Y}).");

        }

        private static void HLine(Step step)
        {
            Console.WriteLine(" ");
            Console.Write(" |");
            Console.Write(new string('-', step.Map.Width * 10 - 1));
            Console.Write("| ");
            Console.WriteLine(" ");
        }

        private static void DrawPoint(Step step, int x, int y)
        {
            var wall = step.Walls.FirstOrDefault(w => w.X == x && w.Y == y);
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
                    {
                        if (str.Length % 2 == 0)
                            str = " " + str;
                        else
                            str += " ";
                    }
                    Console.Write(str);
                }
                else
                    Console.Write(GetSingleCharCell(bots[0].Id.ToString()[0]));

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

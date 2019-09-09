using System.Collections.Generic;
using System.Linq;
using BotCombat.Web.Models;

namespace BotCombat.Web.Converters
{
    public static class GameExtensions
    {
        public static Game ToModel(this Abstractions.Models.Game game)
        {
            return new Game
            {
                Map = game.Map.ToModel(),
                Steps = game.Steps.ToModel()
            };
        }

        public static Map ToModel(this Abstractions.Models.Map map)
        {
            return new Map
            {
                Id = map.Id,
                Width = map.Width,
                Height = map.Height,
                Walls = map.Walls.ToModel()
            };
        }

        public static List<Step> ToModel(this IEnumerable<Abstractions.Models.Step> steps)
        {
            return steps.Select(ToModel).ToList();
        }

        public static Step ToModel(this Abstractions.Models.Step step)
        {
            return new Step
            {
                Number = step.Number,
                Bonuses = step.Bonuses.ToModel(),
                Bots = step.Bots.Values.ToModel(),
                DeadBots = step.DeadBots.ToList(),
                Logs = step.Logs.ToModel()
            };
        }

        public static List<Object> ToModel(this IEnumerable<Abstractions.Models.Object> objects)
        {
            return objects.Select(o => new Object
            {
                X = o.X,
                Y = o.Y
            }).ToList();
        }

        public static List<Bot> ToModel(this IEnumerable<Abstractions.Models.Bot> bots)
        {
            return bots.Select(bot => new Bot
            {
                X = bot.X,
                Y = bot.Y,
                Id = bot.Id
            }).ToList();
        }

        public static List<Log> ToModel(this IEnumerable<Abstractions.Models.Log> logs)
        {
            return logs.Select(log => new Log
            {
                X = log.X,
                Y = log.Y,
                SourceId = log.SourceId,
                TargetId = log.TargetId,
                Value = log.Value
            }).ToList();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using BotCombat.Web.JsonModels;

namespace BotCombat.Web.Converters
{
    public static class GameExtensions
    {
        public static Game ToJsonModel(this Abstractions.BotModels.Game game)
        {
            return new Game
            {
                Steps = game.Steps.ToJsonModel()
            };
        }

        public static List<Step> ToJsonModel(this IEnumerable<Abstractions.BotModels.Step> steps)
        {
            return steps.Select(ToJsonModel).ToList();
        }

        public static Step ToJsonModel(this Abstractions.BotModels.Step step)
        {
            return new Step
            {
                Number = step.Number,
                Bonuses = step.Bonuses.ToJsonModel(),
                Bots = step.Bots.Values.ToJsonModel(),
                DeadBots = step.DeadBots.ToList(),
                Logs = step.Logs.ToJsonModel()
            };
        }

        public static List<Bonus> ToJsonModel(this IEnumerable<Abstractions.BotModels.Bonus> objects)
        {
            return objects.Select(o => new Bonus
            {
                Id = o.Id,
                X = o.X,
                Y = o.Y
            }).ToList();
        }

        public static List<Bot> ToJsonModel(this IEnumerable<Abstractions.BotModels.Bot> bots)
        {
            return bots.Select(bot => new Bot
            {
                X = bot.X,
                Y = bot.Y,
                Id = bot.Id
            }).ToList();
        }

        public static List<Log> ToJsonModel(this IEnumerable<Abstractions.BotModels.Log> logs)
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
using System.Collections.Generic;
using System.Linq;
using BotCombat.Abstractions;
using BotCombat.Abstractions.Models;
using BotCombat.Web.Models;
using Bot = BotCombat.Web.Models.Bot;
using Log = BotCombat.Web.Models.Log;

namespace BotCombat.Web.Converters
{
    public static class StepExtensions
    {
        public static IEnumerable<StepModel> ToModel(this IEnumerable<Step> steps)
        {
            return steps.Select(ToModel);
        }

        public static StepModel ToModel(this Step step)
        {
            return new StepModel
            {
                Number = step.Number,
                Bonuses = step.Bonuses.ToBonusModel(),
                Bots = step.Bots.Values.ToModel(),
                DeadBots = step.DeadBots.ToList(),
                Logs = step.Logs.ToModel()
            };
        }

        public static List<Bonus> ToBonusModel(this IEnumerable<Object> bonuses)
        {
            return bonuses.Select(bonus => new Bonus
            {
                X = bonus.X,
                Y = bonus.Y
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
using System.Collections.Generic;
using System.Linq;
using BotCombat.Abstractions;
using BotCombat.Web.Data;

namespace BotCombat.Web.Services
{
    public class BotDataService : BaseDataService
    {
        public BotDataService(ApplicationDbContext db) : base(db)
        {
        }

        internal List<IBot> GetBots(List<int> botIds)
        {
            return Db.Bots
                .Where(bot => botIds.Contains(bot.Id))
                .Select(bot => BotFactory.CreateBot((BotFactory.BotType)bot.Type, bot.Id, bot.Code))
                .ToList();
        }

    }
}
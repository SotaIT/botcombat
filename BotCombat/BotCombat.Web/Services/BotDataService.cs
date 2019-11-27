using BotCombat.Web.Data;
using BotCombat.Web.Data.Domain;

namespace BotCombat.Web.Services
{
    public class BotDataService : BaseDataService<Bot>
    {
        public BotDataService(ApplicationDbContext db) : base(db)
        {
        }
    }
}
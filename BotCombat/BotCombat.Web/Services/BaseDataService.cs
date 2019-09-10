using BotCombat.Web.Data;

namespace BotCombat.Web.Services
{
    public abstract class BaseDataService
    {
        protected readonly ApplicationDbContext Db;

        protected BaseDataService(ApplicationDbContext db)
        {
            Db = db;
        }
    }
}
using BotCombat.Web.Data;
using BotCombat.Web.Data.Domain;

namespace BotCombat.Web.Services
{
    public class ImageDataService : BaseDataService<Image>
    {
        public ImageDataService(ApplicationDbContext db) : base(db)
        {
        }
    }
}
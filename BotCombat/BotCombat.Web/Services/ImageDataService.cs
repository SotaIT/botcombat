using System.Collections.Generic;
using System.Linq;
using BotCombat.Web.Data;
using BotCombat.Web.Data.Domain;

namespace BotCombat.Web.Services
{
    public class ImageDataService : BaseDataService
    {
        public ImageDataService(ApplicationDbContext db) : base(db)
        {
        }

        public IQueryable<Image> GetImages(List<int> imageIds)
        {
            return Db.Images.Where(i => imageIds.Contains(i.Id));
        }

    }
}
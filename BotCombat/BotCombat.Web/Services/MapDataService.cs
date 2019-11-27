using System.Collections.Generic;
using System.Linq;
using BotCombat.Web.Data;
using BotCombat.Web.Data.Domain;

namespace BotCombat.Web.Services
{
    public class MapDataService : BaseDataService<Map>
    {
        public MapDataService(ApplicationDbContext db) : base(db)
        {
        }

        private IQueryable<T> GetMapObjectsQuery<T>() where T: class, IMapObject
        {
            return Db.Set<T>();
        }

        private IQueryable<T> GetMapObjectsQuery<T>(int mapId) where T: class, IMapObject
        {
            return GetMapObjectsQuery<T>().Where(i => i.MapId == mapId);
        }

        public List<T> GetMapObjects<T>(int mapId) where T: class, IMapObject
        {
            return GetMapObjectsQuery<T>(mapId).ToList();
        }
        
    }
}
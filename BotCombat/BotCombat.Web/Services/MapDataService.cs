using System.Linq;
using BotCombat.Web.Data;
using BotCombat.Web.Data.Domain;

namespace BotCombat.Web.Services
{
    public class MapDataService : BaseDataService
    {
        public MapDataService(ApplicationDbContext db) : base(db)
        {
        }

        public Core.Models.Map GetCoreMap(int mapId)
        {
            var dbMap = GetMap(mapId);
            if (dbMap == null)
                return null;

            return new Core.Models.Map(dbMap.Id,
                dbMap.Width,
                dbMap.Height,
                dbMap.Scale,
                dbMap.InitialPower,
                dbMap.StrengthWeight,
                dbMap.StaminaWeight,
                GetMapWalls(mapId).Select(w => new Core.Models.Wall(w.X, w.Y)).ToList(),
                GetMapBonuses(mapId).Select(b => new Core.Models.Bonus(b.Id, b.X, b.Y, b.Power)).ToList(),
                GetMapTraps(mapId).Select(t => new Core.Models.Trap(t.X, t.Y, t.Damage)).ToList());
        }

        public Map GetMap(int mapId)
        {
            return Db.Maps.FirstOrDefault(m => m.Id == mapId);
        }

        public IQueryable<Wall> GetMapWalls(int mapId)
        {
            return Db.Walls.Where(i => i.MapId == mapId);
        }

        public IQueryable<Bonus> GetMapBonuses(int mapId)
        {
            return Db.Bonuses.Where(i => i.MapId == mapId);
        }

        public IQueryable<Trap> GetMapTraps(int mapId)
        {
            return Db.Traps.Where(i => i.MapId == mapId);
        }
    }
}
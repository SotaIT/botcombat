using System.Linq;
using BotCombat.Core.Models;
using BotCombat.Web.Data;

namespace BotCombat.Web.Services
{
    public class MapDataService : BaseDataService
    {
        public MapDataService(ApplicationDbContext db) : base(db)
        {
        }

        internal Map GetCoreMap(int mapId)
        {
            var dbMap = Db.Maps.FirstOrDefault(m => m.Id == mapId);
            if (dbMap == null)
                return null;

            return new Map(dbMap.Id,
                dbMap.Width,
                dbMap.Height,
                dbMap.Scale,
                dbMap.InitialPower,
                dbMap.StrengthWeight,
                dbMap.StaminaWeight,
                Db.Walls.Where(w => w.MapId == mapId).Select(w => new Wall(w.X, w.Y)).ToList(),
                Db.Bonuses.Where(b => b.MapId == mapId).Select(b => new Bonus(b.X, b.Y, b.Power)).ToList());
        }
    }
}
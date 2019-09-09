using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BotCombat.Abstractions
{
    /// <summary>
    /// The model of the current step
    /// </summary>
    public class Step
    {
        public Step(int number, 
            int mapId, 
            int width, 
            int height, 
            IList<Models.Object> walls,
            IList<Models.Object> bonuses,
            IDictionary<int, Models.Bot> bots,
            IList<Models.DamageLog> damageLogs)
        {
            Number = number;
            Map = new Models.Map(mapId, width, height);
            Walls = new ReadOnlyCollection<Models.Object>(walls);
            Bonuses = new ReadOnlyCollection<Models.Object>(bonuses);
            Bots = new ReadOnlyDictionary<int, Models.Bot>(bots);
            DamageLogs = new ReadOnlyCollection<Models.DamageLog>(damageLogs);
        }

        public int Number { get; }
        public Models.Map Map { get; }

        public IReadOnlyList<Models.Object> Walls { get; }

        public IReadOnlyList<Models.Object> Bonuses { get; }

        public IReadOnlyDictionary<int, Models.Bot> Bots { get; }

        public IReadOnlyList<Models.DamageLog> DamageLogs { get; }
    }
}

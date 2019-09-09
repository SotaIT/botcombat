using System.Collections.Generic;
using System.Collections.ObjectModel;
using BotCombat.Abstractions.Models;

// ReSharper disable once CheckNamespace
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
            IList<Object> walls,
            IList<Object> bonuses,
            IDictionary<int, Bot> bots,
            IList<DamageLog> damageLogs)
        {
            Number = number;
            Map = new Map(mapId, width, height);
            Walls = new ReadOnlyCollection<Object>(walls);
            Bonuses = new ReadOnlyCollection<Object>(bonuses);
            Bots = new ReadOnlyDictionary<int, Bot>(bots);
            DamageLogs = new ReadOnlyCollection<DamageLog>(damageLogs);
        }

        public int Number { get; }

        public Map Map { get; }

        public IReadOnlyList<Object> Walls { get; }

        public IReadOnlyList<Object> Bonuses { get; }

        public IReadOnlyDictionary<int, Bot> Bots { get; }

        public IReadOnlyList<DamageLog> DamageLogs { get; }
    }
}
using System.Collections.Generic;

namespace BotCombat.Core
{
    /// <summary>
    /// The model of the current step
    /// </summary>
    public class Step
    {
        public Step(int number, MapSettings mapSettings, IEnumerable<IMapObject> walls, IEnumerable<IMapObject> bonuses, IEnumerable<BotContainer> bots)
        {
            Number = number;
            Map = mapSettings.ToMapModel();
            Walls = walls.ToMapObjectModels();
            Bonuses = bonuses.ToMapObjectModels();
            Bots = bots.ToMapBotModels();
        }

        public int Number { get; }

        public Models.Map Map { get; }

        public IReadOnlyList<Models.Object> Walls { get; }

        public IReadOnlyList<Models.Object> Bonuses { get; }

        public IReadOnlyDictionary<int, Models.Bot> Bots { get; }

    }
}

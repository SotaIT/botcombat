using System.Collections.Generic;

namespace BotCombat.Web
{
    internal class MapJsonModel
    {
        public int Id { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public IEnumerable<MapObjectJsonModel> Walls { get; set; }

        public IEnumerable<MapObjectJsonModel> Traps { get; set; }
    }
}
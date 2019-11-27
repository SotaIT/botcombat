using System.Collections.Generic;

namespace BotCombat.Web
{
    internal class GameRootJsonModel
    {
        public GameJsonModel Game { get; set; }
        public int Scale { get; set; }
        public int Background { get; set; }
        public int BulletSpeed { get; set; }
        public Dictionary<int, int> WallImages { get; set; }
        public Dictionary<int, int> BonusImages { get; set; }
        public Dictionary<int, int> TrapImages { get; set; }
        public Dictionary<int, int> BotImages { get; set; }
        public Dictionary<int, string> Images { get; set; }
    }
}
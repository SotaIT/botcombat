using BotCombat.Core;
using System.Collections.Generic;

namespace BotCombat.Engine.Contracts
{
    public class GameSettings
    {
        public MapSettings MapSettings { get; set; }

        public IEnumerable<BotSettings> Bots { get; set; }

        public bool DebugMode { get; set; }
    }
}
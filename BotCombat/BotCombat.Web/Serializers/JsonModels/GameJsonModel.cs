using System.Collections.Generic;

namespace BotCombat.Web
{
    internal class GameJsonModel
    {
        public MapJsonModel Map { get; set; }
        public IEnumerable<StepJsonModel> Steps { get; set; }
    }
}
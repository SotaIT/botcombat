using System.Collections.Generic;

namespace BotCombat.Web.JsonModels
{
    public class Game
    {
        public Map Map { get; set; }

        public List<Step> Steps { get; set; }
    }
}
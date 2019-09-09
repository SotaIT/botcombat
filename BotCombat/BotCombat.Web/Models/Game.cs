using System.Collections.Generic;

namespace BotCombat.Web.Models
{
    public class Game
    {
        public Map Map { get; set; }

        public List<Step> Steps { get; set; }
    }
}
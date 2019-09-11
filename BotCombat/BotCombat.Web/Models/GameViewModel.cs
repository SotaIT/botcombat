using System.Collections.Generic;
using System.Linq;
using BotCombat.Web.Data.Domain;

namespace BotCombat.Web.Models
{
    public class GameViewModel
    {
        public Map Map { get; set; }
        public List<Wall> Walls { get; set; }
        public List<Bonus> Bonuses { get; set; }
        public List<Trap> Traps { get; set; }
        public List<Bot> Bots { get; set; }
        public List<Image> Images { get; set; }
    }
}

using System.Collections.Generic;

namespace BotCombat.Web.Models
{
    public class StepModel
    {
        public int Number { get; set;}

        public List<Bonus> Bonuses { get; set;}

        public List<Bot> Bots { get; set; }

        public List<Log> Logs { get; set;}

        public List<int> DeadBots { get; set;}
    }
}
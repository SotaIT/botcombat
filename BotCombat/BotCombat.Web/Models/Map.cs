using System.Collections.Generic;

namespace BotCombat.Web.Models
{
    public class Map
    {
        public int Id { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public List<Object> Walls { get; set; }
    }
}
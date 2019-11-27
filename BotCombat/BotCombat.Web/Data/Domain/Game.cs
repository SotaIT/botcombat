using System;

namespace BotCombat.Web.Data.Domain
{
    public class Game: IEntity
    {
        public int Id { get; set; }
        public int MapId { get; set; }
        public int State { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Played { get; set; }
        public string Json { get; set; }
    }
}
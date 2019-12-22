using System;
using BotCombat.Web.Data.Domain;

namespace BotCombat.Web.Models
{
    public class BotListItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
        public DateTime Created { get; set; }
        public BotStatus Status { get; set; }
    }
}
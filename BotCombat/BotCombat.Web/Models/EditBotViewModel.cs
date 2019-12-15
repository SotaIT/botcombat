using System;
using System.ComponentModel.DataAnnotations;
using BotCombat.Web.Data.Domain;

namespace BotCombat.Web.Models
{
    public class EditBotViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Version { get; set; }
        public DateTime Created { get; set; }
        [Required]
        public string Code { get; set; }
        public BotStatus Status { get; set; }
        public AuthorBot[] Versions { get; set; }
        public bool ContinueEdit { get; set; }
    }
}
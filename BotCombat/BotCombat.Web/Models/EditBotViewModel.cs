using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BotCombat.Core;
using BotCombat.Web.Data.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BotCombat.Web.Models
{
    public class EditBotViewModel
    {
        /// <summary>
        /// Root AuthorBot Id
        /// </summary>
        public int Id { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
        public DateTime Created { get; set; }
        [Required]
        public string Code { get; set; }
        public BotStatus Status { get; set; }
        
        [Display(Name="Continue editing after save")]
        public bool ContinueEdit { get; set; }

        [Display(Name="Run the bot after save")]
        public bool Run { get; set; }
        public string Game { get; set; }

        public AuthorBot[] Versions { get; set; }
        public List<SelectListItem> Statuses { get; set; }
    }
}
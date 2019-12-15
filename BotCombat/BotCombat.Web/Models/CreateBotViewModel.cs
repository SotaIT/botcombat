using System.ComponentModel.DataAnnotations;

namespace BotCombat.Web.Models
{
    public class CreateBotViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
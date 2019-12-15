using BotCombat.Web.Data.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BotCombat.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Map> Maps { get; set; }
        public DbSet<Wall> Walls { get; set; }
        public DbSet<Bonus> Bonuses { get; set; }
        public DbSet<Trap> Traps { get; set; }
        public DbSet<StartPoint> StartPoints { get; set; }
        public DbSet<BotImage> BotImages { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Bot> Bots { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameBot> GameBots { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<AuthorBot> AuthorBots { get; set; }
    }
}

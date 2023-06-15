using Microsoft.EntityFrameworkCore;
using GameComponents.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace InspectorGoeServer.Models
{
    public class PlayerContext : IdentityDbContext
    {
        public PlayerContext(DbContextOptions<PlayerContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new PlayerConfiguration());
        }
        public DbSet<Player> Players { get; set; } = null!;
    }
}

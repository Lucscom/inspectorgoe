using Microsoft.EntityFrameworkCore;
using GameComponents.Model;

namespace InspectorGoeServer.Models
{
    public class PlayerContext : DbContext
    {
        public PlayerContext(DbContextOptions<PlayerContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; } = null!;
    }
}

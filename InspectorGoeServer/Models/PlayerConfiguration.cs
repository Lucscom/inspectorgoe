using GameComponents.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InspectorGoeServer.Models
{
    internal class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
#if DEBUG
            builder.HasData(
                new Player
                {
                    UserName = "admin",
                    Password = "admin",
                });
#endif
        }
    }
}
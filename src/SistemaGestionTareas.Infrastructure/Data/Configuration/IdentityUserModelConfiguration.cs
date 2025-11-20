using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaGestionTareas.ApplicationCore.Entities;
using SistemaGestionTareas.Infrastructure.AuthProviders.Identity;

namespace SistemaGestionTareas.Infrastructure.Data.Configuration
{
    public class IdentityUserModelConfiguration : IEntityTypeConfiguration<ApplicationIdentityUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationIdentityUser> builder)
        {
            builder.HasMany<TaskItem>()
               .WithOne()
               .HasForeignKey(taskItem => taskItem.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(user => user.RefreshTokens)
               .WithOne(refreshToken => refreshToken.User)
               .HasForeignKey(refreshToken => refreshToken.UserId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

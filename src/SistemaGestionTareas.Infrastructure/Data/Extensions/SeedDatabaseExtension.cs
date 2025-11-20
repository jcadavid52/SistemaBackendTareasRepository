using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemaGestionTareas.Infrastructure.AuthProviders.Identity;

namespace SistemaGestionTareas.Infrastructure.Data.Extensions
{
    public static class SeedDatabaseExtension
    {
        public static void AddIdentityUsers(ModelBuilder builder)
        {
            var adminUser = new ApplicationIdentityUser
            {
                Id = "d6a0c2c8-7f9b-4f68-a531-21d6c5292b9a",
                Firstname = "User",
                LastName = "Default",
                UserName = "userdefault@system.com",
                NormalizedUserName = "USERDEFAULT@SYSTEM.COM",
                Email = "userdefault@system.com",
                NormalizedEmail = "USERDEFAULT@SYSTEM.COM",
                EmailConfirmed = false,
                PasswordHash = "AQAAAAIAAYagAAAAEFFGUm7R+rXk/dMYDZogx+PJk25imPWXpa4pT5HiucVf6LP+EX8jzNebi3JzWeBE8w==",
                SecurityStamp = "4A3F5D12-9D3A-4C62-AE15-6EFA4B8B0D78",
                ConcurrencyStamp = "E29F91E7-90FA-40A3-AE2F-9C5293EE60C1",
                AccessFailedCount = 0,
                LockoutEnabled = false,
                TwoFactorEnabled = false
            };

            builder.Entity<ApplicationIdentityUser>().HasData(adminUser);
        }

        public static void RenameTablesIdentityUsers(ModelBuilder builder)
        {
            builder.Entity<IdentityUser>(entity =>
            {
                entity.ToTable("AccountUsers");
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable("AccountRoles");
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("AccountUserRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("AccountUserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("AccountUserLogins");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("AccountRoleClaims");
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("AccountUserTokens");
            });
        }
    }
}

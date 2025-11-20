using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaGestionTareas.ApplicationCore.Entities;
using SistemaGestionTareas.Infrastructure.AuthProviders.Identity;
using SistemaGestionTareas.Infrastructure.Data.Extensions;
using SistemaGestionTareas.Infrastructure.TokenProviders;

namespace SistemaGestionTareas.Infrastructure.Data
{
    public class DataContext(DbContextOptions options) : IdentityDbContext(options)
    {
        public DbSet<ApplicationIdentityUser> IdentityUsers { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            SeedDatabaseExtension.RenameTablesIdentityUsers(builder);
            SeedDatabaseExtension.AddIdentityUsers(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        }

    }
}

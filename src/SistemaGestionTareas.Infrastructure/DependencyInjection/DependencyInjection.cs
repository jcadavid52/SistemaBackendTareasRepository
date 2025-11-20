using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SistemaGestionTareas.ApplicationCore.Abstractions.Interfaces;
using SistemaGestionTareas.Infrastructure.AuthProviders.Identity;
using SistemaGestionTareas.Infrastructure.Common;
using SistemaGestionTareas.Infrastructure.Data;
using SistemaGestionTareas.Infrastructure.Data.Repositories;
using SistemaGestionTareas.Infrastructure.TokenProviders;
using System.Text;

namespace SistemaGestionTareas.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
           this IServiceCollection services,
           IConfiguration configuration
           )
        {
            services.AddTransient<IAuthProvider, AuthIdentityProvider>();
            services.AddTransient<ITokenProvider, TokenProvider>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            var connectionString = configuration.GetConnectionString("db")
            ?? throw new ArgumentNullException(nameof(configuration));

            services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));

            services.AddIdentityCore<ApplicationIdentityUser>(options => { })
               .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<DataContext>()
               .AddTokenProvider<EmailTokenProvider<ApplicationIdentityUser>>("Email");

            var _repositories = AppDomain.CurrentDomain.GetAssemblies()
                            .Where(assembly =>
                            {
                                return assembly.FullName is null || assembly.FullName.Contains("Infrastructure", StringComparison.OrdinalIgnoreCase);
                            })
                            .SelectMany(assembly => assembly.GetTypes())
                            .Where(type => type.CustomAttributes.Any(attribute => attribute.AttributeType == typeof(RepositoryAttribute)));

            foreach (var repository in _repositories)
            {
                foreach (var typeInterface in repository.GetInterfaces())
                {
                    services.AddTransient(typeInterface, repository);
                }
            }

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetValue<string>("ApiSettings:SecretKey")!)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = configuration.GetValue<string>("ApiSettings:Issuer"),
                    ValidAudience = configuration.GetValue<string>("ApiSettings:Audience"),
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true

                };
            });

            return services;
        }
    }
}

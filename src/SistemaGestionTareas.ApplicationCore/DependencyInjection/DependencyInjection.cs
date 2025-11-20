using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SistemaGestionTareas.ApplicationCore.Abstractions;

namespace SistemaGestionTareas.ApplicationCore.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationCore(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;
            services.AddValidatorsFromAssembly(assembly);

            var _appServices = AppDomain.CurrentDomain.GetAssemblies()
                        .Where(assembly =>
                        {
                            return assembly.FullName is null || assembly.FullName.Contains("ApplicationCore", StringComparison.OrdinalIgnoreCase);
                        })
                        .SelectMany(assembly => assembly.GetTypes())
                        .Where(type => type.CustomAttributes.Any(attribute => attribute.AttributeType == typeof(ApplicationServiceAttribute)));

            foreach (var appService in _appServices)
            {
                services.AddTransient(appService);
            }

            return services;
        }
    }
}

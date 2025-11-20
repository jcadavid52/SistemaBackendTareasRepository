using FluentValidation.AspNetCore;
using SistemaGestionTareas.Api.Extensions;

namespace SistemaGestionTareas.Api.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApi(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("FrontendClient", builder =>
                {
                    builder
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();

            services.AddHttpContextAccessor();
            services.AddProblemDetails();
            services.AddSwaggerExtension();

            return services;
        }
    }
}

using SistemaGestionTareas.Api.DependencyInjection;
using SistemaGestionTareas.Api.Extensions;
using SistemaGestionTareas.Api.Infrastructure;
using SistemaGestionTareas.ApplicationCore.DependencyInjection;
using SistemaGestionTareas.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationCore();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddApi();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseMiddleware<AppExceptionHandlerMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerExtension();
}
app.MapControllers();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("FrontendClient");

app.UseAuthentication();

app.UseAuthorization();

app.Run();

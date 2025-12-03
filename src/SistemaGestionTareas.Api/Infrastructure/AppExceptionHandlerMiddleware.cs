using SistemaGestionTareas.ApplicationCore.Exceptions;
using SistemaGestionTareas.Infrastructure.Exceptions;
using System.Net;

namespace SistemaGestionTareas.Api.Infrastructure
{
    public class AppExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AppExceptionHandlerMiddleware> _logger;
        private static readonly Dictionary<Type, HttpStatusCode> StatusCodes = new()
        {
            { typeof(NoAuthorizedException), HttpStatusCode.Unauthorized },
            { typeof(ArgumentException), HttpStatusCode.BadRequest },
            { typeof(ArgumentNullException), HttpStatusCode.BadRequest },
            { typeof(DuplicateUserNameException), HttpStatusCode.Conflict },
            { typeof(InvalidPasswordException), HttpStatusCode.BadRequest },
            { typeof(RefreshTokenException), HttpStatusCode.InternalServerError },
            { typeof(AccessTokenException), HttpStatusCode.InternalServerError },
            { typeof(NoFoundException), HttpStatusCode.NotFound },
            { typeof(InternalRegisterException), HttpStatusCode.InternalServerError },
        };

        public AppExceptionHandlerMiddleware(RequestDelegate next, ILogger<AppExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: {Message}", ex.Message);

                context.Response.Clear();
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = GetStatusCodeForException(ex);

                var response = new ApiResponse(ex.Message, context.Response.StatusCode);
                var result = System.Text.Json.JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(result);
            }

        }

        private static int GetStatusCodeForException(Exception ex)
        {
            return StatusCodes.TryGetValue(ex.GetType(), out var statusCode)
                ? (int)statusCode
                : (int)HttpStatusCode.InternalServerError;
        }
    }

    public record ApiResponse(string Message,int StatusCode);
}

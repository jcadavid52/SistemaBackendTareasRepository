using SistemaGestionTareas.ApplicationCore.Dtos;
using SistemaGestionTareas.ApplicationCore.Dtos.Response;

namespace SistemaGestionTareas.ApplicationCore.Abstractions.Interfaces
{
    public interface ITokenProvider
    {
        string GenerateAccessToken(UserDto user);
        Task<AuthorizedResponseDto?> ValidateRefreshTokenAsync(string refreshToken,CancellationToken cancellationToken);
        Task<string> GenerateRefreshTokenAsync(string userId,CancellationToken cancellationToken);
    }
}

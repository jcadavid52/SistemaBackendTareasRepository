using SistemaGestionTareas.ApplicationCore.Abstractions;
using SistemaGestionTareas.ApplicationCore.Abstractions.Interfaces;
using SistemaGestionTareas.ApplicationCore.Dtos.Request;
using SistemaGestionTareas.ApplicationCore.Dtos.Response;
using SistemaGestionTareas.ApplicationCore.Entities;
using SistemaGestionTareas.ApplicationCore.Exceptions;

namespace SistemaGestionTareas.ApplicationCore.Services
{
    [ApplicationService]
    public class AuthService(
        IAuthProvider authProvider,
        ITokenProvider tokenProvider
        )
    {
        public async Task<AuthorizedResponseDto> LoginAsync(AuthLoginRequestDto loginRequestDto,CancellationToken cancellationToken)
        {
            var user = await authProvider.LoginAsync(loginRequestDto.Username, loginRequestDto.Password)
                ?? throw new NoAuthorizedException("Clave o Usuario inválido");

            string refreshToken = await tokenProvider.GenerateRefreshTokenAsync(user.Id,cancellationToken);

            string accessToken = tokenProvider.GenerateAccessToken(user);

            return new AuthorizedResponseDto(user,accessToken,refreshToken);
        }

        public async Task<AuthorizedResponseDto> RegisterAsync(AuthRegisterRequestDto registerRequestDto, CancellationToken cancellationToken)
        {
            var user = new User(
                registerRequestDto.FirstName,
                registerRequestDto.LastName,
                registerRequestDto.Email
                );

            var userDto = await authProvider.RegisterAsync(
               user,
               registerRequestDto.Password
               ) ?? throw new InternalRegisterException("No se pudo hacer el registro");

            string refreshToken = await tokenProvider.GenerateRefreshTokenAsync(userDto.Id, cancellationToken);

            string accessToken = tokenProvider.GenerateAccessToken(userDto);
            
            return new AuthorizedResponseDto(userDto, accessToken, refreshToken);
        }

        public async Task<AuthorizedResponseDto> RefreshTokenAsync(AuthRefreshTokenRequestDto refreshTokenRequestDto, CancellationToken cancellationToken)
        {
            var validatedRefreshToken = await tokenProvider.ValidateRefreshTokenAsync(refreshTokenRequestDto.RefreshToken,cancellationToken)
                ?? throw new NoAuthorizedException("Refresh Token inválido");

            return validatedRefreshToken;
        }
    }
}

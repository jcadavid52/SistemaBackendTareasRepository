using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SistemaGestionTareas.ApplicationCore.Abstractions.Interfaces;
using SistemaGestionTareas.ApplicationCore.Dtos;
using SistemaGestionTareas.ApplicationCore.Dtos.Response;
using SistemaGestionTareas.ApplicationCore.Entities;
using SistemaGestionTareas.ApplicationCore.Exceptions;
using SistemaGestionTareas.Infrastructure.Data;
using SistemaGestionTareas.Infrastructure.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SistemaGestionTareas.Infrastructure.TokenProviders
{
    public class TokenProvider(
        DataContext dataContext,
        IConfiguration configuration
        ) : ITokenProvider
    {
        public string GenerateAccessToken(UserDto user)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id),
                    new(ClaimTypes.Email, user.Email),
                    new(ClaimTypes.Name, user.FullName),
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["ApiSettings:SecretKey"]!));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: configuration["ApiSettings:Issuer"],
                    audience: configuration["ApiSettings:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new AccessTokenException($"Error al generar access token: {ex.Message}");
            }
           
        }

        public async Task<string> GenerateRefreshTokenAsync(string userId, CancellationToken cancellationToken)
        {
            var refreshTokenString = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var refreshToken = new RefreshToken()
            {
                Token = refreshTokenString,
                UserId = userId
            };

            try
            {
                await dataContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
                await dataContext.SaveChangesAsync(cancellationToken);

                return refreshTokenString;
            }
            catch(Exception ex)
            {
                throw new RefreshTokenException($"Error al generar refresh token: {ex.Message}");
            }
        }

        public async Task<AuthorizedResponseDto?> ValidateRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            var refreshTokenResult = await dataContext.RefreshTokens
                .Include(u => u.User)
                .FirstOrDefaultAsync(refresh => refresh.Token == refreshToken, cancellationToken);

            if (refreshTokenResult == null) 
            {
                return null;
            }

            if (refreshTokenResult.IsRevoked || refreshTokenResult.ExpiresAt <= DateTime.UtcNow)
            {
                return null;
            }

            refreshTokenResult.Revoke();

            await dataContext.SaveChangesAsync(cancellationToken);
            var user = new UserDto(
                    refreshTokenResult.User.Id,
                    refreshTokenResult.User.Firstname + " " + refreshTokenResult.User.LastName,
                    refreshTokenResult.User.Email!
                );

            string accessToken = GenerateAccessToken(user);


            string newRefreshToken = await GenerateRefreshTokenAsync(refreshTokenResult.UserId, cancellationToken);
            return new AuthorizedResponseDto(
               user,
               accessToken,
               newRefreshToken
            );
        }
    }
}

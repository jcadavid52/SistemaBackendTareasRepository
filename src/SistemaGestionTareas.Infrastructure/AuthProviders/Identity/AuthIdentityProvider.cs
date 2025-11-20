using Microsoft.AspNetCore.Identity;
using SistemaGestionTareas.ApplicationCore.Abstractions.Interfaces;
using SistemaGestionTareas.ApplicationCore.Dtos;
using SistemaGestionTareas.ApplicationCore.Dtos.Request;
using System.Reflection.Metadata.Ecma335;


namespace SistemaGestionTareas.Infrastructure.AuthProviders.Identity
{
    public class AuthIdentityProvider(
        UserManager<ApplicationIdentityUser> userManager
        ) : IAuthProvider
    {
        public async Task<UserDto?> LoginAsync(string Username, string Password)
        {
            var applicationIdentityUser = await userManager.FindByEmailAsync(Username);

            if (applicationIdentityUser == null)
            {
                return null;
            }

            var passwordChecked = await userManager.CheckPasswordAsync(applicationIdentityUser, Password);

            if (!passwordChecked)
                return null;

            return new UserDto(
                applicationIdentityUser.Id,
                applicationIdentityUser.Firstname + " " + applicationIdentityUser.LastName,
                applicationIdentityUser.Email!
                );
        }
    }
}

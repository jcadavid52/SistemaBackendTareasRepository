using Microsoft.AspNetCore.Identity;
using Microsoft.Win32;
using SistemaGestionTareas.ApplicationCore.Abstractions.Interfaces;
using SistemaGestionTareas.ApplicationCore.Dtos;
using SistemaGestionTareas.ApplicationCore.Entities;
using SistemaGestionTareas.ApplicationCore.Exceptions;
using SistemaGestionTareas.Infrastructure.Exceptions;


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

        public async Task<UserDto?> RegisterAsync(User user, string Password)
        {
            var applicationIdentityUser = ApplicationIdentityUser.MappingToIdentityUser(user);
            var result = await userManager.CreateAsync(applicationIdentityUser, Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    if (error.Code == "DuplicateUserName")
                    {
                        throw new DuplicateUserNameException($"Error:{error.Code} El usuario '{user.Email}' ya existe");
                    }
                    else if (error.Code == "PasswordRequiresNonAlphanumeric" || error.Code == "PasswordRequiresDigit" || error.Code == "PasswordRequiresUpper")
                    {
                        throw new InvalidPasswordException("La contraseña no cumple con la seguridad mínima: Debe contener Mayúsculas, Dígitos y Alfanuméricos");
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return new UserDto(
                applicationIdentityUser.Id,
                applicationIdentityUser.Firstname + " " + applicationIdentityUser.LastName,
                applicationIdentityUser.Email!
            );
        }
    }
}

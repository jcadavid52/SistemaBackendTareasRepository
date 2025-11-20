using Microsoft.AspNetCore.Identity;
using SistemaGestionTareas.ApplicationCore.Entities;
using SistemaGestionTareas.Infrastructure.TokenProviders;

namespace SistemaGestionTareas.Infrastructure.AuthProviders.Identity
{
    public class ApplicationIdentityUser: IdentityUser
    {
        public string Firstname { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public ICollection<TaskItem> TaskItems { get; set; } = [];
        public ICollection<RefreshToken> RefreshTokens { get; set; } = [];

        public static User MappingToUser(ApplicationIdentityUser identityUser)
        {
            return new User(identityUser.Firstname, identityUser.LastName, identityUser.Email ?? "")
            {
                Id = identityUser.Id
            };
        }

        public static ApplicationIdentityUser MappingToIdentityUser(User user)
        {
            return new ApplicationIdentityUser()
            {
                Firstname = user.Firstname,
                LastName = user.LastName,
                Email = user.Email
            };
        }
    }   
}

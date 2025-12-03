using SistemaGestionTareas.ApplicationCore.Dtos;
using SistemaGestionTareas.ApplicationCore.Entities;

namespace SistemaGestionTareas.ApplicationCore.Abstractions.Interfaces
{
    public interface IAuthProvider
    {
        Task<UserDto?> LoginAsync(string Username, string Password);
        Task<UserDto?> RegisterAsync(User user,string Password);
    }
}

using SistemaGestionTareas.ApplicationCore.Dtos;

namespace SistemaGestionTareas.ApplicationCore.Abstractions.Interfaces
{
    public interface IAuthProvider
    {
        Task<UserDto?> LoginAsync(string Username, string Password);
    }
}

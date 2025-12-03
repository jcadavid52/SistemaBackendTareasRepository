namespace SistemaGestionTareas.ApplicationCore.Dtos.Request
{
    public record AuthRegisterRequestDto(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string ConfirmPassword
        );
}

namespace SistemaGestionTareas.ApplicationCore.Dtos.Response
{
    public record AuthorizedResponseDto(
        UserDto User,
        string AccessToken,
        string RefreshToken
        );
}

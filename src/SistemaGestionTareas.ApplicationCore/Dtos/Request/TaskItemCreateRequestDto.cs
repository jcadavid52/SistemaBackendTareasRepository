namespace SistemaGestionTareas.ApplicationCore.Dtos.Request
{
    public record TaskItemCreateRequestDto(
        string Title,
        string Description
    );
}

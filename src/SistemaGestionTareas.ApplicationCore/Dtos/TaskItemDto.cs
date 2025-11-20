namespace SistemaGestionTareas.ApplicationCore.Dtos
{
    public record TaskItemDto(
        int Id,
        string Title,
        string Description,
        string Status,
        string CreateAt
    );
}

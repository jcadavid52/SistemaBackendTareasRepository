namespace SistemaGestionTareas.ApplicationCore.Dtos.Response
{
    public record GetAllTaskItemResponseDto(
        IEnumerable<TaskItemDto> Tasks,
        int PageSize,
        int TotalCount
        );
}

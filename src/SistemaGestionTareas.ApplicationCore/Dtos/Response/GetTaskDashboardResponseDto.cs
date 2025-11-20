namespace SistemaGestionTareas.ApplicationCore.Dtos.Response
{
    public record GetTaskDashboardResponseDto(
        int Total,
        int Pending,
        int Completed
        );
}

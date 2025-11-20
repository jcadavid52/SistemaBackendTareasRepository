using SistemaGestionTareas.ApplicationCore.Common.Contants;

namespace SistemaGestionTareas.ApplicationCore.Dtos.Queries
{
    public record GetAllTaskItemQueryDto(
        int? PageSize,
        int? PageNumber,
        string? Status,
        string SortOrder = "desc"
    );
}

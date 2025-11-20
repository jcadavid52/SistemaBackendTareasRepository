using SistemaGestionTareas.ApplicationCore.Entities;
using System.Linq.Expressions;

namespace SistemaGestionTareas.ApplicationCore.Abstractions.Interfaces
{
    public interface ITaskItemRepository:IGenericRepository<TaskItem>
    {
        Task<IEnumerable<TaskItem>> GetAllAsync(
            Expression<Func<TaskItem, bool>>[]? filters = null,
            Func<IQueryable<TaskItem>, IOrderedQueryable<TaskItem>>? orderBy = null,
            int? skip = null,
            int? take = null,
            string? userId = null
            );

        Task<TaskItem?> GetByIdAsync(
            int id,
            string? userId = null
            );
        Task<int> CountByStatusAsync(
            string? status = "",
            string? userId = null
            );
    }
}

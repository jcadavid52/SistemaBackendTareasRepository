using Microsoft.EntityFrameworkCore;
using SistemaGestionTareas.ApplicationCore.Abstractions.Interfaces;
using SistemaGestionTareas.ApplicationCore.Common.Contants;
using SistemaGestionTareas.ApplicationCore.Entities;
using System.Linq.Expressions;

namespace SistemaGestionTareas.Infrastructure.Data.Repositories
{
    [Repository]
    public class TaskItemRepository(DataContext dataContext) : GenericRepository<TaskItem>(dataContext), ITaskItemRepository
    {
        public async Task<IEnumerable<TaskItem>> GetAllAsync(
            Expression<Func<TaskItem, bool>>[]? filters = null,
            Func<IQueryable<TaskItem>, IOrderedQueryable<TaskItem>>? orderBy = null,
            int? skip = null,
            int? take = null,
            string? userId = null
            )
        {
            var taskItemQuery = Query();


            if (userId != null)
            {
                taskItemQuery = taskItemQuery.Where(ti => ti.UserId == userId);
            }

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    taskItemQuery = taskItemQuery.Where(filter);
                }
            }

            if (orderBy != null)
            {
                taskItemQuery = orderBy(taskItemQuery);
            }

            if (skip.HasValue)
            {
                taskItemQuery = taskItemQuery.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                taskItemQuery = taskItemQuery.Take(take.Value);
            }


            return await taskItemQuery
                .ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(
            int id,
            string? userId = null
            )
        {
            var taskItemQuery = Query();

            if (userId != null)
            {
                taskItemQuery = taskItemQuery.Where(ti => ti.UserId == userId);
            }

            return await taskItemQuery.FirstOrDefaultAsync(ti => ti.Id == id);
        }

        public async Task<int> CountByStatusAsync(string? status = StatusTaskItem.Todos, string? userId = null)
        {
            var taskItemQuery = Query();

            if (userId != null)
            {
                taskItemQuery = taskItemQuery.Where(ti => ti.UserId == userId);
            }

            if(status != StatusTaskItem.Todos)
            {
                taskItemQuery = taskItemQuery.Where(ti => ti.Status == status);
            }

            return await taskItemQuery.CountAsync();
        }
    }
}

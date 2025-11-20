using SistemaGestionTareas.ApplicationCore.Abstractions;
using SistemaGestionTareas.ApplicationCore.Abstractions.Interfaces;
using SistemaGestionTareas.ApplicationCore.Common.Contants;
using SistemaGestionTareas.ApplicationCore.Dtos;
using SistemaGestionTareas.ApplicationCore.Dtos.Queries;
using SistemaGestionTareas.ApplicationCore.Dtos.Request;
using SistemaGestionTareas.ApplicationCore.Dtos.Response;
using SistemaGestionTareas.ApplicationCore.Entities;
using SistemaGestionTareas.ApplicationCore.Exceptions;
using System.Linq.Expressions;

namespace SistemaGestionTareas.ApplicationCore.Services
{
    [ApplicationService]
    public class TaskItemService(
        ITaskItemRepository taskItemRepository,
        IUnitOfWork unitOfWork
        )
    {
        public async Task<GetAllTaskItemResponseDto> GetAllTaskItemAsync(GetAllTaskItemQueryDto query,string userId)
        {
            int totalCountByStatus = 0;
            var filters = new List<Expression<Func<TaskItem, bool>>>();
            Func<IQueryable<TaskItem>, IOrderedQueryable<TaskItem>> orderBy = a => a.OrderByDescending(b => b.CreatedAt);

            if (query.SortOrder.Trim() == "asc")
            {
                orderBy = a => a.OrderBy(b => b.CreatedAt);
            }

            if (!string.IsNullOrWhiteSpace(query.Status))
            {
                filters.Add(b => b.Status.Contains(query.Status.Trim()));
                totalCountByStatus = await taskItemRepository.CountByStatusAsync(status: query.Status, userId: userId);
            }
            else
            {
                totalCountByStatus = await taskItemRepository.CountByStatusAsync(userId: userId);
            }  
            
            if(query.PageNumber <= 0 || query.PageSize <= 0)
            {
                throw new ArgumentException("Argumentos inválidos para la paginación");
            }

            var taskItems = await taskItemRepository.GetAllAsync(
                skip:(query.PageNumber - 1) * query.PageSize,
                take:query.PageSize,
                filters: [.. filters],
                orderBy:orderBy,
                userId:userId
            );

            var taskItemsDto = taskItems.Select(task =>
            {
                string createDateFormat = task.CreatedAt.Day + " de " + task.CreatedAt.ToString("MMMM") + ", " + task.CreatedAt.Year + ", Hora: " + task.CreatedAt.Hour + ":" + task.CreatedAt.Minute + ":" + task.CreatedAt.Second;

                var taskItemDto = new TaskItemDto(
                    task.Id,
                    task.Title,
                    task.Description,
                    task.Status,
                    createDateFormat
                );

                return taskItemDto;
            });

            int pageSize = taskItemsDto.Count();

            return new GetAllTaskItemResponseDto(taskItemsDto, pageSize, totalCountByStatus);
        }

        public async Task<GetByIdTaskItemResponseDto> GetByIdTaskItemAsync(GetByIdTaskItemQueryDto query, string userId)
        {
            var taskItem = await taskItemRepository.GetByIdAsync(query.Id, userId)
               ?? throw new NoFoundException($"No se econtró tarea con id '{query.Id}'");

            string createDateFormat = taskItem.CreatedAt.Day + " de " + taskItem.CreatedAt.ToString("MMMM") + ", " + taskItem.CreatedAt.Year + ", Hora: " + taskItem.CreatedAt.Hour + ":" + taskItem.CreatedAt.Minute + ":" + taskItem.CreatedAt.Second;

            var taskItemDto = new TaskItemDto(taskItem.Id,taskItem.Title,taskItem.Description,taskItem.Status, createDateFormat);

            return new GetByIdTaskItemResponseDto(taskItemDto);
        }

        public async Task<GetTaskDashboardResponseDto> GetTaskDashboardAsync(string userId)
        {
            var total = await taskItemRepository.CountByStatusAsync(userId: userId);
            var pending = await taskItemRepository.CountByStatusAsync(status:StatusTaskItem.Pendiente, userId:userId);
            var completed = await taskItemRepository.CountByStatusAsync(status: StatusTaskItem.Completado, userId: userId);

            return new GetTaskDashboardResponseDto(total, pending, completed);
        }

        public async Task<CreateTaskResponseDto> CreateTaskAsync(TaskItemCreateRequestDto request, string userId,CancellationToken cancellationToken)
        {
            var taskItem = new TaskItem(
                request.Title,
                request.Description,
                userId
            );

            await taskItemRepository.CreateAsync(taskItem);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateTaskResponseDto(taskItem.Id);
        }

        public async Task UpdateTaskAsync(TaskItemUpdateRequestDto request, string userId, CancellationToken cancellationToken)
        {
            var taskItem = await taskItemRepository.GetByIdAsync(request.Id, userId)
                ?? throw new NoFoundException($"No se econtró tarea con id '{request.Id}'");

            taskItem.Update(request.Title, request.Description, request.Status);

            taskItemRepository.Update(taskItem);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteTaskAsync(int id, string userId, CancellationToken cancellationToken)
        {
            var taskItem = await taskItemRepository.GetByIdAsync(id, userId)
                ?? throw new NoFoundException($"No se econtró tarea con id '{id}'");

            taskItemRepository.Delete(taskItem);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}

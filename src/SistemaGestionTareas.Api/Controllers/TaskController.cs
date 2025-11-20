using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaGestionTareas.Api.Infrastructure;
using SistemaGestionTareas.ApplicationCore.Dtos.Queries;
using SistemaGestionTareas.ApplicationCore.Dtos.Request;
using SistemaGestionTareas.ApplicationCore.Dtos.Response;
using SistemaGestionTareas.ApplicationCore.Services;
using System.Security.Claims;

namespace SistemaGestionTareas.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController(TaskItemService taskItemService) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(GetAllTaskItemResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAsync([FromQuery] GetAllTaskItemQueryDto query)
        {
            var taskItems = await taskItemService.GetAllTaskItemAsync(query,GetUserId());

            return Ok(taskItems);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetByIdTaskItemResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var taskItem = await taskItemService.GetByIdTaskItemAsync(new GetByIdTaskItemQueryDto(id), GetUserId());

            return Ok(taskItem);
        }

        [HttpGet("dashboard")]
        [ProducesResponseType(typeof(GetTaskDashboardResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetDashboardAsync()
        {
            var dashboard = await taskItemService.GetTaskDashboardAsync(GetUserId());

            return Ok(dashboard);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateTaskResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync(
            [FromBody] TaskItemCreateRequestDto request,
            CancellationToken cancellationToken = default
            )
        {
            var createResult = await taskItemService.CreateTaskAsync(request,GetUserId(), cancellationToken);
            var uri = $"api/task/{createResult.TaskId}";

            return Created(uri, createResult);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CreateTaskResponseDto), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync(
            int id,
            [FromBody] TaskItemUpdateRequestDto request,
            CancellationToken cancellationToken = default

            )
        {
            request = request with { Id = id };

            await taskItemService.UpdateTaskAsync(request, GetUserId(), cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(CreateTaskResponseDto), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default
            )
        {

            await taskItemService.DeleteTaskAsync(id, GetUserId(), cancellationToken);

            return NoContent();
        }

        private string GetUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return userId ?? throw new ArgumentNullException();
        }
    }
}

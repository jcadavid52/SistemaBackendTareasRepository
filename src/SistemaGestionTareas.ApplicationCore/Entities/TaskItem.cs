using SistemaGestionTareas.ApplicationCore.Abstractions;
using SistemaGestionTareas.ApplicationCore.Common.Contants;

namespace SistemaGestionTareas.ApplicationCore.Entities
{
    public class TaskItem:DomainEntity<int>
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = StatusTaskItem.Pendiente;
        public string UserId { get; set; } = string.Empty;
        public User User { get; private set; } = default!;

        private TaskItem() { }

        public TaskItem(string title, string description, string userId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(title);
            ArgumentException.ThrowIfNullOrWhiteSpace(description);
            ArgumentException.ThrowIfNullOrWhiteSpace(userId);

            Title = title.Trim().ToUpper();
            Description = description.Trim().ToLower();
            UserId = userId;
        }

        public void Update(string? title, string? description, string? status)
        {
            StatusTaskItem.ValidateStatus(status);

            ArgumentException.ThrowIfNullOrWhiteSpace(title ?? Title);
            ArgumentException.ThrowIfNullOrWhiteSpace(description ?? Description);
            ArgumentException.ThrowIfNullOrWhiteSpace(status ?? Status);

            Title = (title ?? Title).Trim().ToUpper();
            Description = (description ?? Description).Trim().ToLower();
            Status = (status ?? Status).Trim();
        }
    }
}

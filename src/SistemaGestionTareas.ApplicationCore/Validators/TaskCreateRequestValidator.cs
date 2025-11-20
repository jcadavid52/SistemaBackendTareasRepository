using FluentValidation;
using SistemaGestionTareas.ApplicationCore.Common.Contants;
using SistemaGestionTareas.ApplicationCore.Dtos.Request;

namespace SistemaGestionTareas.ApplicationCore.Validators
{
    public class TaskCreateRequestValidator: AbstractValidator<TaskItemCreateRequestDto>
    {
        public TaskCreateRequestValidator()
        {
            RuleFor(task => task.Title)
            .NotEmpty().WithMessage("El título es requerido.")
            .MaximumLength(100).WithMessage($"El título no puede tener más de {PropertieLengthTaskItem.TitleLength} caracteres.");

            RuleFor(task => task.Description)
                .NotEmpty().WithMessage("La descripción es requerida.")
                .MaximumLength(500).WithMessage($"La descripción no puede tener más de {PropertieLengthTaskItem.DescriptionLength} caracteres.");
        }
    }
}

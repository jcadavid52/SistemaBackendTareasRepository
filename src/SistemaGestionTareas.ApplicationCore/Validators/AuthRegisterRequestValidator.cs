using FluentValidation;
using SistemaGestionTareas.ApplicationCore.Dtos.Request;

namespace SistemaGestionTareas.ApplicationCore.Validators
{
    public class AuthRegisterRequestValidator : AbstractValidator<AuthRegisterRequestDto>
    {
        public AuthRegisterRequestValidator()
        {
            RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("El nombre es obligatorio.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El apellido es obligatorio.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es obligatorio.")
                .EmailAddress().WithMessage("Debe ingresar un email válido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Debe confirmar la contraseña.");

            RuleFor(x => x)
                .Must(x => x.Password == x.ConfirmPassword)
                .WithMessage("Las contraseñas no coinciden.");
        }
    }
}

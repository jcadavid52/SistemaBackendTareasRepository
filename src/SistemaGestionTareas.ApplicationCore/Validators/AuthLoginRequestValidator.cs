using FluentValidation;
using SistemaGestionTareas.ApplicationCore.Dtos.Request;

namespace SistemaGestionTareas.ApplicationCore.Validators
{
    public class AuthLoginRequestValidator: AbstractValidator<AuthLoginRequestDto>
    {
        public AuthLoginRequestValidator()
        {
            RuleFor(loginRequest => loginRequest.Username)
                .Cascade(CascadeMode.Continue)
                .NotEmpty().WithMessage("El usuario es requerido")
                .NotNull().WithMessage("El usuario es requerido");

            RuleFor(loginRequest => loginRequest.Password)
                .Cascade(CascadeMode.Continue)
                .NotEmpty()
                .NotNull().WithMessage("La contraseña es requerida")
                .WithMessage("La contraseña es requerida");
        }
    }
}

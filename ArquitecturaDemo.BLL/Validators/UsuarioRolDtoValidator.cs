using ArquitecturaDemo.DTO.Users;
using ArquitecturaDemo.Shared.Helpers;
using FluentValidation;

namespace ArquitecturaDemo.BLL.Validators
{
    public class UsuarioRolDtoValidator : AbstractValidator<UsuarioRolDto>
    {
        public UsuarioRolDtoValidator()
        {
            RuleFor(x => x.UsuarioId).Must(BeValidInt).WithMessage(Const.InvalidMessage);
            RuleFor(x => x.RolId).Must(BeValidInt).WithMessage(Const.InvalidMessage);
        }

        private bool BeValidInt(int value) => value > Const.MIN_VALUE;
    }
}
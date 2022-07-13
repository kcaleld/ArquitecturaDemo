using ArquitecturaDemo.DAL;
using ArquitecturaDemo.DTO.Users;
using ArquitecturaDemo.Shared.Helpers;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ArquitecturaDemo.BLL.Validators
{
    public class UsuarioDtoValidator : AbstractValidator<UsuarioDto>
    {
        private readonly UsersContext _context;
        public UsuarioDtoValidator(UsersContext context)
        {
            _context = context;

            RuleFor(x => x.Codigo)
                .NotNull().WithMessage(Const.NullOrEmptyMessage)
                .Must(BeValidString).WithMessage(Const.InvalidMessage);
            RuleFor(x => x.Password)
                .NotNull().WithMessage(Const.NullOrEmptyMessage)
                .Must(BeValidString).WithMessage(Const.InvalidMessage);
            RuleFor(x => x.CorreoElectronico)
                .NotNull().WithMessage(Const.NullOrEmptyMessage)
                .EmailAddress().WithMessage(Const.InvalidMessage)
                .Must(BeValidString).WithMessage(Const.InvalidMessage);
            RuleFor(x => x)
                .MustAsync(async (user, cancellation) => 
                { 
                    return await UniqueEmail(user.CorreoElectronico, user.Id); 
                }).WithMessage(Const.EmailExists);
            RuleFor(x => x.Nombre)
                .NotNull().WithMessage(Const.NullOrEmptyMessage)
                .Must(BeValidString).WithMessage(Const.InvalidMessage);
            RuleFor(x => x.Apellido)
                .NotNull().WithMessage(Const.NullOrEmptyMessage)
                .Must(BeValidString).WithMessage(Const.InvalidMessage);
        }

        private bool BeValidString(string? value) => !string.IsNullOrEmpty(value);
        private async Task<bool> UniqueEmail(string email, int id)
            => !await _context.Usuarios.AnyAsync(x => x.CorreoElectronico.ToLower().Equals(email.ToLower()) && x.Id != id);
    }
}
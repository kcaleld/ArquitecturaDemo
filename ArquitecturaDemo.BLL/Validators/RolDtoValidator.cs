using ArquitecturaDemo.DTO.Users;
using ArquitecturaDemo.Shared.Helpers;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaDemo.BLL.Validators
{
    public class RolDtoValidator : AbstractValidator<RolDto>
    {
        public RolDtoValidator()
        {
            RuleFor(x => x.Nombre).NotNull().WithMessage(Const.NullOrEmptyMessage).Must(BeValidString).WithMessage(Const.InvalidMessage);
        }

        private bool BeValidString(string? value) => !string.IsNullOrEmpty(value);
    }
}
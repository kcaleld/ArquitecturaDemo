using AutoMapper;
using ArquitecturaDemo.CBL.v1;
using ArquitecturaDemo.DAL;
using ArquitecturaDemo.DAL.Entities;
using ArquitecturaDemo.DTO.Users;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ArquitecturaDemo.BLL.v1
{
    public class RolesBL : GenericRepository<Rol, RolDto>, IRolesBL
    {
        public RolesBL(UsersContext context, IMapper mapper, IValidator<RolDto> validator, ILogger<GenericRepository<Rol, RolDto>> logger)
            : base(context, mapper, validator, logger) { }

        public async Task AsignarRoles()
        {
            var rol = await GetByIdAsync(1);
            var usuario = await _context.Usuarios.FindAsync(1);
        }
    }
}

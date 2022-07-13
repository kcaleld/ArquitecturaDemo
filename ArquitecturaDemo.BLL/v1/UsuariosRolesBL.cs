using AutoMapper;
using ArquitecturaDemo.CBL.v1;
using ArquitecturaDemo.DAL;
using ArquitecturaDemo.DAL.Entities;
using ArquitecturaDemo.DTO.Users;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ArquitecturaDemo.BLL.v1
{
    public class UsuariosRolesBL : GenericRepository<UsuarioRol, UsuarioRolDto>, IUsuariosRolesBL
    {
        public UsuariosRolesBL(UsersContext context, IMapper mapper, IValidator<UsuarioRolDto> validator, ILogger<GenericRepository<UsuarioRol, UsuarioRolDto>> logger)
            : base(context, mapper, validator, logger) { }
    }
}
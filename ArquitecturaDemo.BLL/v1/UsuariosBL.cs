using AutoMapper;
using ArquitecturaDemo.CBL.v1;
using ArquitecturaDemo.DAL;
using ArquitecturaDemo.DAL.Entities;
using ArquitecturaDemo.DTO.Users;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ArquitecturaDemo.BLL.v1
{
    public class UsuariosBL : GenericRepository<Usuario, UsuarioDto>, IUsuariosBL
    {
        public UsuariosBL(UsersContext context, IMapper mapper, IValidator<UsuarioDto> validator, ILogger<GenericRepository<Usuario, UsuarioDto>> logger)
            : base(context, mapper, validator, logger) { }
    }
}
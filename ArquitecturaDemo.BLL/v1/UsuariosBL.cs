using AutoMapper;
using ArquitecturaDemo.CBL.v1;
using ArquitecturaDemo.DAL;
using ArquitecturaDemo.DAL.Entities;
using ArquitecturaDemo.DTO.Users;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ArquitecturaDemo.BLL.Extensions;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using ArquitecturaDemo.Shared.Models;
using ArquitecturaDemo.Shared.Models.DTOs.Users;

namespace ArquitecturaDemo.BLL.v1
{
    public class UsuariosBL : GenericRepository<Usuario, UsuarioDto>, IUsuariosBL
    {
        private readonly IRolesBL _rolesBL;
        private readonly IUsuariosRolesBL _usersBL;

        public UsuariosBL(UsersContext context, IMapper mapper, IValidator<UsuarioDto> validator, ILogger<GenericRepository<Usuario, UsuarioDto>> logger, IRolesBL rolesBL, IUsuariosRolesBL usersBL)
            : base(context, mapper, validator, logger)
        {
            _rolesBL = rolesBL;
            _usersBL = usersBL;
        }

        public async Task<InfinitePagination<CustomUserDto>> ObtieneUsuariosPaginado(int skip, int take, string orden, int columnaOrden, string filtroGeneral, string[] filtros)
        {
            var filters = new List<FilterInfo>()
            {
                new FilterInfo() { Value = filtros[0] },
                new FilterInfo() { Value = filtros[1] },
                new FilterInfo() { Value = filtros[2] },
                new FilterInfo() { Value = filtros[3] },
                new FilterInfo() { Value = filtros[4] },
                new FilterInfo() { Value = filtros[5] },
                new FilterInfo() { Value = filtros[6], Operator = OperatorsEnum.GreaterThanOrEqual },
                new FilterInfo() { Value = filtros[7], Operator = OperatorsEnum.Equals },
            };

            return await (from d in _context.Usuarios
                          join e in _context.UsuarioRols on d.Id equals e.UsuarioId
                          join f in _context.Rols on e.RolId equals f.Id
                          let fechaNacimiento = d.FechaNacimiento
                          select new CustomUserDto
                          {
                              Id = d.Id,
                              Codigo = d.Codigo,
                              CorreoElectronico = d.CorreoElectronico,
                              Nombre = d.Nombre,
                              Apellido = d.Apellido,
                              Rol = f.Nombre,
                              FechaNacimiento = fechaNacimiento,
                              RolId = f.Id
                          }).InfinitePaginate(skip, take, orden, columnaOrden, filtroGeneral, filters);
        }
    }
}
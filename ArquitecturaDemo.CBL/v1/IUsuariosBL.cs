using ArquitecturaDemo.DAL.Entities;
using ArquitecturaDemo.DTO.Users;
using ArquitecturaDemo.Shared.Models;
using ArquitecturaDemo.Shared.Models.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaDemo.CBL.v1
{
    public interface IUsuariosBL : IGenericRepository<Usuario, UsuarioDto>
    {
        Task<InfinitePagination<CustomUserDto>> ObtieneUsuariosPaginado(int skip, int take, string orden, int columnaOrden, string filtroGeneral, string[] filtros);
    }
}
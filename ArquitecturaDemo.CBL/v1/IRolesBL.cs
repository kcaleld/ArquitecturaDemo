using ArquitecturaDemo.DAL.Entities;
using ArquitecturaDemo.DTO.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaDemo.CBL.v1
{
    public interface IRolesBL : IGenericRepository<Rol, RolDto>
    {
    }
}

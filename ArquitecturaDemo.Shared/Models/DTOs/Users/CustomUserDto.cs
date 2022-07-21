using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaDemo.Shared.Models.DTOs.Users
{
    public class CustomUserDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = null!;
        public string CorreoElectronico { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }
        public int RolId { get; set; }
    }
}
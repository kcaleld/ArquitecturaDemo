using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaDemo.DTO.Users
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string CorreoElectronico { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string NombreCompleto { get; set; } = null!;
    }
}

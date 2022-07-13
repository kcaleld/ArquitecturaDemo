using System;
using System.Collections.Generic;

namespace ArquitecturaDemo.DAL.Entities
{
    public partial class Usuario
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string CorreoElectronico { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
    }
}

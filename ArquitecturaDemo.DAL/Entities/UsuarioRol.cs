using System;
using System.Collections.Generic;

namespace ArquitecturaDemo.DAL.Entities
{
    public partial class UsuarioRol
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int RolId { get; set; }
    }
}

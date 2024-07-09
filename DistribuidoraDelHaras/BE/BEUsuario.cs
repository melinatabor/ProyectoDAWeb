
using Abstraccion;
using System.Collections.Generic;

namespace BE
{
    public class BEUsuario : IUsuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Activo { get; set; }
        public int Rol { get; set; }
        public string DigitoVerificadorH { get; set; }
    }
}

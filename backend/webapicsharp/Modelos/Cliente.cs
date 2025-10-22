using System.ComponentModel.DataAnnotations.Schema;

namespace webapicsharp.Modelos
{
    public class Cliente : Usuario
    {
        public int EcoPuntos { get; set; }

        public Cliente(int id, string nombre, string cedula, string correo, string direccion, string telefono, string contrasena, int ecoPuntos)
            : base(id, nombre, cedula, correo, direccion, telefono, contrasena)

        {
            EcoPuntos = ecoPuntos;
        }
    }

    public class ClienteDto : UsuarioDto
    {
        public int EcoPuntos { get; set; }

    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace webapicsharp.Modelos
{
    public class Cliente : Usuario
    {

        public Cliente(int id, string nombre, string cedula, string correo, string direccion, string telefono, string contrasena, int ecoPuntos)
            : base(id, nombre, cedula, correo, direccion, telefono, contrasena)

        {
            EcoPuntos = ecoPuntos;
        }
        [JsonInclude]
        public int EcoPuntos { get; set; }
    }

    public class ClienteDto : UsuarioDto
    {
        public int EcoPuntos { get; set; }

    }

    public class ClienteDetalladoDto : ClienteDto
    {
        public int TotalEntregas { get; set; }
        public double PesoTotalEntregado { get; set; }
        public string? UltimoEcopunto { get; set; }
    }
}

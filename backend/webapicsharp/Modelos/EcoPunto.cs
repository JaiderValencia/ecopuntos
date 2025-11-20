using System.ComponentModel.DataAnnotations;

namespace webapicsharp.Modelos
{
    public class EcoPunto
    {
        public int Id { get; set; }
        public string? Horario { get; set; }
        public string? Nombre { get; set; }
        public Trabajador? Trabajador { get; set; }
        public List<Material>? MaterialesAceptados { get; set; } = new List<Material>();
        public Ubicacion? Ubicacion { get; set; }
    }

    public class ActualizarEcoPuntoDto
    {
        public string? CodigoDeEmpleado { get; set; }

        public string? Latitud { get; set; }

        public string? Longitud { get; set; }

        public string? Direccion { get; set; }

        public string? Horario { get; set; }
        public string? Nombre { get; set; }
        public List<Material> Materiales { get; set; } = new();
    }
}

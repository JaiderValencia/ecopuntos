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
        [Required]
        public string? CodigoDeEmpleado { get; set; }

        [Required]
        public string? Latitud { get; set; }

        [Required]
        public string? Longitud { get; set; }

        [Required]
        public string? Direccion { get; set; }

        [Required]
        public string? Horario { get; set; }
        [Required]
        public string? Nombre { get; set; }

        [Required]
        public List<Material> Materiales { get; set; } = new();
    }
}

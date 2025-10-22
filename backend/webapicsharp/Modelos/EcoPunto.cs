namespace webapicsharp.Modelos
{
    public class EcoPunto
    {
        public int Id { get; set; }
        public Trabajador? Trabajador { get; set; }
        public List<Material>? MaterialesAceptados { get; set; } = new();
        public Ubicacion? Ubicacion { get; set; }
        public string? Horario { get; set; }
    }
}

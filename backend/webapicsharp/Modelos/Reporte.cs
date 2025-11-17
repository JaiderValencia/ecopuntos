namespace webapicsharp.Modelos
{
    public class Reporte
    {
        public int Id { get;set }
        public Cliente? Cliente { get; set; }
        public Trabajador? Trabajador { get; set; }
        public List<Dictionary<string, object>?>? MaterialesEntrega { get; set; }
        public Dictionary<string, string>? top3 { get; set; }
        public Dictionary<string, double>? totales { get; set; }
    }

    public class ReporteAllDto
    {
        public int IdReporte { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string? NombreEcopunto{ get; set; }
        public string? Responsable { get; set; }
    }
}

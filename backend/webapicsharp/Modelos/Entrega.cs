namespace webapicsharp.Modelos
{
    public class Entrega
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int IdTrabajador { get; set; }
        public int IdEcoPunto { get; set; }
        public List<EntregaMaterial>? MaterialesEntrega{ get; set; } = new List<EntregaMaterial>();
    }


    public class CrearEntregaDto
    {
        public string? CedulaCliente { get; set; }
        public int IdTrabajador { get; set; }
        public int IdEcoPunto { get; set; }
        public List<CrearEntregaMaterialDto>? MaterialesEntrega { get; set; } = new List<CrearEntregaMaterialDto>();
    }

    public class EntregaResponse
    {
        public int Id { get; set; }
        public int IdEcopunto { get; set; }
        public Cliente? Cliente { get; set; }
        public Trabajador? Trabajador { get; set; }
        public List<EntregaMaterial>? MaterialesEntrega { get; set; } = new List<EntregaMaterial>();
        public Dictionary<string, string>? Top3 { get; set; }
        public Dictionary<string, double>? Totales { get; set; }
    }
}

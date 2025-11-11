
namespace webapicsharp.Modelos
{
    public class EntregaMaterial
    {
        public int IdEntrega { get; set; }
        public int IdMaterial { get; set; }
        public double Peso { get; set; }
        public int Puntos { get; set; }
        public bool Estado { get; set; }

        public EntregaMaterial(int idEntrega, int idMaterial, double peso, int puntos, bool estado)
        {
            IdEntrega = idEntrega;
            IdMaterial = idMaterial;
            Peso = peso;
            Puntos = puntos;
            Estado = estado;
        }
    }

    public class CrearEntregaMaterialDto
    {
        public int IdMaterial { get; set; }
        public double Peso { get; set; }
        public int Puntos { get; set; }
        public bool Estado { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace webapicsharp.Modelos
{
    public class Material
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public double Peso { get; set; }

        public Material (int id, string nombre, double peso)
        {
            Id = id;
            Nombre = nombre;
            Peso = peso;
        }

    }
    public class MaterialDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public double Peso { get; set; }
    }
}

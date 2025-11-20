using System.Text.Json.Serialization;

namespace webapicsharp.Modelos
{
    public class Material
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }

        public Material (int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }

    }
    public class MaterialDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
    }
}

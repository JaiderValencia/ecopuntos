using webapicsharp.Modelos;

namespace webapicsharp.Interface.Servicios.Abstracciones
{
    public interface IServicioMaterial
    {
        public  Task<Material?> CrearMaterialAsync(Material material);
        public Task<IReadOnlyList<Dictionary<string, object?>>> ObtenerMaterialesAsync(int? limite);

        public Task<Material?> ObtenerMaterioalPorNombreAsync(string nombre);

    }
}

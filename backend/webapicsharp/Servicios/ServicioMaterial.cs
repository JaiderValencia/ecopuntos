using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using webapicsharp.Interface.Servicios.Abstracciones;
using webapicsharp.Modelos;
using webapicsharp.Repositorios.Abstracciones;

namespace webapicsharp.Servicios
{
    public class ServicioMaterial : IServicioMaterial
    {
        private readonly IRepositorioBusquedaPorCampoTabla _repoBusqueda;
        private readonly IRepositorioEscrituraTabla _repoEscritura;
        private readonly IRepositorioActualizarTabla _repoActualizar;
        private readonly IRepositorioEliminarTabla _repoEliminar;
        private readonly IRepositorioLecturaTabla _repoLectura;

        public ServicioMaterial(
            IRepositorioEscrituraTabla repoEscritura,
            IRepositorioActualizarTabla repoActualizar,
            IRepositorioEliminarTabla repoEliminar,
            IRepositorioBusquedaPorCampoTabla repoBusqueda,
            IRepositorioLecturaTabla repoLectura)
        {
            _repoEscritura = repoEscritura;
            _repoBusqueda = repoBusqueda;
            _repoActualizar = repoActualizar;
            _repoEliminar = repoEliminar;
            _repoLectura = repoLectura;
        }

        public async Task<Material?> CrearMaterialAsync(Material material)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(material.Nombre))
                    throw new Exception ("El nombre del material es obligatorio.");

                if (material.Peso <= 0)
                    throw new Exception ("El peso del material debe ser mayor que cero.");
                
                var existe = await _repoBusqueda.BuscarPorCampoAsync("Material", "Nombre", material.Nombre);
                if (existe != null)
                    throw new Exception ("Ya existe un material con ese nombre.");

                var valores = new Dictionary<string, object?>
                {
                    { "Nombre", material.Nombre },
                    { "Peso", material.Peso }
                };

                var dictCreado = await _repoEscritura.InsertarAsync("Material", valores);

                if (dictCreado is null)
                    throw new Exception($"Error al registrar el material");

                var clienteFiltrado = new Material(
                    int.TryParse(dictCreado["Id"]?.ToString(), out var id) ? id : 0,
                    dictCreado["Nombre"]?.ToString() ?? "",
                    double.TryParse(dictCreado["Peso"]?.ToString(), out var ecoPuntos) ? ecoPuntos : 0
                );

                return clienteFiltrado;
            }
            catch(Exception e)
            {
                throw new Exception($"Error al crear material {e.Message}");
            }
        }

        public async Task<IReadOnlyList<Dictionary<string, object?>>> ObtenerMaterialesAsync(int limite)
        {
            try
            {
                if (!int.TryParse(limite.ToString(), out _) || limite < 1)
                {
                    throw new Exception("Debe proporcionar un limite positivo.");
                }

                var materiales = await _repoLectura.ObtenerFilasAsync("Material", null, limite);

                return materiales;
            }
            catch(Exception e)
            {
                throw new Exception($"Error al obtener los materiales {e.Message}");
            }
        }

        public async Task<Material?> ObtenerMaterioalPorNombreAsync(string nombre)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                    throw new Exception( "El nombre del material es obligatorio.");

                var material = await _repoBusqueda.BuscarPorCampoAsync("Material", "Nombre", nombre);
                if (material == null)
                    throw new Exception("Ocurrio un error buscando el material");

                var materialFiltrado = new Material(
                    int.TryParse(material["Id"]?.ToString(), out var id) ? id : 0,
                    material["Nombre"]?.ToString() ?? "",
                    double.TryParse(material["Peso"]?.ToString(), out var ecoPuntos) ? ecoPuntos : 0
                );

                return materialFiltrado;
            } catch (Exception e)
            {
                throw new Exception($"Error al obtener el material {e.Message}");
            }
        }
    }
}

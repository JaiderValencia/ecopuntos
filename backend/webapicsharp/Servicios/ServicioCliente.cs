using System.Collections.Generic;
using System.Threading.Tasks;
using webapicsharp.Modelos; // para usar la clase Usuario
using webapicsharp.Repositorios.Abstracciones;
using webapicsharp.Servicios.Abstracciones;

namespace webapicsharp.Servicios
{
    public class ServicioCliente : IServicioCliente
    {
        private readonly IRepositorioBusquedaPorCampoTabla _repoBusqueda;
        private readonly IRepositorioEscrituraTabla _repoEscritura;
        private readonly IRepositorioActualizarTabla _repoActualizar;
        private readonly IRepositorioEliminarTabla _repoEliminar;
        private readonly IRepositorioSubconsulta _repoSubconsulta;
        private readonly IRepositorioConsultaPersonalizada _repoConsultaPersonalizada;

        public ServicioCliente(
            IRepositorioEscrituraTabla repoEscritura,
            IRepositorioActualizarTabla repoActualizar,
            IRepositorioEliminarTabla repoEliminar,
            IRepositorioBusquedaPorCampoTabla repoBusqueda,
            IRepositorioSubconsulta repoSubconsulta,
            IRepositorioConsultaPersonalizada repoConsultaPersonalizada)
        {
            _repoEscritura = repoEscritura;
            _repoBusqueda = repoBusqueda;
            _repoActualizar = repoActualizar;
            _repoEliminar= repoEliminar;
            _repoSubconsulta = repoSubconsulta;
            _repoConsultaPersonalizada = repoConsultaPersonalizada;
        }


        public async Task<Cliente?> CrearClienteAsync(Cliente cliente)
        {
            try
            {
                var cedula = cliente.ObtenerCedula();
                var correo = cliente.ObtenerCorreo();

                var usuarioExiste = await _repoBusqueda.BuscarPorCampoAsync("usuario", "Correo", correo!);
                var existeCedula = await _repoBusqueda.BuscarPorCampoAsync("usuario", "Cedula", cedula!);

                if (usuarioExiste != null)
                    throw new Exception("Ya existe usuario con este correo");

                if (existeCedula != null && existeCedula[0]["Correo"]?.ToString() != correo)
                    throw new Exception("Existe otro usuario con la misma cedula");

                var datosUsuario = new Dictionary<string, object?>
                {
                    ["Nombre"] = cliente.ObtenerNombre(),
                    ["Cedula"] = cliente.ObtenerCedula(),
                    ["Correo"] = cliente.ObtenerCorreo(),
                    ["Direccion"] = cliente.ObtenerDireccion(),
                    ["Telefono"] = cliente.ObtenerTelefono(),
                    ["Contrasena"] = cliente.ObtenerContrasena(),
                };

                var dictUsuario = await _repoEscritura.InsertarAsync("Usuario", datosUsuario);
                if (dictUsuario is null)
                    throw new Exception("El cliente no se pudo crear correctamente");


                var datosCliente = new Dictionary<string, object?>
                {
                    ["Id"] = dictUsuario!["Id"],
                    ["EcoPuntos"] = cliente.EcoPuntos
                };

                var dictCliente = await _repoEscritura.InsertarAsync("Cliente", datosCliente);
                if (dictCliente is null)
                    throw new Exception("El cliente no se pudo crear correctamente");

                var clienteCreado= new Cliente(
                    int.TryParse(dictUsuario["Id"]?.ToString(), out var id) ? id : 0,
                    dictUsuario["Nombre"]?.ToString() ?? "",
                    dictUsuario["Cedula"]?.ToString() ?? "",
                    dictUsuario["Correo"]?.ToString() ?? "",
                    dictUsuario["Direccion"]?.ToString() ?? "",
                    dictUsuario["Telefono"]?.ToString() ?? "",
                    dictUsuario["Contrasena"]?.ToString() ?? "",
                    int.TryParse(dictCliente["EcoPuntos"]?.ToString(), out var ecoPuntos) ? ecoPuntos : 0
                );

                return clienteCreado;
            }
            catch (Exception e)
            {
                throw new Exception($"Error inesperado al crear cliente: {e.Message}");
            }
        }
        public async Task<Dictionary<string, object?>?> BuscarClientePorCorreoAsync(string correo)
        {
            try
            {
                var cliente = await _repoSubconsulta.EjecutarSubconsultaAsync(
                    "Cliente",
                    "Usuario",
                    campoRelacionExterna: "Id",
                    campoRelacionInterna: "Id",
                    campoFiltro: "Correo",
                    valorFiltro: correo
                    );


                if (cliente == null || cliente.Count == 0)
                {
                    return null;
                }

                var idCliente = int.TryParse(cliente[0]["Id"]?.ToString(), out var id) ? id : 0;

                // Consulta para obtener estadísticas de entregas
                var consultaEstadisticas = @"
                    SELECT 
                        COUNT(DISTINCT e.Id) AS TotalEntregas,
                        ISNULL(SUM(me.Peso), 0) AS PesoTotalEntregado,
                        (
                            SELECT TOP 1 ep.NombreEcopunto
                            FROM [dbo].[Entrega] e2
                            LEFT JOIN [dbo].[EcoPunto] ep ON e2.IdEcopunto = ep.Id
                            WHERE e2.IdCliente = @IdCliente
                            ORDER BY e2.FechaCreacion DESC
                        ) AS UltimoEcopunto
                    FROM [dbo].[Entrega] e
                    LEFT JOIN [dbo].[MaterialEntrega] me ON e.Id = me.IdEntrega
                    WHERE e.IdCliente = @IdCliente
                ";

                var parametros = new Dictionary<string, object?>
                {
                    ["@IdCliente"] = idCliente
                };

                var estadisticas = await _repoConsultaPersonalizada.EjecutarConsultaAsync(consultaEstadisticas, parametros);

                var resultado = new Dictionary<string, object?>
                {
                    ["Id"] = idCliente,
                    ["Nombre"] = cliente[0]["Nombre"]?.ToString() ?? "",
                    ["Cedula"] = cliente[0]["Cedula"]?.ToString() ?? "",
                    ["Correo"] = cliente[0]["Correo"]?.ToString() ?? "",
                    ["Direccion"] = cliente[0]["Direccion"]?.ToString() ?? "",
                    ["Telefono"] = cliente[0]["Telefono"]?.ToString() ?? "",                    
                    ["EcoPuntos"] = int.TryParse(cliente[0]["EcoPuntos"]?.ToString(), out var ecoPuntos) ? ecoPuntos : 0,
                    ["TotalEntregas"] = estadisticas.Count > 0 && estadisticas[0]["TotalEntregas"] != null ? Convert.ToInt32(estadisticas[0]["TotalEntregas"]) : 0,
                    ["PesoTotalEntregado"] = estadisticas.Count > 0 && estadisticas[0]["PesoTotalEntregado"] != null ? Convert.ToDouble(estadisticas[0]["PesoTotalEntregado"]) : 0.0,
                    ["UltimoEcopunto"] = estadisticas.Count > 0 ? estadisticas[0]["UltimoEcopunto"]?.ToString() : null
                };

                return resultado;
            }
            catch (Exception e)
            {
                throw new Exception($"Error inesperado al buscar cliente: {e.Message}");
            }

        }

        public async Task<Cliente?> ActualizarClientePorCorreoAsync(string correo, Cliente cliente)
        {
            try
            {
                var cedula = cliente.ObtenerCedula();

                var usuarioExiste = await _repoBusqueda.BuscarPorCampoAsync("usuario", "Correo", correo);
                var existeCedula = await _repoBusqueda.BuscarPorCampoAsync("usuario", "Cedula", cedula!);

                if (usuarioExiste == null)
                    throw new Exception ("No existe usuario con este correo");

                if (existeCedula != null && existeCedula[0]["Correo"]?.ToString() != correo)
                    throw new Exception ("Existe otro usuario con la misma cedula");

                var datosUsuario = new Dictionary<string, object?>
                {
                    {"Nombre", cliente.ObtenerNombre()},
                    {"Cedula", cliente.ObtenerCedula()},
                    {"Correo", cliente.ObtenerCorreo()},
                    {"Direccion", cliente.ObtenerDireccion()},
                    {"Telefono", cliente.ObtenerTelefono()},
                    {"Contrasena", cliente.ObtenerContrasena()},
                };

                var datosCliente = new Dictionary<string, object?>
                {
                    {"EcoPuntos", cliente.EcoPuntos},
                };

                var dictUsuario = await _repoActualizar.ActualizarPorCampoAsync("usuario", "Correo", correo, datosUsuario);
                var dictCliente = await _repoActualizar.ActualizarPorCampoAsync("cliente", "Id", usuarioExiste[0]["Id"]!, datosCliente);

                if (dictUsuario is null || dictCliente is null)
                {
                    throw new Exception ("Usuario no se pudo actualizar");
                }

                var clienteActualizado = new Cliente(
                    int.TryParse(dictUsuario["Id"]?.ToString(), out var id) ? id : 0,
                    dictUsuario["Nombre"]?.ToString() ?? "",
                    dictUsuario["Cedula"]?.ToString() ?? "",
                    dictUsuario["Correo"]?.ToString() ?? "",
                    dictUsuario["Direccion"]?.ToString() ?? "",
                    dictUsuario["Telefono"]?.ToString() ?? "",
                    dictUsuario["Contrasena"]?.ToString() ?? "",
                    int.TryParse(dictCliente["EcoPuntos"]?.ToString(), out var ecoPuntos) ? ecoPuntos : 0
                );

                return clienteActualizado;

            }
            catch (Exception e)
            {
                throw new Exception($"Error inesperado al crear cliente: {e.Message}");
            }
        }

        public async Task<string> EliminarClientePorCorreoAsync(string correo)
        {
            try
            {
                var existeUsuario = await _repoBusqueda.BuscarPorCampoAsync("usuario", "Correo", correo);

                if (existeUsuario == null)
                {
                    return $"No existe usuario con este correo: {correo}";
                }

                var existeCliente = await _repoBusqueda.BuscarPorCampoAsync("cliente", "Id", existeUsuario[0]["Id"]!);

                bool respuestaCliente = await _repoEliminar.EliminarPorCampoAsync("Cliente", "Id", existeUsuario![0]["Id"]!);
                bool respuestaUsuario = await _repoEliminar.EliminarPorCampoAsync("usuario", "Correo", correo);

                if (!respuestaUsuario || !respuestaCliente)
                {
                    return "Cliente no se pudo eliminar";
                }

                return "Cliente eliminado correctamente";

            }
            catch (Exception e)
            {
                throw new Exception($"Error inesperado al crear cliente: {e.Message}");
            }
        }
    }
}

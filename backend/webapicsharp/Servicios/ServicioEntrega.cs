using Swashbuckle.AspNetCore.SwaggerGen;
using webapicsharp.Interface.Servicios.Abstracciones;
using webapicsharp.Modelos;
using webapicsharp.Repositorios.Abstracciones;

namespace webapicsharp.Servicios
{
    public class ServicioEntrega : IServicioEntrega
    {

        private readonly IRepositorioBusquedaPorCampoTabla _repoBusqueda;
        private readonly IRepositorioEscrituraTabla _repoEscritura;
        private readonly IRepositorioActualizarTabla _repoActualizar;
        private readonly IRepositorioEliminarTabla _repoEliminar;
        private readonly IRepositorioSubconsulta _repoSubconsulta;
        private readonly IRepositorioJoin _repoJoinTresFiltrado;

        public ServicioEntrega(
            IRepositorioEscrituraTabla repoEscritura,
            IRepositorioActualizarTabla repoActualizar,
            IRepositorioEliminarTabla repoEliminar,
            IRepositorioBusquedaPorCampoTabla repoBusqueda,
            IRepositorioSubconsulta repoSubconsulta,
            IRepositorioJoin repoJoinTresFiltrado)
        {
            _repoEscritura = repoEscritura;
            _repoBusqueda = repoBusqueda;
            _repoActualizar = repoActualizar;
            _repoEliminar = repoEliminar;
            _repoSubconsulta = repoSubconsulta;
            _repoJoinTresFiltrado = repoJoinTresFiltrado;
        }

        public async Task<EntregaResponse> CrearEntregAsync(CrearEntregaDto entrega)
        {
            try
            {
                if (entrega.IdEcoPunto == 0 || entrega.IdTrabajador == 0)
                {
                    throw new Exception("Los ids deben ser validos");
                }

                if (entrega.MaterialesEntrega!.Count() < 1)
                {
                    throw new Exception("La entrega debe tener minimo un material");

                }

                var clienteDb = await _repoSubconsulta.EjecutarSubconsultaAsync("Cliente", "Usuario", "Id", "Id", "Cedula", entrega.CedulaCliente!);

                if (clienteDb![0] == null)
                {
                    throw new Exception("No existe cliente con esa cedula");
                }

                var idClienteDb = int.Parse(clienteDb![0]["Id"]!.ToString()!);


                var datosEntrega = new Dictionary<string, object?>
                {
                    ["IdCliente"] = idClienteDb,
                    ["IdTrabajador"] = entrega.IdTrabajador,
                    ["IdEcopunto"] = entrega.IdEcoPunto
                };

                var entregaDB = await _repoEscritura.InsertarAsync("Entrega", datosEntrega);

                var materiales = entrega.MaterialesEntrega;
                if(materiales!.Count() == 0)
                {
                    throw new Exception("Los materiales estan vacios");

                }

                List<EntregaMaterial> materialesDB = new List<EntregaMaterial>();
                foreach (var material in materiales!)
                {
                    var datosMateriales = new Dictionary<string, object?>
                    {
                        ["IdEntrega"] = entregaDB.GetValueOrDefault("Id"),
                        ["IdMaterial"] = material.IdMaterial,
                        ["Peso"] = material.Peso,
                        ["Puntos"] = material.Puntos,
                        ["Estado"] = material.Estado
                    };

                    var materialDB = await _repoEscritura.InsertarAsync("MaterialEntrega", datosMateriales);
                    
                    materialesDB.Add(new EntregaMaterial(
                        idEntrega: Convert.ToInt32(materialDB["IdMaterial"]),
                        idMaterial: Convert.ToInt32(materialDB["IdMaterial"]),
                        peso: Convert.ToDouble(materialDB["Peso"]),
                        puntos: Convert.ToInt32(materialDB["Puntos"]),
                        estado: Convert.ToBoolean(materialDB["Estado"])
                    ));
                }

                return await ObtenerResultadoOperacion(entrega: entregaDB, materiales: materialesDB, cliente: clienteDb);

            }
            catch (Exception e)
            {
                throw new Exception($"Error inesperado al crear Entrega: {e.Message}");
            }
        }

        public async Task<EntregaResponse> ObtenerResultadoOperacion(
            Dictionary<string, object?> entrega, 
            List<EntregaMaterial>materiales, 
            List<Dictionary<string, object?>> cliente)
        {
            try
            {
                var trabajadorDatos = await _repoJoinTresFiltrado.JoinTresTablasAsync(
                   "Usuario",
                   "Empleado",
                   "Trabajador",
                   "Id",
                   "Id",
                   "Id",
                   "Id",
                   columnasSeleccionadas: "*",
                   tipoJoin: "INNER",
                   limite: null,
                   campoFiltro: "Id",
                   valorFiltro: Convert.ToInt32(entrega["IdTrabajador"])!
                   );

                var ecopuntosCliente = Convert.ToInt32(cliente![0]["EcoPuntos"]);
                var ecopuntosGanados = materiales.Sum(m => m.Puntos);
                var ecopuntosTotales = new Dictionary<string, object>
                {
                    ["EcoPuntos"] = ecopuntosGanados + ecopuntosCliente
                };

                var clienteActualizado = await _repoActualizar.ActualizarPorCampoAsync(
                    "Cliente",
                    "Id",
                    Convert.ToInt32(cliente[0]["Id"]),
                    ecopuntosTotales!
                );

                var resultado = new EntregaResponse
                {
                    Id = Convert.ToInt32(entrega["Id"]),
                    IdEcopunto = Convert.ToInt32(entrega["IdEcopunto"]),
                    Cliente = new Cliente(
                        id: Convert.ToInt32(cliente![0]["Id"]),
                        nombre: Convert.ToString(cliente![0]["Nombre"]) ?? "",
                        cedula: Convert.ToString(cliente![0]["Cedula"]) ?? "",
                        correo: Convert.ToString(cliente![0]["Correo"]) ?? "",
                        direccion: Convert.ToString(cliente![0]["Direccion"]) ?? "",
                        telefono: Convert.ToString(cliente![0]["Telefono"]) ?? "",
                        contrasena: "",
                        ecoPuntos: Convert.ToInt32(clienteActualizado!["EcoPuntos"])
                        ),
                    Trabajador = new Trabajador(
                        id: Convert.ToInt32(trabajadorDatos[0]["Id"]),
                        nombre: trabajadorDatos[0]["Nombre"]?.ToString() ?? "",
                        cedula: trabajadorDatos[0]["Cedula"]?.ToString() ?? "",
                        correo: trabajadorDatos[0]["Correo"]?.ToString() ?? "",
                        direccion: trabajadorDatos[0]["Direccion"]?.ToString() ?? "",
                        telefono: trabajadorDatos[0]["Telefono"]?.ToString() ?? "",
                        contrasena: "",
                        codigoDeEmpleado: trabajadorDatos[0]["CodigoDeEmpleado"]?.ToString() ?? "",
                        horario: trabajadorDatos[0]["Horario"]?.ToString() ?? ""
                    ),
                    MaterialesEntrega = materiales,
                };
                return resultado;


            } catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}

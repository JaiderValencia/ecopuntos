using webapicsharp.Interface.Servicios.Abstracciones;
using webapicsharp.Modelos;
using webapicsharp.Repositorios.Abstracciones;

namespace webapicsharp.Servicios
{
    public class ServicioReporte : IServicioReporte
    {

        private readonly IRepositorioBusquedaPorCampoTabla _repoBusqueda;
        private readonly IRepositorioEscrituraTabla _repoEscritura;
        private readonly IRepositorioActualizarTabla _repoActualizar;
        private readonly IRepositorioEliminarTabla _repoEliminar;
        private readonly IRepositorioSubconsulta _repoSubconsulta;
        private readonly IRepositorioJoin _repoJoins;

        public ServicioReporte(
            IRepositorioEscrituraTabla repoEscritura,
            IRepositorioActualizarTabla repoActualizar,
            IRepositorioEliminarTabla repoEliminar,
            IRepositorioBusquedaPorCampoTabla repoBusqueda,
            IRepositorioSubconsulta repoSubconsulta,
            IRepositorioJoin repoJoins)
        {
            _repoEscritura = repoEscritura;
            _repoBusqueda = repoBusqueda;
            _repoActualizar = repoActualizar;
            _repoEliminar = repoEliminar;
            _repoSubconsulta = repoSubconsulta;
            _repoJoins = repoJoins;
        }

        public async Task<List<ReporteAllDto>> ObtenerReportesPorIdClienteAsync(int IdCliente)
        {
            try
            {
                if (IdCliente <= 0)
                {
                    throw new Exception("El id del cliente debe ser valido");
                }

                var entregasDb = await _repoBusqueda.BuscarPorCampoAsync("Entrega", "IdCliente", IdCliente);

                if (entregasDb![0] == null)
                {
                    throw new Exception("El cliente no tiene reportes para obtener");
                }

                List<ReporteAllDto> listaReportes = new List<ReporteAllDto>();

                foreach (var entrega in entregasDb)
                {

                    var idEntregaDB = int.Parse(entrega["Id"]!.ToString()!);
                    var fechaCreacion = DateTime.Parse(entrega["FechaCreacion"]!.ToString()!);

                    var idEcopuntoDB = int.Parse(entrega["IdEcopunto"]!.ToString()!);
                    var idEmpleadoDB = int.Parse(entrega["IdTrabajador"]!.ToString()!);

                    var ecopuntoDB = await _repoBusqueda.BuscarPorCampoAsync("EcoPunto", "Id", idEcopuntoDB);
                    var trabajadorDB = await _repoBusqueda.BuscarPorCampoAsync("Usuario", "Id", idEmpleadoDB);

                    if (ecopuntoDB == null || trabajadorDB == null)
                    {
                        throw new Exception("El ecopunto o el trabajador no existe");
                    }

                    var reporte = new ReporteAllDto()
                    {
                        IdReporte = idEntregaDB,
                        FechaCreacion = fechaCreacion,
                        NombreEcopunto = ecopuntoDB![0]["NombreEcopunto"]!.ToString(),
                        Responsable = trabajadorDB![0]["Nombre"]!.ToString()
                    };
                    listaReportes.Add(reporte);
                }

                return listaReportes;


            }
            catch (Exception e)
            {
                throw new Exception($"Error inesperado al obtener el reporte: {e.Message}");
            }
        }

        public async Task<List<ReporteAllDto>> ObtenerReportesPorIdTrabajadorAsync(int IdTrabajador)
        {
            try
            {
                if (IdTrabajador <= 0)
                {
                    throw new Exception("El id del trabajador debe ser valido");
                }

                var entregasDb = await _repoBusqueda.BuscarPorCampoAsync("Entrega", "IdTrabajador", IdTrabajador);

                if (entregasDb![0] == null)
                {
                    throw new Exception("El trabajador no tiene reportes para obtener");
                }

                List<ReporteAllDto> listaReportes = new List<ReporteAllDto>();

                foreach (var entrega in entregasDb)
                {

                    var idEntregaDB = int.Parse(entrega["Id"]!.ToString()!);
                    var fechaCreacion = DateTime.Parse(entrega["FechaCreacion"]!.ToString()!);

                    var idEcopuntoDB = int.Parse(entrega["IdEcopunto"]!.ToString()!);

                    var ecopuntoDB = await _repoBusqueda.BuscarPorCampoAsync("EcoPunto", "Id", idEcopuntoDB);

                    if (ecopuntoDB == null)
                    {
                        throw new Exception("El ecopunto no existe");
                    }

                    var reporte = new ReporteAllDto()
                    {
                        IdReporte = idEntregaDB,
                        FechaCreacion = fechaCreacion,
                        NombreEcopunto = ecopuntoDB![0]["NombreEcopunto"]!.ToString(),
                        Responsable = "" // No es necesario incluir el nombre del empleado en este caso
                    };
                    listaReportes.Add(reporte);
                }

                return listaReportes;
            }
            catch (Exception e)
            {
                throw new Exception($"Error inesperado al obtener los reportes: {e.Message}");
            }
        }

        public async Task<Reporte> ObtenerInformacionReportePorIDAsync(int idReporte)
        {
            try
            {
                if (idReporte <= 0)
                {
                    throw new Exception("El id de reporte debe ser valido");
                }

                var Columnas = @"
                    t2.[Id] AS IdCliente,                    
                    t2.[Nombre] AS NombreCliente,
                    t2.[Correo] AS CorreoCliente,
                    t2.[Telefono] AS TelefonoCliente,
                    t2.[Cedula] AS CedulaCliente,
                    t3.[Id] AS IdEmpleado,
                    t3.[Nombre] AS NombreEmpleado,
                    t3.[Correo] AS CorreoEmpleado,
                    t3.[Telefono] AS TelefonoEmpleado,
                    t3.[Cedula] AS CedulaEmpleado";

                var entregaDB = await _repoJoins.JoinTresTablasFiltradoAsync(
                    "Entrega",
                    "Usuario",
                    "Usuario",
                    "IdCliente",
                    "Id",
                    "IdTrabajador",
                    "Id",
                    Columnas,
                    limite: null,
                    campoFiltro: "Id",
                    valorFiltro: idReporte
                    );

                var cColumnas = @"c.[Nombre] as NombreMaterial";

                var materialesEntregaDB = await _repoSubconsulta.EjecutarSubconsultaAsync(
                    "Material",
                    "MaterialEntrega",
                    "Id",
                    "IdMaterial",
                    "IdEntrega",
                    idReporte,
                    cColumnas: cColumnas
                );

                if (materialesEntregaDB!.Count() <= 0)
                {
                    throw new Exception($"No hay suficientes materiales entregados para calcular el top 3 y los totales");
                }

                var top3 = CalcularTop3(materialesEntregaDB!);
                var totales = CalcularTotales(materialesEntregaDB!);

                var reporte = new Reporte()
                {
                    Id = idReporte,
                    Cliente = new Cliente(
                        id: Convert.ToInt32(entregaDB![0]["IdCliente"]),
                        nombre: Convert.ToString(entregaDB![0]["NombreCliente"]) ?? "",
                        cedula: Convert.ToString(entregaDB![0]["CedulaCliente"]) ?? "",
                        correo: Convert.ToString(entregaDB![0]["CorreoCliente"]) ?? "",
                        direccion: "",
                        telefono: Convert.ToString(entregaDB![0]["TelefonoCliente"]) ?? "",
                        contrasena: "",
                        ecoPuntos: 0
                        ),
                    Trabajador = new Trabajador(
                        id: Convert.ToInt32(entregaDB[0]["IdEmpleado"]),
                        nombre: entregaDB[0]["NombreEmpleado"]?.ToString() ?? "",
                        cedula: entregaDB[0]["CedulaEmpleado"]?.ToString() ?? "",
                        correo: entregaDB[0]["CorreoEmpleado"]?.ToString() ?? "",
                        direccion: "",
                        telefono: entregaDB[0]["TelefonoEmpleado"]?.ToString() ?? "",
                        contrasena: "",
                        codigoDeEmpleado: "",
                        horario: ""
                    ),
                    top3 = top3,
                    totales = totales,
                    MaterialesEntrega = materialesEntregaDB!,
                };

                return reporte;
            }
            catch (Exception e)
            {
                throw new Exception($"Error inesperado al obtener la entrega: {e.Message}");
            }
        }

        public Dictionary<string, string> CalcularTop3(List<Dictionary<string, object>> materialesEntregaDB)
        {
            try
            {
                var totalesPorMaterial = new Dictionary<string, double>();

                foreach (var material in materialesEntregaDB)
                {
                    string nombreMaterial = material["NombreMaterial"]?.ToString() ?? "Desconocido";

                    double peso = 0;
                    if (material.ContainsKey("Peso") && material["Peso"] != null)
                    {
                        peso = Convert.ToDouble(material["Peso"]);
                    }

                    if (totalesPorMaterial.ContainsKey(nombreMaterial))
                    {
                        totalesPorMaterial[nombreMaterial] += peso;
                    }
                    else
                    {
                        totalesPorMaterial[nombreMaterial] = peso;
                    }
                }

                var top3 = totalesPorMaterial
                    .OrderByDescending(x => x.Value)
                    .Take(3)
                    .Select((item, index) => (
                        posicion: $"{index + 1}°",
                        nombre: item.Key
                    ))
                    .ToDictionary(x => x.posicion, x => x.nombre);

                return top3;
            }
            catch (Exception e)
            {
                throw new Exception($"Error inesperado al calcular el top 3: {e.Message}");
            }
        }

        public Dictionary<string, double> CalcularTotales(List<Dictionary<string, object>> materialesDB)
        {
            try
            {
                var totalEntregado = 0.0;
                var totalAceptado = 0.0;
                var totalRechazado = 0.0;
                var totalPuntos = 0;

                foreach (var material in materialesDB)
                {
                    var peso = Convert.ToDouble(material["Peso"]);
                    var aceptado = Convert.ToBoolean(material["Estado"]);
                    var puntos = Convert.ToInt32(material["Puntos"]);

                    if (!aceptado)
                    {
                        totalRechazado += peso;
                    }
                    else
                    {
                        totalAceptado += peso;
                    }
                    totalEntregado += peso;
                    totalPuntos += puntos;
                }

                var totales = new Dictionary<string, double>
                {
                    ["totalEntregado"] = totalEntregado,
                    ["totalAceptado"] = totalAceptado,
                    ["totalRechazado"] = totalRechazado,
                    ["totalPuntos"] = totalPuntos,
                };

                return totales;
            }
            catch (Exception e)
            {
                throw new Exception($"Error inesperado al calcular los totales del reporte: {e.Message}");
            }
        }
    }
}

using System.ComponentModel;
using webapicsharp.Interface.Servicios.Abstracciones;
using webapicsharp.Modelos;
using webapicsharp.Repositorios.Abstracciones;

namespace webapicsharp.Servicios
{
    public class ServicioEcoPunto : IServicioEcoPunto
    {

        private readonly IRepositorioBusquedaPorCampoTabla _repoBusqueda;
        private readonly IRepositorioEscrituraTabla _repoEscritura;
        private readonly IRepositorioActualizarTabla _repoActualizar;
        private readonly IRepositorioEliminarTabla _repoEliminar;
        private readonly IRepositorioLecturaTabla _repoLectura;
        private readonly IRepositorioJoinTresTablas _repoJoinTres;

        public ServicioEcoPunto(
            IRepositorioEscrituraTabla repoEscritura,
            IRepositorioActualizarTabla repoActualizar,
            IRepositorioEliminarTabla repoEliminar,
            IRepositorioBusquedaPorCampoTabla repoBusqueda,
            IRepositorioLecturaTabla repoLectura,
            IRepositorioJoinTresTablas repoJoinTres)
        {
            _repoEscritura = repoEscritura;
            _repoBusqueda = repoBusqueda;
            _repoActualizar = repoActualizar;
            _repoEliminar = repoEliminar;
            _repoLectura = repoLectura;
            _repoJoinTres = repoJoinTres;
        }

        public async Task<EcoPunto> CrearEcoPuntoAsync(
            int idTrabajador,
            string latitud,
            string longitud,
            string direccion,
            string horario,
            List<int> materiales)
        {
            try
            {
                // 1️⃣ Insertar EcoPunto
                var datosEcoPunto = new Dictionary<string, object?>
                {
                    { "IdTrabajador", idTrabajador },
                    { "Latitud", latitud },
                    { "Longitud", longitud },
                    { "Direccion", direccion },
                    { "Horario", horario }
                };

                var ecoPuntoCreado = await _repoEscritura.InsertarAsync("EcoPunto", datosEcoPunto);
                int idEcoPunto = Convert.ToInt32(ecoPuntoCreado["Id"]);

                // 2️⃣ Insertar materiales en tabla intermedia
                foreach (var idMaterial in materiales)
                {
                    var datosRelacion = new Dictionary<string, object?>
                    {
                        { "IdEcoPunto", idEcoPunto },
                        { "IdMaterial", idMaterial }
                    };
                    await _repoEscritura.InsertarAsync("MaterialEcoPunto", datosRelacion);
                }

                var trabajadorDatos = await _repoJoinTres.JoinTresTablasAsync(
                   "Usuario",
                   "Empleado",
                   "Trabajador",
                   "Id",
                   "Id",
                   "Id",
                   "Id",
                   "*",
                   "INNER"
                   );

                var materialesDatos = await _repoLectura.ObtenerFilasAsync("Material", null, null);

                var respuesta = new EcoPunto
                {
                    Id = idEcoPunto,
                    Horario = horario,
                    Ubicacion = new Ubicacion
                    {
                        Latitud = latitud,
                        Longitud = longitud,
                        Direccion = direccion
                    },
                    Trabajador = new Trabajador(
                        int.TryParse(trabajadorDatos[0]["Id"]?.ToString(), out var id) ? id : 0,
                        trabajadorDatos[0]["Nombre"]?.ToString() ?? "",
                        trabajadorDatos[0]["Cedula"]?.ToString() ?? "",
                        trabajadorDatos[0]["Correo"]?.ToString() ?? "",
                        trabajadorDatos[0]["Direccion"]?.ToString() ?? "",
                        trabajadorDatos[0]["Telefono"]?.ToString() ?? "",
                        trabajadorDatos[0]["Contrasena"]?.ToString() ?? "",
                        trabajadorDatos[0]["CodigoDeEmpleado"]?.ToString() ?? "",
                        trabajadorDatos[0]["Horario"]?.ToString() ?? ""
                    ){},
                    MaterialesAceptados = materialesDatos.Select(fila => new Material(
                        int.TryParse(fila["Id"]?.ToString(), out var id) ? id : 0,
                        fila["Nombre"]?.ToString() ?? "",
                        double.TryParse(fila["Peso"]?.ToString(), out var peso) ? peso : 0
                    ){
                    }).ToList()
                };

                return respuesta;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear el EcoPunto: {ex.Message}");
            }
        }
    }
}

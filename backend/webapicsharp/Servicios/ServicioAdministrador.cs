using webapicsharp.Interface.Servicios.Abstracciones;
using webapicsharp.Modelos;
using webapicsharp.Repositorios.Abstracciones;

namespace webapicsharp.Servicios
{
    public class ServicioAdministrador : IServicioAdministrador
    {
        private readonly IRepositorioBusquedaPorCampoTabla _repoBusqueda;
        private readonly IRepositorioEscrituraTabla _repoEscritura;
        private readonly IRepositorioActualizarTabla _repoActualizar;
        private readonly IRepositorioEliminarTabla _repoEliminar;
        private readonly IRepositorioBuscarUltimoTabla _repoBuscarUltimo;
        private readonly IRepositorioJoinTresTablas _repoJoinTres;

        public ServicioAdministrador(
            IRepositorioEscrituraTabla repoEscritura,
            IRepositorioActualizarTabla repoActualizar,
            IRepositorioEliminarTabla repoEliminar,
            IRepositorioBusquedaPorCampoTabla repoBusqueda,
            IRepositorioBuscarUltimoTabla repoBuscarUltimo,
            IRepositorioJoinTresTablas repoJoinTres)
        {
            _repoEscritura = repoEscritura;
            _repoBusqueda = repoBusqueda;
            _repoActualizar = repoActualizar;
            _repoEliminar = repoEliminar;
            _repoBuscarUltimo = repoBuscarUltimo;
            _repoJoinTres = repoJoinTres;
        }

        public async Task<Administrador?> CrearAdministradorAsync(Administrador administrador)
        {
            try
            {
                var cedula = administrador.ObtenerCedula();
                var correo = administrador.ObtenerCorreo();

                var usuarioExiste = await _repoBusqueda.BuscarPorCampoAsync("usuario", "Correo", correo!);
                var existeCedula = await _repoBusqueda.BuscarPorCampoAsync("usuario", "Cedula", cedula!);

                if (usuarioExiste != null)
                    throw new Exception( "Ya existe usuario con este correo");

                if (existeCedula != null && existeCedula["Correo"]?.ToString() != correo)
                    throw new Exception("Existe otro usuario con la misma cedula");

                var datosUsuario = new Dictionary<string, object?>
                {
                    ["Nombre"] = administrador.ObtenerNombre(),
                    ["Cedula"] = administrador.ObtenerCedula(),
                    ["Correo"] = administrador.ObtenerCorreo(),
                    ["Direccion"] = administrador.ObtenerDireccion(),
                    ["Telefono"] = administrador.ObtenerTelefono(),
                    ["Contrasena"] = administrador.ObtenerContrasena(),
                };

                var dictUsuario = await _repoEscritura.InsertarAsync("Usuario", datosUsuario);
                if (dictUsuario is null)
                    throw new Exception("El registro usuario no se pudo crear correctamente");

                var idUsuario = dictUsuario["Id"];

                var ultimoEmpleado = await _repoBuscarUltimo.BuscarUltimoAsync("Empleado", "Id");
                var codigoEmpleado = CalculadorCodigoEmpleado(ultimoEmpleado!["CodigoDeEmpleado"].ToString()!);

                var datosEmpleado = new Dictionary<string, object?>
                {
                    ["id"] = idUsuario,
                    ["CodigoDeEmpleado"] = codigoEmpleado
                };
                var dictEmpleado = await _repoEscritura.InsertarAsync("Empleado", datosEmpleado);
                if (dictEmpleado is null)
                    throw new Exception("El registro empleado no se pudo crear correctamente");

                var datosAdministrador = new Dictionary<string, object?>
                {
                    ["id"] = idUsuario ,
                    ["NivelAcceso"] = administrador.NivelDeAcceso
                };
                var dictAdministrador = await _repoEscritura.InsertarAsync("Administrador", datosAdministrador);
                if (dictAdministrador is null)
                    throw new Exception("El registro administrador no se pudo crear correctamente");

                var administradorCreado = new Administrador(
                    int.TryParse(datosUsuario["Id"]?.ToString(), out var id) ? id : 0,
                    datosUsuario["Nombre"]?.ToString() ?? "",
                    datosUsuario["Cedula"]?.ToString() ?? "",
                    datosUsuario["Correo"]?.ToString() ?? "",
                    datosUsuario["Direccion"]?.ToString() ?? "",
                    datosUsuario["Telefono"]?.ToString() ?? "",
                    datosUsuario["Contrasena"]?.ToString() ?? "",
                    datosEmpleado["CodigoDeEmpleado"]?.ToString() ?? "",
                    int.TryParse(dictAdministrador["NivelAcceso"]?.ToString(), out var nivelAcceso) ? nivelAcceso : 0
                    );

                return administradorCreado;
            }
            catch (Exception e)
            {
                throw new Exception($"Error inesperado al crear administrador: {e.Message}");
            }
        }

        public async Task<Administrador?> BuscarAdministradorPorCorreoAsync(string correo)
        {
            try
            {
                var administradorDatos = await _repoJoinTres.JoinTresTablasAsync(
                    "Usuario",
                    "Empleado",
                    "Administrador",
                    "Id",
                    "Id",
                    "Id",
                    "Id",
                    "*",
                    "INNER"
                    );

                var administrador = new Administrador(
                    int.TryParse(administradorDatos[0]["Id"]?.ToString(), out var id) ? id : 0,
                    administradorDatos[0]["Nombre"]?.ToString() ?? "",
                    administradorDatos[0]["Cedula"]?.ToString() ?? "",
                    administradorDatos[0]["Correo"]?.ToString() ?? "",
                    administradorDatos[0]["Direccion"]?.ToString() ?? "",
                    administradorDatos[0]["Telefono"]?.ToString() ?? "",
                    administradorDatos[0]["Contrasena"]?.ToString() ?? "",
                    administradorDatos[0]["CodigoDeEmpleado"]?.ToString() ?? "",
                    int.TryParse(administradorDatos[0]["NivelAcceso"]?.ToString(), out var nivelAcceso) ? nivelAcceso : 0
                    );

                return administrador;

            } catch (Exception e)
            {
                throw new Exception($"Error inesperado al Buscar administrador: {e.Message}");
            }
        } 

        public string CalculadorCodigoEmpleado(string codigoEmpleado)
        {
            List<string> split = codigoEmpleado.Split("-").ToList();

            var numero = int.Parse(split[1]) + 1;
            return split[0] + "-" + numero.ToString();
        }
    }
}

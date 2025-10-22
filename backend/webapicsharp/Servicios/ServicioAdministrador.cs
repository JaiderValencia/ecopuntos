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

        public ServicioAdministrador(
            IRepositorioEscrituraTabla repoEscritura,
            IRepositorioActualizarTabla repoActualizar,
            IRepositorioEliminarTabla repoEliminar,
            IRepositorioBusquedaPorCampoTabla repoBusqueda,
            IRepositorioBuscarUltimoTabla repoBuscarUltimo)
        {
            _repoEscritura = repoEscritura;
            _repoBusqueda = repoBusqueda;
            _repoActualizar = repoActualizar;
            _repoEliminar = repoEliminar;
            _repoBuscarUltimo = repoBuscarUltimo;
        }

        public async Task<string> CrearAdministradorAsync(Administrador administrador)
        {
            try
            {
                var cedula = administrador.ObtenerCedula();
                var correo = administrador.ObtenerCorreo();

                var usuarioExiste = await _repoBusqueda.BuscarPorCampoAsync("usuario", "Correo", correo!);
                var existeCedula = await _repoBusqueda.BuscarPorCampoAsync("usuario", "Cedula", cedula!);

                if (usuarioExiste != null)
                    return "Ya existe usuario con este correo";

                if (existeCedula != null && existeCedula["Correo"]?.ToString() != correo)
                    return "Existe otro usuario con la misma cedula";

                var datosUsuario = new Dictionary<string, object?>
                {
                    ["Nombre"] = administrador.ObtenerNombre(),
                    ["Cedula"] = administrador.ObtenerCedula(),
                    ["Correo"] = administrador.ObtenerCorreo(),
                    ["Direccion"] = administrador.ObtenerDireccion(),
                    ["Telefono"] = administrador.ObtenerTelefono(),
                    ["Contrasena"] = administrador.ObtenerContrasena(),
                };
                if (!await _repoEscritura.InsertarAsync("Usuario", datosUsuario))
                    return "El registro usuario no se pudo crear correctamente";

                var idUsuario = await _repoBusqueda.BuscarPorCampoAsync("usuario", "Correo", correo!);

                var ultimoEmpleado = await _repoBuscarUltimo.BuscarUltimoAsync("Empleado", "Id");
                var codigoEmpleado = CalculadorCodigoEmpleado(ultimoEmpleado!["CodigoDeEmpleado"].ToString()!);

                var datosEmpleado = new Dictionary<string, object?>
                {
                    ["id"] = idUsuario!["Id"],
                    ["CodigoDeEmpleado"] = codigoEmpleado
                };
                if (!await _repoEscritura.InsertarAsync("Empleado", datosEmpleado))
                    return "El registro empleado no se pudo crear correctamente";

                var datosAdministrador = new Dictionary<string, object?>
                {
                    ["id"] = idUsuario!["Id"],
                    ["NivelAcceso"] = administrador.NivelDeAcceso
                };
                if (!await _repoEscritura.InsertarAsync("Administrador", datosAdministrador))
                    return "El registro administrador no se pudo crear correctamente";

                return "El Administrador fue creado exitosamente";
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
                var usuarioDatos = await _repoBusqueda.BuscarPorCampoAsync("Usuario", "Correo", correo);
                var empleadoDatos = await _repoBusqueda.BuscarPorCampoAsync("Empleado", "Id", usuarioDatos!["Id"]!);
                var administradorDatos = await _repoBusqueda.BuscarPorCampoAsync("Administrador", "Id", usuarioDatos!["Id"]!);

                var administrador = new Administrador(
                    int.TryParse(usuarioDatos["Id"]?.ToString(), out var id) ? id : 0,
                    usuarioDatos["Nombre"]?.ToString() ?? "",
                    usuarioDatos["Cedula"]?.ToString() ?? "",
                    usuarioDatos["Correo"]?.ToString() ?? "",
                    usuarioDatos["Direccion"]?.ToString() ?? "",
                    usuarioDatos["Telefono"]?.ToString() ?? "",
                    usuarioDatos["Contrasena"]?.ToString() ?? "",
                    empleadoDatos!["CodigoDeEmpleado"]?.ToString() ?? "",
                    int.TryParse(administradorDatos!["NivelAcceso"]?.ToString(), out var nivelAcceso) ? nivelAcceso : 0
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

using webapicsharp.Interface.Servicios.Abstracciones;
using webapicsharp.Modelos;
using webapicsharp.Repositorios.Abstracciones;



namespace webapicsharp.Servicios
{
    public class ServicioTrabajador : IServicioTrabajador
    {
        private readonly IRepositorioBusquedaPorCampoTabla _repoBusqueda;
        private readonly IRepositorioEscrituraTabla _repoEscritura;
        private readonly IRepositorioActualizarTabla _repoActualizar;
        private readonly IRepositorioBuscarUltimoTabla _repoBuscarUltimo;
        private readonly IRepositorioEliminarTabla _repoEliminar;
        private readonly IRepositorioLecturaJoin _repoJoin;

        public ServicioTrabajador(
            IRepositorioEscrituraTabla repoEscritura,
            IRepositorioActualizarTabla repoActualizar,
            IRepositorioEliminarTabla repoEliminar,
            IRepositorioBusquedaPorCampoTabla repoBusqueda, 
            IRepositorioBuscarUltimoTabla repoBuscarUltimo,
            IRepositorioLecturaJoin repoJoin)
        {
            _repoEscritura = repoEscritura;
            _repoBusqueda = repoBusqueda;
            _repoActualizar = repoActualizar;
            _repoEliminar = repoEliminar;
            _repoBuscarUltimo = repoBuscarUltimo;
            _repoJoin = repoJoin;
        }

        public async Task<string> CrearTrabajadorAsync(Trabajador trabajador)
        {
            try
            {
                var cedula = trabajador.ObtenerCedula();
                var correo = trabajador.ObtenerCorreo();

                var usuarioExiste = await _repoBusqueda.BuscarPorCampoAsync("usuario", "Correo", correo!);
                var existeCedula = await _repoBusqueda.BuscarPorCampoAsync("usuario", "Cedula", cedula!);

                if (usuarioExiste != null)
                    return "Ya existe usuario con este correo";

                if (existeCedula != null && existeCedula["Correo"]?.ToString() != correo)
                    return "Existe otro usuario con la misma cedula";

                var datosUsuario = new Dictionary<string, object?>
                {
                    ["Nombre"] = trabajador.ObtenerNombre(),
                    ["Cedula"] = trabajador.ObtenerCedula(),
                    ["Correo"] = trabajador.ObtenerCorreo(),
                    ["Direccion"] = trabajador.ObtenerDireccion(),
                    ["Telefono"] = trabajador.ObtenerTelefono(),
                    ["Contrasena"] = trabajador.ObtenerContrasena(),
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

                var datosTrabajador = new Dictionary<string, object?>
                {
                    ["id"] = idUsuario!["Id"],
                    ["Horario"] = trabajador.Horario
                };
                if (!await _repoEscritura.InsertarAsync("Trabajador", datosTrabajador))
                    return "El registro trabajador no se pudo crear correctamente";

                return "El Trabajador fue creado exitosamente";
            }
            catch (Exception e)
            {
                throw new Exception($"Error inesperado al crear trabajador: {e.Message}");
            }
        }

        public async Task<Trabajador?> BuscarTrabajadorPorCorreoAsync(string correo)
        {
            try
            {
                var usuarioDatos = await _repoBusqueda.BuscarPorCampoAsync("Usuario", "Correo", correo);
                var empleadoDatos = await _repoBusqueda.BuscarPorCampoAsync("Empleado", "Id", usuarioDatos!["Id"]!);
                var trabajadorDatos = await _repoBusqueda.BuscarPorCampoAsync("Trabajador", "Id", usuarioDatos!["Id"]!);

                var trabajador = new Trabajador(
                    int.TryParse(usuarioDatos["Id"]?.ToString(), out var id) ? id : 0,
                    usuarioDatos["Nombre"]?.ToString() ?? "",
                    usuarioDatos["Cedula"]?.ToString() ?? "",
                    usuarioDatos["Correo"]?.ToString() ?? "",
                    usuarioDatos["Direccion"]?.ToString() ?? "",
                    usuarioDatos["Telefono"]?.ToString() ?? "",
                    usuarioDatos["Contrasena"]?.ToString() ?? "",
                    empleadoDatos!["CodigoDeEmpleado"]?.ToString() ?? "",
                    trabajadorDatos!["Horario"]?.ToString() ?? ""
                    );

                return trabajador;

            }
            catch (Exception e)
            {
                throw new Exception($"Error inesperado al Buscar trabajadores: {e.Message}");
            }
        }

        public async Task<IReadOnlyList<Dictionary<string, object?>>> ObtenerTrabajadoresAsync(int limite)
        {
            try
            {
                var trabajadores = await _repoJoin.ListarTrabajadoresAsync(limite);

                return trabajadores;
            }
            catch (Exception e)
            {
                throw new Exception($"Error inesperado al obtner los trabajadores: {e.Message}");
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

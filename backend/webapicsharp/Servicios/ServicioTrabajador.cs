using System.Collections.Generic;
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
        private readonly IRepositorioJoinTresTablasFiltrado _repoJoinTresFiltrado;

        public ServicioTrabajador(
            IRepositorioEscrituraTabla repoEscritura,
            IRepositorioActualizarTabla repoActualizar,
            IRepositorioEliminarTabla repoEliminar,
            IRepositorioBusquedaPorCampoTabla repoBusqueda, 
            IRepositorioBuscarUltimoTabla repoBuscarUltimo,
            IRepositorioJoinTresTablasFiltrado repoJoinTresFiltrado
            )
        {
            _repoEscritura = repoEscritura;
            _repoBusqueda = repoBusqueda;
            _repoActualizar = repoActualizar;
            _repoEliminar = repoEliminar;
            _repoBuscarUltimo = repoBuscarUltimo;
            _repoJoinTresFiltrado = repoJoinTresFiltrado;
        }

        public async Task<Trabajador?> CrearTrabajadorAsync(Trabajador trabajador)
        {
            try
            {
                var cedula = trabajador.ObtenerCedula();
                var correo = trabajador.ObtenerCorreo();

                var usuarioExiste = await _repoBusqueda.BuscarPorCampoAsync("usuario", "Correo", correo!);
                var existeCedula = await _repoBusqueda.BuscarPorCampoAsync("usuario", "Cedula", cedula!);

                if (usuarioExiste != null)
                    throw new Exception("Ya existe usuario con este correo");

                if (existeCedula != null && existeCedula[0]["Correo"]?.ToString() != correo)
                    throw new Exception("Existe otro usuario con la misma cedula");

                var datosUsuario = new Dictionary<string, object?>
                {
                    ["Nombre"] = trabajador.ObtenerNombre(),
                    ["Cedula"] = trabajador.ObtenerCedula(),
                    ["Correo"] = trabajador.ObtenerCorreo(),
                    ["Direccion"] = trabajador.ObtenerDireccion(),
                    ["Telefono"] = trabajador.ObtenerTelefono(),
                    ["Contrasena"] = trabajador.ObtenerContrasena(),
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

                var datosTrabajador = new Dictionary<string, object?>
                {
                    ["id"] = idUsuario,
                    ["Horario"] = trabajador.Horario
                };
                var dictTrabajador = await _repoEscritura.InsertarAsync("Trabajador", datosTrabajador);
                if (dictTrabajador is null)
                    throw new Exception("El registro trabajador no se pudo crear correctamente");

                var trabajadorCreado = new Trabajador(
                    int.TryParse(dictUsuario["Id"]?.ToString(), out var id) ? id : 0,
                    dictUsuario["Nombre"]?.ToString() ?? "",
                    dictUsuario["Cedula"]?.ToString() ?? "",
                    dictUsuario["Correo"]?.ToString() ?? "",
                    dictUsuario["Direccion"]?.ToString() ?? "",
                    dictUsuario["Telefono"]?.ToString() ?? "",
                    dictUsuario["Contrasena"]?.ToString() ?? "",
                    datosEmpleado["CodigoDeEmpleado"]?.ToString() ?? "",
                    dictTrabajador["Horario"]?.ToString() ?? ""
                    );

                return trabajadorCreado;
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
                var trabajadorDatos = await _repoJoinTresFiltrado.JoinTresTablasAsync(
                   "Usuario",
                   "Empleado",
                   "Trabajador",
                   "Id",
                   "Id",
                   "Id",
                   "Id",
                   columnasSeleccionadas:"*",
                   tipoJoin:"INNER",
                   limite: null,
                   campoFiltro: "Correo",
                   valorFiltro: correo
                   );

                var trabajador = new Trabajador(
                    int.TryParse(trabajadorDatos[0]["Id"]?.ToString(), out var id) ? id : 0,
                    trabajadorDatos[0]["Nombre"]?.ToString() ?? "",
                    trabajadorDatos[0]["Cedula"]?.ToString() ?? "",
                    trabajadorDatos[0]["Correo"]?.ToString() ?? "",
                    trabajadorDatos[0]["Direccion"]?.ToString() ?? "",
                    trabajadorDatos[0]["Telefono"]?.ToString() ?? "",
                    trabajadorDatos[0]["Contrasena"]?.ToString() ?? "",
                    trabajadorDatos[0]["CodigoDeEmpleado"]?.ToString() ?? "",
                    trabajadorDatos[0]["Horario"]?.ToString() ?? ""
                    );

                return trabajador;

            }
            catch (Exception e)
            {
                throw new Exception($"Error inesperado al Buscar trabajadores: {e.Message}");
            }
        }

        public async Task<IReadOnlyList<Dictionary<string, object?>>> ObtenerTrabajadoresAsync(int? limite = 15)
        {
            try
            {
                if (!int.TryParse(limite.ToString(), out _) || limite < 1)
                {
                    throw new Exception("Debe proporcionar un limite positivo.");
                }

                var trabajadores = await _repoJoinTresFiltrado.JoinTresTablasAsync(
                    "Usuario",
                    "Empleado",
                    "Trabajador",
                    "Id",
                    "Id",
                    "Id",
                    "Id",
                    columnasSeleccionadas: "*",
                    tipoJoin: "INNER",
                    limite: limite,
                    campoFiltro: null,
                    valorFiltro: null
                    );

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

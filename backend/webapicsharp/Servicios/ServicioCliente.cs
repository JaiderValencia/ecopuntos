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
        
        public ServicioCliente(
            IRepositorioEscrituraTabla repoEscritura,
            IRepositorioActualizarTabla repoActualizar,
            IRepositorioEliminarTabla repoEliminar,
            IRepositorioBusquedaPorCampoTabla repoBusqueda)
        {
            _repoEscritura = repoEscritura;
            _repoBusqueda = repoBusqueda;
            _repoActualizar = repoActualizar;
            _repoEliminar= repoEliminar;
        }


        public async Task<string> CrearClienteAsync(Cliente cliente)
        {
            var cedula = cliente.ObtenerCedula();
            var correo = cliente.ObtenerCorreo();

            var usuarioExiste = await _repoBusqueda.BuscarPorCampoAsync("usuario", "Correo", correo!);
            var existeCedula = await _repoBusqueda.BuscarPorCampoAsync("usuario", "Cedula", cedula!);
           
            if (usuarioExiste != null)
                return "Ya existe usuario con este correo";

            if (existeCedula != null && existeCedula["Correo"]?.ToString() != correo)
                return "Existe otro usuario con la misma cedula";
            
            var datosUsuario = new Dictionary<string, object?>
            {
                ["Nombre"] = cliente.ObtenerNombre(),
                ["Cedula"] = cliente.ObtenerCedula(),
                ["Correo"] = cliente.ObtenerCorreo(),
                ["Direccion"] = cliente.ObtenerDireccion(),
                ["Telefono"] = cliente.ObtenerTelefono(),
                ["Contrasena"] = cliente.ObtenerContrasena(),
            };

            if (!await _repoEscritura.InsertarAsync("Usuario", datosUsuario))
                return "El cliente no se pudo crear correctamente";


            var nuevoUsuario = await _repoBusqueda.BuscarPorCampoAsync("usuario", "Correo", correo!);

            var datosCliente = new Dictionary<string, object?>
            {
                ["Id"] = nuevoUsuario!["Id"],
                ["EcoPuntos"] = cliente.EcoPuntos
            };

            if (!await _repoEscritura.InsertarAsync("Cliente", datosCliente))
                return "El cliente no se pudo crear correctamente";

            return "El cliente fue creado correctamente";
        }
        public async Task<Cliente?> BuscarClientePorCorreoAsync(string correo)
        {
            var usuario = await _repoBusqueda.BuscarPorCampoAsync("Usuario", "Correo", correo);

            if (usuario == null)
            {
                return null;
            }

            var idUsuario = usuario["Id"];
            var cliente = await _repoBusqueda.BuscarPorCampoAsync("Cliente", "Id", idUsuario!);
            if (cliente == null)
            {
                return null;
            }

            var clienteFiltrado = new Cliente(
                int.TryParse(usuario["Id"]?.ToString(), out var id) ? id : 0,
                usuario["Nombre"]?.ToString() ?? "",
                usuario["Cedula"]?.ToString() ?? "",
                usuario["Correo"]?.ToString() ?? "",
                usuario["Direccion"]?.ToString() ?? "",
                usuario["Telefono"]?.ToString() ?? "",
                usuario["Contrasena"]?.ToString() ?? "",
                int.TryParse(cliente["EcoPuntos"]?.ToString(), out var ecoPuntos) ? ecoPuntos : 0
            );

            return clienteFiltrado;

        }

        public async Task<string> ActualizarClientePorCorreoAsync(string correo, Cliente cliente)
        {
            var cedula = cliente.ObtenerCedula();

            var usuarioExiste = await _repoBusqueda.BuscarPorCampoAsync("usuario", "Correo", correo);
            var existeCedula = await _repoBusqueda.BuscarPorCampoAsync("usuario", "Cedula", cedula!);

            if (usuarioExiste == null)
                return "No existe usuario con este correo";

            if (existeCedula != null && existeCedula["Correo"]?.ToString() != correo)
                return "Existe otro usuario con la misma cedula";

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

            bool respuestaUsuario = await _repoActualizar.ActualizarPorCampoAsync("usuario", "Correo", correo, datosUsuario);
            bool respuestaCliente = await _repoActualizar.ActualizarPorCampoAsync("cliente", "Id", usuarioExiste["Id"]!, datosCliente);

            if (!respuestaUsuario && !respuestaCliente)
            {
                return "Usuario no se pudo actualizar";
            }

            return "Usuario actualizado correctamente";
        }

        public async Task<string> EliminarClientePorCorreoAsync(string correo)
        {
            var existeUsuario = await _repoBusqueda.BuscarPorCampoAsync("usuario", "Correo", correo);

            if (existeUsuario == null)
            {
                return $"No existe usuario con este correo: {correo}";
            }

            var existeCliente = await _repoBusqueda.BuscarPorCampoAsync("cliente", "Id", existeUsuario["Id"]!);

            bool respuestaCliente = await _repoEliminar.EliminarPorCampoAsync("Cliente", "Id", existeUsuario!["Id"]!);
            bool respuestaUsuario = await _repoEliminar.EliminarPorCampoAsync("usuario", "Correo", correo);

            if (!respuestaUsuario || !respuestaCliente)
            {
                return "Cliente no se pudo eliminar";
            }

            return "Cliente eliminado correctamente";
        }
    }
}

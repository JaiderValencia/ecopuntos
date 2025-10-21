using System.Collections.Generic;
using System.Threading.Tasks;
using webapicsharp.Modelos; // para usar la clase Usuario
using webapicsharp.Repositorios.Abstracciones;
using webapicsharp.Servicios.Abstracciones;

namespace webapicsharp.Servicios
{
    public class ServicioUsuario : IServicioUsuario
    {
        private readonly IRepositorioBusquedaPorCampoTabla _repoBusqueda;
        private readonly IRepositorioEscrituraTabla _repoEscritura;
        private readonly IRepositorioActualizarTabla _repoActualizar;
        private readonly IRepositorioEliminarTabla _repoEliminar;
        
        public ServicioUsuario(
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


        public async Task<string> CrearUsuarioAsync(Usuario usuario)
        {
            var cedula = usuario.ObtenerCedula();
            var correo = usuario.ObtenerCorreo();

            var usuarioActual = await _repoBusqueda.BuscarPorCampoAsync("usuario", "Correo", correo!);
            var existeCedula = await _repoBusqueda.BuscarPorCampoAsync("usuario", "Cedula", cedula!);
           
            if (usuarioActual != null)
            {
                return "Ya existe usuario con este correo";
            }

            if (existeCedula != null && existeCedula["Correo"]?.ToString() != correo)
            {
                return "Existe otro usuario con la misma cedula";
            }
            
            var datos = new Dictionary<string, object?>
            {
                ["Nombre"] = usuario.ObtenerNombre(),
                ["Cedula"] = usuario.ObtenerCedula(),
                ["Correo"] = usuario.ObtenerCorreo(),
                ["Direccion"] = usuario.ObtenerDireccion(),
                ["Telefono"] = usuario.ObtenerTelefono(),
                ["Contraseña"] = usuario.ObtenerContraseña(),
            };

            bool usuarioCreado = await _repoEscritura.InsertarAsync("usuario", datos);

            if (!usuarioCreado)
            {
                return "El usuario no se pudo crear correctamente";
            }

            return "El usuario fue creado correctamente";
        }
        public async Task<Dictionary<string, object?>?> BuscarUsuarioPorCorreoAsync(string correo)
        {
            var resultado = await _repoBusqueda.BuscarPorCampoAsync("Usuario", "Correo", correo);

            if (resultado == null)
            {
                return null;
            }

            var usuarioFiltrado = new Dictionary<string, object?>
            {
                ["Nombre"] = resultado["Nombre"],
                ["Cedula"] = resultado["Cedula"],
                ["Correo"] = resultado["Correo"],
                ["Direccion"] = resultado["Direccion"],
                ["Telefono"] = resultado["Telefono"],
            };

            return usuarioFiltrado;
        }
        public async Task<string> ActualizarUsuarioPorCorreoAsync(string correo, Usuario usuario)
        {
            var cedula = usuario.ObtenerCedula();
            
            var usuarioActual = await _repoBusqueda.BuscarPorCampoAsync("usuario", "Correo", correo);
            var existeCedula = await _repoBusqueda.BuscarPorCampoAsync("usuario", "Cedula", cedula!);
            
            if (usuarioActual == null)
            {
                return "No existe usuario con este correo";
            }

            if (existeCedula != null && existeCedula["Correo"]?.ToString() != correo)
            {
                return "Existe otro usuario con la misma cedula";
            }

            var nuevosDatos = new Dictionary<string, object?>
            {
                {"Nombre", usuario.ObtenerNombre()},
                {"Cedula", usuario.ObtenerCedula()},
                {"Correo", usuario.ObtenerCorreo()},
                {"Direccion", usuario.ObtenerDireccion()},
                {"Telefono", usuario.ObtenerTelefono()},
                {"Contraseña", usuario.ObtenerContraseña()}
            };

            bool respuesta = await _repoActualizar.ActualizarPorCampoAsync("usuario", "Correo", correo, nuevosDatos);

            if (!respuesta)
            {
                return "Usuario no se pudo actualizar";
            }

            return "Usuario actualizado correctamente";
        }

        public async Task<string> EliminarUsuarioPorCorreoAsync(string correo)
        {
            var existeUsuario = await _repoBusqueda.BuscarPorCampoAsync("usuario", "Correo", correo);

            if (existeUsuario == null)
            {
                return $"No existe usuario con este correo: {correo}";
            }

            bool respuesta = await _repoEliminar.EliminarPorCampoAsync("usuario", "Correo", correo);

            if (!respuesta)
            {
                return "Usuario no se pudo eliminar";
            }

            return "Usuario eliminado correctamente";
        }
    }
}

using System.Threading.Tasks;
using webapicsharp.Modelos;

namespace webapicsharp.Servicios.Abstracciones
{
    public interface IServicioUsuario
    {
        Task<string> CrearUsuarioAsync(Usuario usuario);
        Task<Dictionary<string, object?>?> BuscarUsuarioPorCorreoAsync(string correo);
        Task<string> ActualizarUsuarioPorCorreoAsync(string correo, Usuario usuario);
        Task<string> EliminarUsuarioPorCorreoAsync(string correo);

    }
}


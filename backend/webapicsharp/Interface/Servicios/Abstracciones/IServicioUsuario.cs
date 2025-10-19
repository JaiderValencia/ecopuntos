using System.Threading.Tasks;

namespace webapicsharp.Servicios.Abstracciones
{
    public interface IServicioUsuario
    {
        Task<int> CrearUsuarioAsync(string Nombre, string Cedula, string Correo, string Direccion, string Telefono, string Contraseña);
    }
}


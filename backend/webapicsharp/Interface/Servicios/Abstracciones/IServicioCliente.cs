using System.Threading.Tasks;
using webapicsharp.Modelos;

namespace webapicsharp.Servicios.Abstracciones
{
    public interface IServicioCliente
    {
        Task<string> CrearClienteAsync(Cliente cliente);
        Task<Cliente?> BuscarClientePorCorreoAsync(string correo);
        Task<string> ActualizarClientePorCorreoAsync(string correo, Cliente cliente);
        Task<string> EliminarClientePorCorreoAsync(string correo);

    }
}


using System.Threading.Tasks;
using webapicsharp.Modelos;

namespace webapicsharp.Servicios.Abstracciones
{
    public interface IServicioCliente
    {
        Task<Cliente?> CrearClienteAsync(Cliente cliente);
        Task<Cliente?> BuscarClientePorCorreoAsync(string correo);
        Task<Cliente?> ActualizarClientePorCorreoAsync(string correo, Cliente cliente);
        Task<string> EliminarClientePorCorreoAsync(string correo);

    }
}


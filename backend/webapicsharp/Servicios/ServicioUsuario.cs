using System.Collections.Generic;
using System.Threading.Tasks;
using webapicsharp.Servicios.Abstracciones;            
using webapicsharp.Repositorios.Abstracciones;

namespace webapicsharp.Servicios
{
    public class ServicioUsuario : IServicioUsuario
    {
        private readonly IRepositorioEscrituraTabla _repo;

        public ServicioUsuario(IRepositorioEscrituraTabla repo)
        {
            _repo = repo;
        }

        public async Task<int> CrearUsuarioAsync(string Nombre, string Cedula, string Correo, string Direccion, string Telefono, string Contraseña)
        {
            var datos = new Dictionary<string, object?>
            {
                ["Nombre"] = Nombre,
                ["Cedula"] = Cedula,
                ["Correo"] = Correo,
                ["Direccion"] = Direccion,
                ["Telefono"] = Telefono,
                ["Contraseña"] = Contraseña,
            };

            return await _repo.InsertarAsync("usuario", datos);
        }
    }
}
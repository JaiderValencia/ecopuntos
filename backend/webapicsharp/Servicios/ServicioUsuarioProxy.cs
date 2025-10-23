
using webapicsharp.Servicios.Abstracciones;

namespace webapicsharp.Servicios
{
    public class ServicioUsuarioProxy : IServicioUsuario
    {
        private readonly IServicioUsuario _ServicioReal;
        private readonly IServicioAuditoria _auditoria;

        public ServicioUsuarioProxy(IServicioUsuario ServicioReal, IServicioAuditoria auditoria)
        {
            _ServicioReal = ServicioReal;   
            _auditoria = auditoria;  
        }

        public async Task<int> CrearUsuarioAsync(string Nombre, string Cedula, string Correo, string Direccion, string Telefono, string Contraseña)
        {
            await _auditoria.RegistrarAccionAsync($"Intento de creación de usuario: {Nombre}");
            var resultado = await _ServicioReal.CrearUsuarioAsync(Nombre, Cedula, Correo, Direccion, Telefono, Contraseña);
            await _auditoria.RegistrarAccionAsync($"Usuario '{Nombre}' creado correctamente con ID {resultado}");

            return resultado;
        }
    }
}

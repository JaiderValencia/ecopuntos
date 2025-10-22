using webapicsharp.Modelos;

namespace webapicsharp.Interface.Servicios.Abstracciones
{
    public interface IServicioAdministrador
    {
        public Task<string> CrearAdministradorAsync(Administrador administrador);
        public string CalculadorCodigoEmpleado(string codigoEmpleado);
        public Task<Administrador?> BuscarAdministradorPorCorreoAsync(string correo);

    }
}

using webapicsharp.Modelos;

namespace webapicsharp.Interface.Servicios.Abstracciones
{
    public interface IServicioAdministrador
    {
        public Task<Administrador?> CrearAdministradorAsync(Administrador administrador);
        public string CalculadorCodigoEmpleado(string codigoEmpleado);
        public Task<Administrador?> BuscarAdministradorPorCorreoAsync(string correo);

    }
}

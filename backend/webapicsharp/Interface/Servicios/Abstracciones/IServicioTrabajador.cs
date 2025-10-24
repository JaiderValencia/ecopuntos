using webapicsharp.Modelos;

namespace webapicsharp.Interface.Servicios.Abstracciones
{
    public interface IServicioTrabajador
    {
        public Task<Trabajador?> CrearTrabajadorAsync(Trabajador trabajador);
        public string CalculadorCodigoEmpleado(string codigoEmpleado);
        public Task<Trabajador?> BuscarTrabajadorPorCorreoAsync(string correo);
        public Task<IReadOnlyList<Dictionary<string, object?>>> ObtenerTrabajadoresAsync(int? limite);

    }
}

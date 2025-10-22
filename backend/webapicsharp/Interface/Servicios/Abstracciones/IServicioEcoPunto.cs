using webapicsharp.Modelos;

namespace webapicsharp.Interface.Servicios.Abstracciones
{
    public interface IServicioEcoPunto
    {
        public Task<EcoPunto> CrearEcoPuntoAsync(
            int idTrabajador,
            string latitud,
            string longitud,
            string direccion,
            string horario,
            List<int> materiales);
        
    }
}

using webapicsharp.Modelos;

namespace webapicsharp.Interface.Servicios.Abstracciones
{
    public interface IServicioEcoPunto
    {
        public Task<EcoPunto> CrearEcoPuntoAsync(
            string CodigoDeEmpleado,
            string latitud,
            string longitud,
            string direccion,
            string horario,
            List<Material> materiales);
        public Task<EcoPunto> ActualizarEcoPuntoPorIDAsync(
            int idEcoPunto,
            string codigoDeEmpleado,
            string latitud,
            string longitud,
            string direccion,
            string horario,
            List<Material> materiales);
        

        public  Task<EcoPunto> BuscarEcoPuntoPorIDAsync(int id);
        public Task<List<EcoPunto>> ObtenerEcoPuntosAsync(int? limite);
        public Task ActualizarRelacionesMaterialEcoPunto(int idEcoPunto, List<Material> materiales);
        }

}

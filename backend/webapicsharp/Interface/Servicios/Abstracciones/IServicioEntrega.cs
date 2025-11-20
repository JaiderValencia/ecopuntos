using webapicsharp.Modelos;

namespace webapicsharp.Interface.Servicios.Abstracciones
{
    public interface IServicioEntrega
    {
        public Task<EntregaResponse> CrearEntregAsync(CrearEntregaDto entrega);

        public Task<EntregaResponse> ObtenerResultadoOperacion(
            Dictionary<string, object?> entrega, 
            List<EntregaMaterial> materiales, 
            List<Dictionary<string, object?>> cliente);

    }
}

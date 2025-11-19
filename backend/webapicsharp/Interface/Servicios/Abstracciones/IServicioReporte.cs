using webapicsharp.Modelos;

namespace webapicsharp.Interface.Servicios.Abstracciones
{
    public interface IServicioReporte
    {
        public Task<List<ReporteAllDto>> ObtenerReportesPorIdClienteAsync(int IdCliente);
        public Task<Reporte> ObtenerInformacionReportePorIDAsync(int idReporte);
    }
}

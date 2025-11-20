using webapicsharp.Modelos;

namespace webapicsharp.Interface.Servicios.Abstracciones
{
    public interface IServicioReporte
    {
        public Task<List<ReporteAllDto>> ObtenerReportesPorIdClienteAsync(int IdCliente);
        public Task<List<ReporteAllDto>> ObtenerReportesPorIdTrabajadorAsync(int IdTrabajador);
        public Task<Reporte> ObtenerInformacionReportePorIDAsync(int idReporte);
    }
}

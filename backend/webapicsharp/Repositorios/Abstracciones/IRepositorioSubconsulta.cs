namespace webapicsharp.Repositorios.Abstracciones
{
    public interface IRepositorioSubconsulta
    {
        public Task<List<Dictionary<string, object?>>?> EjecutarSubconsultaAsync(
            string tablaExterna,
            string tablaInterna,
            string campoRelacionExterna,
            string campoRelacionInterna,
            string campoFiltro,
            object valorFiltro
        );
    }
}
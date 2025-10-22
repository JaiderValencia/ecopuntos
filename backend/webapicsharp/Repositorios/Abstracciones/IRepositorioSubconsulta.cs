namespace webapicsharp.Repositorios.Abstracciones
{
    public interface IRepositorioSubconsulta
    {
        Task<Dictionary<string, object?>?> EjecutarSubconsultaAsync(
            string tablaExterna,
            string tablaInterna,
            string campoRelacion,
            string campoFiltro,
            object valorFiltro
        );
    }
}
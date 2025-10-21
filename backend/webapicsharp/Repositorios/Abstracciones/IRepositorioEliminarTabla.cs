namespace webapicsharp.Repositorios.Abstracciones
{
    public interface IRepositorioEliminarTabla
    {
        Task<bool> EliminarPorCampoAsync(string nombreTabla, string campoFiltro, object valorFiltro);
    }
}

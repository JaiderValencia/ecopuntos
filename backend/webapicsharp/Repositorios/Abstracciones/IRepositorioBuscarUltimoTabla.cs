namespace webapicsharp.Repositorios.Abstracciones
{
    public interface IRepositorioBuscarUltimoTabla
    {
        public Task<Dictionary<string, object>?> BuscarUltimoAsync(string nombreTabla, string campoOrden);
    }
}

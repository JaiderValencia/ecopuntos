using System.Collections.Generic;

namespace webapicsharp.Repositorios.Abstracciones
{
    public interface IRepositorioBusquedaPorCampoTabla
    {
        Task<List<Dictionary<string, object?>>?> BuscarPorCampoAsync(string tabla, string campo, object valor);
    }
}

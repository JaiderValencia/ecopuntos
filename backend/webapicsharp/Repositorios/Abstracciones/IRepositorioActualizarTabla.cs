using System.Collections.Generic;
using System.Threading.Tasks;

namespace webapicsharp.Repositorios.Abstracciones
{
    public interface IRepositorioActualizarTabla
    {
        Task<bool> ActualizarPorCampoAsync(string tabla, string campoFiltro, object valorFiltro, Dictionary<string, object?> nuevosValores);
    }
}
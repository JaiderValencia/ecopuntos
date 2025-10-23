using System.Collections.Generic;
using System.Threading.Tasks;

namespace webapicsharp.Repositorios.Abstracciones
{
    public interface IRepositorioActualizarTabla
    {
        Task<Dictionary<string, object?>> ActualizarPorCampoAsync(string tabla, string campoFiltro, object valorFiltro, Dictionary<string, object?> nuevosValores);
    }
}
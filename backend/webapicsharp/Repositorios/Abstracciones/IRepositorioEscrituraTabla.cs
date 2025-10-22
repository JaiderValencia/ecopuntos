using System.Collections.Generic;
using System.Threading.Tasks;

namespace webapicsharp.Repositorios.Abstracciones
{
    public interface IRepositorioEscrituraTabla
    {
        public Task<bool> InsertarAsync(string nombreTabla, Dictionary<string, object?> valores);
    } 
}
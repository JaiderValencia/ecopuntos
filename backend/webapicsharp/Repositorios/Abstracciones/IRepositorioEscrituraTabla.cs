using System.Collections.Generic;
using System.Threading.Tasks;

namespace webapicsharp.Repositorios.Abstracciones
{
    public interface IRepositorioEscrituraTabla
    {
        public Task<Dictionary<string, object?>> InsertarAsync(string nombreTabla, Dictionary<string, object?> valores);
    } 
}
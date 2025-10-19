
using System.Collections.Generic;   
using System.Threading.Tasks;       

namespace webapicsharp.Repositorios.Abstracciones
{
    public interface IRepositorioLecturaTabla
    {
        Task<IReadOnlyList<Dictionary<string, object?>>> ObtenerFilasAsync(
            string nombreTabla,
            string? esquema,
            int? limite
        );
    }
}

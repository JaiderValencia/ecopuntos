
using System.Collections.Generic;   
using System.Threading.Tasks;      

namespace webapicsharp.Servicios.Abstracciones
{

   public interface IServicioCrud
   {

       Task<IReadOnlyList<Dictionary<string, object?>>> ListarAsync(
           string nombreTabla,    
           string? esquema,       
           int? limite           
       );
   }
}

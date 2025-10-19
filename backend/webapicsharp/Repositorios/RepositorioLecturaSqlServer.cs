using System;                                        
using System.Collections.Generic;                     
using System.Threading.Tasks;                         
using Microsoft.Data.SqlClient;                      
using webapicsharp.Repositorios.Abstracciones;        
using webapicsharp.Servicios.Abstracciones;          

namespace webapicsharp.Repositorios
{

   public class RepositorioLecturaSqlServer : IRepositorioLecturaTabla
   {

       private readonly IProveedorConexion _proveedorConexion;


       public RepositorioLecturaSqlServer(IProveedorConexion proveedorConexion)
       {

           _proveedorConexion = proveedorConexion ?? throw new ArgumentNullException(
               nameof(proveedorConexion),
               "IProveedorConexion no puede ser null. Verificar registro de servicios en Program.cs."
           );
       }

       public async Task<IReadOnlyList<Dictionary<string, object?>>> ObtenerFilasAsync(
           string nombreTabla, 
           string? esquema,    
           int? limite          
       )
       {

           if (string.IsNullOrWhiteSpace(nombreTabla))
               throw new ArgumentException(
                   "El nombre de la tabla no puede estar vacío.", 
                   nameof(nombreTabla)
               );

           string esquemaFinal = string.IsNullOrWhiteSpace(esquema) ? "dbo" : esquema.Trim();

           int limiteFinal = limite ?? 1000;

           string consultaSql = $"SELECT TOP ({limiteFinal}) * FROM [{esquemaFinal}].[{nombreTabla}]";

           var resultados = new List<Dictionary<string, object?>>();
           
           try
           {

               string cadenaConexion = _proveedorConexion.ObtenerCadenaConexion();

               using var conexion = new SqlConnection(cadenaConexion);

               await conexion.OpenAsync();

               using var comando = new SqlCommand(consultaSql, conexion);

               using var lector = await comando.ExecuteReaderAsync();

               while (await lector.ReadAsync())
               {

                   var fila = new Dictionary<string, object?>();
                   

                   for (int indiceColumna = 0; indiceColumna < lector.FieldCount; indiceColumna++)
                   {
                       string nombreColumna = lector.GetName(indiceColumna);
                       object? valorColumna = lector.IsDBNull(indiceColumna) 
                           ? null    
                           : lector.GetValue(indiceColumna); 
                       fila[nombreColumna] = valorColumna;
                   }
                   resultados.Add(fila);
               }
           }
           catch (SqlException excepcionSql)
           {
               throw new InvalidOperationException(
                   $"Error de SQL Server al consultar la tabla '{esquemaFinal}.{nombreTabla}': {excepcionSql.Message}. " +
                   $"Código de error SQL Server: {excepcionSql.Number}. " +
                   $"Verificar que la tabla existe y se tienen permisos de lectura.", 
                   excepcionSql
               );
           }
           catch (Exception excepcionGeneral)
           {

               throw new InvalidOperationException(
                   $"Error inesperado al acceder a SQL Server para tabla '{esquemaFinal}.{nombreTabla}': {excepcionGeneral.Message}. " +
                   $"Verificar conectividad y configuración del servidor.", 
                   excepcionGeneral  
               );
           }
           return resultados;
       }
   }
}

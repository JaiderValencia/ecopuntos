
using System;                                          
using Microsoft.AspNetCore.Authorization;                
using Microsoft.AspNetCore.Mvc;                     
using System.Threading.Tasks;                          
using Microsoft.Extensions.Logging;                     
using Microsoft.Extensions.Configuration;              
using webapicsharp.Servicios.Abstracciones;             

namespace webapicsharp.Controllers
{

   [Route("api/{tabla}")]                                
   [ApiController]                                
   public class EntidadesController : ControllerBase
   {

       private readonly IServicioCrud _servicioCrud;          
       private readonly ILogger<EntidadesController> _logger;  
       private readonly IConfiguration _configuration;        

       public EntidadesController(
           IServicioCrud servicioCrud,          
           ILogger<EntidadesController> logger, 
           IConfiguration configuration         
       )
       {

           _servicioCrud = servicioCrud ?? throw new ArgumentNullException(
               nameof(servicioCrud), 
               "IServicioCrud no fue inyectado correctamente. Verificar registro de servicios en Program.cs"
           );
           
           _logger = logger ?? throw new ArgumentNullException(
               nameof(logger), 
               "ILogger no fue inyectado correctamente. Problema en configuración de logging de ASP.NET Core"
           );
           
           _configuration = configuration ?? throw new ArgumentNullException(
               nameof(configuration), 
               "IConfiguration no fue inyectado correctamente. Problema en configuración base de ASP.NET Core"
           );
       }

       [AllowAnonymous]                               
       [HttpGet]                                
       public async Task<IActionResult> ListarAsync(
           string tabla,                               
           [FromQuery] string? esquema,               
           [FromQuery] int? limite                      
       )
       {
           try
           {

               _logger.LogInformation(
                   "INICIO consulta - Tabla: {Tabla}, Esquema: {Esquema}, Límite: {Limite}",
                   tabla,                              
                   esquema ?? "por defecto",            
                   limite?.ToString() ?? "por defecto" 
               );

               var filas = await _servicioCrud.ListarAsync(tabla, esquema, limite);

               _logger.LogInformation(
                   "RESULTADO exitoso - Registros obtenidos: {Cantidad} de tabla {Tabla}", 
                   filas.Count,  
                   tabla          
               );


               if (filas.Count == 0)
               {
                   _logger.LogInformation(
                       "SIN DATOS - Tabla {Tabla} consultada exitosamente pero no contiene registros", 
                       tabla
                   );
                   

                   return NoContent();
               }

               return Ok(new
               {

                   tabla = tabla,                         
                   esquema = esquema ?? "por defecto",        
                   limite = limite,                           
                   total = filas.Count,                       

                   datos = filas                         

               });
           }

           catch (ArgumentException excepcionArgumento)
           {

               _logger.LogWarning(
                   "ERROR DE VALIDACIÓN - Petición rechazada - Tabla: {Tabla}, Error: {Mensaje}",
                   tabla,                          
                   excepcionArgumento.Message      
               );

               return BadRequest(new
               {
                   estado = 400,                                   
                   mensaje = "Parámetros de entrada inválidos.",    
                   detalle = excepcionArgumento.Message,           
                   tabla = tabla                                  
               });
           }
           catch (InvalidOperationException excepcionOperacion)
           {
 
               _logger.LogError(excepcionOperacion,
                   "ERROR DE OPERACIÓN - Fallo en consulta - Tabla: {Tabla}, Error: {Mensaje}",
                   tabla,                              
                   excepcionOperacion.Message          
               );

               return NotFound(new
               {
                   estado = 404,                                    
                   mensaje = "El recurso solicitado no fue encontrado.", 
                   detalle = excepcionOperacion.Message,              
                   tabla = tabla,                                    
                   sugerencia = "Verifique que la tabla y el esquema existan en la base de datos"
               });
           }
           catch (UnauthorizedAccessException excepcionAcceso)
            {
                _logger.LogWarning(
                    "ACCESO DENEGADO - Tabla restringida: {Tabla}, Error: {Mensaje}",
                    tabla,
                    excepcionAcceso.Message
                );

                return StatusCode(403, new
                {
                    estado = 403,
                    mensaje = "Acceso denegado.",
                    detalle = excepcionAcceso.Message,
                    tabla = tabla
                });
            }
           catch (Exception excepcionGeneral)
            {

                _logger.LogError(excepcionGeneral,
                    "ERROR CRÍTICO - Falla inesperada en consulta - Tabla: {Tabla}",
                    tabla             
                );
                return StatusCode(500, new
                {
                    estado = 500,                                       
                    mensaje = "Error interno del servidor.",            
                    tabla = tabla,                                      
                    detalle = "Contacte al administrador del sistema.",
                    timestamp = DateTime.UtcNow                        
                });
            }
       }

       [AllowAnonymous]                              
       [HttpGet]                                  
       [Route("api/info")]                     
       public IActionResult ObtenerInformacion()
       {
           return Ok(new
           {

               controlador = "EntidadesController",
               version = "1.0",
               descripcion = "Controlador genérico para consultar tablas de base de datos",

               endpoints = new[]
               {
                   "GET /api/{tabla} - Lista registros de una tabla",
                   "GET /api/{tabla}?esquema={esquema} - Lista con esquema específico",
                   "GET /api/{tabla}?limite={numero} - Lista con límite de registros",
                   "GET /api/info - Muestra esta información"
               },

               ejemplos = new[]
               {
                   "GET /api/usuarios",
                   "GET /api/productos?esquema=ventas",
                   "GET /api/clientes?limite=50",
                   "GET /api/pedidos?esquema=ventas&limite=100"
               }
           });
       }
       [AllowAnonymous]                                 
       [HttpGet("/")]                                 
       public IActionResult Inicio()
       {
           return Ok(new
           {

               Mensaje = "Bienvenido a la API Genérica en C#",
               Version = "1.0",
               Descripcion = "API genérica para operaciones CRUD sobre cualquier tabla de base de datos",
               Documentacion = "Para más detalles, visita /swagger",
               FechaServidor = DateTime.UtcNow,         

               Enlaces = new
               {
                   Swagger = "/swagger",                 
                   Info = "/api/info",                
                   EjemploTabla = "/api/MiTabla"        
               },

               Uso = new[]
               {
                   "GET /api/{tabla} - Lista registros de una tabla",
                   "GET /api/{tabla}?limite=50 - Lista con límite específico",
                   "GET /api/{tabla}?esquema=dbo - Lista con esquema específico"
               }
           });
       }
   }
}

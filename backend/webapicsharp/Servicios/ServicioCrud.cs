
using System;                                            
using System.Collections.Generic;                      
using System.Threading.Tasks;                            
using webapicsharp.Servicios.Abstracciones;            
using webapicsharp.Repositorios.Abstracciones;         

namespace webapicsharp.Servicios
{

    public class ServicioCrud : IServicioCrud
    {

        private readonly IRepositorioLecturaTabla _repositorioLectura;

        private readonly IConfiguration _configuration;
        public ServicioCrud(IRepositorioLecturaTabla repositorioLectura, IConfiguration configuration)
        {
            _repositorioLectura = repositorioLectura ?? throw new ArgumentNullException(nameof(repositorioLectura));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IReadOnlyList<Dictionary<string, object?>>> ListarAsync(
            string nombreTabla,
            string? esquema,
            int? limite
        )
        {

            if (string.IsNullOrWhiteSpace(nombreTabla))
                throw new ArgumentException("El nombre de la tabla no puede estar vacío.", nameof(nombreTabla));

            var tablasProhibidas = _configuration.GetSection("TablasProhibidas").Get<string[]>() ?? Array.Empty<string>();
            if (tablasProhibidas.Contains(nombreTabla, StringComparer.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException($"La tabla '{nombreTabla}' está restringida y no puede ser consultada.");

            string? esquemaNormalizado = string.IsNullOrWhiteSpace(esquema) ? null : esquema.Trim();

            int? limiteNormalizado = (limite is null || limite <= 0) ? null : limite;

            var filas = await _repositorioLectura.ObtenerFilasAsync(nombreTabla, esquemaNormalizado, limiteNormalizado);
            return filas;
        }
    }
}

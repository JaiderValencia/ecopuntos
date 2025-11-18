using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using webapicsharp.Interface.Servicios.Abstracciones;

namespace webapicsharp.Servicios.Proxy
{
    public class ServicioJwtProxy : IServicioJwt
    {
        private readonly IServicioJwt _servicioJwtReal;
        private readonly IMemoryCache _cache;
        private readonly ILogger<ServicioJwtProxy> _logger;

        public ServicioJwtProxy(
            IServicioJwt servicioJwtReal,
            IMemoryCache cache,
            ILogger<ServicioJwtProxy> logger)
        {
            _servicioJwtReal = servicioJwtReal;
            _cache = cache;
            _logger = logger;
        }

        public string GenerarToken(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
            {
                _logger.LogWarning("Intento de generar token con correo vacío");
                throw new ArgumentException("El correo no puede estar vacío");
            }

            _logger.LogInformation("Generando token para: {Correo}", correo);

            var cacheKey = $"token_{correo}";
            if (_cache.TryGetValue(cacheKey, out string? tokenCacheado))
            {
                _logger.LogInformation("Token recuperado del caché para: {Correo}", correo);
                return tokenCacheado!;
            }

            var token = _servicioJwtReal.GenerarToken(correo);

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };
            _cache.Set(cacheKey, token, cacheOptions);

            _logger.LogInformation("Token generado exitosamente para: {Correo}", correo);

            return token;
        }

        public string ValidacionCorreoRol(string correo)
        {
            _logger.LogDebug("Validando rol para correo: {Correo}", correo);

            if (string.IsNullOrWhiteSpace(correo))
            {
                _logger.LogWarning("Intento de validación con correo vacío");
                throw new ArgumentException("El correo no puede estar vacío");
            }

            var cacheKey = $"rol_{correo}";
            if (_cache.TryGetValue(cacheKey, out string? rolCacheado))
            {
                _logger.LogDebug("Rol recuperado del caché: {Rol}", rolCacheado);
                return rolCacheado!;
            }

            var rol = _servicioJwtReal.ValidacionCorreoRol(correo);

            _cache.Set(cacheKey, rol, TimeSpan.FromMinutes(30));

            _logger.LogInformation("Rol determinado para {Correo}: {Rol}", correo, rol);

            return rol;
        }

        public string HashearContrasena(string contrasena)
        {
            if (string.IsNullOrWhiteSpace(contrasena))
            {
                _logger.LogWarning("Intento de hashear contraseña vacía");
                throw new ArgumentException("La contraseña no puede estar vacía");
            }

            if (contrasena.Length < 6)
            {
                _logger.LogWarning("Intento de hashear contraseña demasiado corta");
                throw new ArgumentException("La contraseña debe tener al menos 6 caracteres");
            }

            _logger.LogInformation("Hasheando contraseña");

            return _servicioJwtReal.HashearContrasena(contrasena);
        }

        public bool CompararContrasenas(string hashContrasena, string contrasenaPlana)
        {
            if (string.IsNullOrWhiteSpace(hashContrasena) || string.IsNullOrWhiteSpace(contrasenaPlana))
            {
                _logger.LogWarning("Intento de comparación con parámetros vacíos");
                return false;
            }

            _logger.LogDebug("Comparando contraseñas");

            var resultado = _servicioJwtReal.CompararContrasenas(hashContrasena, contrasenaPlana);

            if (resultado)
            {
                _logger.LogInformation("Contraseña verificada correctamente");
            }
            else
            {
                _logger.LogWarning("Intento de autenticación fallido");
            }

            return resultado;
        }
    }
}
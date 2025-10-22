using Microsoft.Data.SqlClient;
using System.Data;
using webapicsharp.Repositorios.Abstracciones;
using webapicsharp.Servicios.Abstracciones;

namespace webapicsharp.Repositorios
{
    public class RepositorioSubconsultaSqlServer : IRepositorioSubconsulta
    {
        private readonly IProveedorConexion _proveedor;

        public RepositorioSubconsultaSqlServer(IProveedorConexion proveedor)
        {
            _proveedor = proveedor;
        }

        /// <summary>
        /// Ejecuta una subconsulta SQL con una condición anidada.
        /// Ejemplo:
        /// SELECT * FROM Cliente WHERE Id = (SELECT Id FROM Usuario WHERE Correo = @correo);
        /// </summary>
        public async Task<Dictionary<string, object?>?> EjecutarSubconsultaAsync(
            string tablaExterna,      // Ej: "Cliente"
            string tablaInterna,      // Ej: "Usuario"
            string campoRelacion,     // Ej: "Id"
            string campoFiltro,       // Ej: "Correo"
            object valorFiltro        // Ej: "correo@ejemplo.com"
        )
        {
            try
            {
                using var conexion = new SqlConnection(_proveedor.ObtenerCadenaConexion());
                await conexion.OpenAsync();

                // 🔹 Construcción del SQL: incluye los campos de ambas tablas
                string consulta = $@"
                SELECT 
                    u.*, 
                    c.*
                FROM [{tablaExterna}] AS c
                INNER JOIN [{tablaInterna}] AS u
                    ON c.[{campoRelacion}] = u.[{campoRelacion}]
                WHERE u.[{campoFiltro}] = @valor;
                ";

                using var comando = new SqlCommand(consulta, conexion);
                comando.Parameters.AddWithValue("@valor", valorFiltro ?? DBNull.Value);

                using var lector = await comando.ExecuteReaderAsync(CommandBehavior.SingleRow);

                if (!await lector.ReadAsync())
                    return null;

                var resultado = new Dictionary<string, object?>();
                for (int i = 0; i < lector.FieldCount; i++)
                    resultado[lector.GetName(i)] = await lector.IsDBNullAsync(i) ? null : lector.GetValue(i);

                return resultado;
            }
            catch (SqlException excepcionSql)
            {
                throw new InvalidOperationException(
                   $"Error de SQL Server al consultar la subconsulta: {excepcionSql.Message}. " +
                   $"Código de error SQL Server: {excepcionSql.Number}. " +
                   $"Verificar que la tabla existe y se tienen permisos de actualizacion.",
                   excepcionSql
                );
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using webapicsharp.Repositorios.Abstracciones;
using webapicsharp.Servicios.Abstracciones;

namespace webapicsharp.Repositorios
{
    public class RepositorioJoinTresTablasSqlServer : IRepositorioLecturaJoin
    {
        private readonly IProveedorConexion _proveedorConexion;

        public RepositorioJoinTresTablasSqlServer(IProveedorConexion proveedorConexion)
        {
            _proveedorConexion = proveedorConexion;
        }

        public async Task<IReadOnlyList<Dictionary<string, object?>>> ListarTrabajadoresAsync(int? limite = null)
        {
            try
            {
                int limiteFinal = (limite is null || limite <= 0) ? 1000 : limite.Value;

                var sql = $@"
                SELECT TOP ({limiteFinal})
                    u.Nombre,
                    u.Cedula,
                    u.Correo,
                    u.Direccion,
                    u.Telefono,
                    u.Contrasena,
                    e.CodigoDeEmpleado,
                    t.Horario
                FROM Usuario u
                INNER JOIN Empleado e ON e.Id = u.Id
                INNER JOIN Trabajador t ON t.Id = u.Id
                ORDER BY u.Nombre;
            ";

                var resultados = new List<Dictionary<string, object?>>();

                try
                {
                    using var conexion = new SqlConnection(_proveedorConexion.ObtenerCadenaConexion());
                    await conexion.OpenAsync();

                    using var comando = new SqlCommand(sql, conexion);
                    using var lector = await comando.ExecuteReaderAsync();

                    while (await lector.ReadAsync())
                    {
                        var fila = new Dictionary<string, object?>();
                        for (int i = 0; i < lector.FieldCount; i++)
                        {
                            string nombreColumna = lector.GetName(i);
                            object? valorColumna = lector.IsDBNull(i) ? null : lector.GetValue(i);
                            fila[nombreColumna] = valorColumna;
                        }
                        resultados.Add(fila);
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException(
                        $"Error de SQL Server al consultar los trabajadores: {ex.Message}. Código SQL: {ex.Number}.",
                        ex
                    );
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        $"Error inesperado al acceder a SQL Server: {ex.Message}.",
                        ex
                    );
                }

                return resultados;
            }
            catch(SqlException excepcionSql)
            {
                throw new InvalidOperationException(
                   $"Error de SQL Server al consultar: {excepcionSql.Message}. " +
                   $"Código de error SQL Server: {excepcionSql.Number}. " +
                   $"Verificar que la tabla existe y se tienen permisos de actualizacion.",
                   excepcionSql
                );
            }
        }
    }
}
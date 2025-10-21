using System.Data;
using Microsoft.Data.SqlClient;
using webapicsharp.Repositorios.Abstracciones;
using webapicsharp.Servicios.Abstracciones;

namespace webapicsharp.Repositorios
{
    public class RepositorioBuscarPorCampoSqlServer : IRepositorioBusquedaPorCampoTabla
    {
        private readonly IProveedorConexion _proveedor;

        public RepositorioBuscarPorCampoSqlServer(IProveedorConexion proveedor)
        {
            _proveedor = proveedor;
        }

        public async Task<Dictionary<string, object?>?> BuscarPorCampoAsync(string tabla, string campo, object valor)
        {
            if (string.IsNullOrWhiteSpace(tabla))
                throw new ArgumentException("El nombre de la tabla no puede estar vacío.");

            if (campo == null )
                throw new ArgumentException("Debe proporcionar un campo donde buscar.");

            using var conexion = new SqlConnection(_proveedor.ObtenerCadenaConexion());
            var consulta = new SqlCommand($"SELECT * FROM [{tabla}] WHERE [{campo}] = @valor", conexion);
            consulta.Parameters.AddWithValue("@valor", valor ?? DBNull.Value);

            await conexion.OpenAsync();
            using var lector = await consulta.ExecuteReaderAsync(CommandBehavior.SingleRow);

            if (!await lector.ReadAsync())
                return null;

            var resultado = new Dictionary<string, object?>();
            for (int i = 0; i < lector.FieldCount; i++)
                resultado[lector.GetName(i)] = await lector.IsDBNullAsync(i) ? null : lector.GetValue(i);

            return resultado;
        }
    }
}
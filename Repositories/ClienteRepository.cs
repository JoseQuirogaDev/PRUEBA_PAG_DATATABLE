using Dapper;
using Microsoft.Data.SqlClient;
using PRUEBA_PAG_DATATABLE.Models;

namespace PRUEBA_PAG_DATATABLE.Repositories
{
    public class ClienteRepository
    {
        private readonly IConfiguration _configuration;

        public ClienteRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<ClienteModel>> ObtenerClientesAsync()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using var connection = new SqlConnection(connectionString);
            var query = "SELECT Id, NombreCompleto FROM Clientes";

            return await connection.QueryAsync<ClienteModel>(query);
        }

        public async Task<(IEnumerable<ClienteModel> Datos, int TotalRegistros, int TotalFiltrados)>
            ObtenerClientesPaginadosFiltradosAsync(int page, int pageSize, string filtro)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var offset = (page - 1) * pageSize;

            using var connection = new SqlConnection(connectionString);

            // Total sin filtros
            var totalRegistros = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Clientes");

            // Agregar filtro si viene
            string where = string.IsNullOrWhiteSpace(filtro)
                ? ""
                : "WHERE NombreCompleto LIKE @Filtro";

            // Total con filtro
            var totalFiltradosQuery = $"SELECT COUNT(*) FROM Clientes {where}";
            var totalFiltrados = await connection.ExecuteScalarAsync<int>(totalFiltradosQuery, new { Filtro = $"%{filtro}%" });

            // Datos paginados con filtro
            var query = $@"
                SELECT Id, NombreCompleto
                FROM Clientes
                {where}
                ORDER BY Id
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            var datos = await connection.QueryAsync<ClienteModel>(query, new
            {
                Offset = offset,
                PageSize = pageSize,
                Filtro = $"%{filtro}%"
            });

            return (datos, totalRegistros, totalFiltrados);
        }

    }
}

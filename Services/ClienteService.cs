using Dapper;
using Microsoft.Data.SqlClient;
using PRUEBA_PAG_DATATABLE.Models;
using PRUEBA_PAG_DATATABLE.Repositories;

namespace PRUEBA_PAG_DATATABLE.Services
{
    public class ClienteService
    {
        private readonly ClienteRepository _clienteRepository;

        public ClienteService(ClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<IEnumerable<ClienteModel>> ObtenerClientesAsync()
        {
            return await _clienteRepository.ObtenerClientesAsync();
        }

        public async Task<(IEnumerable<ClienteModel> Datos, int TotalRegistros, int TotalFiltrados)>
            ObtenerClientesPaginadosFiltradosAsync(int page, int pageSize, string filtro)
        {
            return await _clienteRepository.ObtenerClientesPaginadosFiltradosAsync(page, pageSize, filtro);
        }

    }
}

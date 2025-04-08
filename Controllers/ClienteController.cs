using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRUEBA_PAG_DATATABLE.Services;

namespace PRUEBA_PAG_DATATABLE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClienteController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> Get()
        {
            var clientes = await _clienteService.ObtenerClientesAsync();
            return Ok(clientes);
        }

        [HttpGet("page")]
        public async Task<IActionResult> Buscar([FromQuery] int page = 1, [FromQuery] int pageSize = 25, [FromQuery] string filtro = "")
        {
            var resultado = await _clienteService.ObtenerClientesPaginadosFiltradosAsync(page, pageSize, filtro);

            return Ok(new
            {
                totalRegistros = resultado.TotalRegistros,
                totalFiltrados = resultado.TotalFiltrados,
                datos = resultado.Datos
            });
        }

    }
}

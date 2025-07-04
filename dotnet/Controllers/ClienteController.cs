using Microsoft.AspNetCore.Mvc;
using Api.Repositories.Interfaces;
using Api.Models.Dtos;
using Api.Models.ViewModels;
using Api.Services.Interfaces;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    // [Authorize]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteRepository _clienteRepository;

        private readonly IPersonalService _personalService;

        public ClienteController(IClienteRepository clienteRepository, IPersonalService personalService)
        {
            _clienteRepository = clienteRepository;
            _personalService = personalService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteViewModel>>> GetClientes(
           [FromQuery] string? busqueda,
           [FromQuery] uint? id,
           [FromQuery] uint? interno,
           [FromQuery] uint? vendedor,
           [FromQuery] int? estado
        )
        {
            var clientes = await _clienteRepository.GetClientes(busqueda,id, interno, vendedor, estado);
            return Ok(clientes);
        }

        [HttpGet("id")]
        public async Task<ActionResult<ClienteViewModel>> GetClienteById(uint id)
        {
            var cliente = await _clienteRepository.GetClientePorId(id);
            return Ok(cliente);
        }

        [HttpGet("defecto")]
        public async Task<ActionResult<ClienteViewModel>> GetClienteDefecto()
        {
            var cliente = await _personalService.GetClientePorDefecto();
            return Ok(cliente);
        }


    }
}

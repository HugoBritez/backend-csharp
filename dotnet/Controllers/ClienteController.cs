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

        private readonly IClienteService _clienteService;

        public ClienteController(IClienteRepository clienteRepository, IPersonalService personalService, IClienteService clienteService)
        {
            _clienteRepository = clienteRepository;
            _personalService = personalService;
            _clienteService = clienteService;
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

        [HttpGet("{clienteRuc}")]
        public async Task<ActionResult<ClienteViewModel>> GetClienteByRuc(string clienteRuc)
        {
            var cliente = await _clienteService.GetClienteViewModelByRuc(clienteRuc);
            return Ok(cliente);
        }

        [HttpGet("defecto")]
        public async Task<ActionResult<ClienteViewModel>> GetClienteDefecto()
        {
            var cliente = await _personalService.GetClientePorDefecto();
            return Ok(cliente);
        }

        [HttpGet("ultimo-cobro/{clienteRuc}")]
        public async Task<ActionResult<UltimoCobroClienteViewModel>> GetUltimoCobroCliente(string clienteRuc)
        {
            var ultimoCobro = await _clienteService.GetUltimoCobroCliente(clienteRuc);
            return Ok(ultimoCobro);
        }

        [HttpGet("deuda/{clienteRuc}")]
        public async Task<ActionResult<decimal>> GetDeudaCliente(string clienteRuc)
        {
            var deuda = await _clienteService.GetDeudaCliente(clienteRuc);
            return Ok(deuda);
        }


    }
}

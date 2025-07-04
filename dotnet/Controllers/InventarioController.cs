using Microsoft.AspNetCore.Mvc;
using Api.Repositories.Interfaces;
using Api.Models.Dtos;
using Api.Services.Interfaces;
using Api.Models.Entities;
using Mysqlx.Crud;
using Api.Models.ViewModels;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/inventarios")]
    // [Authorize]
    public class InventarioController : ControllerBase
    {
        private readonly IInventarioService _inventarioService;
        private readonly IInventarioRepository _inventarioRepository;
        private readonly IDetalleInventarioRepository _detalleInventarioRepository;

        public InventarioController(IInventarioService inventarioService, IInventarioRepository inventarioRepository, IDetalleInventarioRepository detalleInventarioRepository)
        {
            _inventarioService = inventarioService;
            _inventarioRepository = inventarioRepository;
            _detalleInventarioRepository = detalleInventarioRepository;
        }

        [HttpPost("anular")]
        public async Task<ActionResult<InventarioAuxiliar>> AnularInventario([FromBody] uint id)
        {
            var inventarioAnulado = await _inventarioService.AnularInventario(id);
            return Ok(inventarioAnulado);
        }

        public class CerrarInventarioRequest
        {
            public uint Id { get; set; }
            public uint UserId { get; set; }
        }

        [HttpPost("cerrar")]
        public async Task<ActionResult<InventarioAuxiliar>> CerrarInventario([FromBody] CerrarInventarioRequest request)
        {
            var inventarioCerrado = await _inventarioService.CerrarInventario(request.Id, request.UserId);
            return Ok(inventarioCerrado);
        }

        [HttpGet("autorizar")]
        public async Task<ActionResult<InventarioAuxiliar>> AutorizarInventario([FromQuery] uint id)
        {
            var inventarioAutorizado = await _inventarioService.AutorizarInventario(id);
            return Ok(inventarioAutorizado);
        }

        [HttpPost("revertir")]
        public async Task<ActionResult<InventarioAuxiliar>> RevertirInventario([FromBody] uint id)
        {
            var inventarioRevertido = await _inventarioService.RevertirInventario(id);
            return Ok(inventarioRevertido);
        }

        public class InsertarInventarioRequest
        {
            public required InsertarItemsDTO Item { get; set; }
            public uint IdInventario { get; set; }
        }

        [HttpPost("items")]
        public async Task<ActionResult<InventarioAuxiliarItems>> InsertarItems(InsertarInventarioRequest req)
        {
            var item = await _inventarioService.InsertarItems(req.Item, req.IdInventario);
            return Ok(item);
        }

        [HttpGet("items")]
        public async Task<ActionResult<DetalleInventarioViewModel>> ListarItemsInventario
        (
            [FromQuery] uint idInventario,
            [FromQuery] int filtro,
            [FromQuery] FiltroInventarioEnum tipo,
            [FromQuery] int valor,
            [FromQuery] bool stock,
            [FromQuery] string? busqueda
        )
        {
            var lista = await _inventarioService.ListarItemsInventario(idInventario, filtro, tipo, valor, busqueda, stock);
            return Ok(lista);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventarioViewModel>>> ListarInventarios(
            [FromQuery] uint? estado,
            [FromQuery] uint? deposito,
            [FromQuery] string? nro_inventario
        )
        {
            var lista = await _inventarioRepository.ListarInventarios(estado, deposito, nro_inventario);
            return Ok(lista);
        }

        [HttpPost]
        public async Task<ActionResult<InventarioAuxiliar>> CrearInventario([FromBody] InventarioAuxiliar inventario)
        {
            var inv = await _inventarioRepository.CrearInventario(inventario);
            return Ok(inv);
        }

    }
}
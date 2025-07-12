using Microsoft.AspNetCore.Mvc;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Models.ViewModels;
using Api.Repositories.Interfaces;
using Api.Repositories.Implementations;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/pedidos")]
    // [Authorize]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidosService _pedidosService;
        private readonly IPedidosRepository _pedidosRepository;

        private readonly IDetallePedidoRepository _detallePedidosRepository;

        public PedidosController(IPedidosService pedidosService, IPedidosRepository pedidosRepository, IDetallePedidoRepository detallePedidoRepository)
        {
            _pedidosService = pedidosService;
            _pedidosRepository = pedidosRepository;
            _detallePedidosRepository = detallePedidoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoDetalladoViewModel>>> GetPedidos(
            [FromQuery] string? fechaDesde,
            [FromQuery] string? fechaHasta,
            [FromQuery] string? nroPedido,
            [FromQuery] int? articulo,
            [FromQuery] string? clientes,
            [FromQuery] string? vendedores,
            [FromQuery] string? sucursales,
            [FromQuery] string? estado,
            [FromQuery] int? moneda,
            [FromQuery] string? factura
        )
        {
            var pedidos = await _pedidosService.GetPedidos(
                fechaDesde, fechaHasta, nroPedido, articulo, clientes, vendedores, sucursales, estado, moneda, factura
            );

            return Ok(pedidos);
        }

        [HttpGet("detalles")]
        public async Task<ActionResult<IEnumerable<PedidoDetalleViewModel>>> GetDetalles(
            [FromQuery] uint codigo
        )
        {
            var detalles = await _detallePedidosRepository.GetDetallesPedido(codigo);

            return Ok(detalles);
        }

        [HttpPost]
        public async Task<ActionResult<Pedido>> CrearPedido(
            [FromBody] CrearPedidoDTO crearPedidoDTO
            )
        {
            var pedidoCreado = await _pedidosService.CrearPedido(crearPedidoDTO.Pedido, crearPedidoDTO.DetallePedido);
            return CreatedAtAction(nameof(CrearPedido), new { id = pedidoCreado.Codigo }, pedidoCreado);
        }

        [HttpPost("anular")]
        public async Task<ActionResult<string>> AnularPedido(
            [FromBody] AnularPedidoDTO anularPedidoDTO
        )
        {
            var res = await _pedidosService.AnularPedido(anularPedidoDTO.codigo, anularPedidoDTO.motivo);

            return Ok(res);
        }

        public class AnularFaltanteDTO
    {
        public uint detalleFaltante { get; set; }
    }

        [HttpPost("anular-faltante")]
        public async Task<ActionResult<string>> AnularFaltante(
            [FromBody] AnularFaltanteDTO request
        )
        {
            var res = await _pedidosService.AnularFaltante(request.detalleFaltante);
            return Ok(res);
        }

        [HttpPost("autorizar/")]
        public async Task<ActionResult<ResponseViewModel<Pedido>>> AutorizarPedido(
            [FromBody] AutorizarPedidoDTO parametros
        )
        {
            var response =await  _pedidosService.AutorizarPedido(parametros.idPedido, parametros.Usuario, parametros.Password);
            return Ok(response);
        }

        [HttpGet("reporte-pedidos-facturados")]
        public async Task<ActionResult<IEnumerable<ReportePedidosFacturadosViewModel>>> ReportePedidosFacturados(
            [FromQuery] DateTime? fechaDesde,
            [FromQuery] DateTime? fechaHasta,
            [FromQuery] uint? articulo,
            [FromQuery] uint? vendedor,
            [FromQuery] uint? cliente,
            [FromQuery] uint? sucursal
        )
        {
            var response = await _pedidosRepository.ReportePedidosFacturados(fechaDesde, fechaHasta, articulo, vendedor, cliente, sucursal);
            return Ok(response);
        }

        [HttpGet("reporte-pedidos-proveedor")]
        public async Task<ActionResult<IEnumerable<ReportePedidosPorProveedor>>> GetReportePedidosPorProveedor(
            [FromQuery] string? fecha_desde,
            [FromQuery] string? fecha_hasta,
            [FromQuery] uint? proveedor,
            [FromQuery] uint? cliente,
            [FromQuery] uint? nroPedido,
            [FromQuery] uint? vendedor,
            [FromQuery] uint? articulo,
            [FromQuery] uint? moneda,
            [FromQuery] int? estado
        )
        {
            var reporte = await _pedidosRepository.GetReportePedidosPorProveedor(fecha_desde, fecha_hasta, proveedor, cliente, nroPedido, vendedor, articulo, moneda, estado);
            return Ok(reporte);
        }

        public class CambiarLoteDetallePedidoDTO
        {
            public uint idDetallePedido { get; set; }
            public string lote { get; set; } = string.Empty;
            public uint idLote { get; set; }
        }

        [HttpPost("cambiar-lote")]
        public async Task<ActionResult<DetallePedido>> CambiarLoteDetallePedido(
            [FromBody] CambiarLoteDetallePedidoDTO request
        )
        {
            var response = await _pedidosService.CambiarLoteDetallePedido(
                request.idDetallePedido, 
                request.lote, 
                request.idLote
            );
            return Ok(response);
        }
    }
}
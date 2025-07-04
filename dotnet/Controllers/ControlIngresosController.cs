using Microsoft.AspNetCore.Mvc;
using Api.Repositories.Interfaces;
using Api.Models.ViewModels;
using Api.Services.Interfaces;
using Api.Models.Dtos;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/control-ingreso")]
    // [Authorize]
    public class ControlIngresosController : ControllerBase
    {
        private readonly IControlIngresoService _controlIngresoService;
        private readonly IControlIngresoRepository _controlIngresoRepository;

        public ControlIngresosController(IControlIngresoService controlIngresoService, IControlIngresoRepository controlIngresoRepository)
        {
            _controlIngresoRepository = controlIngresoRepository;
            _controlIngresoService = controlIngresoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompraConsultaViewModel>>> GetFacturas(
            [FromQuery] uint? deposito,
            [FromQuery] uint? proveedor,
            [FromQuery] DateTime? fechadesde,
            [FromQuery] DateTime? fechahasta,
            [FromQuery] string? factura,
            [FromQuery] uint? verificado
        )
        {
            var facturas = await _controlIngresoRepository.GetFacturas(deposito, proveedor, fechadesde, fechahasta, factura, verificado);

            return Ok(facturas);
        }

        [HttpGet("items")]
        public async Task<ActionResult<IEnumerable<DetalleCompraConsultaViewModel>>> GetItems(
            uint idCompra,
            string? busqueda,
            bool aVerificar
        )
        {
            var items = await _controlIngresoRepository.GetItems(idCompra, busqueda, aVerificar);

            return Ok(items);
        }

        [HttpGet("reporte")]
        async Task<ActionResult<IEnumerable<ReporteIngresosViewModel>>> Reporte(
            uint? deposito,
            uint? proveedor,
            DateTime? fechadesde,
            DateTime? fechahasta,
            string? factura,
            uint? verificado
        )
        {
            var reporte = await _controlIngresoRepository.Reporte(deposito, proveedor, fechadesde, fechahasta, factura, verificado);

            return Ok(reporte);
        }

        public class VerificarCompraRequest
        {
            public uint idCompra { get; set; }
            public uint userId { get; set; }
        }

        public class VerificarItemRequest
        {
            public uint detalle { get; set; }
            public decimal cantidad { get; set; }
            public string lote { get; set; } = string.Empty;
            public DateTime vencimiento { get; set; }
        }

        public class ConfirmarVerificacionRequest
        {
            public uint idCompra { get; set; }
            public string factura { get; set; } = string.Empty;
            public uint deposito_inicial { get; set; }
            public uint deposito_destino { get; set; }
            public uint user_id { get; set; }
            public uint confirmador_id { get; set; }
            public IEnumerable<ItemConfirmarVerificacionDTO> items { get; set; } = [];
        }

        [HttpPost("verificar-compra")]
        public async Task<ActionResult<bool>> VerificarCompra([FromBody] VerificarCompraRequest data)
        {
            var res = await _controlIngresoService.VerificarCompra(data.idCompra, data.userId);
            return Ok(res);
        }

        [HttpPost("verificar-item")]
        public async Task<ActionResult<bool>> VerificarItem([FromBody] VerificarItemRequest data)
        {
            var res = await _controlIngresoService.VerificarItem(data.detalle, data.cantidad, data.lote, data.vencimiento);

            return Ok(res);
        }

        [HttpPost("confirmar")]
        public async Task<ActionResult<bool>> ConfirmarVerificacion([FromBody] ConfirmarVerificacionRequest data)
        {
            var res = await _controlIngresoService.ConfirmarVerificacion(data.idCompra, data.factura, data.deposito_inicial, data.deposito_destino, data.user_id, data.confirmador_id, data.items);
            return Ok(res);
        }


    }
}

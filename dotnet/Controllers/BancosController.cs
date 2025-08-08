using Microsoft.AspNetCore.Mvc;
using Api.Models.ViewModels;
using Api.Services.Interfaces;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class BancosController : ControllerBase
    {
        private readonly IBancoService _bancoService;
        public BancosController(IBancoService bancoService)
        {
            _bancoService = bancoService;
        }

        [HttpGet("cuentas")]
        public async Task<ActionResult<IEnumerable<CuentaBancariaViewModel>>> GetCuentasBancarias(
            [FromQuery] int? estado,
            [FromQuery] uint? moneda,
            [FromQuery] string? fechaInicio,
            [FromQuery] string? fechaFin,
            [FromQuery] int? situacion,
            [FromQuery] int? checkSaldo,
            [FromQuery] int? chequeTransferencia
        )
        {
            var res = await _bancoService.ConsultarCuentasBancarias(estado, moneda, fechaInicio, fechaFin, situacion, checkSaldo, chequeTransferencia);
            return Ok(res);
        }

        [HttpGet("consultas")]
        public async Task<ActionResult<IEnumerable<MovimientoBancarioViewModel>>> GetConsultasMovimientosBancarios(
            [FromQuery] string fechaInicio,
            [FromQuery] string fechaFin,
            [FromQuery] int? estado,
            [FromQuery] string? cheque,
            [FromQuery] uint? codigoCuenta,
            [FromQuery] int? tipoFecha,
            [FromQuery] int? chequeTransferencia,
            [FromQuery] int? checkSaldo,
            [FromQuery] int? situacion,
            [FromQuery] string? busqueda,
            [FromQuery] int? aplicado
        )
        {
            var res = await _bancoService.ConsultaMovimientosBancarios(fechaInicio, fechaFin, estado, cheque, codigoCuenta, tipoFecha, chequeTransferencia, checkSaldo, situacion, busqueda, aplicado);
            return Ok(res);
        }
    }
}


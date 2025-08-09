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
            [FromQuery]string fechaInicio,
            [FromQuery]string fechaFin,
            [FromQuery]int? estado,
            [FromQuery]string? cheque,
            [FromQuery]int? tipoFecha,
            [FromQuery]int? chequeTransferencia,
            [FromQuery]int? checkSaldo,
            [FromQuery]int? situacion,
            [FromQuery]string? busqueda,
            [FromQuery]int? aplicado,
            [FromQuery]uint? moneda
        )
        {
            var res = await _bancoService.GetCuentasBancarias(fechaInicio, fechaFin, estado, cheque, tipoFecha, chequeTransferencia, checkSaldo, situacion, busqueda, aplicado, moneda);
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


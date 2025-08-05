using Microsoft.AspNetCore.Mvc;
using Api.Repositories.Interfaces;
using Api.Models.Dtos.Sucursal;
using Microsoft.AspNetCore.Authorization;
using Api.Models.Entities;
using Api.Models.ViewModels;
namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class BancosController : ControllerBase
    {
        private readonly IBancoRepository _bancoRepository;

        public BancosController(IBancoRepository bancoRepository)
        {
            _bancoRepository = bancoRepository;
        }

        [HttpGet("cuentas")]
        public async Task<ActionResult<IEnumerable<CuentaBancariaViewModel>>> GetCuentasBancarias(
            [FromQuery] int? estado,
            [FromQuery] uint? moneda
        )
        {
            var res = await _bancoRepository.ConsultarCuentasBancarias(estado, moneda);
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
            [FromQuery] int? guardarCobroTarjeta,
            [FromQuery] int? chequeTransferencia    
        )
        {
            var res = await _bancoRepository.ConsultaMovimientosBancarios(fechaInicio, fechaFin, estado, cheque, codigoCuenta, tipoFecha, guardarCobroTarjeta, chequeTransferencia);
            return Ok(res);
        }
    }
}


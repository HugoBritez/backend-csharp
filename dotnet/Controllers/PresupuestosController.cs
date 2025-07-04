using Microsoft.AspNetCore.Mvc;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Models.ViewModels;
using Api.Repositories.Interfaces;
using Api.Repositories.Implementations;
using System.Xml.Schema;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/presupuestos")]
    public class PresupuestosController : ControllerBase
    {
        private readonly IPresupuestosRepository _presupuestosRepository;

        private readonly IDetallePresupuestoRepository _detalleRepository;

        private readonly IPresupuestoService _presupuestoService;

        public PresupuestosController(IPresupuestosRepository presupuestosRepository, IDetallePresupuestoRepository detallesRepository, IPresupuestoService presupuestoService)
        {
            _presupuestosRepository = presupuestosRepository;
            _detalleRepository = detallesRepository;
            _presupuestoService = presupuestoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PresupuestoViewModel>>> GetPresupuesto(
            [FromQuery] string? fecha_desde,
            [FromQuery] string? fecha_hasta,
            [FromQuery] uint? sucursal,
            [FromQuery] uint? cliente,
            [FromQuery] uint? vendedor,
            [FromQuery] uint? articulo,
            [FromQuery] uint? moneda,
            [FromQuery] uint? estado,
            [FromQuery] string? busqueda
        )
        {
            var presupuestos = await _presupuestosRepository.GetTodos(
                fecha_desde,
                fecha_hasta,
                sucursal,
                cliente,
                vendedor,
                articulo,
                moneda,
                estado,
                busqueda
            );

            return Ok(presupuestos);
        }

        [HttpGet("detalles")]
        public async Task<ActionResult<IEnumerable<DetallePresupuestoViewModel>>> GetDetalles(
            [FromQuery] uint presupuestoId
        )
        {
            var detalles = await _detalleRepository.GetDetallePresupuesto(presupuestoId);
            return Ok(detalles);
        }

        [HttpGet("recuperar")]
        public async Task<ActionResult<RecuperarPresupuestoViewModel>> RecuperarPresupuesto(uint idPresupuesto)
        {
            var presupuestoRecuperado = await _presupuestoService.RecuperarPresupuesto(idPresupuesto);
            return Ok(presupuestoRecuperado);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseViewModel<Presupuesto>>> CrearPresupuesto(
            [FromBody] CrearPresupuestoDTO data
        )
        {
            var presupuestoCreado = await _presupuestoService.CrearPresupuesto(
                data.Presupuesto,
                data.Observacion,
                data.DetallePresupuesto
            );

            return Ok(presupuestoCreado);
        }

    }
}
using Microsoft.AspNetCore.Mvc;
using Api.Repositories.Interfaces;
using Api.Models.ViewModels;
using Api.Services.Interfaces;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/proveedores")]
    // [Authorize]
    public class ProveedoresController : ControllerBase
    {
        private readonly IProveedoresRepository _proveedoresRepository;
        private readonly IVentaService _ventaService;

        public ProveedoresController(IProveedoresRepository proveedoresRepository, IVentaService ventaService)
        {
            _proveedoresRepository = proveedoresRepository;
            _ventaService = ventaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProveedorViewModel>>> GetProveedores(
            [FromQuery] string? busqueda
        )
        {
            var proveedores = await _proveedoresRepository.GetProveedores(busqueda);
            return Ok(proveedores);
        }

        [HttpGet("reporte")]
        public async Task<ActionResult<ReporteVentaPorProveedorViewModel>> GetReporteProveedores(
            [FromQuery] string fechaDesde,
            [FromQuery] string fechaHasta,
            [FromQuery] uint? proveedor,
            [FromQuery] uint? cliente
        )
        {
            var reporte = await _ventaService.GetReporteVentasPorProveedor(fechaDesde, fechaHasta, proveedor, cliente);
            return Ok(reporte);
        }
    }
}

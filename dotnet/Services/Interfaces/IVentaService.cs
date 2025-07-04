using Api.Models.Dtos;
using Api.Models.Entities;

namespace Api.Services.Interfaces
{
    public interface IVentaService
    {
        Task<Venta> CrearVenta(VentaDTO venta, IEnumerable<DetalleVentaDTO> detalleVenta);

        Task<ReporteVentaAnual> GenerarReporte(ParametrosReporte parametros);


    }
}
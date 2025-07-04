using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Models.ViewModels;
using Azure;


namespace Api.Repositories.Interfaces
{
    public interface IVentaRepository
    {
        Task<Venta> CrearVenta(Venta venta);
        Task<IEnumerable<VentaViewModel>> ConsultaVentas(
            string? fecha_desde,
            string? fecha_hasta,
            uint? sucursal,
            uint? cliente,
            uint? vendedor,
            uint? articulo,
            uint? moneda,
            string? factura,
            uint? venta,
            uint? estadoVenta,
            uint? remisiones,
            bool? listaFacturasSinCDC,
            int? page = 1,
            int itemsPorPagina = 50
        );

        Task<IEnumerable<DetalleVentaViewModel>> ConsultaDetalles(uint ventaId);
        Task<Venta?> GetById(uint? id);

        Task<IEnumerable<Impresionventa>> GetImpresion(uint venta);

        Task<IEnumerable<DetalleVentaAnual>> ObtenerVentasAnuales(ParametrosReporte parametros);
        Task<decimal> ObtenerTotalNotasCredito(ParametrosReporte parametros);
        Task<decimal> ObtenerTotalNotasCreditoSinItems(ParametrosReporte parametros);
        Task<decimal> ObtenerTotalNotasDevolucion(ParametrosReporte parametros);
    }
}
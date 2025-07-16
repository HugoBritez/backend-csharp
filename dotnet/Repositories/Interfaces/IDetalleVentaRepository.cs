using Api.Models.Entities;
using Api.Models.ViewModels;

namespace Api.Repositories.Interfaces
{
    public interface IDetalleVentaRepository
    {
        Task<DetalleVenta> CrearDetalleVenta(DetalleVenta detalleVenta);

        Task<IEnumerable<DetalleVentaViewModel>> GetDetalleParaProveedor(uint proveedor, uint venta_id);
    }
}
using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface IDetalleComprasVencimientoRepository
    {
        Task<DetalleCompraVencimiento?> GetByDetalleCompra(uint detalleCompra);

        Task<DetalleCompraVencimiento> Update(DetalleCompraVencimiento detalleCompraVencimiento);
    }
}
using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface IDetalleVentaVencimientoRepository
    {
        Task<DetalleVentaVencimiento> CrearDetalleVencimiento(DetalleVentaVencimiento detalleVencimiento);
    }
}
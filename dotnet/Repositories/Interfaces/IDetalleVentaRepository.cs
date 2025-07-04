using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface IDetalleVentaRepository
    {
        Task<DetalleVenta> CrearDetalleVenta(DetalleVenta detalleVenta);
    }
}
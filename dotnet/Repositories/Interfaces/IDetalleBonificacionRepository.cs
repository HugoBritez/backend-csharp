using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface IDetalleBonificacionRepository
    {
        Task<DetalleVentaBonificacion> CrearDetalleBonificacion(DetalleVentaBonificacion detalleVentaBonificacion);
    }
}
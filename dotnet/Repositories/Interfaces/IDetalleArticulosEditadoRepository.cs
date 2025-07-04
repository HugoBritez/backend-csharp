using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface IDetalleArticulosEditadoRepository
    {
        Task<DetalleArticulosEditado> CrearDetalleArticulosEditado(DetalleArticulosEditado detalleArticulosEditado);
    }
}
using Api.Models.Entities;
using Api.Models.ViewModels;

namespace Api.Repositories.Interfaces
{
    public interface IDetallePresupuestoRepository
    {
        Task<DetallePresupuesto> GetById(uint detalleId);
        Task<IEnumerable<DetallePresupuesto>> GetByPresupuesto(uint pedidoId);
        Task<IEnumerable<DetallePresupuestoViewModel>> GetDetallePresupuesto(uint presupuestoId);
        Task<DetallePresupuesto> Crear(DetallePresupuesto detallePresupuesto);
        Task<DetallePresupuesto> Update(DetallePresupuesto detallePresupuesto);
        Task<bool> Delete(uint idPresupuesto);
    }
}
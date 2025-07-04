using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Models.ViewModels;

namespace Api.Services.Interfaces
{
    public interface IPresupuestoService
    {
        Task<ResponseViewModel<Presupuesto>> CrearPresupuesto(Presupuesto venta, PresupuestoObservacion observacion, IEnumerable<DetallePresupuesto> detalleVenta);
        Task<RecuperarPresupuestoViewModel> RecuperarPresupuesto(uint idPresupuesto);
    }
}
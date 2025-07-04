using Api.Models.Entities;
using Api.Models.ViewModels;
using Google.Protobuf.WellKnownTypes;

namespace Api.Repositories.Interfaces
{
    public interface IPresupuestoObservacionRepository
    {
        Task<PresupuestoObservacion> Crear(PresupuestoObservacion presupuesto);
        Task<PresupuestoObservacion> GetById(uint id);
        Task<PresupuestoObservacion> Update(PresupuestoObservacion observacion);
    }
}
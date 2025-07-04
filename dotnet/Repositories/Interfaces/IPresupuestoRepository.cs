using Api.Models.Entities;
using Api.Models.ViewModels;

namespace Api.Repositories.Interfaces
{
    public interface IPresupuestosRepository
    {
        Task<IEnumerable<PresupuestoViewModel>> GetTodos(
            string? fecha_desde,
            string? fecha_hasta,
            uint? sucursal,
            uint? cliente,
            uint? vendedor,
            uint? articulo,
            uint? moneda,
            uint? estado,
            string? busqueda
        );
        Task<Presupuesto> Crear(Presupuesto presupuesto);
        Task<Presupuesto> GetById(uint id);
        Task<Presupuesto> Update(Presupuesto presupuesto);
        Task<string> ProcesarPresupuesto(int idPresupuesto);
    }
}
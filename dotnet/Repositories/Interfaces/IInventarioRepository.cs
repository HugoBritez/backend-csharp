using Api.Models.Entities;
using Api.Models.ViewModels;

namespace Api.Repositories.Interfaces
{
    public interface IInventarioRepository
    {
        Task<InventarioAuxiliar> CrearInventario(InventarioAuxiliar inventario);
        Task<InventarioAuxiliar?> UpdateInventario(InventarioAuxiliar inventario);
        Task<InventarioAuxiliar?> GetById(uint id);
        Task<IEnumerable<InventarioViewModel>> ListarInventarios(
            uint? estado, uint? deposito, string? nro_inventario
        );
    }
}
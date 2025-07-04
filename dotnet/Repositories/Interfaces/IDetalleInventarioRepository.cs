using Api.Models.Entities;
using Api.Models.ViewModels;

namespace Api.Repositories.Interfaces
{
    public interface IDetalleInventarioRepository
    {
        Task<InventarioAuxiliarItems> InsertarItem(InventarioAuxiliarItems item);
        Task<IEnumerable<DetalleInventarioViewModel>> ListarItems(uint idInventario, string? busqueda);
        Task<InventarioAuxiliarItems?> GetById(uint id);
        Task<IEnumerable<InventarioAuxiliarItems>> GetByInventario(uint id_inventario);
        Task<InventarioAuxiliarItems?> EditarItem(InventarioAuxiliarItems item);
        Task<bool> BuscarLoteExistente(uint inventario_id, uint id_lote);
        Task<bool> BuscarLoteExistenteGeneral(uint id_lote);
    }
}
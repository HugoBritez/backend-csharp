using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Models.ViewModels;

namespace Api.Services.Interfaces
{
    public interface IInventarioService
    {
        Task<InventarioAuxiliar> AnularInventario(uint id);
        Task<InventarioAuxiliar> CerrarInventario(uint id, uint userId);
        Task<InventarioAuxiliar> AutorizarInventario(uint id);
        Task<InventarioAuxiliar> RevertirInventario(uint id);
        Task<InventarioAuxiliarItems> InsertarItems(InsertarItemsDTO item, uint idInventario);
        Task<IEnumerable<DetalleInventarioViewModel>> ListarItemsInventario(uint idInventario, int filtro, FiltroInventarioEnum tipo, int valor, string? busqueda, bool stock);
    }
}
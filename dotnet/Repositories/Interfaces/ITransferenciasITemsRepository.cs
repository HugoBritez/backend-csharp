using Api.Models.Dtos;
using Api.Models.Entities;

namespace Api.Repositories.Interfaces{
    public interface ITransferenciasItemsRepository
    {
        Task<TransferenciaItem?> GetById(uint id);
        Task<TransferenciaItem> Crear(TransferenciaItem trans);
        Task<TransferenciaItem> Update(TransferenciaItem detalle);
        Task<IEnumerable<TransferenciaItem>> GetByTrans(uint id); 
    }
}

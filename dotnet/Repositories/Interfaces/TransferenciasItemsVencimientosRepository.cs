using Api.Models.Dtos;
using Api.Models.Entities;

namespace Api.Repositories.Interfaces{
    public interface ITransferenciasItemsVencimientoRepository
    {
        Task<TransferenciaItemVencimiento> Crear(TransferenciaItemVencimiento trans);
    }
}

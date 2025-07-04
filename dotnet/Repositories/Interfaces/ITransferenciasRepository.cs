using Api.Models.Dtos;
using Api.Models.Entities;

namespace Api.Repositories.Interfaces{
    public interface ITransferenciasRepository
    {
        Task<Transferencia?> GetById(uint id);
        Task<Transferencia> Crear(Transferencia trans);
        Task<Transferencia> Update(Transferencia detalle);
    }
}

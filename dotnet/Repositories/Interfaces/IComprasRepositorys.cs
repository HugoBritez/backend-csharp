using Api.Models.Dtos;
using Api.Models.Entities;

namespace Api.Repositories.Interfaces{
    public interface IComprasRepository
    {
        Task<Compra?> GetById(uint id);
        Task<Compra> Update(Compra compra);
    }
}

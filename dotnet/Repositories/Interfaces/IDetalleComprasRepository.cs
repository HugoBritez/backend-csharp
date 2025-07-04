using Api.Models.Dtos;
using Api.Models.Entities;

namespace Api.Repositories.Interfaces{
    public interface IDetalleComprasRepository
    {
        Task<DetalleCompra?> GetById(uint id);
        Task<DetalleCompra> Update(DetalleCompra detalle);
        Task<IEnumerable<DetalleCompra?>> GetByCompra(uint IdCompra);
    }
}

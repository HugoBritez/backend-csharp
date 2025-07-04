using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface IListaPrecioRepository
    {
        Task<IEnumerable<ListaPrecio>> GetAll();

        Task<IEnumerable<ListaPrecio>> GetByCliente(uint clienteId);

        Task<ListaPrecio> InsertarPorCliente(ClientesListaDePrecio data);
    }
}
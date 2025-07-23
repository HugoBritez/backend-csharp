using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Models.ViewModels;

namespace Api.Repositories.Interfaces{
    public interface IClienteRepository
    {
        Task<IEnumerable<ClienteViewModel>> GetClientes(string? busqueda, uint? id, uint? interno, uint? vendedor, int? estado);

        Task<Cliente> CrearCliente(Cliente data);

        Task<Cliente> UpdateCliente(Cliente data);

        Task<IEnumerable<Cliente>> GetAll(string? Busqueda);

        Task<Cliente?> GetByRuc(string ruc);

        Task<Cliente?> GetById(uint id);

        Task<ClienteViewModel?> GetClientePorId(uint id);

        Task<decimal> GetDeudaCliente(uint cliente);

        Task<UltimoCobroClienteViewModel?> GetUltimoCobroCliente(uint cliente);

    }
}

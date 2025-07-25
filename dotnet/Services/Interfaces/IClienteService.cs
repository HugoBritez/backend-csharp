using Api.Models.ViewModels;

namespace Api.Services.Interfaces
{
    public interface IClienteService
    {
        Task<UltimoCobroClienteViewModel?> GetUltimoCobroCliente(string clienteRuc);

        Task<decimal> GetDeudaCliente(string clienteRuc);

        Task<ClienteViewModel?> GetClienteViewModelByRuc(string clienteRuc);
    }
}
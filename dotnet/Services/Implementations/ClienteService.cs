using Api.Models.ViewModels;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;

namespace Api.Services.Implementations
{
    public class ClienteService (IClienteRepository clienteRepository) : IClienteService
    {
        private readonly IClienteRepository _clienteRepository = clienteRepository;

        public async Task<UltimoCobroClienteViewModel?> GetUltimoCobroCliente(string clienteRuc)
        {
            var cliente = await _clienteRepository.GetByRuc(clienteRuc) ?? throw new Exception("Cliente no encontrado");
            var ultimoCobro = await _clienteRepository.GetUltimoCobroCliente(cliente.Codigo);
            return ultimoCobro;
        }

        public async Task<decimal> GetDeudaCliente(string clienteRuc)
        {
            var cliente = await _clienteRepository.GetByRuc(clienteRuc) ?? throw new Exception("Cliente no encontrado");
            var deuda = await _clienteRepository.GetDeudaCliente(cliente.Codigo);
            return deuda;
        }
    }
}
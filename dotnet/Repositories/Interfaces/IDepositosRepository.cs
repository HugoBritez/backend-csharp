using Api.Models.Dtos.Deposito;

namespace Api.Repositories.Interfaces
{
    public interface IDepositosRepository
    {
        Task<IEnumerable<DepositoDTO>> GetDepositos(
            uint? deposito = null,
            uint? usuario = null,
            string? descripcion = null
        );

    }
}
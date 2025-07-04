using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface IClienteGruposRepository
    {
        Task<GrupoCliente> CreateAsync(GrupoCliente grupoCliente);
        Task<IEnumerable<GrupoCliente>> GetAll();
    }
}
using Api.Models.Entities;

namespace Api.Repositories.Interfaces{
    public interface IConfiguracionRepository
    {
        Task<Configuracion?> GetById(uint id);
    }
}

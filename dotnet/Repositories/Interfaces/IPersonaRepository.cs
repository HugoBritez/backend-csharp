using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface IPersonaRepository
    {
        Task<Persona> CreateAsync(Persona persona);
        Task<Persona> UpdatePersona(Persona persona);
        Task<IEnumerable<Persona>> GetAll(string? Busqueda);
        Task<Persona?> GetByRuc(string ruc);

        Task<Persona?> GetById(uint id);

        Task<string?> GetUltimoCodigoInterno();
    }
}
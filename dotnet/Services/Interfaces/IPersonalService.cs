using Api.Models.Dtos;
using Api.Models.ViewModels;

namespace Api.Services.Interfaces
{
    public interface IPersonalService
    {
        Task<bool?> CrearPersona(CrearPersonaDTO data);
        Task<IEnumerable<PersonaViewModel>> GetPersonas(string? Busqueda, int Tipo);
        Task<CrearPersonaDTO?> GetPersonaByRuc(uint id, int Tipo);

        Task<ClienteViewModel?> GetClientePorDefecto();
    }
}
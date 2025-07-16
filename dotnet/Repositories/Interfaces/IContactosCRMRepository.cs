using Api.Models.Entities;
using Api.Models.ViewModels;

namespace Api.Repositories.Interfaces
{
    public interface IContactosCRMRepository
    {
        Task<ContactoCRM> CrearContacto(ContactoCRM contacto);

        Task<ContactoCRM> ActualizarContacto(ContactoCRM contacto);

        Task<ContactoCRM?> GetContactoById(uint id);

        Task<IEnumerable<ContactoCRM>> GetContactos();

        Task<IEnumerable<ContactoViewModel>> GetContactosCompletos();

        Task<ContactoViewModel?> GetContactoCompletoById(uint id);
    }
}
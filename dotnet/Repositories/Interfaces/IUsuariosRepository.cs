using Api.Models.Dtos;
using Api.Models.Entities;

namespace Api.Repositories.Interfaces{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<UsuarioViewModel>> GetUsuarios(string? busqueda, uint? id, uint? rol);

        Task<Operador> CrearOperador(Operador data);

        Task<Operador?> GetOperadorById(uint id);
    }
}

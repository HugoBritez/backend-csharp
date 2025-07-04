using Api.Models.Dtos;
using Api.Models.Entities;
using Mysqlx.Crud;

namespace Api.Repositories.Interfaces{
    public interface ILocalizacionesRepository
    {
        Task<Localizacion> Crear(Localizacion localizacion);
        Task<Localizacion?> GetById(uint id);
        Task<Localizacion?> GetByAgenda(uint idAgenda);
        Task<Localizacion> Update(Localizacion localizacion);
    }
}

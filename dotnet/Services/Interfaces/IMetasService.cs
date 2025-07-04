using Api.Models.Entities;

namespace Api.Services.Interfaces
{
    public interface IMetasService
    {
        Task<Dictionary<uint, decimal>> GetMetasPorArticulo(IEnumerable<uint> articulosIds, int anio, uint?  operadorId);
        Task<Dictionary<uint, decimal>> GetMetasGeneralPorArticulo(IEnumerable<uint> articulosIds, int anio);
    }
}
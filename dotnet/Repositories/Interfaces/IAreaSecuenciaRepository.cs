using Api.Models.Entities;
using Api.Models.Dtos.ArticuloLote;
namespace Api.Repositories.Interfaces
{
    public interface IAreaSecuenciaRepository
    {
        Task<uint> GetSiguienteArea(uint areaActual);
    }
}
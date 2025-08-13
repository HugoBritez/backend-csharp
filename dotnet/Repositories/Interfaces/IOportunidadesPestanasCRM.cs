using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
   public interface IOportunidadesPestanasCRMRepository
   {
      Task<IEnumerable<uint>> GetOportunidadesByPestana(uint pestana);
      Task<OportunidadPestanaCRM> CrearOportunidadPestana(OportunidadPestanaCRM oportunidadPestana);
      Task<bool> EliminarOportunidadPestana(uint oportunidad, uint pestana);
   }
}
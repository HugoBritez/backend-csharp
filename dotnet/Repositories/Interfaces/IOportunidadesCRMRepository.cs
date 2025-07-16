using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    public interface IOportunidadesCRMRepository
    {
        Task<OportunidadCRM> CrearOportunidad(OportunidadCRM oportunidad);

        Task<OportunidadCRM> ActualizarOportunidad(OportunidadCRM oportunidad);

        Task<OportunidadCRM?> GetOportunidadById(uint id);

        Task<IEnumerable<OportunidadCRM>> GetOportunidades();

        Task<IEnumerable<OportunidadCRM>> GetOportunidadesByCliente(uint cliente);

        Task<IEnumerable<OportunidadCRM>> GetOportunidadesByOperador(uint operador);
        
    }
}
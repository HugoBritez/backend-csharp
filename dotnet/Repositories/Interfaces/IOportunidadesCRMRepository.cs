using Api.Models.Entities;
using Api.Models.ViewModels;

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

        Task<OportunidadViewModel?> GetOportunidadCompletaById(uint id);

        Task<IEnumerable<OportunidadViewModel>> GetOportunidadesCompletas(DateTime? fechaInicio = null, DateTime? fechaFin = null);

        Task<IEnumerable<OportunidadViewModel>> GetOportunidadesCompletasByCliente(uint cliente);

        Task<IEnumerable<OportunidadViewModel>> GetOportunidadesCompletasByOperador(uint operador);

    }
}
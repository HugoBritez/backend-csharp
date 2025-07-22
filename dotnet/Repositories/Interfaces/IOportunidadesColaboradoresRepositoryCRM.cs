using Api.Models.Entities;
using Api.Models.ViewModels.CRM;

namespace Api.Repositories.Interfaces
{
    public interface IProyectosColaboradoresRepositoryCRM
    {
        Task<ProyectosColaboradoresCRM?> GetById(uint id);

        Task<IEnumerable<ProyectosColaboradoresCRM>> GetByOperador(uint operadorId);

        Task<IEnumerable<ColaboradoresViewModel>> GetByProyecto(uint proyectoId);

        Task<ProyectosColaboradoresCRM> Create(ProyectosColaboradoresCRM proyectoColaborador);

        Task<ProyectosColaboradoresCRM> Update(ProyectosColaboradoresCRM proyectoColaborador);
        
        Task<bool> SoftDeleteByProyecto(uint proyectoId);
        
        Task<bool> SoftDeleteByProyectoAndColaborador(uint proyectoId, uint colaboradorId);
        
        Task<bool> ActivarColaborador(uint proyectoId, uint colaboradorId);
    }
}
using Api.Audit.Models;

namespace Api.Audit.Services
{
    public interface IAuditoriaService
    {
        Task<Auditoria> RegistrarAuditoria(
            int entidad,
            int accion,
            int idReferencia,
            string usuario,
            int vendedor,
            string obs
        );
        
    }
}
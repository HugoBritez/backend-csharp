using Api.Models.Entities;
using Api.Models.ViewModels;

namespace Api.Repositories.Interfaces{
    public interface IAgendasRepository
    {
        Task<Agenda> Crear(Agenda agenda);
        Task<Agenda> Update(Agenda agenda);
        Task<Agenda?> GetById(uint id);
        Task<IEnumerable<AgendaViewModel>> GetTodas(
            string? fechaDesde = null,
            string? fechaHasta = null,
            string? cliente = null,
            string? vendedor = null,
            int visitado = -1,
            int estado = -1,
            int planificacion = -1,
            int notas = -1,
            string orden = "0");
    }
}

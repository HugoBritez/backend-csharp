using Api.Models.Dtos;
using Api.Models.Entities;

namespace Api.Services.Interfaces
{
    public interface IAgendasService
    {
        Task<Agenda> RegistrarLlegada(RegistrarLlegadaDTO data);
        Task<Agenda> RegistrarSalida(uint idAgenda);

        Task<Agenda> ReagendarVisita(ReagendarVisitaDTO data);
        Task<Agenda> AnularVisita(uint id_agenda);

    }
}
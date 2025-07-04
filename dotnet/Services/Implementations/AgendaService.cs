using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Api.Audit.Services;
using Api.Models.ViewModels;
using Api.Auth.Services;
using Api.Auth.Models;
using Api.Models.Dtos;
using Google.Protobuf.WellKnownTypes;
namespace Api.Services.Implementations
{
    public class AgendaService : IAgendasService
    {
        private readonly IAgendasRepository _agendasRepository;
        private readonly IAgendasNotasRepository _agendasNotasRepository;
        private readonly IAgendaSubvisitaRepository _agendasSubvisitaRepository;
        private readonly ILocalizacionesRepository _localizacionesRepository;

        public AgendaService(
            IAgendasRepository agendasRepository,
            IAgendasNotasRepository agendasNotasRepository,
            IAgendaSubvisitaRepository agendaSubvisitaRepository,
            ILocalizacionesRepository localizacionesRepository
        )
        {
            _agendasRepository = agendasRepository;
            _agendasNotasRepository = agendasNotasRepository;
            _agendasSubvisitaRepository = agendaSubvisitaRepository;
            _localizacionesRepository = localizacionesRepository;
        }

        public async Task<Agenda> RegistrarLlegada(RegistrarLlegadaDTO data)
        {
            var agendaData = await _agendasRepository.GetById(data.AgendaId) ?? throw new KeyNotFoundException($"No se encontró una agenda con el ID {data.AgendaId}.");
            agendaData.Longitud = data.Longitud;
            agendaData.Latitud = data.Latitud;
            await _agendasRepository.Update(agendaData);

            var localizacionACrear = new Localizacion
            {
                Id = 0,
                Agenda = data.AgendaId,
                Fecha = DateTime.Now,
                HoraInicio = DateTime.Now.ToString("HH:mm:ss"),
                HoraFin = null,
                Obs = "LLegada marcada",
                Cliente = agendaData.Cliente,
                Operador = agendaData.Operador,
                Longitud = data.Longitud,
                Latitud = data.Latitud,
                Acuraci = 1,
                Estado = 1
            };
            await _localizacionesRepository.Crear(localizacionACrear);

            return agendaData;
        }

        public async Task<Agenda> RegistrarSalida(uint idAgenda)
        {
            var agendaData = await _agendasRepository.GetById(idAgenda) ?? throw new KeyNotFoundException($"No se encontró una agenda con el ID {idAgenda}.");

            agendaData.Visitado = 1;

            await _agendasRepository.Update(agendaData);

            var localizacionData = await _localizacionesRepository.GetByAgenda(idAgenda) ?? throw new KeyNotFoundException($"No se encontró una localizacion con el ID de agenda {idAgenda}.");

            localizacionData.HoraFin = DateTime.Now.ToString("HH:mm:ss");
            localizacionData.Obs = "LLegada/Salida marcada";
            await _localizacionesRepository.Update(localizacionData);
            return agendaData;
        }

        public async Task<Agenda> ReagendarVisita(ReagendarVisitaDTO data)
        {
            var agendaData = await _agendasRepository.GetById(data.IdAgenda) ?? throw new KeyNotFoundException($"No se encontró una agenda con el ID {data.IdAgenda}.");

            agendaData.HoraProx = data.ProximaHora;
            agendaData.VisitadoProx = 1;
            agendaData.ProximaLlamada = data.ProximaFecha;
            await _agendasRepository.Update(agendaData);

            var nuevaAgenda = new Agenda
            {
                Id = 0,
                Fecha = data.ProximaFecha,
                Hora = data.ProximaHora,
                Dias = "",
                Cliente = agendaData.Cliente,
                Operador = agendaData.Operador,
                Vendedor = agendaData.Vendedor,
                Planificacion = agendaData.Planificacion,
                Prioridad = agendaData.Prioridad,
                Observacion = agendaData.Observacion,
                ProximaLlamada = DateTime.MinValue,
                HoraProx = "",
                ProximaActi = "",
                Visitado = 0,
                VisitadoProx = 0,
                Latitud = "",
                Longitud = ""
            };

            var nuevaAgendaCreada = await _agendasRepository.Crear(nuevaAgenda);

            return nuevaAgendaCreada;
        }

        public async Task<Agenda> AnularVisita(uint id_agenda)
        {
            var agendaAAnular = await _agendasRepository.GetById(id_agenda) ?? throw new KeyNotFoundException($"No se encontró una agenda con el ID {id_agenda}.");

            agendaAAnular.Estado = 0;
            await _agendasRepository.Update(agendaAAnular);
            return agendaAAnular;
        }
    }
}
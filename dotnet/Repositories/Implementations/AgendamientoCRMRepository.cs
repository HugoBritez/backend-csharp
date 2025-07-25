using Api.Data;
using Api.Models.Entities;
using Api.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class AgendamientoCRMRepository(ApplicationDbContext context) :  IAgendamientoCRMRepository
    {
        private readonly ApplicationDbContext _context = context;


        public async Task<IEnumerable<AgendamientoCRM>> GetAll()
        {
            return await _context.AgendamientosCRM.ToListAsync();
        }

        public async Task<IEnumerable<AgendamientosCRMViewModel>> GetAllComplete()
        {
            var query = from agendamiento in _context.AgendamientosCRM
                        join paciente in _context.Pacientes on agendamiento.Paciente equals paciente.Codigo
                        join doctor in _context.Doctores on agendamiento.Doctor equals doctor.Codigo
                        select new AgendamientosCRMViewModel
                        {
                            Codigo = agendamiento.Codigo,
                            FechaInicio = agendamiento.FechaInicio,
                            FechaAgendamiento = agendamiento.FechaAgendamiento,
                            HoraAgendamiento = agendamiento.HoraAgendamiento,
                            Titulo = agendamiento.Titulo,
                            Descripcion = agendamiento.Descripcion,
                            PacienteNombre = paciente.Nombres + " " + paciente.Apellidos,
                            DoctorNombre = doctor.Nombres + " " + doctor.Apellidos,
                            Paciente = agendamiento.Paciente,
                            Doctor = agendamiento.Doctor,
                            Cliente = agendamiento.Cliente,
                            Operador = agendamiento.Operador,
                            Estado = agendamiento.Estado,
                        };

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<AgendamientoCRM>> GetByOperador(uint operador)
        {
            return await _context.AgendamientosCRM.Where(ag => ag.Operador == operador).ToListAsync();
        }

        public async Task<IEnumerable<AgendamientoCRM>> GetByDoctor(uint doctor)
        {
            return await _context.AgendamientosCRM.Where(ag => ag.Doctor == doctor).ToListAsync();
        }

        public async Task<AgendamientoCRM?> GetById(uint id)
        {
            return await _context.AgendamientosCRM.FirstOrDefaultAsync(ag => ag.Codigo == id);
        }

        public async Task<AgendamientoCRM> Create(AgendamientoCRM agendamiento)
        {
            await _context.AgendamientosCRM.AddAsync(agendamiento);
            await _context.SaveChangesAsync();
            return agendamiento;
        }

        public async Task<AgendamientoCRM> Update(AgendamientoCRM agendamiento)
        {
            _context.AgendamientosCRM.Update(agendamiento);
            await _context.SaveChangesAsync();
            return agendamiento;
        }
    }

}
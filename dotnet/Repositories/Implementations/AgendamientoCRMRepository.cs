using Api.Data;
using Api.Models.Entities;
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
using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class PacientesRepository(ApplicationDbContext context) : IPacientesRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<Paciente>> GetAll()
        {
            return await _context.Pacientes.Where(p => p.Estado == 1).ToListAsync();
        }

        public async Task<Paciente?> GetById(uint id)
        {
            return await _context.Pacientes.FirstOrDefaultAsync(p => p.Codigo == id);
        }
    }
}
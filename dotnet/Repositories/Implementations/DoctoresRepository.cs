using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class DoctoresRepository(ApplicationDbContext context) : IDoctoresRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<Doctor>> GetAll()
        {
            return await _context.Doctores.Where(d => d.Estado == 1).ToListAsync();
        }

        public async Task<Doctor?> GetById(uint id)
        {
            return await _context.Doctores.FirstOrDefaultAsync(d => d.Codigo == id);
        }
    }
}
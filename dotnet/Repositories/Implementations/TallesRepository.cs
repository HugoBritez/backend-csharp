using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class TallesRepository : ITallesRepository
    {
        private readonly ApplicationDbContext _context;

        public TallesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Talle>> GetAll()
        {
            return await _context.Talles.ToListAsync();
        }

    }
}
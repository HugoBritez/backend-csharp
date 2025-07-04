using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class MonedaRepository : IMonedaRepository
    {
        private readonly ApplicationDbContext _context;

        public MonedaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Moneda>> GetAll()
        {
            var monedas = await _context.Moneda
                               .Where(m => m.MoEstado == 1)
                               .ToListAsync();

            return monedas;
        }
    }
}

using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class TipoPlazoRepository(ApplicationDbContext context) : ITipoPlazoRepository
    {
        public readonly ApplicationDbContext _context = context;


        public async Task<IEnumerable<TipoPlazo>> GetAll()
        {
            var plazo = await _context.TipoPlazos.ToListAsync();
            return plazo;
        }
    }
}
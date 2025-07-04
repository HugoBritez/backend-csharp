using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Api.Repositories.Implementations
{
    public class SububicacionRepository : ISububicacionRepository
    {
        private readonly ApplicationDbContext _context;

        public SububicacionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sububicacion>> GetSubUbicaciones(string? busqueda)
        {
            if (!string.IsNullOrEmpty(busqueda))
            {
                var Sububicaciones = await _context.SubUbicaciones
                    .Where(ca => ca.SDescripcion.ToLower()
                        .Contains(busqueda.ToLower()))
                    .ToListAsync();
                return Sububicaciones;
            }
            
            return await _context.SubUbicaciones.ToListAsync();
        }

        public async Task<Sububicacion?> GetById(uint id)
        {
            var sububi = await _context.SubUbicaciones.FirstOrDefaultAsync(ca => ca.SCodigo == id);
            return sububi;
        }
    }
}

using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Api.Repositories.Implementations
{
    public class MarcaRepository : IMarcaRepository
    {
        private readonly ApplicationDbContext _context;

        public MarcaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Marca>> GetMarcas(string? busqueda)
        {
            if (!string.IsNullOrEmpty(busqueda))
            {
                var marcas = await _context.Marcas
                    .Where(ca => ca.MaDescripcion.ToLower()
                        .Contains(busqueda.ToLower()))
                    .ToListAsync();
                return marcas;
            }
            
            return await _context.Marcas.ToListAsync();
        }

        public async Task<Marca?> GetById(uint id)
        {
            var marcas = await _context.Marcas.FirstOrDefaultAsync(ca => ca.MaCodigo == id);
            return marcas;
        }
    }
}

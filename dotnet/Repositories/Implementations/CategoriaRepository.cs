using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Api.Repositories.Implementations
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoriaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categoria>> GetCategorias(string? busqueda)
        {
            if (!string.IsNullOrEmpty(busqueda))
            {
                var categorias = await _context.Categorias
                    .Where(ca => ca.CaDescripcion.ToLower()
                        .Contains(busqueda.ToLower()))
                    .ToListAsync();
                return categorias;
            }
            
            return await _context.Categorias.ToListAsync();
        }

        public async Task<Categoria?> GetById(uint id)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(ca => ca.CaCodigo == id);
            return categoria;
        }
    }
}

using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class DepartamentoRepository : IDepartamentoRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartamentoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Departamento>> GetAll(string? busqueda)
        {
            var query = _context.Departamentos.AsQueryable();

            query = query.Where(dep => dep.Estado == 1);

            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                query = query.Where(dep => dep.Descripcion.ToLower().Contains(busqueda.ToLower()));

            }

            return await query.ToListAsync();
        }

    }
}
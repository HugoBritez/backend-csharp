using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Api.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class CiudadesRepository : DapperRepositoryBase, ICiudadesRepository
    {
        private readonly ApplicationDbContext _context;

        public CiudadesRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }
        public async Task<IEnumerable<Ciudad>> GetAll(string? Busqueda)
        {
            if (!string.IsNullOrWhiteSpace(Busqueda))
            {
                var ciudades = await _context.Ciudades.Where(ciu => ciu.Descripcion == Busqueda).ToListAsync();
                return ciudades;
            }
            else
            {
                var ciudades = await _context.Ciudades.Where(ciu=> ciu.Estado == 1).ToListAsync();
                return ciudades;
            }
        }
    }
}

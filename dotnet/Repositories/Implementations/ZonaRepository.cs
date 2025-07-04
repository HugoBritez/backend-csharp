using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Api.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class ZonaRepository : DapperRepositoryBase, IZonaRepository
    {
        private readonly ApplicationDbContext _context;

        public ZonaRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }
        public async Task<IEnumerable<Zona>> GetAll()
        {
            var zonas = await _context.Zonas.ToListAsync();
            return zonas;
        }
    }
}

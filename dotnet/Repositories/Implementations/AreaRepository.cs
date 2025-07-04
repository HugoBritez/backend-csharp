using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Api.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class AreaRepository : DapperRepositoryBase, IAreaRepository
    {
        private readonly ApplicationDbContext _context;

        public AreaRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }
        public async Task<IEnumerable<Area>> GetAll()
        {
            var areas = await _context.Areas.ToListAsync();
            return areas;
        }
    }
}

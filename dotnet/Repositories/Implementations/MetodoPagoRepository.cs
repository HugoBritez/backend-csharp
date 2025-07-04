using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Api.Repositories.Implementations
{
    public class MetodoPagoRepository : IMetodoPagoRepository
    {
        private readonly ApplicationDbContext _context;

        public MetodoPagoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MetodoPago>> GetAll()
        {
            return await _context.MetodosPago.ToListAsync();
        }
    }
}

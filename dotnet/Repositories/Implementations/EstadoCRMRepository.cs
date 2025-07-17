using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class EstadoCRMRepository : IEstadoCRMRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EstadoCRMRepository> _logger;

        public EstadoCRMRepository(ApplicationDbContext context, ILogger<EstadoCRMRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<EstadoCRM>> GetEstados()
        {
            return await _context.EstadoCRM.ToListAsync();
        }

        public async Task<EstadoCRM> UpdateDescripcion(uint codigo, string descripcion)
        {
            var estado = await _context.EstadoCRM.FindAsync(codigo);
            if (estado == null)
            {
                throw new Exception("Estado no encontrado");
            }
            estado.Descripcion = descripcion;
            await _context.SaveChangesAsync();
            return estado;
        }
    }
}
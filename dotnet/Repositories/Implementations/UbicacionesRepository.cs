using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Api.Repositories.Implementations
{
    public class UbicacionesRepository : IUbicacionesRepository
    {
        private readonly ApplicationDbContext _context;

        public UbicacionesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ubicacion>> GetUbicaciones(string? busqueda)
        {
            if (!string.IsNullOrEmpty(busqueda))
            {
                var ubicaciones = await _context.Ubicaciones
                    .Where(ca => ca.UbDescripcion.ToLower()
                        .Contains(busqueda.ToLower()))
                    .ToListAsync();
                return ubicaciones;
            }
            
            return await _context.Ubicaciones.ToListAsync();
        }

        public async Task<Ubicacion?> GetById(uint id)
        {
            var ubicacion = await _context.Ubicaciones.FirstOrDefaultAsync(ca => ca.UbCodigo == id);
            return ubicacion;
        }
    }
}

using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Api.Repositories.Implementations
{
    public class UnidadesMedidaRepository : IUnidadMedidaRepository
    {
        private readonly ApplicationDbContext _context;

        public UnidadesMedidaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UnidadMedida>> GetUnidadMedidas(string? busqueda)
        {
            if (!string.IsNullOrEmpty(busqueda))
            {
                var unidades = await _context.UnidadMedidas
                    .Where(um => um.Descripcion.ToLower()
                        .Contains(busqueda.ToLower()))
                    .ToListAsync();
                return unidades;
            }
            
            return await _context.UnidadMedidas.ToListAsync();
        }

        public async Task<UnidadMedida?> GetById(uint id)
        {
            var unidad = await _context.UnidadMedidas.FirstOrDefaultAsync(ca => ca.Codigo == id);
            return unidad;
        }
    }
}

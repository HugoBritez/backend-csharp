using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class OportunidadesCRMRepository(ApplicationDbContext context) : IOportunidadesCRMRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<OportunidadCRM> CrearOportunidad(OportunidadCRM oportunidad)
        {
            var oportunidadCreado = await _context.Oportunidades.AddAsync(oportunidad);
            await _context.SaveChangesAsync();
            return oportunidadCreado.Entity;
        }

        public async Task<OportunidadCRM> ActualizarOportunidad(OportunidadCRM oportunidad)
        {
            var oportunidadActualizada = _context.Oportunidades.Update(oportunidad);
            await _context.SaveChangesAsync();
            return oportunidadActualizada.Entity;
        }

        public async Task<OportunidadCRM?> GetOportunidadById(uint id)
        {
            return await _context.Oportunidades.FindAsync(id);
        }

        public async Task<IEnumerable<OportunidadCRM>> GetOportunidades()
        {
            return await _context.Oportunidades.ToListAsync();
        }

        public async Task<IEnumerable<OportunidadCRM>> GetOportunidadesByCliente(uint cliente)
        {
            return await _context.Oportunidades.Where(o => o.Cliente == cliente).ToListAsync();
        }

        public async Task<IEnumerable<OportunidadCRM>> GetOportunidadesByOperador(uint operador)
        {
            return await _context.Oportunidades.Where(o => o.Operador == operador).ToListAsync();
        }
    }
}
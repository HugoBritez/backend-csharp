using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class OportunidadPestanaRepository (ApplicationDbContext context) : IOportunidadesPestanasCRMRepository
    {
        private readonly ApplicationDbContext _context = context;


        public async Task<IEnumerable<uint>> GetOportunidadesByPestana(uint pestana)
        {
            var oportunidades = await _context.OportunidadesPestanasCRM.Where(op => op.Pestana == pestana).Select(op => op.Oportunidad).ToListAsync();
            return oportunidades;
        }


        public async Task<OportunidadPestanaCRM> CrearOportunidadPestana(OportunidadPestanaCRM data)
        {
            var oportunidadPestanaCreado = await _context.OportunidadesPestanasCRM.AddAsync(data);
            await _context.SaveChangesAsync();
            return oportunidadPestanaCreado.Entity;
        }

        public async Task<bool> EliminarOportunidadPestana(uint oportunidad, uint pestana)
        {
            var oportunidadPestana = await _context.OportunidadesPestanasCRM.FirstOrDefaultAsync(op => op.Oportunidad == oportunidad && op.Pestana == pestana);
            if (oportunidadPestana == null)
            {
                return false;
            }
            _context.OportunidadesPestanasCRM.Remove(oportunidadPestana);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
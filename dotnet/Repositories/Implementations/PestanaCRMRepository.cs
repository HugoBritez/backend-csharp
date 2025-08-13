using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class PestanaCRMRepository(ApplicationDbContext context) : IPestanaCRMRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<PestanaCRM>> ListarPestanas()
        {
            var pestanas = await _context.PestanasCRM.Where(p => p.Estado == 1).ToListAsync();
            return pestanas;
        }

        public async Task<PestanaCRM> CrearPestana(PestanaCRM datos)
        {
            var pestanaCreada = await _context.PestanasCRM.AddAsync(datos);
            await _context.SaveChangesAsync();

            return pestanaCreada.Entity;
        }

        public async Task<PestanaCRM> UpdatePestana(PestanaCRM datos)
        {
            _context.PestanasCRM.Update(datos);
            await _context.SaveChangesAsync();

            return datos;
        }
    }
}
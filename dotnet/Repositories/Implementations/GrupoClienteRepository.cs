using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class GrupoClienteRepository : IClienteGruposRepository
    {
        private readonly ApplicationDbContext _context;

        public GrupoClienteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GrupoCliente> CreateAsync(GrupoCliente grupoCliente)
        {
            if (grupoCliente == null)
            {
                throw new ArgumentNullException(nameof(grupoCliente), "El grupo de cliente no puede ser nulo.");
            }

            _context.GrupoClientes.Add(grupoCliente);
            await _context.SaveChangesAsync();
            return grupoCliente;
        }

        public async Task<IEnumerable<GrupoCliente>> GetAll()
        {
            return await _context.GrupoClientes.ToListAsync();
        }
    }
}
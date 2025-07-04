using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Base;
using Api.Repositories.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class AgendasNotasRepository : DapperRepositoryBase, IAgendasNotasRepository
    {
        private readonly ApplicationDbContext _context;

        public AgendasNotasRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }

        public async Task<AgendasNotas> Crear(AgendasNotas nota)
        {
            var notaCreada = await _context.AgendasNotas.AddAsync(nota);
            await _context.SaveChangesAsync();
            return notaCreada.Entity;
        }

        public async Task<AgendasNotas> Update(AgendasNotas nota)
        {
            _context.AgendasNotas.Update(nota);
            await _context.SaveChangesAsync();
            return nota;
        }

        public async Task<AgendasNotas?> GetById(uint id)
        {
            var nota = await _context.AgendasNotas.FirstOrDefaultAsync(nota => nota.Id == id) ?? throw new InvalidOperationException("Nota con ese id no encontrado");
            return nota;
        }

        public async Task<IEnumerable<AgendasNotas>> GetByAgenda(uint idAgenda)
        {
            var notas = await _context.AgendasNotas.Where(nota => nota.AgendaId == idAgenda).ToListAsync() ?? throw new InvalidOperationException("No existen notas en esta agenda");

            return notas;
        }


    }
}

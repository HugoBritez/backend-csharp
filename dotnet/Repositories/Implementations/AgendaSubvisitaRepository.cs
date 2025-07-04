using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Base;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Api.Repositories.Implementations
{
    public class AgendaSubvisitaRepository : DapperRepositoryBase, IAgendaSubvisitaRepository
    {
        private readonly ApplicationDbContext _context;

        public AgendaSubvisitaRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }

        public async Task<AgendaSubvisita> Crear(AgendaSubvisita subvisita)
        {
            var subVisita = await _context.AgendaSubvisitas.AddAsync(subvisita);
            await _context.SaveChangesAsync();
            return subVisita.Entity;
        }

        public async Task<AgendaSubvisita> Update(AgendaSubvisita sub)
        {
            _context.AgendaSubvisitas.Update(sub);
            await _context.SaveChangesAsync();
            return sub;
        }

        public async Task<AgendaSubvisita?> GetById(uint id)
        {
            var nota = await _context.AgendaSubvisitas.FirstOrDefaultAsync(nota => nota.Id == id) ?? throw new InvalidOperationException("Subvisita con ese id no encontrado");
            return nota;
        }

        public async Task<IEnumerable<AgendaSubvisita?>> GetByAgenda(uint idAgenda)
        {
            var notas = await _context.AgendaSubvisitas.Where(nota => nota.IdAgenda == idAgenda).ToListAsync() ?? throw new InvalidOperationException("No existen subvisitas en esta agenda");

            return notas;
        }
    }
}
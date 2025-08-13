using System.Runtime.CompilerServices;
using Api.Data;
using Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Signers;

namespace Api.Repositories.Interfaces
{
    public class TareaDinamicaCRMRepository(ApplicationDbContext context) : ITareaDinamicaRepository
    {
        private readonly ApplicationDbContext _context = context;


        public async Task<IEnumerable<TareaDinamicaCRM>> ListarTareasPorPestana(uint idPestana)
        {
            var tareas = await _context.TareasDinamicasCRM.Where(t => t.Pestana == idPestana && t.Estado == 1).ToListAsync();

            return tareas;
        }


        public async Task<IEnumerable<TareaDinamicaCRM>> GetAllTareas()
        {
            var tareas = await _context.TareasDinamicasCRM.Where( t => t.Estado == 1).ToListAsync();

            return tareas;
        }

        public async Task<TareaDinamicaCRM> CreateTareaDinamica(TareaDinamicaCRM tarea)
        {
            var tareaCreada = await _context.TareasDinamicasCRM.AddAsync(tarea);
            await _context.SaveChangesAsync();
            return tareaCreada.Entity;
        }

        public async Task<TareaDinamicaCRM> UpdateTarea(TareaDinamicaCRM tarea)
        {
            var tareaActualizada = _context.TareasDinamicasCRM.Update(tarea);
            await _context.SaveChangesAsync();
            return tareaActualizada.Entity;
        }
    }
}
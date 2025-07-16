using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class TareasCRMRepository(ApplicationDbContext context) : ITareasCRMRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<TareaCRM> CrearTarea(TareaCRM tarea)
        {
            var tareaCreado = await _context.Tareas.AddAsync(tarea);
            await _context.SaveChangesAsync();
            return tareaCreado.Entity;
        }

        public async Task<TareaCRM> ActualizarTarea(TareaCRM tarea)
        {
            var tareaActualizada = _context.Tareas.Update(tarea);
            await _context.SaveChangesAsync();
            return tareaActualizada.Entity;
        }

        public async Task<TareaCRM?> GetTareaById(uint id)
        {
            return await _context.Tareas.FindAsync(id);
        }

        public async Task<IEnumerable<TareaCRM>> GetTareas()
        {
            return await _context.Tareas.ToListAsync();
        }

        public async Task<IEnumerable<TareaCRM>> GetTareasByOportunidad(uint oportunidad)
        {
            return await _context.Tareas.Where(t => t.Oportunidad == oportunidad).ToListAsync();
        }

        public async Task<IEnumerable<TareaCRM>> GetTareasByContacto(uint contacto)
        {
            return await _context.Tareas
                .Where(t => _context.Oportunidades
                    .Where(o => o.Cliente == contacto)
                    .Select(o => o.Codigo)
                    .Contains(t.Oportunidad))
                .ToListAsync();
        }
    }
}
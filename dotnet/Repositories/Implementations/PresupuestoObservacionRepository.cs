using Api.Repositories.Base;
using Api.Repositories.Interfaces;
using Api.Data;
using Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class PresupuestoObservacionRepository : DapperRepositoryBase, IPresupuestoObservacionRepository
    {
        private readonly ApplicationDbContext _context;

        public PresupuestoObservacionRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }

        public async Task<PresupuestoObservacion> Crear(PresupuestoObservacion presupuesto)
        {
            var presupuestoObservacionCreado = await _context.PresupuestoObservacion.AddAsync(presupuesto);
            await _context.SaveChangesAsync();
            return presupuestoObservacionCreado.Entity;
        }

        public async Task<PresupuestoObservacion> GetById(uint id)
        {
            var presupuesto = await _context.PresupuestoObservacion.FirstOrDefaultAsync(
             preObs => preObs.Presupuesto == id
            );

            return presupuesto ?? new PresupuestoObservacion();
        }
        
        public async Task<PresupuestoObservacion> Update(PresupuestoObservacion observacion)
        {
            var observacionExistente = await _context.PresupuestoObservacion.FirstOrDefaultAsync(de => de.Presupuesto == observacion.Presupuesto);

            if (observacionExistente == null)
            {
                return new PresupuestoObservacion();
            }
            _context.Entry(observacionExistente).CurrentValues.SetValues(observacion);
            await _context.SaveChangesAsync();

            return observacionExistente;
        }
    }
}
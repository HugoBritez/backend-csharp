using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class MetaVentaArticulosRepository(ApplicationDbContext context) : IMetaVentaRepository
    {
        public readonly ApplicationDbContext _context = context;

        public async Task<MetaVentaArticulo> CrearMeta(MetaVentaArticulo data)
        {
            // Validación más estricta de meta existente
            var metaExistente = await _context.MetaVentaArticulos
                .FirstOrDefaultAsync(m =>
                    m.ArticuloId == data.ArticuloId &&
                    m.OperadorId == data.OperadorId &&
                    m.Periodo == data.Periodo);

            if (metaExistente != null)
            {
                // Actualizar la meta existente
                metaExistente.MetaAcordada = data.MetaAcordada;
                metaExistente.Estado = data.Estado;
                await _context.SaveChangesAsync();
                return metaExistente;
            }

            // Asegurarse de que el ID sea 0 para que la base de datos lo genere automáticamente
            data.Id = 0;

            try
            {
                // Crear nueva meta
                var metaCreada = await _context.MetaVentaArticulos.AddAsync(data);
                await _context.SaveChangesAsync();
                return metaCreada.Entity;
            }
            catch (DbUpdateException ex)
            {
                // Si ocurre un error de clave duplicada, intentamos obtener la meta existente
                if (ex.InnerException?.Message.Contains("Duplicate entry") == true)
                {
                    var metaDuplicada = await _context.MetaVentaArticulos
                        .FirstOrDefaultAsync(m =>
                            m.ArticuloId == data.ArticuloId &&
                            m.OperadorId == data.OperadorId &&
                            m.Periodo == data.Periodo);

                    if (metaDuplicada != null)
                    {
                        // Actualizar la meta existente
                        metaDuplicada.MetaAcordada = data.MetaAcordada;
                        metaDuplicada.Estado = data.Estado;
                        await _context.SaveChangesAsync();
                        return metaDuplicada;
                    }
                }
                throw;
            }
        }

        public async Task<MetaVentaArticulo?> GetMetaPorArticulo(uint ArticuloId)
        {
            var meta = await _context.MetaVentaArticulos.FirstOrDefaultAsync(meta => meta.ArticuloId == ArticuloId);
            return meta;
        }

        public async Task<IEnumerable<MetaVentaArticulo>> GetMetasEnPeriodo(int anio)
        {
            var metas = _context.MetaVentaArticulos
                .Where(meta => meta.Periodo == anio)
                .ToListAsync();

            return await metas;
        }
    }
}
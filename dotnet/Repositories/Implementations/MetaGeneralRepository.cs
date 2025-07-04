using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace Api.Repositories.Implementations
{
    public class MetaVentaGeneralRepository(ApplicationDbContext context, ILogger<MetaVentaGeneralRepository> logger) : IMetaGeneralRepository
    {
        public readonly ApplicationDbContext _context = context;
        private readonly ILogger<MetaVentaGeneralRepository> _logger = logger;
        public async Task<MetasVentaGeneral> CrearMetaGeneral(MetasVentaGeneral data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            try
            {
                // Buscar primero si existe
                var metaExistente = await _context.MetasVentaGeneral
                    .FirstOrDefaultAsync(m =>
                        m.ArCodigo == data.ArCodigo &&
                        m.Periodo == data.Periodo);

                if (metaExistente != null)
                {
                    // Actualizar existente
                    metaExistente.MetaGeneral = data.MetaGeneral;
                    metaExistente.Estado = data.Estado;
                    await _context.SaveChangesAsync();
                    return metaExistente;
                }
                else
                {
                    // Crear nuevo
                    data.Id = 0;
                    var metaCreada = await _context.MetasVentaGeneral.AddAsync(data);
                    await _context.SaveChangesAsync();
                    return metaCreada.Entity;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CrearMetaGeneral: {ex.Message}");
                throw;
            }
        }

        private static bool IsDuplicateKeyException(DbUpdateException ex)
        {
            // Verificar si es una excepción de MySQL
            if (ex.InnerException is MySqlException mySqlEx)
            {
                // Código de error para clave duplicada en MySQL
                return mySqlEx.Number == 1062;
            }

            // Verificación adicional por mensaje
            var message = ex.InnerException?.Message ?? ex.Message;
            return message.Contains("Duplicate entry") ||
                   message.Contains("idx_meta_venta_general_unique") ||
                   message.Contains("1062");
        }

        public async Task<MetasVentaGeneral?> GetMetaGeneralPorArticulo(uint IdArticulo)
        {
            return await _context.MetasVentaGeneral
                .FirstOrDefaultAsync(m => m.ArCodigo == IdArticulo);
        }

        public async Task<IEnumerable<MetasVentaGeneral>> GetMetasGeneralEnPeriodo(int anio)
        {
            return await _context.MetasVentaGeneral
                .Where(m => m.Periodo == anio)
                .ToListAsync();
        }
    }
}
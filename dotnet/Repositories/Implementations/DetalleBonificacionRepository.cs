using Dapper;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Api.Data;

using Api.Models.Entities;

using Api.Repositories.Interfaces;
using Api.Repositories.Base;

namespace Api.Repositories.Implementations
{
    public class DetalleBonificacionRepository : DapperRepositoryBase, IDetalleBonificacionRepository
    {

        private readonly ApplicationDbContext _context;
        public DetalleBonificacionRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }

        public async Task<DetalleVentaBonificacion> CrearDetalleBonificacion(DetalleVentaBonificacion detalleVentaBonificacion)
        {
            var detalleBonificacionCreado = await _context.DetalleVentaBonificacion.AddAsync(detalleVentaBonificacion);
            await _context.SaveChangesAsync();

            return detalleBonificacionCreado.Entity;
        }

    }
}
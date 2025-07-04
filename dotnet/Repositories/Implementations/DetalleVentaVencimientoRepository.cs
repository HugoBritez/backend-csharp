using Dapper;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Api.Data;


using Api.Models.Entities;

using Api.Repositories.Interfaces;
using Api.Repositories.Base;

namespace Api.Repositories.Implementations
{
    public class DetalleVencimientoRepository : DapperRepositoryBase, IDetalleVentaVencimientoRepository
    {

        private readonly ApplicationDbContext _context;
        public DetalleVencimientoRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }

        public async Task<DetalleVentaVencimiento> CrearDetalleVencimiento(DetalleVentaVencimiento detalleVentaVencimiento)
        {
            var detalleVencimientoCreado = await _context.DetalleVentaVencimiento.AddAsync(detalleVentaVencimiento);
            await _context.SaveChangesAsync();

            return detalleVencimientoCreado.Entity;
        }

    }
}
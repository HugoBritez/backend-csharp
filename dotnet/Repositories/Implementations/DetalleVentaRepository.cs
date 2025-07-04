using Dapper;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Api.Data;

using Api.Models.Entities;

using Api.Repositories.Interfaces;
using Api.Repositories.Base;

namespace Api.Repositories.Implementations
{
    public class DetalleVentaRepository : DapperRepositoryBase, IDetalleVentaRepository
    {

        private readonly ApplicationDbContext _context;
        public DetalleVentaRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }

        public async Task<DetalleVenta> CrearDetalleVenta(DetalleVenta detalleVenta)
        {
            var detalleVentaCreada = await _context.DetalleVenta.AddAsync(detalleVenta);
            await _context.SaveChangesAsync();

            return detalleVentaCreada.Entity;
        }

    }
}
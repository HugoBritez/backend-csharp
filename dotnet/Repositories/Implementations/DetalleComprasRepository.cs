using Api.Data;
using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Repositories.Base;
using Api.Repositories.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class DetalleComprasRepository : DapperRepositoryBase, IDetalleComprasRepository
    {
        private readonly ApplicationDbContext _context;

        public DetalleComprasRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }

        public async Task<DetalleCompra?> GetById(uint id)
        {
            var detalle = await _context.DetalleCompras.FirstOrDefaultAsync(det => det.Id == id);
            return detalle;
        }

        public async Task<IEnumerable<DetalleCompra?>> GetByCompra(uint id)
        {
            var detalles = await _context.DetalleCompras
            .Where(det => det.Compra == id)
            .ToListAsync();
            return detalles;
        }

        public async Task<DetalleCompra> Update(DetalleCompra detalle)
        {
            _context.DetalleCompras.Update(detalle);
            await _context.SaveChangesAsync();
            return detalle;
        }

       
    }
}
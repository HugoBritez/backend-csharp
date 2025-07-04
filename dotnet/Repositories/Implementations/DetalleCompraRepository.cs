using Api.Data;
using Api.Repositories.Interfaces;
using Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class DetalleCompraVencimientoRepository : IDetalleComprasVencimientoRepository
    {
        private readonly ApplicationDbContext _context;

        public DetalleCompraVencimientoRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<DetalleCompraVencimiento?> GetByDetalleCompra(uint detalleCompra)
        {
            var detalleCompraVencimiento = await _context.DetalleCompraVencimiento.FirstOrDefaultAsync(x => x.DetalleCompra == detalleCompra);
            return detalleCompraVencimiento;
        }


        public async Task<DetalleCompraVencimiento> Update(DetalleCompraVencimiento detalleCompraVencimiento)
        {
            _context.DetalleCompraVencimiento.Update(detalleCompraVencimiento);
            await _context.SaveChangesAsync();
            return detalleCompraVencimiento;
        }

    }
}
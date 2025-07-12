using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Base;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class DetallePedidoFaltanteRepository : DapperRepositoryBase, IDetallePedidoFaltanteRepository
    {
        private readonly ApplicationDbContext _context;

        public DetallePedidoFaltanteRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }

        public async Task<DetallePedidoFaltante> Crear(DetallePedidoFaltante detalle)
        {
            var detallePedidoCreado = await _context.DetallePedidoFaltante.AddAsync(detalle);
            await _context.SaveChangesAsync();

            return detallePedidoCreado.Entity;
        }

        public async Task<DetallePedidoFaltante?> GetById(uint id)
        {
            return await _context.DetallePedidoFaltante.FirstOrDefaultAsync(x => x.Codigo == id);
        }

        public async Task<DetallePedidoFaltante?> GetByPedido(uint pedido)
        {
            return await _context.DetallePedidoFaltante.FirstOrDefaultAsync(x => x.DetallePedido == pedido);
        }

        public async Task<DetallePedidoFaltante> Update(DetallePedidoFaltante detalle)
        {
            var detalleExistente = await _context.DetallePedidoFaltante.FindAsync(detalle.Codigo);
            if (detalleExistente == null)
            {
                throw new InvalidOperationException($"Detalle faltante no encontrado: {detalle.Codigo}");
            }
            
            detalleExistente.Cantidad = detalle.Cantidad;
            detalleExistente.Situacion = detalle.Situacion;
            detalleExistente.Observacion = detalle.Observacion;
            
            _context.Entry(detalleExistente).State = EntityState.Modified;
            
            await _context.SaveChangesAsync();
            
            return detalleExistente;
        }
    }
}
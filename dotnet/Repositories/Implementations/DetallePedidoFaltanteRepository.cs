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
    }
}
using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Api.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class PedidoEstadoRepository : DapperRepositoryBase, IPedidoEstadoRepository
    {
        private readonly ApplicationDbContext _context;

        public PedidoEstadoRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }

        public async Task<PedidosEstados> Crear(PedidosEstados pedidoEstado)
        {
            await _context.PedidoEstado.AddAsync(pedidoEstado);
            await _context.SaveChangesAsync();
            return pedidoEstado;
        }
        }
}

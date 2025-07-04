
using Api.Data;

using Api.Models.Entities;

using Api.Repositories.Interfaces;
using Api.Repositories.Base;


namespace Api.Repositories.Implementations
{
    public class DetalleArticuloEditadoRepository : DapperRepositoryBase, IDetalleArticulosEditadoRepository
    {

        private readonly ApplicationDbContext _context;
        public DetalleArticuloEditadoRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }

        public async Task<DetalleArticulosEditado> CrearDetalleArticulosEditado(DetalleArticulosEditado detalleArticulosEditado)
        {
            var detalleArticuloEditadoCreado = await _context.DetalleArticulosEditados.AddAsync(detalleArticulosEditado);
            await _context.SaveChangesAsync();

            return detalleArticuloEditadoCreado.Entity;
        }

    }
}
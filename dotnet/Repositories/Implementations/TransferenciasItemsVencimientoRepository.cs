using Api.Data;
using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Repositories.Base;
using Api.Repositories.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class TransferenciaItemsVencimientoRepository : DapperRepositoryBase, ITransferenciasItemsVencimientoRepository
    {
        private readonly ApplicationDbContext _context;

        public TransferenciaItemsVencimientoRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }
        public async Task<TransferenciaItemVencimiento> Crear(TransferenciaItemVencimiento item)
        {
            var itemCreado = await _context.TransferenciaItemsVencimiento.AddAsync(item);
            await _context.SaveChangesAsync();
            return itemCreado.Entity;
        }
    }
}
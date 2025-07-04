using Api.Data;
using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Repositories.Base;
using Api.Repositories.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class TransferenciasItemsRepository : DapperRepositoryBase, ITransferenciasItemsRepository
    {
        private readonly ApplicationDbContext _context;

        public TransferenciasItemsRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }


        public async Task<TransferenciaItem?> GetById(uint id)
        {
            var item = await _context.TransferenciaItems.FirstOrDefaultAsync(item => item.Id == id);
            return item;
        }

        public async Task<TransferenciaItem> Crear(TransferenciaItem item)
        {
            var itemCreado = await _context.TransferenciaItems.AddAsync(item);
            await _context.SaveChangesAsync();
            return itemCreado.Entity;
        }

        public async Task<TransferenciaItem> Update(TransferenciaItem item)
        {
            _context.TransferenciaItems.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<TransferenciaItem>> GetByTrans(uint id)
        {
            var items = await _context.TransferenciaItems.Where(item => item.Transferencia == id).ToListAsync();
            return items;
        }
    }
}
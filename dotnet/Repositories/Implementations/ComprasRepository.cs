using Api.Data;
using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Repositories.Base;
using Api.Repositories.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class ComprasRepository : DapperRepositoryBase, IComprasRepository
    {
        private readonly ApplicationDbContext _context;

        public ComprasRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }

        public async Task<Compra?> GetById(uint id)
        {
            var compra = await _context.Compras.FirstOrDefaultAsync(co => co.Codigo == id);
            return compra;
        }

        public async Task<Compra> Update(Compra compra)
        {
            _context.Compras.Update(compra);
            await _context.SaveChangesAsync();
            return compra;
        }
       
    }
}
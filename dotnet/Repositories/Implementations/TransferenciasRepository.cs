using Api.Data;
using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Repositories.Base;
using Api.Repositories.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class TransferenciasRepository : DapperRepositoryBase, ITransferenciasRepository
    {
        private readonly ApplicationDbContext _context;

        public TransferenciasRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }

        public async Task<Transferencia?> GetById(uint id)
        {
            var transferencia = await _context.Transferencias.FirstOrDefaultAsync(trans => trans.Id == id);
            return transferencia;
        }

        public async Task<Transferencia> Crear(Transferencia trans)
        {
            var entityEntry =await  _context.Transferencias.AddAsync(trans);
            await _context.SaveChangesAsync();
            return entityEntry.Entity;
        }

        public async Task<Transferencia> Update(Transferencia trans)
        {
            var existing = await _context.Transferencias.FindAsync(trans.Id);
            if (existing == null)
            {
            throw new KeyNotFoundException($"Transferencia con ese id no existe");
            }

            _context.Entry(existing).CurrentValues.SetValues(trans);
            await _context.SaveChangesAsync();
            return existing;
        }

       
    }
}
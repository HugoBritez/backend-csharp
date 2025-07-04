using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class ListaPrecioRepository : IListaPrecioRepository
    {
        private readonly ApplicationDbContext _context;

        public ListaPrecioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ListaPrecio>> GetAll()
        {
            var listaPrecio = await _context.ListaPrecio
                               .Where(lp => lp.LpEstado == 1)
                               .ToListAsync();

            return listaPrecio;
        }

        public async Task<IEnumerable<ListaPrecio>> GetByCliente(uint clienteId)
        {
            return await _context.ClientesListaDePrecios
                .Where(clp => clp.ClienteId == clienteId)
                .Include(clp => clp.ListaDePrecio)
                .Select(clp => clp.ListaDePrecio!)
                .ToListAsync();
        }

        public async Task<ListaPrecio> InsertarPorCliente(ClientesListaDePrecio data)
        {
            var newListaPrecioPorCliente = await _context.ClientesListaDePrecios.AddAsync(data);
            await _context.SaveChangesAsync();
            return newListaPrecioPorCliente.Entity.ListaDePrecio!;
        }
    }
}
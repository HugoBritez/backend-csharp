using Api.Data;
using Api.Models.Entities;
using Api.Models.ViewModels;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Api.Repositories.Implementations
{
    public class ProveedoresRepository : IProveedoresRepository
    {
        private readonly ApplicationDbContext _context;
        public ProveedoresRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProveedorViewModel>> GetProveedores(string? busqueda)
        {
        var query = _context.Proveedores
        .Include(p => p.Zona)
        .Select(p => new ProveedorViewModel
        {
            ProCodigo = p.Codigo,
            ProRazon = p.Razon,
            ProZona = p.Zona != null ? p.Zona.Descripcion : null
        });

            if (!string.IsNullOrEmpty(busqueda))
            {
                query = query.Where(p => p.ProRazon.Contains(busqueda));
            }

            return await query.Take(25).ToListAsync();
        }

        public async Task<Proveedor?> GetById(uint id)
        {
            var proveedor = await _context.Proveedores.FirstOrDefaultAsync(ca => ca.Codigo == id);
            return proveedor;
        }

        public async Task<Proveedor> CrearProveedor(Proveedor proveedor)
        {
            // Clear the navigation property to prevent EF from trying to create a new Zona
            proveedor.Zona = null;
            var proveedorCreado = await _context.Proveedores.AddAsync(proveedor);
            await _context.SaveChangesAsync();

            // Reload the proveedor with its zona information
            return await _context.Proveedores
                .Include(p => p.Zona)
                .FirstOrDefaultAsync(p => p.Codigo == proveedorCreado.Entity.Codigo)
                ?? throw new InvalidOperationException("Failed to create proveedor");
        }

        public async Task<IEnumerable<Proveedor>> GetAll(string? Busqueda)
        {
            var query = _context.Proveedores.AsQueryable();

            query = query.Where(pro => pro.Estado == 1);

            if (!string.IsNullOrWhiteSpace(Busqueda))
            {
                query = query.Where(pro => pro.NombreComun.ToLower().Contains(Busqueda.ToLower()) ||
                pro.Razon.ToLower().Contains(Busqueda.ToLower()));
            }

            return await query.Take(500).ToListAsync();
        }

        public async Task<Proveedor?> GetByRuc(string ruc)
        {
            var proveedor = await _context.Proveedores.FirstOrDefaultAsync(pro => pro.Ruc == ruc);
            return proveedor;
        }

        public async Task<Proveedor> UpdateProveedor(Proveedor data)
        {
            _context.Proveedores.Update(data);
            await _context.SaveChangesAsync();
            return data;
        }
    }
}

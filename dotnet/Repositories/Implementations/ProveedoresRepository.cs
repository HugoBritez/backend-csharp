
using Api.Models.Entities;
using Api.Models.ViewModels;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Dapper;
using Api.Data;
using Api.Repositories.Base;


namespace Api.Repositories.Implementations
{
    public class ProveedoresRepository : DapperRepositoryBase, IProveedoresRepository
    {
        private readonly ApplicationDbContext _context;
        public ProveedoresRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
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

        public async Task<IEnumerable<ReporteProveedores>> GetReporteProveedores(string? fechaDesde, string? fechaHasta, uint? proveedor)
        {
            try{
                using var connection = await GetConnectionAsync();
                var parameters = new DynamicParameters();
                parameters.Add("FechaDesde", fechaDesde);
                parameters.Add("FechaHasta", fechaHasta);
                parameters.Add("Proveedor", proveedor);

                var query = @"
                    SELECT
                        ar.ar_codigo as CodigoProducto,
                        ar.ar_descripcion as DescripcionProducto,
                        COALESCE(SUM(al.al_cantidad), 0) as TotalStock,
                        COALESCE(SUM(dv.deve_cantidad), 0) as TotalItems,
                        COALESCE(SUM(dv.deve_exentas + dv.deve_cinco + dv.deve_diez),0) as TotalImporte,
                        COALESCE(SUM(
                            CASE
                                WHEN v.ve_saldo = 0 THEN (dv.deve_exentas + dv.deve_cinco + dv.deve_diez)
                                ELSE 0
                            END
                        ), 0) as MontoCobrado,
                        (
                            SELECT SUM(dc.dc_cantidad)
                            FROM detalle_compras dc
                            INNER JOIN compras co ON dc.dc_compra = co.co_codigo
                            WHERE co.co_proveedor = @Proveedor
                            AND co_fecha BETWEEN @FechaDesde AND @FechaHasta
                            AND dc.dc_articulo = ar.ar_codigo
                            GROUP BY dc.dc_articulo
                         ) as TotalCompras
                    FROM articulos ar
                    INNER JOIN articulos_lotes al ON ar.ar_codigo = al.al_articulo
                    INNER JOIN articulos_proveedores ap ON ar.ar_codigo = ap.arprove_articulo
                    INNER JOIN proveedores p ON ap.arprove_prove = p.pro_codigo
                    LEFT JOIN detalle_ventas dv ON ar.ar_codigo = dv.deve_articulo
                    LEFT JOIN ventas v ON dv.deve_venta = v.ve_codigo
                    WHERE v.ve_estado = 1
                    AND p.pro_codigo = @Proveedor
                    AND v.ve_fecha BETWEEN @FechaDesde AND @FechaHasta
                    GROUP BY ar.ar_codigo, ar.ar_descripcion, ap.arprove_prove, p.pro_razon
                    ORDER BY ar.ar_codigo, ap.arprove_prove;
                ";

                var result = await connection.QueryAsync<ReporteProveedores>(query, parameters);
                return result;
            } catch (Exception ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
                throw;
            }
        }
    }
}

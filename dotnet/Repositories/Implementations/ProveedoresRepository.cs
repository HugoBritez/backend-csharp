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
                // todo: agregar filtrado por cliente


                var query = @"
                    SELECT
                        ar.ar_codigo as CodigoProducto,
                        ar.ar_descripcion as DescripcionProducto,
                        COALESCE(stock.TotalStock, 0) as TotalStock,
                        COALESCE(ventas.TotalItems, 0) as TotalItems,
                        COALESCE(ventas.TotalImporte, 0) as TotalImporte,
                        COALESCE(ventas.MontoCobrado, 0) as MontoCobrado,
                        COALESCE(compras.TotalCompras, 0) as TotalCompras
                    FROM articulos ar
                    INNER JOIN articulos_proveedores ap ON ar.ar_codigo = ap.arprove_articulo
                    INNER JOIN proveedores p ON ap.arprove_prove = p.pro_codigo
                    LEFT JOIN (
                        SELECT 
                            al.al_articulo,
                            SUM(al.al_cantidad) as TotalStock
                        FROM articulos_lotes al
                        GROUP BY al.al_articulo
                    ) stock ON ar.ar_codigo = stock.al_articulo
                    INNER JOIN (
                        SELECT
                            dv.deve_articulo,
                            SUM(dv.deve_cantidad) as TotalItems,
                            SUM(dv.deve_exentas + dv.deve_cinco + dv.deve_diez) as TotalImporte,
                            SUM(
                                CASE
                                    WHEN v.ve_saldo = 0 THEN (dv.deve_exentas + dv.deve_cinco + dv.deve_diez)
                                    ELSE 0
                                END
                            ) as MontoCobrado
                        FROM detalle_ventas dv
                        INNER JOIN ventas v ON dv.deve_venta = v.ve_codigo
                        WHERE v.ve_estado = 1
                        AND v.ve_fecha BETWEEN @FechaDesde AND @FechaHasta
                        GROUP BY dv.deve_articulo
                    ) ventas ON ar.ar_codigo = ventas.deve_articulo
                    LEFT JOIN (
                        SELECT 
                            dc.dc_articulo,
                            SUM(dc.dc_cantidad) as TotalCompras
                        FROM detalle_compras dc
                        INNER JOIN compras co ON dc.dc_compra = co.co_codigo
                        WHERE co.co_proveedor = @Proveedor
                        AND co.co_fecha BETWEEN @FechaDesde AND @FechaHasta
                        GROUP BY dc.dc_articulo
                    ) compras ON ar.ar_codigo = compras.dc_articulo
                    WHERE p.pro_codigo = @Proveedor
                    AND ventas.TotalItems > 0
                    ORDER BY ar.ar_codigo;
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

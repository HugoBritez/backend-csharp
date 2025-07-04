using Api.Data;
using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Models.ViewModels;
using Api.Repositories.Base;
using Api.Repositories.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class DetalleInventarioRepository : DapperRepositoryBase, IDetalleInventarioRepository
    {
        private readonly ApplicationDbContext _context;

        public DetalleInventarioRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }

        public async Task<InventarioAuxiliarItems> InsertarItem(InventarioAuxiliarItems item)
        {
            var itemInsertado = await _context.InventarioAuxiliarItems.AddAsync(item);
            await _context.SaveChangesAsync();
            return itemInsertado.Entity;
        }

        public async Task<InventarioAuxiliarItems?> EditarItem(InventarioAuxiliarItems item)
        {
            var itemAEditar = await _context.InventarioAuxiliarItems.FirstOrDefaultAsync(i => i.Id == item.Id);

            if (itemAEditar == null)
            {
                return null;
            }

            _context.Entry(itemAEditar).CurrentValues.SetValues(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task<InventarioAuxiliarItems?> GetById(uint id)
        {
            var item = await _context.InventarioAuxiliarItems.FirstOrDefaultAsync(i => i.Id == id);
            return item;
        }

        public async Task<IEnumerable<InventarioAuxiliarItems>> GetByInventario(uint id_inventario)
        {
            var items = await _context.InventarioAuxiliarItems.Where(item => item.IdInventario == id_inventario).ToListAsync();
            return items;
        }

        public async Task<IEnumerable<DetalleInventarioViewModel>> ListarItems(
            uint idInventario,
            string? busqueda
        )
        {
            using var connection = GetConnection();
            var parameters = new DynamicParameters();
            var where = "WHERE 1=1 ";

            // Add inventory ID parameter
            where += " AND inventario_auxiliar_items.id_inventario = @idInventario";
            parameters.Add("idInventario", idInventario);

            // Handle search with multiple words
            if (!string.IsNullOrEmpty(busqueda))
            {
                var palabras = busqueda.Split(' ').Where(p => !string.IsNullOrEmpty(p));
                if (palabras.Any())
                {
                    var condiciones = palabras.Select((palabra, index) =>
                    {
                        var paramName = $"busqueda{index}";
                        parameters.Add(paramName, $"%{palabra}%");
                        return $@"(ar.ar_descripcion LIKE @{paramName} 
                                 OR al.al_codbarra = @{paramName} 
                                 OR ar.ar_codbarra = @{paramName})";
                    });
                    where += $" AND ({string.Join(" AND ", condiciones)})";
                }
            }

            var query = $@"
                SELECT
                    id_articulo as articulo_id,
                    ar.ar_cod_interno as cod_interno,
                    id_lote as lote_id,
                    ar.ar_descripcion as descripcion,
                    DATE_FORMAT(fecha_vencimiento, '%d/%m/%Y') as vencimiento,
                    ar.ar_vencimiento as control_vencimiento,
                    ub.ub_descripcion as ubicacion,
                    s.s_descripcion as sub_ubicacion,
                    al.al_lote as lote,
                    al.al_codbarra as codigo_barra_lote,
                    ar.ar_codbarra as cod_barra_articulo,
                    CAST(inventario_auxiliar_items.cantidad_inicial AS SIGNED) as cantidad_inicial,
                    CAST(inventario_auxiliar_items.cantidad_scanner AS SIGNED) as cantidad_final,
                    CAST(al.al_cantidad AS SIGNED) as cantidad_actual,
                    CASE 
                        WHEN inventario_auxiliar_items.cantidad_scanner IS NOT NULL 
                        THEN CAST(inventario_auxiliar_items.cantidad_scanner AS SIGNED) 
                        ELSE CAST(inventario_auxiliar_items.cantidad_inicial AS SIGNED) 
                    END as stock
                FROM inventario_auxiliar_items
                INNER JOIN articulos ar ON inventario_auxiliar_items.id_articulo = ar.ar_codigo
                INNER JOIN articulos_lotes al ON inventario_auxiliar_items.id_lote = al.al_codigo
                INNER JOIN inventario_auxiliar ia ON inventario_auxiliar_items.id_inventario = ia.id
                INNER JOIN ubicaciones ub ON ar.ar_ubicacicion = ub.ub_codigo
                INNER JOIN sub_ubicacion s ON ar.ar_sububicacion = s.s_codigo
                {where}
                ORDER BY ar.ar_descripcion";

            return await connection.QueryAsync<DetalleInventarioViewModel>(query, parameters);
        }

        public async Task<bool> BuscarLoteExistente(uint inventario_id, uint id_lote)
        {
            using var connection = GetConnection();
            var parameters = new DynamicParameters();
            var query = @"
                SELECT 1 
                FROM inventario_auxiliar_items 
                WHERE id_lote = @idlote 
                AND id_inventario = @idinventario
                LIMIT 1";

            parameters.Add("idlote", id_lote);
            parameters.Add("idinventario", inventario_id);

            var res = await connection.QueryFirstOrDefaultAsync<int?>(query, parameters);

            return res.HasValue;
        }

        public async Task<bool> BuscarLoteExistenteGeneral(uint id_lote)
        {
            using var connection = GetConnection();
            var parameters = new DynamicParameters();
            var query = @"
                SELECT 1 
                FROM inventario_auxiliar_items items
                INNER JOIN inventarios i ON i.id = items.id_inventario
                WHERE items.id_lote = @idlote 
                AND i.estado = 0
                LIMIT 1";

            parameters.Add("idlote", id_lote);

            var res = await connection.QueryFirstOrDefaultAsync<int?>(query, parameters);

            return res.HasValue;
        }
    }
}
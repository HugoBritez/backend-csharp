using Dapper;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Api.Data;

using Api.Models.Entities;

using Api.Repositories.Interfaces;
using Api.Repositories.Base;
using Api.Models.ViewModels;

namespace Api.Repositories.Implementations
{
    public class DetalleVentaRepository : DapperRepositoryBase, IDetalleVentaRepository
    {

        private readonly ApplicationDbContext _context;
        public DetalleVentaRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }

        public async Task<DetalleVenta> CrearDetalleVenta(DetalleVenta detalleVenta)
        {
            var detalleVentaCreada = await _context.DetalleVenta.AddAsync(detalleVenta);
            await _context.SaveChangesAsync();

            return detalleVentaCreada.Entity;
        }

        public async Task<IEnumerable<DetalleVentaViewModel>> GetDetalleParaProveedor(uint proveedor, uint venta_id)
        {
            using var connection = GetConnection();
            var parameters = new DynamicParameters();
            var query =
            @$"
              SELECT
                    deve.deve_codigo AS det_codigo,
                    ar.ar_codigo AS art_codigo,
                    ar.ar_codbarra AS codbarra,
                    ar.ar_descripcion AS descripcion,
                    FORMAT(FLOOR(deve.deve_cantidad), 0, 'es_ES') AS cantidad,
                    FORMAT(FLOOR(deve.deve_precio), 0, 'es_ES') AS precio,
                    deve.deve_precio as precio_number,
                    FORMAT(FLOOR(deve.deve_descuento), 0, 'es_ES') AS descuento,
                    deve.deve_descuento as descuento_number,
                    FORMAT(FLOOR(deve.deve_exentas), 0, 'es_ES') AS exentas,
                    deve.deve_exentas as exentas_number,
                    FORMAT(FLOOR(deve.deve_cinco), 0, 'es_ES') AS cinco,
                    deve.deve_cinco as cinco_number,
                    FORMAT(FLOOR(deve.deve_diez), 0, 'es_ES') AS diez,
                    deve.deve_diez as diez_number,
                    al.al_lote AS lote,
                    DATE_FORMAT(al.al_vencimiento, '%Y-%m-%d') AS vencimiento,
                    m.m_largo AS largura,
                    m.m_altura AS altura,
                    m.m_mt2 AS mt2,
                    COALESCE(dae.a_descripcion, '') AS descripcion_editada,
                    ar.ar_kilos AS kilos,
                    um.um_cod_set as unidad_medida
                  FROM
                    detalle_ventas deve
                    LEFT JOIN articulos ar ON ar.ar_codigo = deve.deve_articulo
                    LEFT JOIN detalle_ventas_vencimiento dvv ON dvv.id_detalle_venta = deve.deve_codigo
                    LEFT JOIN articulos_lotes al ON al.al_codigo = dvv.loteid
                    LEFT JOIN detalle_articulo_mt2 m ON m.m_detalle_venta = deve.deve_codigo
                    LEFT JOIN detalle_articulos_editado dae ON deve.deve_codigo = dae.a_detalle_venta
                    LEFT JOIN unidadmedidas um ON um.um_codigo = ar.ar_unidadmedida
                    LEFT JOIN articulos_proveedores ap ON ap.arprove_articulo = ar.ar_codigo
                  WHERE
                    deve.deve_venta = @VentaId
                    AND ap.arprove_prove = @Proveedor
                  ORDER BY
                    deve.deve_codigo
            ";
            parameters.Add("@VentaId", venta_id);
            parameters.Add("@Proveedor", proveedor);

            return await connection.QueryAsync<DetalleVentaViewModel>(query, parameters);
        }

    }
}
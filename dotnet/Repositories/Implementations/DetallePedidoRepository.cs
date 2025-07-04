using Api.Data;
using Api.Models.Entities;
using Api.Repositories.Base;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Api.Models.ViewModels;
using Dapper;
using Org.BouncyCastle.Asn1.Cms;

namespace Api.Repositories.Implementations
{
  public class DetallePedidoRepository : DapperRepositoryBase, IDetallePedidoRepository
  {
    private readonly ApplicationDbContext _context;

    public DetallePedidoRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
    {
      _context = context;
    }

    public async Task<DetallePedido> Crear(DetallePedido detalle)
    {
      var detallePedidoCreado = await _context.DetallePedido.AddAsync(detalle);
      await _context.SaveChangesAsync();

      return detallePedidoCreado.Entity;
    }

    public async Task<IEnumerable<DetallePedido>> GetByPedido(uint id)
    {
      var detalles = await _context.DetallePedido.Where(det => det.Pedido == id).ToListAsync();
      return detalles;
    }

    public async Task<IEnumerable<PedidoDetalleViewModel>> GetDetallesPedido(Pedido pedido)
    {
      using var connection = GetConnection();
      var parameters = new DynamicParameters();

      var query = @$"
              SELECT
                  dp.dp_articulo AS codigo,
                  IF(dp.dp_descripcion_art = '', ar.ar_descripcion, dp.dp_descripcion_art) AS descripcion_articulo,
                  dp.dp_cantidad - IFNULL((
                    SELECT SUM(dv.deve_cantidad)
                    FROM detalle_ventas dv
                    INNER JOIN ventas v ON dv.deve_venta = v.ve_codigo
                    INNER JOIN detalle_ventas_vencimiento dvv ON dv.deve_codigo = dvv.id_detalle_venta
                    WHERE
                      v.ve_pedido = dp.dp_pedido
                      AND v.ve_estado = 1
                      AND dv.deve_articulo = dp.dp_articulo
                      AND dvv.loteid = dp.dp_codigolote
                  ), 0) AS cantidad_vendida,
                  IF(dp.dp_bonif = 0, 'V', 'B') AS bonificacion,
                  IFNULL(df.d_cantidad, 0) AS d_cantidad,
                  FORMAT(ROUND(dp.dp_precio, 0), 0) AS precio,
                  FORMAT(ROUND(IFNULL((
                    SELECT (dc.dc_precio + dc.dc_recargo)
                    FROM detalle_compras dc
                    INNER JOIN compras c ON dc.dc_compra = c.co_codigo
                    WHERE
                      dc.dc_articulo = dp.dp_articulo
                      AND c.co_moneda = @Moneda
                      AND c.co_estado = 1
                      AND c.co_fecha <= @Fecha
                    ORDER BY c.co_fecha DESC
                    LIMIT 1
                  ), ar.ar_pcg), 0), 0) AS ultimo_precio,
                  ROUND(IFNULL(((dp.dp_precio - IFNULL((
                    SELECT (dc.dc_precio + dc.dc_recargo)
                    FROM detalle_compras dc
                    INNER JOIN compras c ON dc.dc_compra = c.co_codigo
                    WHERE
                      dc.dc_articulo = dp.dp_articulo
                      AND c.co_moneda = @Moneda
                      AND c.co_estado = 1
                      AND c.co_fecha <= @Fecha
                    ORDER BY c.co_fecha DESC
                    LIMIT 1
                  ), ar.ar_pcg)) * 100) / IFNULL((
                    SELECT (dc.dc_precio + dc.dc_recargo)
                    FROM detalle_compras dc
                    INNER JOIN compras c ON dc.dc_compra = c.co_codigo
                    WHERE
                      dc.dc_articulo = dp.dp_articulo
                      AND c.co_moneda = @Moneda
                      AND c.co_estado = 1
                      AND c.co_fecha <= @Fecha
                    ORDER BY c.co_fecha DESC
                    LIMIT 1
                  ), ar.ar_pcg), 0), 2) AS porc_costo,
                  ROUND(IFNULL(((dp.dp_precio - IFNULL((
                    SELECT (dc.dc_precio + dc.dc_recargo)
                    FROM detalle_compras dc
                    INNER JOIN compras c ON dc.dc_compra = c.co_codigo
                    WHERE
                      dc.dc_articulo = dp.dp_articulo
                      AND c.co_moneda = @Moneda
                      AND c.co_estado = 1
                      AND c.co_fecha <= @Fecha
                    ORDER BY c.co_fecha DESC
                    LIMIT 1
                  ), ar.ar_pcg)) * 100) / dp.dp_precio, 0), 2) AS porcentaje,
                  FORMAT(ROUND(dp.dp_descuento, 0), 0) AS descuento,
                  FORMAT(ROUND(dp.dp_exentas - IFNULL((
                    SELECT SUM(dv.deve_exentas)
                    FROM detalle_ventas dv
                    INNER JOIN ventas v ON dv.deve_venta = v.ve_codigo
                    INNER JOIN detalle_ventas_vencimiento t ON dv.deve_codigo = t.id_detalle_venta
                    WHERE v.ve_pedido = dp.dp_pedido
                    AND v.ve_estado = 1
                    AND t.loteid = dp.dp_codigolote
                  ), 0), 0), 0) AS exentas,
                  FORMAT(ROUND(dp.dp_cinco - IFNULL((
                    SELECT SUM(dv.deve_cinco)
                    FROM detalle_ventas dv
                    INNER JOIN ventas v ON dv.deve_venta = v.ve_codigo
                    INNER JOIN detalle_ventas_vencimiento t ON dv.deve_codigo = t.id_detalle_venta
                    WHERE v.ve_pedido = dp.dp_pedido
                    AND v.ve_estado = 1
                    AND t.loteid = dp.dp_codigolote
                  ), 0), 0), 0) AS cinco,
                  FORMAT(ROUND(dp.dp_diez - IFNULL((
                    SELECT SUM(dv.deve_diez)
                    FROM detalle_ventas dv
                    INNER JOIN ventas v ON dv.deve_venta = v.ve_codigo
                    INNER JOIN detalle_ventas_vencimiento t ON dv.deve_codigo = t.id_detalle_venta
                    WHERE v.ve_pedido = dp.dp_pedido
                    AND v.ve_estado = 1
                    AND t.loteid = dp.dp_codigolote
                  ), 0), 0), 0) AS diez,
                  IFNULL(dp.dp_lote, '') AS dp_lote,
                  IF(dp.dp_vence = '0001-01-01', '', DATE_FORMAT(dp.dp_vence, '%d/%m/%Y')) AS vencimiento,
                  IF(dp.dp_porcomision = 0, dp.dp_porcomision, ar.ar_comision) AS comision,
                  dp.dp_actorizado AS actorizado,
                  IFNULL(dp.dp_obs, '') AS obs,
                  IFNULL(
                    IF(dp.dp_codigolote = 0,
                      (SELECT ad.artdep_cantidad
                       FROM articulos_depositos ad
                       WHERE ad.artdep_articulo = ar.ar_codigo
                       AND ad.artdep_deposito = @Deposito),
                      (SELECT al.al_cantidad
                       FROM articulos_lotes al
                       WHERE al.al_articulo = ar.ar_codigo
                       AND al.al_deposito = @Deposito
                       AND al.al_codigo = dp.dp_codigolote)
                    ), 0
                  ) AS cant_stock,
                  dp.dp_codigolote,
                  IFNULL((
                    SELECT SUM(g.dp_cantidad)
                    FROM detalle_pedido g
                    INNER JOIN pedidos s ON g.dp_pedido = s.p_codigo
                    WHERE
                      s.p_estado = 1
                      AND g.dp_articulo = dp.dp_articulo
                      AND g.dp_codigolote = dp.dp_codigolote
                  ), 0) AS cant_pendiente,
                  IF(
                    dp.dp_cantidad = (dp.dp_cantidad - IFNULL((
                      SELECT SUM(dv.deve_cantidad)
                      FROM detalle_ventas dv
                      INNER JOIN ventas v ON dv.deve_venta = v.ve_codigo
                      INNER JOIN detalle_ventas_vencimiento t ON dv.deve_codigo = t.id_detalle_venta
                      WHERE
                        v.ve_pedido = dp.dp_pedido AND
                        v.ve_estado = 1 AND
                        t.loteid = dp.dp_codigolote
                    ), 0)), 1, 0
                  ) AS cantidad_verificada
                FROM detalle_pedido dp
                INNER JOIN articulos ar ON dp.dp_articulo = ar.ar_codigo
                LEFT JOIN detalle_faltante df ON dp.dp_codigo = df.d_detalle_pedido
                INNER JOIN sub_ubicacion su ON ar.ar_sububicacion = su.s_codigo
                WHERE dp.dp_pedido = @Codigo
                AND dp.dp_cantidad > 0;
            ";

      parameters.Add("@Codigo", pedido.Codigo);
      parameters.Add("@Moneda", pedido.Moneda);
      parameters.Add("@Fecha", pedido.Fecha);
      parameters.Add("@Deposito", pedido.Deposito);
      return await connection.QueryAsync<PedidoDetalleViewModel>(query, parameters);
    }


  }
}
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
    private readonly IPedidosRepository _pedidosRepository;
    private readonly ApplicationDbContext _context;

    public DetallePedidoRepository(IConfiguration configuration, ApplicationDbContext context, IPedidosRepository pedidosRepository) : base(configuration)
    {
      _context = context;
      _pedidosRepository = pedidosRepository;
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

    public async Task<IEnumerable<PedidoDetalle>> GetDetallesPedido(uint codigo)
    {
      using var connection = GetConnection();
      var parameters = new DynamicParameters();
      parameters.Add("@Codigo", codigo);

      var query = @"
        SELECT
          dp.dp_codigo AS det_codigo,
          ar.ar_codigo AS art_codigo,
          ar.ar_codbarra AS codbarra,
          ar.ar_descripcion AS descripcion,
          ar.ar_pcg as costo,
          dp.dp_cantidad AS cantidad,
          dp.dp_precio AS precio,
          dp.dp_descuento AS descuento,
          dp.dp_exentas AS exentas,
          dp.dp_cinco AS cinco,
          dp.dp_diez AS diez,
          dp.dp_codigolote AS codlote,
          dp.dp_lote AS lote, 
          ar.ar_editar_desc,
          dp.dp_bonif as bonificacion,                
          '' AS largura,
          '' AS altura,
          '' AS mt2,
          (SELECT dc.dc_precio 
           FROM detalle_compras dc 
           INNER JOIN compras c ON dc.dc_compra = c.co_codigo
           WHERE dc.dc_articulo = dp.dp_articulo 
             AND c.co_fecha <= p.p_fecha
           ORDER BY c.co_fecha DESC 
           LIMIT 1) AS precio_compra,
          ROUND(
            ((dp.dp_precio - COALESCE((SELECT dc.dc_precio 
             FROM detalle_compras dc 
             INNER JOIN compras c ON dc.dc_compra = c.co_codigo
             WHERE dc.dc_articulo = dp.dp_articulo 
               AND c.co_fecha <= p.p_fecha
             ORDER BY c.co_fecha DESC 
             LIMIT 1), ar.ar_pcg)) * 100) / dp.dp_precio, 2
          ) AS porcentaje_utilidad
        FROM
          detalle_pedido dp
          INNER JOIN articulos ar ON ar.ar_codigo = dp.dp_articulo
          INNER JOIN pedidos p ON p.p_codigo = dp.dp_pedido
        WHERE
          dp.dp_pedido = @Codigo
          AND dp_cantidad > 0
        ORDER BY
          dp.dp_codigo";

      return await connection.QueryAsync<PedidoDetalle>(query, parameters);
    }

    public async Task<DetallePedido?> GetById(uint id)
    {
      return await _context.DetallePedido.Where(dp => dp.Codigo == id).FirstOrDefaultAsync();
    }

    public async Task<DetallePedido> Update(DetallePedido detalle)
    {
      Console.WriteLine($"Iniciando Update en repositorio - ID: {detalle.Codigo}, Lote: {detalle.Lote ?? "NULL"}, CodigoLote: {detalle.CodigoLote}");
      
      // Obtener la entidad existente del contexto
      var detalleExistente = await _context.DetallePedido.FindAsync(detalle.Codigo);
      if (detalleExistente == null)
      {
          Console.WriteLine($"Error: No se encontró el detalle pedido en el contexto con ID: {detalle.Codigo}");
          throw new InvalidOperationException($"Detalle pedido no encontrado: {detalle.Codigo}");
      }
      
      Console.WriteLine($"Detalle existente encontrado - Lote: {detalleExistente.Lote ?? "NULL"}, CodigoLote: {detalleExistente.CodigoLote}");
      
      // Actualizar solo los campos específicos
      detalleExistente.Lote = detalle.Lote;
      detalleExistente.CodigoLote = detalle.CodigoLote;
      
      Console.WriteLine($"Valores actualizados en contexto - Lote: {detalleExistente.Lote}, CodigoLote: {detalleExistente.CodigoLote}");
      
      // Marcar como modificado
      _context.Entry(detalleExistente).State = EntityState.Modified;
      
      // Guardar cambios
      var rowsAffected = await _context.SaveChangesAsync();
      Console.WriteLine($"SaveChanges completado - Filas afectadas: {rowsAffected}");
      
      return detalleExistente;
    }
  }
}
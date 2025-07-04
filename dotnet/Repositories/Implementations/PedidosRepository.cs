using Dapper;
using Api.Repositories.Base;
using Api.Repositories.Interfaces;
using Api.Data;
using Api.Models.Entities;
using Api.Models.ViewModels;

namespace Api.Repositories.Implementations
{
    public class PedidosRepository : DapperRepositoryBase, IPedidosRepository
    {
        private readonly ApplicationDbContext _context;
        public PedidosRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }

        public async Task<Pedido> GetById(uint codigo)
        {
            var pedido = await _context.Pedido.FindAsync(codigo);
            if (pedido != null)
            {
                return pedido;
            }
            else
            {
                return new Pedido { };
            }
        }

        public async Task<Pedido> CrearPedido(Pedido pedido)
        {
            var pedidoCreado = await _context.Pedido.AddAsync(pedido);
            await _context.SaveChangesAsync();

            return pedidoCreado.Entity;
        }

        public async Task<string> ProcesarPedido(int idPedido)
        {
            using var connection = GetConnection();


            var query = @"
                UPDATE pedidos
                SET p_estado = 2
                WHERE p_id = @idPedido
                ";

            var parameter = new DynamicParameters();
            parameter.Add("idPedido", idPedido);
            var result = await connection.ExecuteAsync(query, parameter);
            if (result > 0)
            {
                return "Pedido procesado correctamente";
            }
            else
            {
                return "Error al procesar el pedido";
            }
        }

        public async Task<IEnumerable<PedidoViewModel>> GetPedidos(
            string? fechaDesde,
            string? fechaHasta,
            string? nroPedido,
            int? articulo,
            string? clientes,
            string? vendedores,
            string? sucursales,
            string? estado,
            int? moneda,
            string? factura
        )
        {
            using var connection = GetConnection();
            var where = "1 = 1 ";

            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(nroPedido))
            {
                where += " AND p.p_codigo = @nroPedido";
                parameters.Add("nroPedido", nroPedido);
            }
            else
            {
                if (!string.IsNullOrEmpty(fechaDesde) && !string.IsNullOrEmpty(fechaHasta) && fechaDesde.Length != 0 && fechaHasta.Length != 0)
                {
                    where += " AND p.p_fecha BETWEEN @fechaDesde AND @fechaHasta";
                    parameters.Add("fechaDesde", $"{fechaDesde}");
                    parameters.Add("fechaHasta", $"{fechaHasta}");
                }
                else if (!string.IsNullOrEmpty(fechaDesde))
                {
                    where += " AND p.p_fecha >= @fechaDesde";
                    parameters.Add("fechaDesde", $"{fechaDesde}");
                }
                else if (!string.IsNullOrEmpty(fechaHasta))
                {
                    where += " AND p.p_fecha <= @fechaHasta";
                    parameters.Add("fechaHasta", $"{fechaHasta}");
                }

                if (articulo.HasValue)
                {
                    where += " AND p.p_codigo IN (SELECT z.dp_pedido FROM detalle_pedido z WHERE dp_articulo = @articulo)";
                    parameters.Add("articulo", articulo);
                }

                if (!string.IsNullOrEmpty(clientes))
                {
                    where += " AND p.p_cliente IN @clientes";
                    parameters.Add("clientes", clientes);
                }

                if (!string.IsNullOrEmpty(vendedores))
                {
                    where += " AND p.p_vendedor IN @vendedores";
                    parameters.Add("vendedores", vendedores);
                }

                if (!string.IsNullOrEmpty(sucursales))
                {
                    where += " AND p.p_sucursal IN @sucursales";
                    parameters.Add("sucursales", sucursales);
                }

                if (moneda.HasValue)
                {
                    where += " AND p.p_moneda = @moneda";
                    parameters.Add("moneda", moneda);
                }

                if (!string.IsNullOrEmpty(factura))
                {
                    where += " AND p.p_codigo IN (SELECT x.ve_pedido FROM ventas x WHERE ve_factura = @factura)";
                    parameters.Add("factura", factura);
                }

                if (estado == "1") where += " AND p.p_estado = 1";
                if (estado == "2") where += " AND p.p_estado = 2";
                if (estado == "3") where += " AND (p.p_estado = 1 OR p.p_estado = 2)";
            }
            var query = @"
                            SELECT
                                p.p_codigo AS pedido_id,
                                cli.cli_razon as cliente,
                                m.mo_descripcion as moneda,
                                p.p_fecha as fecha,
                                IFNULL(vp.ve_factura, '') as factura,
                                a.a_descripcion as area,
                                IFNULL((SELECT
                                    area.a_descripcion AS descripcion
                                    FROM area_secuencia arse
                                    INNER JOIN area area ON arse.ac_secuencia_area = area.a_codigo
                                    WHERE arse.ac_area = p.p_area
                                ), '-') as siguiente_area,
                                IF(p.p_estado = 1, 'Pendiente', IF(p.p_estado = 2, 'Facturado', 'Todos')) as estado,
                                p.p_estado as estado_num,
                                IF(p.p_credito = 1, 'Crédito', 'Contado') as condicion,
                                op.op_nombre as operador,
                                ope.op_nombre as vendedor,
                                dep.dep_descripcion as deposito,
                                p.p_cantcuotas,
                                p.p_entrega,
                                p.p_autorizar_a_contado,
                                p.p_imprimir as imprimir,
                                p.p_imprimir_preparacion as imprimir_preparacion,
                                p.p_cliente as cliente_id,
                                p.p_cantidad_cajas as cantidad_cajas,
                                IFNULL(p.p_obs, '') as obs,
                                FORMAT(ROUND(SUM(dp.dp_cantidad  * (dp.dp_precio - dp.dp_descuento)), 0), 0) as total,
                                IF(p.p_acuerdo = 1, 'Tiene acuerdo comercial', '') as acuerdo
                            FROM pedidos p
                            LEFT JOIN clientes cli ON p.p_cliente = cli.cli_codigo
                            LEFT JOIN monedas m ON p.p_moneda = m.mo_codigo
                            LEFT JOIN operadores op ON p.p_operador = op.op_codigo
                            LEFT JOIN operadores ope ON p.p_vendedor = ope.op_codigo
                            LEFT JOIN area a ON p.p_area = a.a_codigo
                            LEFT JOIN depositos dep ON p.p_deposito = dep.dep_codigo
                            LEFT JOIN ventas vp ON p.p_codigo = vp.ve_pedido
                            LEFT JOIN detalle_pedido dp ON p.p_codigo = dp.dp_pedido
                            WHERE " + where + @"
                            GROUP BY p.p_codigo
                            ORDER BY p.p_cliente DESC";


            return await connection.QueryAsync<PedidoViewModel>(query, parameters);
        }

        public async Task<IEnumerable<ReportePedidosFacturadosViewModel>> ReportePedidosFacturados(
            DateTime? fechaDesde,
            DateTime? fechaHasta,
            uint? articulo,
            uint? vendedor,
            uint? cliente,
            uint? sucursal
        )
        {
            using var connection = GetConnection();
            var where = "1 = 1 ";

            var parameters = new DynamicParameters();

            if (fechaDesde.HasValue)
            {
                where += " AND p.p_fecha >= @fechaDesde";
                parameters.Add("fechaDesde", fechaDesde.Value);
            }
            if (fechaHasta.HasValue)
            {
                where += " AND p.p_fecha <= @fechaHasta";
                parameters.Add("fechaHasta", fechaHasta.Value);
            }

            if (articulo.HasValue)
            {
                where += " AND dp.dp_articulo = @articulo";
                parameters.Add("articulo", articulo.Value);
            }

            if (vendedor.HasValue)
            {
                where += " AND p.p_vendedor = @vendedor";
                parameters.Add("vendedor", vendedor.Value);
            }

            if (cliente.HasValue)
            {
                where += " AND p.p_cliente = @cliente";
                parameters.Add("cliente", cliente.Value);
            }

            if (sucursal.HasValue)
            {
                where += " AND p.p_sucursal = @sucursal";
                parameters.Add("sucursal", sucursal.Value);
            }

            var query = @"
              SELECT
                  op.op_nombre as vendedor,
                  cli.cli_codigo as codCliente,
                  cli.cli_razon as nombreCliente,
                  ve.ve_pedido as nroPedido,
                  DATE_FORMAT(ve.ve_fecha, '%Y-%m-%d') as fechaPedido,
                  ar.ar_codigo as codProducto,
                  ar.ar_descripcion as producto,
                  ma.ma_descripcion as marca,
                  dp.dp_cantidad as cantidadPedido,
                  dv.deve_cantidad as cantidadFacturada,
                  (dp.dp_cantidad - dv.deve_cantidad) as cantidadFaltante,
                  (dp.dp_cantidad * ar.ar_pvg) as totalPedido,
                  (dv.deve_cantidad * ar.ar_pvg) as totalVenta,
                  (dp.dp_cantidad * ar.ar_pvg -  dv.deve_cantidad * ar.ar_pvg) as diferenciaTotal
                FROM detalle_ventas dv
                INNER JOIN ventas ve ON dv.deve_venta = ve.ve_codigo
                INNER JOIN clientes cli ON ve.ve_cliente = cli.cli_codigo
                INNER JOIN operadores op ON ve.ve_vendedor = op.op_codigo
                INNER JOIN articulos ar ON dv.deve_articulo = ar.ar_codigo
                INNER JOIN pedidos p ON ve.ve_pedido = p.p_codigo
                INNER JOIN detalle_pedido dp ON p.p_codigo = dp.dp_pedido AND dv.deve_articulo = dp.dp_articulo
                LEFT JOIN marcas ma ON ar.ar_marca = ma.ma_codigo
                WHERE ve.ve_pedido != 0 AND " + where + @"
                ORDER BY dv.deve_codigo DESC                
            ";

            return await connection.QueryAsync<ReportePedidosFacturadosViewModel>(query, parameters);
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

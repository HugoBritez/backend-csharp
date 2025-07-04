using Api.Data;
using Api.Models.Dtos;
using Api.Models.ViewModels;
using Api.Repositories.Base;
using Api.Repositories.Interfaces;
using Dapper;

namespace Api.Repositories.Implementations
{
    public class ControlIngresoRepository : DapperRepositoryBase, IControlIngresoRepository
    {
        private readonly ApplicationDbContext _context;

        public ControlIngresoRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }

        public async Task<IEnumerable<CompraConsultaViewModel>> GetFacturas(
            uint? deposito,
            uint? proveedor,
            DateTime? fechadesde,
            DateTime? fechahasta,
            string? factura,
            uint? verificado
        )
        {
            using var connection = GetConnection();
            var parameters = new DynamicParameters();
            var where = "WHERE 1=1 ";

            if (!string.IsNullOrEmpty(factura))
            {
                where += " AND co.co_factura = @factura";
                parameters.Add("factura", factura);
            }
            else
            {
                if (proveedor.HasValue && proveedor != 0)
                {
                    where += " AND oc.proveedor = @proveedor";
                    parameters.Add("proveedor", proveedor);
                }

                if (fechadesde.HasValue && fechadesde.HasValue)
                {
                    where += " AND oc.fecha BETWEEN @fechadesde AND @fechahasta";
                    parameters.Add("fechadesde", fechadesde);
                    parameters.Add("fechahasta", fechahasta);
                }

                if (fechadesde.HasValue && !fechahasta.HasValue)
                {
                    where += " AND oc.fecha >= @fechadesde";
                    parameters.Add("fechadesde", fechadesde);
                }
                if (!fechadesde.HasValue && fechahasta.HasValue)
                {
                    where += " AND oc.fecha <= @fechahasta";
                    parameters.Add("fechahasta", fechahasta);
                }
                if (deposito.HasValue)
                {
                    where += " AND co.co_deposito = @deposito";
                    parameters.Add("deposito", deposito);
                }
                if (verificado.HasValue && verificado != -1)
                {
                    where += " AND co.co_verificado = @verificado";
                    parameters.Add("verificado", verificado);
                }
            }

            var query =
            @$"
                SELECT
                  co.co_codigo as id_compra,
                  date_format(co.co_fecha, '%d/%m/%Y') as fecha_compra,
                  co.co_deposito as deposito,
                  dep.dep_descripcion as deposito_descripcion,
                  co.co_factura as nro_factura,
                  oc.id as id_orden,
                  oc.proveedor as nro_proveedor,
                  pr.pro_razon as proveedor,
                  oc.proveedor as proveedor_id,
                  co.co_verificado as verificado,
                  co.co_responsable_ubicacion as responsable_ubicacion,
                  case
                    when co.co_verificado = 0 then 'SIN VERIFICAR'
                    when co.co_verificado = 1 then 'VERIFICADO'
                    when co.co_verificado = 2 then 'CONFIRMADO'
                  end as estado
                FROM ordenes_compra oc 
                INNER JOIN compras co ON oc.id = co.co_orden
                INNER JOIN depositos dep ON co.co_deposito = dep.dep_codigo
                INNER JOIN proveedores pr ON oc.proveedor = pr.pro_codigo
                WHERE co.co_estado = 1
                {where}
                GROUP BY co.co_codigo, co.co_fecha, co.co_deposito, co.co_factura
                ORDER BY co.co_fecha DESC
            ";
            return await connection.QueryAsync<CompraConsultaViewModel>(query, parameters);
        }

        public async Task<IEnumerable<DetalleCompraConsultaViewModel>> GetItems(uint idCompra, string? busqueda, bool aVerificar)
        {
            using var connection = GetConnection();
            var parameters = new DynamicParameters();
            var where = "";

            if (!string.IsNullOrEmpty(busqueda))
            {
                where += " AND ar.ar_descripcion LIKE @busqueda";
                parameters.Add("busqueda", busqueda);
            }

            if (aVerificar)
            {
                where += " AND dc.dc_cantidad_verificada = 0";
            }

            var query = @$"
                SELECT
                  dc.dc_id as detalle_compra,
                  dc.dc_articulo as articulo_id,
                  ar.ar_descripcion as articulo_descripcion,
                  ar.ar_codbarra as articulo_codigo_barras,
                  FLOOR(dc.dc_cantidad) as cantidad,
                  dc.dc_cantidad_verificada as cantidad_verificada,
                  dc.dc_lote as lote,
                  date_format(dc.dc_vence, '%d/%m/%Y') as vencimiento,
                  op.op_nombre as responsable
                FROM detalle_compras dc
                INNER JOIN articulos ar ON dc.dc_articulo = ar.ar_codigo
                INNER JOIN compras co ON dc.dc_compra = co.co_codigo
                LEFT JOIN operadores op ON co.co_responsable_ubicacion = op.op_codigo
                WHERE dc.dc_compra = {idCompra}
                {where}
            ";
            return await connection.QueryAsync<DetalleCompraConsultaViewModel>(query, parameters);
        }

        public async Task<IEnumerable<ReporteIngresosViewModel>> Reporte(
            uint? deposito,
            uint? proveedor,
            DateTime? fechadesde,
            DateTime? fechahasta,
            string? factura,
            uint? verificado
        )
        {
            using   var connection = GetConnection();
            var parameters = new DynamicParameters();
            var where = "";

            if (!string.IsNullOrEmpty(factura))
            {
                where += " AND co.co_factura = @factura";
                parameters.Add("factura", factura);
            }
            else
            {
                if (proveedor.HasValue && proveedor != 0)
                {
                    where += " AND oc.proveedor = @proveedor";
                    parameters.Add("proveedor", proveedor);
                }

                if (fechadesde.HasValue && fechadesde.HasValue)
                {
                    where += " AND oc.fecha BETWEEN @fechadesde AND @fechahasta";
                    parameters.Add("fechadesde", fechadesde);
                    parameters.Add("fechahasta", fechahasta);
                }

                if (fechadesde.HasValue && !fechahasta.HasValue)
                {
                    where += " AND oc.fecha >= @fechadesde";
                    parameters.Add("fechadesde", fechadesde);
                }
                if (!fechadesde.HasValue && fechahasta.HasValue)
                {
                    where += " AND oc.fecha <= @fechahasta";
                    parameters.Add("fechahasta", fechahasta);
                }
                if (deposito.HasValue)
                {
                    where += " AND co.co_deposito = @deposito";
                    parameters.Add("deposito", deposito);
                }
                if (verificado.HasValue && verificado != -1)
                {
                    where += " AND co.co_verificado = @verificado";
                    parameters.Add("verificado", verificado);
                }
            }

            var query = @$"
              SELECT
                co.co_codigo as id_compra,
                date_format(co.co_fecha, '%d/%m/%Y') as fecha_compra,
                co.co_deposito as deposito,
                dep.dep_descripcion as deposito_descripcion,
                co.co_factura as nro_factura,
                oc.id as id_orden,
                oc.proveedor as nro_proveedor,
                pr.pro_razon as proveedor,
                oc.proveedor as proveedor_id,
                co.co_verificado as verificado,
                op.op_nombre as responsable_ubicacion,
                op2.op_nombre as responsable_verificacion,
                op3.op_nombre as responsable_confirmacion,
                case
                  when co.co_verificado = 0 then 'SIN VERIFICAR'
                  when co.co_verificado = 1 then 'VERIFICADO'
                  when co.co_verificado = 2 then 'CONFIRMADO'
                end as estado,
                (
                  JSON_ARRAYAGG(
                    JSON_OBJECT(
                      'detalle_compra', dc.dc_id,
                      'articulo_id', dc.dc_articulo,
                      'articulo_descripcion', ar.ar_descripcion,
                      'articulo_codigo_barras', ar.ar_codbarra,
                      'cantidad', dc.dc_cantidad,
                      'cantidad_verificada', dc.dc_cantidad_verificada,
                      'lote', dc.dc_lote,
                      'vencimiento', date_format(al.al_vencimiento, '%d/%m/%Y')
                    )
                  )
                ) as items
              FROM ordenes_compra oc 
              INNER JOIN compras co ON oc.id = co.co_orden
              INNER JOIN depositos dep ON co.co_deposito = dep.dep_codigo
              INNER JOIN proveedores pr ON oc.proveedor = pr.pro_codigo
              INNER JOIN detalle_compras dc ON co.co_codigo = dc.dc_compra
              INNER JOIN detalle_compras_vencimineto d ON dc.dc_id = d.dv_detalle_compra
              INNER JOIN articulos ar ON dc.dc_articulo = ar.ar_codigo
              INNER JOIN articulos_lotes al ON d.loteid = al.al_codigo
              LEFT JOIN operadores op ON co.co_responsable_ubicacion = op.op_codigo
              LEFT JOIN operadores op2 ON co.co_verificador = op2.op_codigo
              LEFT JOIN operadores op3 ON co.co_confirmador = op3.op_codigo
              WHERE co.co_estado = 1
              {where}
              GROUP BY co.co_codigo, co.co_fecha, co.co_deposito, co.co_factura
              ORDER BY co.co_fecha DESC
            ";


            return await connection.QueryAsync<ReporteIngresosViewModel>(query, parameters);

        }




    }
}
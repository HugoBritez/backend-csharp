using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Api.Models.Dtos.Articulo;
using Dapper;
using MySql.Data.MySqlClient;
using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Data;
using Api.Repositories.Base;

namespace Api.Repositories.Implementations
{
    public class ArticuloRepository : DapperRepositoryBase, IArticuloRepository
    {
        public readonly ApplicationDbContext _context;

        public ArticuloRepository(IConfiguration configuration, ApplicationDbContext context)
            : base(configuration)
        {
            _context = context;
        }

        public async Task<ArticuloBusquedaDTO?> BuscarArticuloPorCodigo(
            string codigoBarra,
            uint deposito,
            bool? stock
            )
        {
            var where = "";
            var parameters = new DynamicParameters();

           

            if (deposito != 0)
            {
                where += " AND al.al_deposito = @Deposito";
                parameters.Add("@Deposito", deposito);
            }

            if (stock == true)
            {
                where += " AND (al.al_cantidad > 0)";
            }
            else if (stock == false)
            {
                where += " AND (al.al_cantidad <= 0)";
            }

            if (codigoBarra != null)
            {
                where += " AND (al.al_codbarra = @CodigoBarra OR ar.ar_codbarra = @CodigoBarra)";
                parameters.Add("@CodigoBarra", codigoBarra);
            }

            var query = @"
                SELECT
                    al.al_codigo as IdLote,
                    al.al_lote as Lote,
                    ar.ar_codigo as IdArticulo,
                    ar.ar_codbarra as CodigoBarra,
                    ar.ar_descripcion as Descripcion,
                    ar.ar_stockneg as StockNegativo,
                    ar.ar_pcg as PrecioCosto,
                    ar.ar_pvg as PrecioVentaGuaranies,
                    ar.ar_pvcredito as PrecioCredito,
                    ar.ar_pvmostrador as PrecioMostrador,
                    ar.ar_precio_4 as Precio4,
                    ar.ar_pcd as PrecioCostoDolar,
                    ar.ar_pvd as PrecioVentaDolar,
                    ar.ar_pcp as PrecioCostoPesos,
                    ar.ar_pvp as PrecioVentaPesos,
                    ar.ar_pcr as PrecioCostoReal,
                    ar.ar_pvr as PrecioVentaReal,
                    DATE_FORMAT(al.al_vencimiento, '%d/%m/%Y') as VencimientoLote,
                    al.al_cantidad as CantidadLote,
                    al.al_deposito as Deposito,
                    ub.ub_descripcion as Ubicacion,
                    s.s_descripcion as SubUbicacion,
                    ma.ma_descripcion as Marca,
                    sc.sc_descripcion as Subcategoria,
                    ca.ca_descripcion as Categoria,
                    ar.ar_iva as Iva,
                    ar.ar_vencimiento as VencimientoValidacion,
                    iva.iva_descripcion as IvaDescripcion,
                    ar.ar_editar_desc as EditarNombre,
                    CASE
                        WHEN MIN(DATEDIFF(al.al_vencimiento, CURDATE())) < 0 THEN 'VENCIDO'
                        WHEN MIN(DATEDIFF(al.al_vencimiento, CURDATE())) <= 120 THEN 'PROXIMO'
                        ELSE 'VIGENTE'
                    END as EstadoVencimiento,
                    (
                        SELECT GROUP_CONCAT(DISTINCT p.pro_razon SEPARATOR ', ')
                        FROM articulos_proveedores ap 
                        INNER JOIN proveedores p ON ap.arprove_prove = p.pro_codigo
                        WHERE ap.arprove_articulo = ar.ar_codigo
                    ) as Proveedor,
                    (
                        SELECT DATE_FORMAT(ve.ve_fecha, '%d/%m/%Y')
                        FROM ventas ve
                        INNER JOIN detalle_ventas dv ON ve.ve_codigo = dv.deve_venta
                        WHERE dv.deve_articulo = ar.ar_codigo
                        ORDER BY ve.ve_fecha DESC
                        LIMIT 1
                    ) as FechaUltimaVenta,
                    al.al_pre_compra as PreCompra
                FROM articulos_lotes al
                INNER JOIN articulos ar ON al.al_articulo = ar.ar_codigo
                INNER JOIN depositos de ON al.al_deposito = de.dep_codigo
                INNER JOIN ubicaciones ub ON ar.ar_ubicacicion = ub.ub_codigo
                INNER JOIN sub_ubicacion s ON ar.ar_sububicacion = s.s_codigo
                INNER JOIN marcas ma ON ar.ar_marca = ma.ma_codigo
                INNER JOIN subcategorias sc ON ar.ar_subcategoria = sc.sc_codigo
                INNER JOIN categorias ca ON sc.sc_categoria = ca.ca_codigo
                INNER JOIN iva ON ar.ar_iva = iva.iva_codigo
                WHERE ar.ar_estado = 1
                " + where + @"
                GROUP BY al.al_codigo
                ORDER BY ar.ar_descripcion
                LIMIT 1";

            using var connection = GetConnection();
            Console.WriteLine(query);
            return await connection.QueryFirstOrDefaultAsync<ArticuloBusquedaDTO>(query, parameters);
        }

        public async Task<IEnumerable<ArticuloBusquedaDTO>> BuscarArticulos(
            uint? articuloId = null,
            string? busqueda = null,
            string? codigoBarra = null,
            uint moneda = 1,
            bool? stock = null,
            uint? deposito = null,
            uint? marca = null,
            uint? categoria = null,
            uint? ubicacion = null,
            uint? proveedor = null,
            string? codInterno = null,
            string? lote = null,
            bool? negativo = null)
        {
            Console.WriteLine(
                "estos son los parametros: " +
                $"articuloId: {articuloId}, " +
                $"busqueda: {busqueda}, " +
                $"codigoBarra: {codigoBarra}, " +
                $"moneda: {moneda}, " +
                $"stock: {stock}, " +
                $"deposito: {deposito}, " +
                $"marca: {marca}, " +
                $"categoria: {categoria}, " +
                $"ubicacion: {ubicacion}, " +
                $"proveedor: {proveedor}, " +
                $"codInterno: {codInterno}, " +
                $"lote: {lote}, " +
                $"negativo: {negativo}"
            );
            var where = "";
            var parameters = new DynamicParameters();

            if (articuloId.HasValue)
            {
                where += " AND ar.ar_codigo = @ArticuloId";
                parameters.Add("@ArticuloId", articuloId.Value);
            }
            else if (!string.IsNullOrEmpty(codigoBarra))
            {
                where += " AND ar.ar_codbarra = @CodigoBarra";
                parameters.Add("@CodigoBarra", codigoBarra);
            }
            else if (!string.IsNullOrEmpty(busqueda))
            {
                var palabras = busqueda.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (marca.HasValue || categoria.HasValue || ubicacion.HasValue ||
                    proveedor.HasValue || !string.IsNullOrEmpty(codInterno) || !string.IsNullOrEmpty(lote))
                {
                    var condicionesFiltros = new List<string>();

                    if (marca.HasValue)
                    {
                        condicionesFiltros.Add("ma.ma_descripcion LIKE @Busqueda");
                        parameters.Add("@Busqueda", $"%{busqueda}%");
                    }
                    if (categoria.HasValue)
                    {
                        condicionesFiltros.Add("ca.ca_descripcion LIKE @Busqueda");
                    }
                    if (ubicacion.HasValue)
                    {
                        condicionesFiltros.Add("ub.ub_descripcion LIKE @Busqueda");
                    }
                    if (!string.IsNullOrEmpty(codInterno))
                    {
                        condicionesFiltros.Add("ar.ar_cod_interno LIKE @Busqueda");
                    }
                    if (!string.IsNullOrEmpty(lote))
                    {
                        condicionesFiltros.Add("al.al_lote LIKE @Busqueda");
                    }
                    where += $" AND ({string.Join(" OR ", condicionesFiltros)})";
                }
                else
                {
                    Console.WriteLine("entra a este else de busqueda por palabras");
                    var condiciones = palabras.Select((p, i) =>
                    {
                        parameters.Add($"@Palabra{i}", $"%{p}%");
                        parameters.Add($"@Palabra{i}Exacta", p);
                        Console.WriteLine($"Palabra{i}: {p}");
                        return $"(ar.ar_descripcion LIKE @Palabra{i} OR ar.ar_codbarra = @Palabra{i}Exacta OR al.al_lote = @Palabra{i}Exacta)";
                    });
                    where += $" AND ({string.Join(" AND ", condiciones)})";
                }
            }

            if (deposito.HasValue)
            {
                where += " AND al.al_deposito = @Deposito";
                parameters.Add("@Deposito", deposito.Value);
            }

            if (negativo == true)
            {
                where += " AND (al.al_cantidad < 0)";
            }
            else if (stock == true)
            {
                where += " AND (al.al_cantidad > 0)";
            }

            var query = @"
                SELECT
                    al.al_codigo as IdLote,
                    al.al_lote as Lote,
                    ar.ar_codigo as IdArticulo,
                    ar.ar_codbarra as CodigoBarra,
                    ar.ar_descripcion as Descripcion,
                    ar.ar_stockneg as StockNegativo,
                    ar.ar_pcg as PrecioCosto,
                    ar.ar_pvg as PrecioVentaGuaranies,
                    ar.ar_pvcredito as PrecioCredito,
                    ar.ar_pvmostrador as PrecioMostrador,
                    ar.ar_precio_4 as Precio4,
                    ar.ar_pcd as PrecioCostoDolar,
                    ar.ar_pvd as PrecioVentaDolar,
                    ar.ar_pcp as PrecioCostoPesos,
                    ar.ar_pvp as PrecioVentaPesos,
                    ar.ar_pcr as PrecioCostoReal,
                    ar.ar_pvr as PrecioVentaReal,
                    DATE_FORMAT(al.al_vencimiento, '%d/%m/%Y') as VencimientoLote,
                    al.al_cantidad as CantidadLote,
                    al.al_deposito as Deposito,
                    ub.ub_descripcion as Ubicacion,
                    s.s_descripcion as SubUbicacion,
                    ma.ma_descripcion as Marca,
                    sc.sc_descripcion as Subcategoria,
                    ca.ca_descripcion as Categoria,
                    ar.ar_iva as Iva,
                    ar.ar_vencimiento as VencimientoValidacion,
                    iva.iva_descripcion as IvaDescripcion,
                    ar.ar_editar_desc as EditarNombre,
                    CASE
                        WHEN MIN(DATEDIFF(al.al_vencimiento, CURDATE())) < 0 THEN 'VENCIDO'
                        WHEN MIN(DATEDIFF(al.al_vencimiento, CURDATE())) <= 120 THEN 'PROXIMO'
                        ELSE 'VIGENTE'
                    END as EstadoVencimiento,
                    (
                        SELECT GROUP_CONCAT(DISTINCT p.pro_razon SEPARATOR ', ')
                        FROM articulos_proveedores ap 
                        INNER JOIN proveedores p ON ap.arprove_prove = p.pro_codigo
                        WHERE ap.arprove_articulo = ar.ar_codigo
                    ) as Proveedor,
                    (
                        SELECT DATE_FORMAT(ve.ve_fecha, '%d/%m/%Y')
                        FROM ventas ve
                        INNER JOIN detalle_ventas dv ON ve.ve_codigo = dv.deve_venta
                        WHERE dv.deve_articulo = ar.ar_codigo
                        ORDER BY ve.ve_fecha DESC
                        LIMIT 1
                    ) as FechaUltimaVenta,
                    al.al_pre_compra as PreCompra
                FROM articulos_lotes al
                INNER JOIN articulos ar ON al.al_articulo = ar.ar_codigo
                INNER JOIN depositos de ON al.al_deposito = de.dep_codigo
                INNER JOIN ubicaciones ub ON ar.ar_ubicacicion = ub.ub_codigo
                INNER JOIN sub_ubicacion s ON ar.ar_sububicacion = s.s_codigo
                INNER JOIN marcas ma ON ar.ar_marca = ma.ma_codigo
                INNER JOIN subcategorias sc ON ar.ar_subcategoria = sc.sc_codigo
                INNER JOIN categorias ca ON sc.sc_categoria = ca.ca_codigo
                INNER JOIN iva ON ar.ar_iva = iva.iva_codigo
                WHERE ar.ar_estado = 1
                " + where + @"
                GROUP BY al.al_codigo
                ORDER BY ar.ar_descripcion
                LIMIT 25";

            using var connection = GetConnection();
            Console.WriteLine(query);
            return await connection.QueryAsync<ArticuloBusquedaDTO>(query, parameters);
        }

        public async Task<ArticuloConsultaResponse> ConsultarArticulos(
            string? busqueda = null,
            string? deposito = null,
            string? stock = null,
            string? marca = null,
            string? categoria = null,
            string? subcategoria = null,
            string? proveedor = null,
            string? ubicacion = null,
            bool? servicio = null,
            uint? moneda = null,
            string? unidadMedida = null,
            int pagina = 1,
            int limite = 50,
            string? tipoValorizacionCosto = null)
        {
            try
            {
                pagina = Math.Max(1, pagina);
                limite = Math.Max(1, limite);
                var offset = (pagina - 1) * limite;

                var where = "WHERE 1=1";
                var parameters = new DynamicParameters();

                if (!string.IsNullOrEmpty(busqueda))
                {
                    var palabras = busqueda.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    var condiciones = palabras.Select((palabra, index) =>
                    {
                        parameters.Add($"@Busqueda{index}", $"%{palabra}%");
                        return $"(ar.ar_descripcion LIKE @Busqueda{index} OR ar.ar_cod_interno LIKE @Busqueda{index} OR al.al_lote LIKE @Busqueda{index} OR ar.ar_codbarra LIKE @Busqueda{index})";
                    });
                    where += $" AND ({string.Join(" AND ", condiciones)})";
                }

                if (!string.IsNullOrEmpty(deposito))
                {
                    where += " AND al.al_deposito IN @Deposito";
                    parameters.Add("@Deposito", deposito.Split(',').Select(uint.Parse).ToArray());
                }

                if (!string.IsNullOrEmpty(marca))
                {
                    where += " AND ar.ar_marca IN @Marca";
                    parameters.Add("@Marca", marca.Split(',').Select(uint.Parse).ToArray());
                }

                if (!string.IsNullOrEmpty(categoria))
                {
                    where += " AND ar.ar_subcategoria IN @Categoria";
                    parameters.Add("@Categoria", categoria.Split(',').Select(uint.Parse).ToArray());
                }

                if (!string.IsNullOrEmpty(subcategoria))
                {
                    where += " AND ar.ar_subcategoria IN @Subcategoria";
                    parameters.Add("@Subcategoria", subcategoria.Split(',').Select(uint.Parse).ToArray());
                }

                if (!string.IsNullOrEmpty(proveedor))
                {
                    where += " AND ar.ar_proveedor IN @Proveedor";
                    parameters.Add("@Proveedor", proveedor.Split(',').Select(uint.Parse).ToArray());
                }

                if (!string.IsNullOrEmpty(ubicacion))
                {
                    where += " AND ar.ar_ubicacicion IN @Ubicacion";
                    parameters.Add("@Ubicacion", ubicacion.Split(',').Select(uint.Parse).ToArray());
                }

                if (servicio == true)
                {
                    where += " AND ar.ar_servicio = 1";
                }

                if (moneda.HasValue)
                {
                    where += " AND ar.ar_moneda = @Moneda";
                    parameters.Add("@Moneda", moneda.Value);
                }

                if (!string.IsNullOrEmpty(unidadMedida))
                {
                    where += " AND ar.ar_unidadmedida IN @UnidadMedida";
                    parameters.Add("@UnidadMedida", unidadMedida.Split(',').Select(uint.Parse).ToArray());
                }

                if (stock == "-1")
                {
                    where += " AND al.al_cantidad < 0";
                }
                else if (stock == "0")
                {
                    where += " AND al.al_cantidad = 0";
                }
                else if (stock == "1")
                {
                    where += " AND al.al_cantidad > 0";
                }

                var ultimocostocompra = tipoValorizacionCosto == "2"
                    ? @"COALESCE(
                (SELECT FORMAT((dc.dc_precio + dc.dc_recargo), 0, 'de_DE')
                 FROM detalle_compras dc 
                 INNER JOIN compras c ON dc.dc_compra = c.co_codigo 
                 WHERE c.co_estado = 1
                 AND dc.dc_articulo = ar.ar_codigo
                 AND c.co_moneda = ar.ar_moneda
                 ORDER BY dc.dc_id DESC 
                 LIMIT 1),
                CASE 
                  WHEN ar.ar_moneda = 1 THEN FORMAT(ar.ar_pcg, 0, 'de_DE')
                  WHEN ar.ar_moneda = 2 THEN FORMAT(ar.ar_pcd, 0, 'de_DE')
                  WHEN ar.ar_moneda = 3 THEN FORMAT(ar.ar_pcr, 0, 'de_DE')
                  WHEN ar.ar_moneda = 4 THEN FORMAT(ar.ar_pcp, 0, 'de_DE')
                  ELSE FORMAT(ar.ar_pcg, 0, 'de_DE')
                END
              )"
                    : $@"CASE 
                WHEN {moneda ?? 1} = 1 THEN FORMAT(ar.ar_pcg, 0, 'de_DE')
                WHEN {moneda ?? 1} = 2 THEN FORMAT(ar.ar_pcd, 2, 'de_DE')
                WHEN {moneda ?? 1} = 3 THEN FORMAT(ar.ar_pcr, 2, 'de_DE')
                WHEN {moneda ?? 1} = 4 THEN FORMAT(ar.ar_pcp, 2, 'de_DE')
                ELSE NULL
              END";

                var baseQuery = @"
            FROM articulos_lotes al
            INNER JOIN articulos ar ON al.al_articulo = ar.ar_codigo
            INNER JOIN depositos dep ON al.al_deposito = dep.dep_codigo
            INNER JOIN ubicaciones ub ON ar.ar_ubicacicion = ub.ub_codigo
            INNER JOIN subcategorias sc ON ar.ar_subcategoria = sc.sc_codigo
            INNER JOIN categorias cat ON sc.sc_categoria = cat.ca_codigo
            INNER JOIN marcas ma ON ar.ar_marca = ma.ma_codigo
            INNER JOIN unidadmedidas um ON ar.ar_unidadmedida = um.um_codigo
            LEFT JOIN articulos_proveedores ap ON ar.ar_codigo = ap.arprove_articulo
            LEFT JOIN proveedores pro ON ap.arprove_codigo = pro.pro_codigo";

                var countQuery = $@"
            SELECT COUNT(DISTINCT al.al_codigo) as total
            {baseQuery}
            {where}";

                var query = $@"
            SELECT DISTINCT
                ar.ar_codigo as codigo_articulo,
                (CASE WHEN al.al_codbarra IS NOT NULL AND al.al_codbarra != '' THEN al.al_codbarra ELSE ar.ar_codbarra END) as codigo_barra,
                ar.ar_descripcion as descripcion_articulo,
                {ultimocostocompra} as precio_compra,
                CASE 
                    WHEN {moneda ?? 1} = 1 THEN FORMAT(ar.ar_pvg, 0, 'de_DE')
                    WHEN {moneda ?? 1} = 2 THEN FORMAT(ar.ar_pvd, 2, 'de_DE')
                    WHEN {moneda ?? 1} = 3 THEN FORMAT(ar.ar_pvr, 2, 'de_DE')
                    WHEN {moneda ?? 1} = 4 THEN FORMAT(ar.ar_pvp, 2, 'de_DE')
                    ELSE NULL
                END as precio_venta,
                FORMAT(ar.ar_pvcredito, 0, 'de_DE') as precio_venta_credito,
                FORMAT(ar.ar_pvmostrador, 0, 'de_DE') as precio_venta_mostrador,
                FORMAT(ar.ar_precio_4, 0, 'de_DE') as precio_venta_4,
                pro.pro_razon as proveedor,
                dep.dep_descripcion as Deposito,
                ub.ub_descripcion as ubicacion,
                cat.ca_descripcion as categoria,
                sc.sc_descripcion as subcategoria,
                ma.ma_descripcion as marca,
                um.um_descripcion as unidad_medida,
                FORMAT(al.al_cantidad, 0, 'de_DE') as stock_actual,
                FORMAT(ar.ar_stkmin, 0, 'de_DE') as stock_minimo,
                al.al_codigo as lote,
                DATE_FORMAT(al.al_vencimiento, '%Y-%m-%d') as vencimiento,
                CASE 
                    WHEN {moneda ?? 1} = 1 THEN 'Gs'
                    WHEN {moneda ?? 1} = 2 THEN 'USD'
                    WHEN {moneda ?? 1} = 3 THEN 'BRL'
                    WHEN {moneda ?? 1} = 4 THEN 'ARS'
                    ELSE 'Gs'
                END as moneda
            {baseQuery}
            {where}
            ORDER BY ar.ar_descripcion
            LIMIT {limite} OFFSET {offset}";

                using var connection = GetConnection();
                var resultados = await connection.QueryAsync<ConsultaArticulosDto>(query, parameters);
                var totalRegistros = await connection.QueryFirstOrDefaultAsync<dynamic>(countQuery, parameters);

                return new ArticuloConsultaResponse
                {
                    Datos = resultados,
                    Paginacion = new PaginacionDTO
                    {
                        Pagina = pagina,
                        Limite = limite,
                        Total = (int)(totalRegistros?.total ?? 0),
                        Paginas = (int)Math.Ceiling((totalRegistros?.total ?? 0) / (double)limite)
                    }
                };
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error en BuscarArticulosNuevo: {ex}");
                throw;
            }
        }

        public async Task<IEnumerable<ArticuloLoteDTO>> ConsultarArticuloSimple(
            uint? articulo_id,
            string? busqueda,
            string? codigo_barra,
            uint? moneda = 1,
            bool? stock = null,
            uint? deposito = null,
            uint? marca = null,
            uint? categoria = null,
            uint? ubicacion = null,
            uint? proveedor = null,
            string? cod_interno = null
        )
        {
            try
            {
                var queryBuilder = new ArticuloQueryBuilder();
                var parameters = new DynamicParameters();

                // Aplicar filtros de búsqueda principal
                queryBuilder.ApplySearchFilters(articulo_id, busqueda, codigo_barra, cod_interno, parameters);
                
                // Aplicar filtros adicionales
                queryBuilder.ApplyAdditionalFilters(marca, categoria, ubicacion, proveedor, deposito, stock, parameters);
                
                // Obtener query de moneda
                var monedaQuery = GetMonedaQuery(moneda ?? 1);
                
                var query = $@"
                    SELECT
                       ar.ar_codigo as id_articulo,
                       ar.ar_codbarra as codigo_barra,
                       al.al_codbarra as codigo_barra_lote,
                       ar.ar_cod_interno as codigo_interno,
                       ar.ar_descripcion as descripcion,
                       ar.ar_stockneg as stock_negativo,
                       CAST(COALESCE(SUM(al.al_cantidad), 0) AS SIGNED) as stock,
                       {monedaQuery}
                       ub.ub_descripcion as ubicacion,
                       s.s_descripcion as sub_ubicacion,
                       ma.ma_descripcion as marca,
                       sc.sc_descripcion as subcategoria,
                       ca.ca_descripcion as categoria,
                       ar.ar_iva as iva,
                       ar.ar_vencimiento as vencimiento_validacion,
                       iva.iva_descripcion as iva_descripcion,
                       ta.t_descripcion as talle,
                       co.c_descripcion as color,
                       ar.ar_editar_desc as editar_nombre,
                       (
                        SELECT 
                          CASE
                            WHEN MIN(DATEDIFF(al2.al_vencimiento, CURDATE())) < 0 THEN 'VENCIDO'
                            WHEN MIN(DATEDIFF(al2.al_vencimiento, CURDATE())) <= 120 THEN 'PROXIMO'
                            ELSE 'VIGENTE'
                          END
                        FROM articulos_lotes al2
                        WHERE al2.al_articulo = ar.ar_codigo 
                        AND al2.al_cantidad > 0
                        AND al2.al_vencimiento != '0001-01-01'
                       ) as estado_vencimiento,
                       (
                        SELECT DATE_FORMAT(ve.ve_fecha, '%d/%m/%Y')
                        FROM ventas ve
                        INNER JOIN detalle_ventas dv ON ve.ve_codigo = dv.deve_venta
                        WHERE dv.deve_articulo = ar.ar_codigo
                        ORDER BY ve.ve_fecha DESC
                        LIMIT 1
                       ) as fecha_ultima_venta,
                       (
                         SELECT GROUP_CONCAT(DISTINCT p.pro_razon SEPARATOR ', ')
                         FROM articulos_proveedores ap 
                         INNER JOIN proveedores p ON ap.arprove_prove = p.pro_codigo
                         WHERE ap.arprove_articulo = ar.ar_codigo
                       ) as proveedor,
                      (
                        SELECT JSON_ARRAYAGG(t.lote_info)
                        FROM (
                          SELECT 
                            JSON_OBJECT(
                              'id', al_codigo,
                              'lote', al_lote,
                              'cantidad', CAST(al_cantidad AS SIGNED),
                              'vencimiento', DATE_FORMAT(al_vencimiento, '%d/%m/%Y'),
                              'deposito', al_deposito
                            ) as lote_info
                          FROM articulos_lotes 
                          WHERE al_articulo = ar.ar_codigo
                          AND al_cantidad > 0
                        ) t
                      ) as lotes_json,
                      (
                        SELECT JSON_ARRAYAGG(t.deposito_info)
                        FROM (
                          SELECT 
                            JSON_OBJECT(
                              'codigo', dep.dep_codigo,
                              'descripcion', dep.dep_descripcion,
                              'stock', CAST(SUM(al2.al_cantidad) AS SIGNED)
                            ) as deposito_info
                          FROM articulos_lotes al2
                          INNER JOIN depositos dep ON al2.al_deposito = dep.dep_codigo
                          WHERE al2.al_articulo = ar.ar_codigo
                          AND al2.al_cantidad > 0
                          GROUP BY dep.dep_codigo, dep.dep_descripcion
                        ) t
                      ) as depositos_json
                    FROM articulos_lotes al
                    LEFT JOIN articulos ar ON ar.ar_codigo = al.al_articulo
                    INNER JOIN ubicaciones ub ON ar.ar_ubicacicion = ub.ub_codigo
                    INNER JOIN sub_ubicacion s ON ar.ar_sububicacion = s.s_codigo
                    INNER JOIN marcas ma ON ar.ar_marca = ma.ma_codigo
                    INNER JOIN subcategorias sc ON ar.ar_subcategoria = sc.sc_codigo
                    INNER JOIN categorias ca ON sc.sc_categoria = ca.ca_codigo
                    INNER JOIN iva ON ar.ar_iva = iva.iva_codigo
                    LEFT JOIN talles ta ON al.al_talle = ta.t_codigo
                    LEFT JOIN colores co ON al.al_color = co.c_codigo
                    WHERE ar.ar_estado = 1
                    {queryBuilder.BuildWhereClause()}
                    GROUP BY al.al_codigo
                    ORDER BY ar.ar_descripcion
                    LIMIT 50";

                using var connection = GetConnection();
                return await connection.QueryAsync<ArticuloLoteDTO>(query, parameters);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error en ConsultarArticuloSimple: {ex}");
                throw;
            }
        }

        // Métodos privados para separar responsabilidades
        private string GetMonedaQuery(uint moneda)
        {
            return moneda switch
            {
                1 => @"
                    ar.ar_pcg as precio_costo,
                    ar.ar_pvg as precio_venta,
                    ar.ar_pvcredito as precio_credito,
                    ar.ar_pvmostrador as precio_mostrador,
                    ar.ar_precio_4 as precio_4,",
                2 => @"
                    ar.ar_pcd as precio_costo,
                    ar.ar_pvd as precio_venta,",
                3 => @"
                    ar.ar_pcr as precio_costo,
                    ar.ar_pvr as precio_venta,",
                4 => @"
                    ar.ar_pcp as precio_costo,
                    ar.ar_pvp as precio_venta,",
                _ => @"
                    ar.ar_pcg as precio_costo,
                    ar.ar_pvg as precio_venta,"
            };
        }

        // Clase auxiliar para construir queries (Single Responsibility Principle)
        private class ArticuloQueryBuilder
        {
            private readonly List<string> _whereConditions = new();

            public void ApplySearchFilters(
                uint? articuloId, 
                string? busqueda, 
                string? codigo_barra, 
                string? cod_interno, 
                DynamicParameters parameters)
            {
                if (articuloId.HasValue)
                {
                    _whereConditions.Add("ar.ar_codigo = @ArticuloId");
                    parameters.Add("@ArticuloId", articuloId.Value);
                }
                else if (!string.IsNullOrWhiteSpace(cod_interno))
                {
                    Console.WriteLine($"CodInterno: {cod_interno}");
                    _whereConditions.Add("ar.ar_cod_interno LIKE @CodInterno");
                    parameters.Add("@CodInterno", $"%{cod_interno}%");
                }
                else if (!string.IsNullOrWhiteSpace(codigo_barra))
                {
                    _whereConditions.Add("(ar.ar_codbarra = @CodigoBarra OR al.al_codbarra = @CodigoBarra)");
                    parameters.Add("@CodigoBarra", codigo_barra);
                }
                else if (!string.IsNullOrWhiteSpace(busqueda))
                {
                    ApplyBusquedaFilter(busqueda, parameters);
                }
            }

            public void ApplyAdditionalFilters(
                uint? marca, 
                uint? categoria, 
                uint? ubicacion, 
                uint? proveedor, 
                uint? deposito, 
                bool? stock, 
                DynamicParameters parameters)
            {
                if (marca.HasValue)
                {
                    _whereConditions.Add("ma.ma_codigo = @Marca");
                    parameters.Add("@Marca", marca.Value);
                }

                if (categoria.HasValue)
                {
                    _whereConditions.Add("ca.ca_codigo = @Categoria");
                    parameters.Add("@Categoria", categoria.Value);
                }

                if (ubicacion.HasValue)
                {
                    _whereConditions.Add("ub.ub_codigo = @Ubicacion");
                    parameters.Add("@Ubicacion", ubicacion.Value);
                }

                if (proveedor.HasValue)
                {
                    _whereConditions.Add("ar.ar_proveedor = @Proveedor");
                    parameters.Add("@Proveedor", proveedor.Value);
                }

                if (deposito.HasValue)
                {
                    _whereConditions.Add("al.al_deposito = @Deposito");
                    parameters.Add("@Deposito", deposito.Value);
                }

                if (stock == true)
                {
                    _whereConditions.Add("al.al_cantidad > 0");
                }
            }

            private void ApplyBusquedaFilter(string busqueda, DynamicParameters parameters)
            {
                var palabras = busqueda.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var condiciones = palabras.Select((palabra, index) =>
                {
                    parameters.Add($"@Palabra{index}", $"%{palabra}%");
                    parameters.Add($"@Palabra{index}Exacta", palabra);
                    return $"(ar.ar_descripcion LIKE @Palabra{index} OR ar.ar_codbarra = @Palabra{index}Exacta OR al.al_lote = @Palabra{index}Exacta)";
                });
                
                _whereConditions.Add($"({string.Join(" AND ", condiciones)})");
            }

            public string BuildWhereClause()
            {
                return _whereConditions.Count > 0 ? $" AND {string.Join(" AND ", _whereConditions)}" : "";
            }
        }

        public async Task<IEnumerable<ArticuloCategoriaResponse>> ArticulosPorCategoria()
        {
            var query = @"
              SELECT
        ca.ca_codigo as id,
        ca.ca_descripcion as nombre,
        (
          SELECT COUNT(*)
          FROM articulos ar
          INNER JOIN subcategorias sc2 ON ar.ar_subcategoria = sc2.sc_codigo
          WHERE sc2.sc_categoria = ca.ca_codigo
          AND ar.ar_estado = 1
        ) as cantidad_articulos
      FROM categorias ca
      WHERE ca.ca_estado = 1
      ORDER BY ca.ca_descripcion
            ";

            using var connection = GetConnection();
            return await connection.QueryAsync<ArticuloCategoriaResponse>(query);
        }

        public async Task<IEnumerable<ArticuloMarcaResponse>> ArticulosPorMarca()
        {
            var query = @"
              SELECT
                ma.ma_codigo as id,
                ma.ma_descripcion as nombre,
                (
                  SELECT COUNT(*)
                  FROM articulos ar
                  WHERE ar.ar_marca = ma.ma_codigo
                  AND ar.ar_estado = 1
                ) as cantidad_articulos
              FROM marcas ma
              WHERE ma.ma_estado = 1
              ORDER BY ma.ma_descripcion
            ";

            using var connection = GetConnection();
            return await connection.QueryAsync<ArticuloMarcaResponse>(query);
        }

        public async Task<IEnumerable<ArticuloSeccionResponse>> ArticulosPorSeccion()
        {
            var query = @"
              SELECT
                s.s_codigo as id,
                s.s_descripcion as nombre,
                (
                  SELECT COUNT(*)
                  FROM articulos ar
                  WHERE ar.ar_seccion = s.s_codigo
                  AND ar.ar_estado = 1
                ) as cantidad_articulos
              FROM secciones s
              WHERE s.s_estado = 1
              ORDER BY s.s_descripcion
            ";

            using var connection = GetConnection();
            return await connection.QueryAsync<ArticuloSeccionResponse>(query);
        }


        public async Task<IEnumerable<ArticuloEnPedidoResponse>> ArticulosEnPedido(int articulo_id, int id_lote)
        {
            var query =
            @"
            (SELECT
                dp.dp_codigo as IdDetallePedido,
                DATE_FORMAT(p.p_fecha, '%d/%m/%Y') as Fecha,
                cli.cli_razon as Cliente,
                dp.dp_cantidad as Cantidad,
                0 as Tipo
            FROM detalle_pedido dp
            INNER JOIN pedidos p ON dp.dp_pedido = p.p_codigo
            INNER JOIN clientes cli ON p.p_cliente = cli.cli_codigo
            WHERE dp.dp_articulo = @ArticuloId
            AND dp.dp_codigolote = @IdLote
            AND p.p_estado = 1
            AND dp.dp_habilitar = 1)
            
            UNION ALL
            
            (SELECT
                ri.id as IdDetallePedido,
                DATE_FORMAT(r.fecha, '%d/%m/%Y') as Fecha,
                cli.cli_razon as Cliente,
                ri.cantidad as Cantidad,
                1 as Tipo
            FROM remisiones_items ri
            INNER JOIN remisiones r ON ri.remision = r.id
            INNER JOIN clientes cli ON r.cliente = cli.cli_codigo
            WHERE ri.articulo = @ArticuloId
            AND ri.codlote = @IdLote
            AND r.estado = 1
            AND r.tipo_estados = 0
            ORDER BY r.id DESC)";

            var parameters = new DynamicParameters();
            parameters.Add("@ArticuloId", articulo_id);
            parameters.Add("@IdLote", id_lote);

            using var connection = GetConnection();
            return await connection.QueryAsync<ArticuloEnPedidoResponse>(query, parameters);
        }

        public async Task<Articulo> GetById(uint id)
        {
            var articulo = await _context.Articulos.FirstOrDefaultAsync(ar => ar.ArCodigo == id);
            return articulo ?? new Articulo();
        }

        public async Task<Articulo?> Update(Articulo articulo)
        {
            var articuloAEditar =await  GetById(articulo.ArCodigo);
            if (articuloAEditar == null)
            {
                return null;
            }

            _context.Entry(articuloAEditar).CurrentValues.SetValues(articulo);
            await _context.SaveChangesAsync();

            return articulo;
        }

        public async Task<Articulo> Create(Articulo articulo)
        {
            await _context.Articulos.AddAsync(articulo);
            await _context.SaveChangesAsync();
            return articulo;
        }
    }
}

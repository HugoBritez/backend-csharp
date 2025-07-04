using Dapper;
using Api.Repositories.Base;
using Api.Repositories.Interfaces;
using Api.Data;
using Api.Models.Entities;
using Api.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
    public class PresupuestosRepository : DapperRepositoryBase, IPresupuestosRepository
    {
        private readonly ApplicationDbContext _context;
        public PresupuestosRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }

        public async Task<IEnumerable<PresupuestoViewModel>> GetTodos(
            string? fecha_desde,
            string? fecha_hasta,
            uint? sucursal,
            uint? cliente,
            uint? vendedor,
            uint? articulo,
            uint? moneda,
            uint? estado,
            string? busqueda
        )
        {
            using var connection = GetConnection();
            var whereConditions = new List<string> { "1=1" };
            var limit = "";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(busqueda))
            {
                whereConditions.Add("(pre.pre_codigo LIKE @Busqueda OR cli.cli_razon LIKE @Busqueda)");
                parameters.Add("@Busqueda", $"%{busqueda}%");
                limit = " LIMIT 10";
            }
            else
            {
                if (!string.IsNullOrEmpty(fecha_desde))
                {
                    whereConditions.Add("pre.pre_fecha >= @FechaDesde");
                    parameters.Add("@FechaDesde", fecha_desde);
                }
                if (!string.IsNullOrEmpty(fecha_hasta))
                {
                    whereConditions.Add("pre.pre_fecha <= @FechaHasta");
                    parameters.Add("@FechaHasta", fecha_hasta);
                }
                if (sucursal.HasValue)
                {
                    whereConditions.Add("pre.pre_sucursal = @Sucursal");
                    parameters.Add("@Sucursal", sucursal);
                }
                if (cliente.HasValue)
                {
                    whereConditions.Add("pre.pre_cliente = @Cliente");
                    parameters.Add("@Cliente", cliente);
                }
                if (vendedor.HasValue)
                {
                    whereConditions.Add("pre.pre_vendedor = @Vendedor");
                    parameters.Add("@Vendedor", vendedor);
                }
                if (articulo.HasValue)
                {
                    whereConditions.Add("pre.pre_codigo IN (SELECT z.depre_presupuesto FROM detalle_presupuesto z WHERE depre_articulo = @Articulo)");
                    parameters.Add("@Articulo", articulo);
                }
                if (moneda.HasValue)
                {
                    whereConditions.Add("pre.pre_moneda = @Moneda");
                    parameters.Add("@Moneda", moneda);
                }
                if (estado.HasValue)
                {
                    whereConditions.Add($"pre.pre_confirmado = {(estado == 1 ? "1" : "0")}");
                }
            }

            var where = string.Join(" AND ", whereConditions);

            var query = $@"
                SELECT
                    pre.pre_codigo AS codigo,
                    cli.cli_codigo AS codcliente,
                    cli.cli_razon AS cliente,
                    mo.mo_descripcion AS moneda,
                    DATE_FORMAT(pre.pre_fecha, '%Y/%m/%d') AS fecha,
                    pre.pre_sucursal AS codsucursal,
                    s.descripcion AS sucursal,
                    v.op_nombre AS vendedor,
                    o.op_nombre AS operador,
                    FORMAT(SUM((depre.depre_precio - depre.depre_descuento) * depre.depre_cantidad) - pre.pre_descuento, 0, 'es_PY') AS total,
                    pre.pre_descuento AS descuento,
                    0 AS saldo,
                    IF (pre.pre_credito = 1, 'Crédito', 'Contado') AS condicion,
                    pre_validez AS vencimiento,
                    '' AS factura,
                    pre.pre_obs AS obs,
                    pre.pre_estado AS estado,
                    IF(pre.pre_confirmado = 1, 'Confirmado', 'Pendiente') AS estado_desc,
                    pre.pre_condicion,
                    pre.pre_plazo,
                    pre.pre_flete
                FROM presupuesto pre
                INNER JOIN clientes cli ON pre.pre_cliente = cli.cli_codigo
                INNER JOIN monedas mo ON pre.pre_moneda = mo.mo_codigo
                INNER JOIN operadores v ON pre.pre_vendedor = v.op_codigo
                INNER JOIN operadores o ON pre.pre_operador = o.op_codigo
                INNER JOIN sucursales s ON pre.pre_sucursal = s.id
                INNER JOIN detalle_presupuesto depre ON depre.depre_presupuesto = pre.pre_codigo
                WHERE {where}
                GROUP BY pre.pre_codigo
                ORDER BY pre.pre_codigo DESC{limit}";

            var res = await connection.QueryAsync<PresupuestoViewModel>(query, parameters);
            return res?.Any() == true ? res : Array.Empty<PresupuestoViewModel>();
        }

        public async Task<Presupuesto> Crear(Presupuesto presu)
        {
            var presupuestoCreado = await _context.Presupuesto.AddAsync(presu);
            await _context.SaveChangesAsync();
            return presupuestoCreado.Entity;
        }

        public async Task<Presupuesto> GetById(uint id)
        {
            var presu = await _context.Presupuesto.FirstOrDefaultAsync(pre => pre.Codigo == id);
            return presu ?? new Presupuesto();
        }

        public async Task<Presupuesto> Update(Presupuesto presupuesto)
        {
            var presupuestoExistente = await _context.Presupuesto.FirstOrDefaultAsync(p => p.Codigo == presupuesto.Codigo);

            if (presupuestoExistente == null)
            {
                return new Presupuesto();
            }

            _context.Entry(presupuestoExistente).CurrentValues.SetValues(presupuesto);
            await _context.SaveChangesAsync();

            return presupuestoExistente;
        }

        public async Task<string> ProcesarPresupuesto(int idPresupuesto)
        {
            using var connection = GetConnection();
            var query = @"
                UPDATE presupuesto
                SET pre_confirmado = 1
                WHERE pre_codigo = @idPresupuesto
                ";

            var parameter = new DynamicParameters();
            parameter.Add("idPresupuesto", idPresupuesto);
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
    }
}
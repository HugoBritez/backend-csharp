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
    public class ClienteRepository : DapperRepositoryBase, IClienteRepository
    {
        private readonly ApplicationDbContext _context;

        public ClienteRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
        {
            _context = context;
        }


        public async Task<Cliente> CrearCliente(Cliente cliente)
        {
            var clienteCreado = await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
            return clienteCreado.Entity;
        }

        public async Task<IEnumerable<ClienteViewModel>> GetClientes(string? busqueda, uint? id, uint? interno, uint? vendedor, int? estado = 1)
        {
            var where = " WHERE cli_estado = 1";
            using var connection = GetConnection();
            var parameters = new DynamicParameters();

            if (estado.HasValue)
            {
                where = " WHERE 1=1";
            }
            if (id.HasValue)
            {
                where += " AND cli_codigo = @id";
                parameters.Add("id", id);
            }

            if (interno.HasValue)
            {
                where += " AND cli_interno = @interno";
                parameters.Add("interno", interno);
            }
            if (vendedor.HasValue)
            {
                where += " AND cli_vendedor = @vendedor";
                parameters.Add("vendedor", vendedor);
            }
            if (!string.IsNullOrEmpty(busqueda))
            {
                where += "  AND (cli_razon LIKE @busqueda OR cli_descripcion LIKE @busqueda OR cli_ruc LIKE @busqueda)";
                parameters.Add("busqueda", $"%{busqueda}%");
            }


            var query = @"
              SELECT
                    cli_codigo,
                    cli_razon, 
                    cli_descripcion,
                    cli_ruc,
                    cli_interno,
                    cli_ciudad,
                    zo.zo_descripcion as zona,
                    ciu.ciu_descripcion as cli_ciudad_descripcion,
                    dep.dep_codigo as cli_departamento,
                    dep.dep_descripcion,
                    d.d_codigo as cli_distrito,
                    d.d_descripcion as cli_distrito_descripcion,
                    FORMAT(ROUND(cli_limitecredito), 0, 'es_ES') AS cli_limitecredito,
                    FORMAT(ROUND(IFNULL((
                       SELECT SUM(ve_saldo)
                       FROM ventas
                       WHERE ve_cliente = cli_codigo
                       AND ve_credito = 1
                       AND ve_estado = 1
                    ), 0)), 0, 'es_ES') AS deuda_actual,
                    FORMAT(ROUND(cli_limitecredito - IFNULL((
                       SELECT COALESCE(SUM(ve_saldo), 0)
                       FROM ventas
                       WHERE ve_cliente = cli_codigo
                       AND ve_credito = 1
                       AND ve_estado = 1
                    ), 0)), 0, 'es_ES') AS credito_disponible,
                    cli_vendedor as vendedor_cliente,
                    cli_dir,
                    cli_tel,
                    cli_mail,
                    cli_ci,
                    cli_tipo_doc,
                    cli_estado as estado
                FROM clientes  
                INNER JOIN ciudades ciu ON cli_ciudad = ciu.ciu_codigo
                INNER JOIN distritos d ON ciu.ciu_distrito = d.d_codigo
                INNER JOIN departamentos dep ON cli_departamento = dep.dep_codigo
                LEFT JOIN zonas zo ON zo.zo_codigo = cli_zona " + where + " LIMIT 50";

            Console.WriteLine(query);
            return await connection.QueryAsync<ClienteViewModel>(query, parameters);
        }

         public async Task<ClienteViewModel?> GetClientePorId(uint id)
        {
            var where = " WHERE cli_estado = 1";
            using var connection = GetConnection();
            var parameters = new DynamicParameters();

            where += " AND cli_codigo = @id";
            parameters.Add("id", id);

            var query = @"
              SELECT
                    cli_codigo,
                    cli_razon, 
                    cli_descripcion,
                    cli_ruc,
                    cli_interno,
                    cli_ciudad,
                    zo.zo_descripcion as zona,
                    ciu.ciu_descripcion as cli_ciudad_descripcion,
                    dep.dep_codigo as cli_departamento,
                    dep.dep_descripcion,
                    d.d_codigo as cli_distrito,
                    d.d_descripcion as cli_distrito_descripcion,
                    FORMAT(ROUND(cli_limitecredito), 0, 'es_ES') AS cli_limitecredito,
                    FORMAT(ROUND(IFNULL((
                       SELECT SUM(ve_saldo)
                       FROM ventas
                       WHERE ve_cliente = cli_codigo
                       AND ve_credito = 1
                       AND ve_estado = 1
                    ), 0)), 0, 'es_ES') AS deuda_actual,
                    FORMAT(ROUND(cli_limitecredito - IFNULL((
                       SELECT COALESCE(SUM(ve_saldo), 0)
                       FROM ventas
                       WHERE ve_cliente = cli_codigo
                       AND ve_credito = 1
                       AND ve_estado = 1
                    ), 0)), 0, 'es_ES') AS credito_disponible,
                    cli_vendedor as vendedor_cliente,
                    cli_dir,
                    cli_tel,
                    cli_mail,
                    cli_ci,
                    cli_tipo_doc,
                    cli_estado as estado
                FROM clientes  
                INNER JOIN ciudades ciu ON cli_ciudad = ciu.ciu_codigo
                INNER JOIN distritos d ON ciu.ciu_distrito = d.d_codigo
                INNER JOIN departamentos dep ON cli_departamento = dep.dep_codigo
                LEFT JOIN zonas zo ON zo.zo_codigo = cli_zona " + where;

            return await connection.QueryFirstOrDefaultAsync<ClienteViewModel>(query, parameters);
        }

        public async Task<IEnumerable<Cliente>> GetAll(string? Busqueda)
        {
            var query = _context.Clientes.AsQueryable();
            query = query.Where(cli => cli.Estado == 1);
            if (!string.IsNullOrWhiteSpace(Busqueda))
            {
                query = query.Where(cli =>
                    (cli.Descripcion != null && cli.Descripcion.ToLower().Contains(Busqueda.ToLower())) ||
                    (cli.Razon != null && cli.Razon.ToLower().Contains(Busqueda))
                );
            }

            return await query.Take(500).ToListAsync();
        }

        public async Task<Cliente?> GetByRuc(string ruc)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(cli => cli.Ruc == ruc);
            return cliente;
        }

        public async Task<Cliente> UpdateCliente(Cliente data)
        {
            _context.Clientes.Update(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<Cliente?> GetById(uint id) 
        {
            var response = await _context.Clientes.FirstOrDefaultAsync(cli=>cli.Codigo == id);
            return response;
        }

        public async Task<decimal> GetDeudaCliente(uint cliente)
        {
            var query = @"SELECT IFNULL(SUM(ve_saldo), 0) FROM ventas WHERE ve_cliente = @cliente  AND ve_estado = 1";
            using var connection = GetConnection();
            var parameters = new DynamicParameters();
            parameters.Add("cliente", cliente);
            return await connection.QueryFirstOrDefaultAsync<decimal>(query, parameters);
        }


        public async Task<UltimoCobroClienteViewModel?> GetUltimoCobroCliente(uint cliente)
        {
            var query = @"
                SELECT
                    ve.ve_fecha as Fecha,
                    dc.d_monto as Monto,
                    op.op_nombre as Vendedor,
                    ve.ve_codigo as Venta,
                    cli.cli_razon as Cliente
                FROM ventas ve
                INNER JOIN detalle_caja_ch_cobro d ON d.dccc_venta = ve.ve_codigo
                INNER JOIN detalle_caja_chica dc ON dc.d_codigo = d.dccc_detalleCaja
                INNER JOIN operadores op ON op.op_codigo = ve.ve_operador
                INNER JOIN clientes cli ON cli.cli_codigo = ve.ve_cliente
                WHERE ve.ve_cliente = @cliente
                ORDER BY ve.ve_fecha DESC
                LIMIT 1
            ";
            using var connection = GetConnection();
            var parameters = new DynamicParameters();
            parameters.Add("cliente", cliente);
            return await connection.QueryFirstOrDefaultAsync<UltimoCobroClienteViewModel>(query, parameters);
        }
    }
}
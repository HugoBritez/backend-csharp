using Api.Data;
using Api.Models.ViewModels;
using Api.Repositories.Base;
using Api.Repositories.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implementations
{
  public class BancoRepository : DapperRepositoryBase, IBancoRepository
  {
    private readonly IConfiguration _configuration;

    private readonly ApplicationDbContext _context;

    public BancoRepository(IConfiguration configuration, ApplicationDbContext context) : base(configuration)
    {
      _configuration = configuration;
      _context = context;
    }

    public async Task<IEnumerable<MovimientoBancarioViewModel>> ConsultaMovimientosBancarios(
        string fechaInicio,
        string fechaFin,
        int? estado,
        string? cheque,
        uint? codigoCuenta,
        int? tipoFecha,
        int? guardarCobroTarjeta,
        int? chequeTransferencia
    )
    {
      var parameters = new DynamicParameters();
      var where = "";
      using var connection = GetConnection();

      if (codigoCuenta != 0)
      {
        where += " AND mc.mc_cuenta = @CodigoCuenta";
        parameters.Add("@CodigoCuenta", codigoCuenta);
      }

      if (tipoFecha == 1)
      {
        if (fechaInicio != null && fechaFin != null)
        {
          where += " AND mc.mc_fecha BETWEEN @FechaInicio AND @FechaFin";
          parameters.Add("@FechaInicio", fechaInicio);
          parameters.Add("@FechaFin", fechaFin);
        }
      }
      else if (tipoFecha == 2)
      {
        if (fechaInicio != null && fechaFin != null)
        {
          where += " AND dmc2.dmc_fechaC BETWEEN @FechaInicio AND @FechaFin";
          parameters.Add("@FechaInicio", fechaInicio);
          parameters.Add("@FechaFin", fechaFin);
        }
      }
      else if (tipoFecha == 3)
      {
        if (fechaInicio != null && fechaFin != null)
        {
          where += " AND mcb.mc_f_conciliado BETWEEN @FechaInicio AND @FechaFin";
          parameters.Add("@FechaInicio", fechaInicio);
          parameters.Add("@FechaFin", fechaFin);
        }
      }

      if (cheque != null)
      {
        where += " AND dc.dc_numero = '@Cheque'";
        parameters.Add("@Cheque", cheque);
      }

      if (estado != 2)
      {
        where += " AND IFNULL(dmc2.dmc_estado, mc.mc_estado) = @Estado";
        parameters.Add("@Estado", estado);
      }


      if (guardarCobroTarjeta == 0 && chequeTransferencia == 0)
      {
        where += " AND mcb.mc_conciliado = 1";
      }

      var query = $@"
                SELECT
                  mcb.mc_codigo as  McCodigo,
                  DATE_FORMAT(mc.mc_fecha, '%d/%m/%Y') as McFecha,
                  IFNULL(DATE_FORMAT(dmc2.dmc_fechaC, '%d/%m/%Y'), '') as McVencimiento,
                  IF(mcb.mc_f_conciliado = '0001-01-01', '', DATE_FORMAT(mcb.mc_f_conciliado, '%d/%m/%Y')) as McFechaConciliado,
                  tmb.tm_descripcion as TmDescripcion,
                  mc.mc_cuenta as McCuenta,
                  cb.cb_descripcion as CdDescripcion,
                  mcb.mc_haber as McHaber,
                  IFNULL(dmc2.dmc_monto, mcb.mc_debe) as McDebito,
                  IFNULL(mc.mc_obs, '') as McOrden,
                  IFNULL(dmc2.dmc_cheque, mc.mc_comprobante) as McNumero,
                  IFNULL(
                    IF(
                      (
                        SELECT
                          conciliacion_bancaria.c_codigo
                        FROM
                          conciliacion_bancaria
                        INNER JOIN
                          detalle_conciliacion ON detalle_conciliacion.d_conciliacion = conciliacion_bancaria.c_codigo
                        INNER JOIN
                          movimientoscuentabco ON detalle_conciliacion.d_referencia_mov = movimientoscuentabco.mc_codigo
                        WHERE
                          detalle_conciliacion.d_referencia_mov = mcb.mc_codigo
                          AND conciliacion_bancaria.c_estado = 1
                          AND movimientoscuentabco.mc_conciliado = 1
                      ) > 0, 1 , 0
                    ), 0
                  ) as McConciliacion,
                  IFNULL(dmc2.dmc_esatdo, mc.mc_estado) as McEstado,
                  IFNULL(dmc2.dmc_referencia, 0 ) as McReferencia,
                  mc.mc_codigo as McCodigoMovimiento,
                  IFNULL(dmc2.dmc_codigo, 0) as McCodigoDetche,
                  1 as McTipoMovimiento,
                  mcb.mc_conciliado as McConciliado,
                  IFNULL(
                    (
                      SELECT
                        dm2.dcm_codigo
                      FROM
                        detalle_caja_metodo dm2 
                      INNER JOIN
                        detalle_caja d2 ON dm2.dcm_deca = d2.deca_codigo
                      WHERE
                        d2.deca_estado = 1
                      AND
                        dm2.referencia = mc.mc_codigo LIMIT 1
                    ),
                    IFNULL(
                      (
                        SELECT
                          dml.dccm_codigo
                        FROM
                          detalle_caja_ch_metodo dm1 
                        INNER JOIN
                          detalle_caja_chica d1 ON dm1.dccm_deca = d1.d_codigo
                        WHERE
                          d1.d_estado = 1
                        AND
                          dm1.referencia = mc.mc_codigo LIMIT 1
                      ), 0
                    )
                  ) AS McTransferencia,
                  IFNULL(dmc2.dmc_tipo_cliente, 0) AS McChequeCobrado
                FROM
                  movimientoscuentabco mcb
                INNER JOIN
                  movcuenta mc ON mcb.mc_movimiento = mc.mc_codigo
                INNER JOIN
                  cuentasbco cb ON mc.mc_cuenta = cb.cb_codigo
                INNER JOIN
                  tipomovbco tmb ON mc.mc_tipo = tmb.tm_codigo
                LEFT JOIN
                  detalle_mov_cheque dmc2 ON dmc2.dmc_mov = mc.mc_codigo
                {where} AND mc.mc_tipo NOT IN (3)
                ORDER BY
                  mcb.mc_codigo
            ";

      Console.WriteLine(query);
      return await connection.QueryAsync<MovimientoBancarioViewModel>(query, parameters);
    }


    public async Task<IEnumerable<MovimientoBancarioViewModel>> GetChequesPendientes(
        string fechaInicio,
        string fechaFin,
        uint? codigoCuenta,
        string? cheque,
        int? estado,
        int? tipoFecha,
        int? checkSaldo,
        int? situacion,
        string? busqueda,
        int? aplicado,
        int? guardarCobroTarjeta,
        int? chequeTransferencia
    )
    {
      var parameters = new DynamicParameters();

      var where = "";

      using var connection = GetConnection();



      if (fechaInicio != null && fechaFin != null)
      {
        where += " AND dmc.dmc_fechap BETWEEN @FechaInicio AND @FechaFin";
        parameters.Add("@FechaInicio", fechaInicio);
        parameters.Add("@FechaFin", fechaFin);
      }
      else
      {
        if (tipoFecha == 3)
        {
          where += " AND (SELECT tsc.c_f_conciliado AS fcon FROM movimientoscuentabco tsC WHERE tsc.mc_movimiento = mc.mc_codigo  ) BETWEEN @FechaInicio AND @FechaFin";
          parameters.Add("@FechaInicio", fechaInicio);
          parameters.Add("@FechaFin", fechaFin);
        }
        else
        {
          where += " AND mc.mc_fecha BETWEEN @FechaInicio AND @FechaFin";
          parameters.Add("@FechaInicio", fechaInicio);
          parameters.Add("@FechaFin", fechaFin);
        }
      }

      if (codigoCuenta.HasValue)
      {
        where += " AND mc.mc_cuenta = @CodigoCuenta";
        parameters.Add("@CodigoCuenta", codigoCuenta);
      }


      if (!string.IsNullOrEmpty(cheque))
      {
        where += " AND dc.dc_numero = @Cheque";
        parameters.Add("@Cheque", cheque);
      }

      if (situacion == 1)
      {
        where += " AND dmc.dmc_situacion = 1";
      }
      else if (situacion == 2)
      {
        where += " AND dmc.dmc_situacion = 0";
      }

      if (!string.IsNullOrEmpty(busqueda))
      {
        where += " AND dmc.dmc_orden LIKE @Busqueda";
        parameters.Add("@Busqueda", $"%{busqueda}%");
      }

      if (aplicado == 1)
      {
        where += " AND dmc.dmc_aplicado = 0";
      }

      if (guardarCobroTarjeta == 0 && chequeTransferencia == 0)
      {
        where += @"
              AND IFNULL(
                IF(
                  (
                    SELECT
                      cb.c_codigo
                    FROM
                      conciliacion_bancaria
                    INNER JOIN
                      detalle_conciliacion dccc ON dccc.d_conciliacion = cb.c_codigo
                    INNER JOIN
                      movimientoscuentabco mb ON dccc.d_referencia_mov = mb.mc_codigo
                    INNER JOIN
                      movcuenta cbc ON mb.mc_movimiento = cbc.mc_codigo
                    WHERE
                      cbc.mc_codigo = mc.mc_codigo
                    AND
                      cb.c_estado = 1
                    AND
                      mb.mc_conciliado = 1
                    LIMIT 1
                  ) > 0 , 1, 0
                ), 0
              ) = 1
            ";
          }

      var query = $@"
            SELECT
		          dmc.dmc_movimiento AS McMovimiento,
		          mc.mc_cuenta AS McCuenta,
		          cb.cb_descripcion AS McDescripcion,
		          mc.mc_fecha AS McFecha,
		          IFNULL((SELECT
		        			    IF(tsc.mc_f_conciliado = '0001-01-01','',DATE_FORMAT(tsc.mc_f_conciliado, '%d/%m/%Y')) AS fcon
		        			FROM
		        			    movimientoscuentabco tsc
		        			WHERE
		        			    tsc.mc_movimiento = mc.mc_codigo LIMIT 1), "") AS McFechaConciliado,
		          dmc.dmc_FechaP AS McFechaP,
		          dmc.dmc_importe AS McImporte,
		          dmc.dmc_orden AS McOrden,
		          dmc.dmc_estado AS McEstado,
		          dc.dc_numero AS McNumero,
		          dmc.dmc_cheque AS McCheque,
		          0 AS McReferencia,
		          mc.mc_codigo AS McCodigoMovimiento,
		          dmc.dmc_codigo AS McCodigoDetalle,
		          dmc.dmc_situacion AS McSituacion,
		          2 AS McTipoMovimiento,
		          IFNULL(IF((SELECT
		        			    cb.c_codigo
		        			FROM
		        			    conciliacion_bancaria cb INNER JOIN
		        			    detalle_conciliacion dccc ON dccc.d_conciliacion = cb.c_codigo INNER JOIN
		        			    movimientoscuentabco mb ON dccc.d_referencia_mov = mb.mc_codigo INNER JOIN
		        			    movcuenta cbc ON mb.mc_movimiento = cbc.mc_codigo
		        			WHERE
		        			    cbc.mc_codigo = mc.mc_codigo AND
		        			    cb.c_estado = 1 AND
		        			    mb.mc_conciliado = 1
		        			LIMIT 1) > 0, 1 , 0), 0) AS McConciliado
		        FROM
		          detalle_mov_chequera dmc INNER JOIN
		          movcuenta mc ON dmc.dmc_movimiento = mc.mc_codigo INNER JOIN
		          cuentasbco cb ON mc.mc_cuenta = cb.cb_codigo LEFT JOIN
		          detalle_chequera dc ON dmc.dmc_cheque = dc.dc_codigo
		        WHERE
		          1=1
		        {where}
		        ORDER BY
		      dmc.dmc_codigo  
        ";
      Console.WriteLine(query);
      return await connection.QueryAsync<MovimientoBancarioViewModel>(query, parameters);
    }


    public async Task<IEnumerable<CuentaBancariaViewModel>> ConsultarCuentasBancarias(
        int? Estado,
        uint? Moneda
    )
    {
      var query = from cuenta in _context.CuentasBancarias
                  join banco in _context.Bancos on cuenta.Banco equals banco.Codigo
                  join moneda in _context.Moneda on cuenta.Moneda equals moneda.MoCodigo
                  join titular in _context.Clientes on cuenta.Titular equals (uint)titular.Codigo
                  where (Estado == null || cuenta.Estado == Estado) &&
                        (Moneda == null || cuenta.Moneda == Moneda)
                  select new CuentaBancariaViewModel
                  {
                    Codigo = cuenta.Codigo,
                    Banco = cuenta.Banco,
                    Descripcion = cuenta.Descripcion,
                    Fecha = cuenta.Fecha,
                    Moneda = cuenta.Moneda,
                    Titular = cuenta.Titular,
                    Contacto = cuenta.Contacto,
                    Mail = cuenta.Mail,
                    Telefono = cuenta.Telefono,
                    Observacion = cuenta.Observacion,
                    Saldo = cuenta.Saldo,
                    Estado = cuenta.Estado,
                    Aplicar = cuenta.Aplicar,
                    Plan = cuenta.Plan,
                    BancoDescripcion = banco.Descripcion,
                    TitularDescripcion = titular.Razon,
                    MonedaDescripcion = moneda.MoDescripcion
                  };

      return await query.ToListAsync();
    }
  }
}
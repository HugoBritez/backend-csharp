using Api.Data;
using Api.Models.Dtos.Banco;
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
      var where = "WHERE 1 = 1";
      using var connection = GetConnection();

      // CORREGIR: Filtro de cuenta como en FoxPro
      if (codigoCuenta.HasValue && codigoCuenta.Value != 0)
      {
        where += " AND mc.mc_cuenta = @CodigoCuenta";
        parameters.Add("@CodigoCuenta", codigoCuenta.Value);
      }

      // CORREGIR: Lógica de fechas como en FoxPro (DO CASE)
      if (!string.IsNullOrEmpty(fechaInicio) && !string.IsNullOrEmpty(fechaFin))
      {
        if (tipoFecha == 1)
        {
          where += " AND mc.mc_fecha BETWEEN @FechaInicio AND @FechaFin";
        }
        else if (tipoFecha == 2)
        {
          where += " AND dmc2.dmc_fechaC BETWEEN @FechaInicio AND @FechaFin";
        }
        else if (tipoFecha == 3)
        {
          where += " AND mcb.mc_f_conciliado BETWEEN @FechaInicio AND @FechaFin";
        }
        parameters.Add("@FechaInicio", fechaInicio);
        parameters.Add("@FechaFin", fechaFin);
      }

      // CORREGIR: Filtro de cheque comentado como en FoxPro
      // if (!string.IsNullOrEmpty(cheque))
      // {
      //     where += " AND dc.dc_numero = @Cheque";
      //     parameters.Add("@Cheque", cheque);
      // }

      // CORREGIR: Filtro de estado como en FoxPro
      if (estado.HasValue && estado.Value != 2)
      {
        where += " AND IFNULL(dmc2.dmc_estado, mc.mc_estado) = @Estado";
        parameters.Add("@Estado", estado.Value);
      }

      // // CORREGIR: Filtro de conciliación como en FoxPro
      // if (guardarCobroTarjeta == 0 && chequeTransferencia == 0)
      // {
      //   where += " AND mcb.mc_conciliado = 1";
      // }

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
                  0  as McSaldo,
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
                  IFNULL(dmc2.dmc_estado, mc.mc_estado) as McEstado,
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
                                      dm1.dccm_codigo
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
                {where} 
                AND mc.mc_tipo NOT IN (3)
                ORDER BY
                  mcb.mc_codigo
            ";

      return await connection.QueryAsync<MovimientoBancarioViewModel>(query, parameters);
    }


    public async Task<IEnumerable<ChequeViewModel>> GetChequesPendientes(
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
        if (checkSaldo == 0)
        {
          where += " AND dmc.dmc_fechap BETWEEN @FechaInicio AND @FechaFin";
        }
        else
        {
          if (tipoFecha == 3)
          {
            where += " AND (SELECT tsc.mc_f_conciliado FROM movimientoscuentabco tsc WHERE tsc.mc_movimiento = mc.mc_codigo) BETWEEN @FechaInicio AND @FechaFin";
          }
          else
          {
            where += " AND mc.mc_fecha BETWEEN @FechaInicio AND @FechaFin";
          }
        }
        parameters.Add("@FechaInicio", fechaInicio);
        parameters.Add("@FechaFin", fechaFin);
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

      if (estado.HasValue && estado != 3)
      {
        where += " AND dmc.dmc_estado = @Estado";
        parameters.Add("@Estado", estado.Value );
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

       if (guardarCobroTarjeta == 1 && chequeTransferencia == 1)
       {
         where += @"
               AND IFNULL(
                 IF(
                   (
                     SELECT
                       cb.c_codigo
                     FROM
                       conciliacion_bancaria cb
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
                IFNULL(
                    (SELECT
                        IF(tsc.mc_f_conciliado = '0001-01-01', '', DATE_FORMAT(tsc.mc_f_conciliado, '%d/%m/%Y'))
                    FROM
                        movimientoscuentabco tsc
                    WHERE
                        tsc.mc_movimiento = mc.mc_codigo 
                    LIMIT 1), 
                    ''
                ) AS McFechaConciliado,
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
                IFNULL(
                    IF(
                        (SELECT
                            cb.c_codigo
                        FROM
                            conciliacion_bancaria cb 
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
                        LIMIT 1) > 0, 1, 0
                    ), 0
                ) AS McConciliado
            FROM
                detalle_mov_chequera dmc 
            INNER JOIN
                movcuenta mc ON dmc.dmc_movimiento = mc.mc_codigo 
            INNER JOIN
                cuentasbco cb ON mc.mc_cuenta = cb.cb_codigo 
            LEFT JOIN
                detalle_chequera dc ON dmc.dmc_cheque = dc.dc_codigo
            WHERE
                1=1
                {where}
            ORDER BY
                dmc.dmc_codigo
        ";

      return await connection.QueryAsync<ChequeViewModel>(query, parameters);
    }

    public async Task<IEnumerable<CuentaBancariaViewModel>> ConsultarCuentasBancarias(
        int? Estado,
        uint? Moneda
    )
    {
      var query = from cuenta in _context.CuentasBancarias
                  join banco in _context.Bancos on cuenta.Banco equals banco.Codigo
                  join moneda in _context.Moneda on cuenta.Moneda equals moneda.MoCodigo
                  join titular in _context.Clientes on cuenta.Titular equals titular.Codigo
                  join plan in _context.PlanDeCuentasSET on cuenta.Plan equals (uint)plan.Codigo

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
                    MonedaDescripcion = moneda.MoDescripcion,
                    CuentaNombre = plan.Titulo ?? ""
                  };

      return await query.ToListAsync();
    }


    public async Task<decimal> CalcularDebeCuentabancaria(
        uint codigoCuenta,
        string? fechaInicio,
        string? fechaFin,
        int? situacion,
        int? checkSaldo,
        int? guardarCobroTarjeta,
        int? chequeTransferencia
    )
    {
        if (string.IsNullOrEmpty(fechaInicio) || string.IsNullOrEmpty(fechaFin))
            return 0;

        using var connection = GetConnection();
        var parameters = new DynamicParameters();
        Console.WriteLine("CALCULAR DEBE CUENTA BANCARIA");
        
        // Construir comando base como en FoxPro
        var comando = "WHERE 1 = 1";
        comando += " AND mc.mc_cuenta = @CodigoCuenta";
        comando += " AND mc.mc_fecha < @FechaInicio";
        
        parameters.Add("@CodigoCuenta", codigoCuenta);
        parameters.Add("@FechaInicio", fechaInicio);

        // Condición de conciliación
        if (guardarCobroTarjeta == 0 && chequeTransferencia == 0)
        {
            comando += " AND mcc.mc_conciliado = 1";
        }

        var query = $@"
            SELECT
                IFNULL(SUM(mcc.mc_debe), 0.00) as saldo
            FROM
                movimientoscuentabco mcc 
                INNER JOIN movcuenta mc ON mcc.mc_movimiento = mc.mc_codigo
            {comando} 
                AND mc.mc_estado = 1 
                AND mc.mc_tipo IN (1, 6)
        ";

        Console.WriteLine(query);

        return await connection.QueryFirstOrDefaultAsync<decimal>(query, parameters);
    }

    public async Task<decimal> CalcularHaberCuentabancaria(
        uint codigoCuenta,
        string? fechaInicio,
        string? fechaFin,
        int? situacion,
        int? checkSaldo,
        int? guardarCobroTarjeta,
        int? chequeTransferencia
    )
    {
        if (string.IsNullOrEmpty(fechaInicio) || string.IsNullOrEmpty(fechaFin))
            return 0;

        using var connection = GetConnection();
        var parameters = new DynamicParameters();

        Console.WriteLine("CALCULAR HABER CUENTA BANCARIA");

        // Construir comando base como en FoxPro
        var comando = "WHERE 1 = 1";
        comando += " AND mc.mc_cuenta = @CodigoCuenta";
        comando += " AND mc.mc_fecha < @FechaInicio";
        
        parameters.Add("@CodigoCuenta", codigoCuenta);
        parameters.Add("@FechaInicio", fechaInicio);

        // Condición de conciliación
        if (guardarCobroTarjeta == 0 && chequeTransferencia == 0)
        {
            comando += " AND mcc.mc_conciliado = 1";
        }

        var query = $@"
            SELECT
                IFNULL(SUM(mcc.mc_haber), 0.00) as saldo
            FROM
                movimientoscuentabco mcc 
                INNER JOIN movcuenta mc ON mcc.mc_movimiento = mc.mc_codigo
            {comando} 
                AND mc.mc_estado = 1 
                AND mc.mc_tipo IN (2, 5)
        ";

        Console.WriteLine(query);

        return await connection.QueryFirstOrDefaultAsync<decimal>(query, parameters);
    }

    public async Task<decimal> CalcularChequeCuentabancaria(
        uint codigoCuenta,
        string? fechaInicio,
        string? fechaFin,
        int? situacion,
        int? checkSaldo,
        int? guardarCobroTarjeta,
        int? chequeTransferencia
    )
    {
        if (string.IsNullOrEmpty(fechaInicio) || string.IsNullOrEmpty(fechaFin))
            return 0;

        using var connection = GetConnection();
        var parameters = new DynamicParameters();

        Console.WriteLine("CALCULAR CHEQUE CUENTA BANCARIA");

        // Construir comando2 base como en FoxPro
        var comando2 = "WHERE 1 = 1";
        comando2 += " AND mc.mc_cuenta = @CodigoCuenta";
        
        parameters.Add("@CodigoCuenta", codigoCuenta);

        // Condición de situación
        if (situacion == 1)
        {
            comando2 += " AND dmvc.dmc_situacion = 1";
        }
        else if (situacion == 2)
        {
            comando2 += " AND dmvc.dmc_situacion = 0";
        }

        // Condición de fecha según checkSaldo
        if (checkSaldo == 0)
        {
            comando2 += " AND dmvc.dmc_fechap < @FechaInicio";
        }
        else
        {
            comando2 += " AND mc.mc_fecha < @FechaInicio";
        }
        
        parameters.Add("@FechaInicio", fechaInicio);

        // Condición de conciliación
        if (guardarCobroTarjeta == 0 && chequeTransferencia == 0)
        {
            comando2 += @" AND IFNULL(IF((
                SELECT cb.c_codigo
                FROM conciliacion_bancaria cb 
                INNER JOIN detalle_conciliacion dccc ON dccc.d_conciliacion = cb.c_codigo 
                INNER JOIN movimientoscuentabco mb ON dccc.d_referencia_mov = mb.mc_codigo 
                INNER JOIN movcuenta cbc ON mb.mc_movimiento = cbc.mc_codigo
                WHERE cbc.mc_codigo = mc.mc_codigo 
                AND cb.c_estado = 1 
                AND mb.mc_conciliado = 1
                LIMIT 1) > 0, 1, 0), 0) = 1";
        }

        var query = $@"
            SELECT
                IFNULL(SUM(dmvc.dmc_importe), 0.00) as saldochepro
            FROM
                movcuenta mc 
                INNER JOIN detalle_mov_chequera dmvc ON mc.mc_codigo = dmvc.dmc_movimiento
            {comando2} 
                AND dmvc.dmc_situacion = 1 
                AND dmvc.dmc_estado = 1
        ";

        Console.WriteLine(query);

        return await connection.QueryFirstOrDefaultAsync<decimal>(query, parameters);
    }

    public async Task<decimal> CalcularChequeRecibidoCuentabancaria(
        uint codigoCuenta,
        string? fechaInicio,
        string? fechaFin,
        int? situacion,
        int? checkSaldo,
        int? guardarCobroTarjeta,
        int? chequeTransferencia
    )
    {
        if (string.IsNullOrEmpty(fechaInicio) || string.IsNullOrEmpty(fechaFin))
            return 0;

        using var connection = GetConnection();
        var parameters = new DynamicParameters();

        Console.WriteLine("CALCULAR CHEQUE RECIBIDO CUENTA BANCARIA");

        // Construir comando base como en FoxPro
        var comando = "WHERE 1 = 1";
        comando += " AND mc.mc_cuenta = @CodigoCuenta";
        comando += " AND mc.mc_fecha < @FechaInicio";
        
        parameters.Add("@CodigoCuenta", codigoCuenta);
        parameters.Add("@FechaInicio", fechaInicio);

        // Condición de conciliación
        if (guardarCobroTarjeta == 0 && chequeTransferencia == 0)
        {
            comando += " AND mcc.mc_conciliado = 1";
        }

        var query = $@"
            SELECT
                IFNULL(SUM(dmch.dmc_monto), 0.00) as saldo
            FROM
                movimientoscuentabco mcc 
                INNER JOIN movcuenta mc ON mcc.mc_movimiento = mc.mc_codigo  
                INNER JOIN detalle_mov_cheque dmch ON mc.mc_codigo = dmch.dmc_mov
            {comando} 
                AND dmch.dmc_estado = 0 
                AND mc.mc_tipo IN (1)
        ";

        Console.WriteLine(query);

        return await connection.QueryFirstOrDefaultAsync<decimal>(query, parameters);
    }
  }
}


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
                WHERE 1 = 1 AND mc.mc_cuenta = 1 AND mc.mc_fecha BETWEEN '2025-07-28' AND '2025-08-07' AND IFNULL(dmc2.dmc_estado, mc.mc_estado) = 1
                AND mc.mc_tipo NOT IN (3)
                ORDER BY
                  mcb.mc_codigo


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
                WHERE 1 = 1 AND mc.mc_cuenta = 1 AND mc.mc_fecha BETWEEN '2025-07-28' AND '2025-08-07' AND IFNULL(dmc2.dmc_estado, mc.mc_estado) = 1 AND mcb.mc_conciliado = 1
                AND mc.mc_tipo NOT IN (3)
                ORDER BY
                  mcb.mc_codigo


Select
dmc.dmc_movimiento,
mc.mc_cuenta,
cb.cb_descripcion,
mc.mc_fecha as dmc_FechaE,
IfNull((Select
if(tsc.mc_f_conciliado = '0001-01-01','',Date_Format(tsc.mc_f_conciliado, "%d/%m/%Y")) As fcon
From
movimientoscuentabco tsc
Where
tsc.mc_movimiento = mc.mc_codigo Limit 1), "") As fconciliado,
dmc.dmc_FechaP,
dmc.dmc_importe,
dmc.dmc_orden,
dmc.dmc_estado,
dc.dc_numero,
dmc.dmc_cheque,
0 as ref,
mc.mc_codigo,
dmc.dmc_codigo,
dmc.dmc_situacion,
2 as tmovimiento,
ifnull(if((Select
cb.c_codigo
From
conciliacion_bancaria cb Inner Join
detalle_conciliacion dccc On dccc.d_conciliacion = cb.c_codigo Inner Join
movimientoscuentabco mb On dccc.d_referencia_mov = mb.mc_codigo Inner Join
movcuenta cbc On mb.mc_movimiento = cbc.mc_codigo
Where
cbc.mc_codigo = mc.mc_codigo And
cb.c_estado = 1 And
mb.mc_conciliado = 1
Limit 1) > 0, 1 , 0), 0) as conc
From
detalle_mov_chequera dmc Inner Join
movcuenta mc On dmc.dmc_movimiento = mc.mc_codigo Inner Join
cuentasbco cb On mc.mc_cuenta = cb.cb_codigo Left Join
detalle_chequera dc On dmc.dmc_cheque = dc.dc_codigo
Where
1=1
 and dmc.dmc_fechap between '20250727' and '20250807' and mc.mc_cuenta = 1 And dmc.dmc_estado= 1 And dmc.dmc_situacion  = 1
Order By
dmc.dmc_codigo

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
                WHERE 1 = 1 AND mc.mc_cuenta = @CodigoCuenta AND mc.mc_fecha BETWEEN @FechaInicio AND @FechaFin AND IFNULL(dmc2.dmc_estado, mc.mc_estado) = @Estado AND mcb.mc_conciliado = 1
                AND mc.mc_tipo NOT IN (3)
                ORDER BY
                  mcb.mc_codigo



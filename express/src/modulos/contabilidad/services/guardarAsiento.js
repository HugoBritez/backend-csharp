
const db = require('../../../DB/mysql.js');
const { obtenerConfigAsiento } = require("../utils/obtenerConfigAsiento");
const { quitarComas } = require("../utils/quitarComas");
const { redondear } = require("../utils/redondearNumero");
const { buscarCodigoPlanCuentaCajaDef } = require("../utils/buscarCodigoPlanCuentaDef");

 const guardarAsientoAutomatico = async (
  params,
) => {
  if (!params.automatico) return;

  // Obtenemos las configuraciones
  const configAsientoVenta = await obtenerConfigAsiento(1); // Ventas S/Factura N°
  const configAsientoVentaComun = await obtenerConfigAsiento(8); // VENTAS S/ NOTA COMUN NRO.

  // Ajustar montos si la moneda es dólares
  let totalExentas = params.totalExentas;
  let total5 = params.total5;
  let total10 = params.total10;
  let totalDebe = params.totalPagar;

  if (params.moneda === 2) {
    totalExentas *= params.cotizacion;
    total5 *= params.cotizacion;
    total10 *= params.cotizacion;
    totalDebe = quitarComas((params.totalPagar * params.cotizacion).toString());
  }

  // Insertar cabecera del asiento
  const queryCabecera = `
    INSERT INTO asiento_contable (
      ac_sucursal,
      ac_moneda,
      ac_operador,
      ac_documento,
      ac_numero,
      ac_fecha,
      ac_fecha_asiento,
      ac_totaldebe,
      ac_totalhaber,
      ac_cotizacion,
      ac_referencia,
      ac_origen
    ) VALUES (
      ${params.sucursal},
      ${params.moneda},
      ${params.operador},
      ${params.factura ? `'${params.factura}'` : params.referencia},
      ${params.numeroAsiento},
      '${params.fecha}',
      '${params.fecha}',
      ${totalDebe},
      ${totalDebe},
      ${params.cotizacion},
      ${params.referencia || ''},
      1
    )
  `;

  const [result] = await db.sql(queryCabecera);
  const idAsiento = result.insertId;

  // Generar concepto del asiento
  const conceptoAsiento = params.factura 
    ? `${configAsientoVenta.con_concepto.trim()} ${params.factura.trim()}`
    : `Nota Interna N°: ${idAsiento}`;

  // Obtener plan de cuenta según tipo de venta
  let planCuenta;
  if (params.tipoVenta === 1) {
    if (params.cajaDefinicion) {
      planCuenta = await buscarCodigoPlanCuentaCajaDef(params.cajaDefinicion);
    } else {
      planCuenta = configAsientoVenta.con_contado;
    }
  } else {
    planCuenta = params.moneda === 1 
      ? configAsientoVenta.con_credito 
      : configAsientoVenta.con_creditod;
  }

  // Insertar detalle de caja/crédito
  const montoDebeCaja = quitarComas((totalExentas + total5 + total10).toString());
  await db.sql(`
    INSERT INTO detalle_asiento_contable (
      dac_asiento,
      dac_plan,
      dac_debe,
      dac_haber,
      dac_concepto
    ) VALUES (
      ${idAsiento},
      ${planCuenta},
      ${montoDebeCaja},
      0,
      '${conceptoAsiento}'
    )
  `);

  // Insertar detalles de IVA y gravadas
  if (total5 > 0) {
    // Gravada 5%
    const planGravada5 = params.imprimirLegal === 1
      ? configAsientoVenta.con_gravada 
      : configAsientoVentaComun.con_gravada;
    const montoGravada5 = quitarComas((total5 - redondear(total5/21)).toString());
    

    await db.sql(`
      INSERT INTO detalle_asiento_contable (
        dac_asiento,
        dac_plan,
        dac_debe,
        dac_haber,
        dac_concepto
      ) VALUES (
        ${idAsiento},
        ${planGravada5},
        0,
        ${montoGravada5},
        '${conceptoAsiento}'
      )
    `);

    // IVA 5%
    const planIva5 = params.imprimirLegal === 1
      ? configAsientoVenta.con_iva5 
      : configAsientoVentaComun.con_iva5;
    const montoIva5 = quitarComas(redondear(total5/21).toString());

    await db.sql(`
      INSERT INTO detalle_asiento_contable (
        dac_asiento,
        dac_plan,
        dac_debe,
        dac_haber,
        dac_concepto
      ) VALUES (
        ${idAsiento},
        ${planIva5},
        0,
        ${montoIva5},
        '${conceptoAsiento}'
      )
    `);
  }

  if (total10 > 0) {
    // Gravada 10%
    const planGravada10 = params.imprimirLegal === 1
      ? configAsientoVenta.con_gravada10 
      : configAsientoVentaComun.con_gravada10;
    const montoGravada10 = quitarComas((total10 - redondear(total10/11)).toString());

    await db.sql(`
      INSERT INTO detalle_asiento_contable (
        dac_asiento,
        dac_plan,
        dac_debe,
        dac_haber,
        dac_concepto
      ) VALUES (
        ${idAsiento},
        ${planGravada10},
        0,
        ${montoGravada10},
        '${conceptoAsiento}'
      )
    `);

    // IVA 10%
    const planIva10 = params.imprimirLegal === 1
      ? configAsientoVenta.con_iva10 
      : configAsientoVentaComun.con_iva10;
    const montoIva10 = quitarComas(redondear(total10/11).toString());

    await db.sql(`
      INSERT INTO detalle_asiento_contable (
        dac_asiento,
        dac_plan,
        dac_debe,
        dac_haber,
        dac_concepto
      ) VALUES (
        ${idAsiento},
        ${planIva10},
        0,
        ${montoIva10},
        '${conceptoAsiento}'
      )
    `);
  }

  if (totalExentas > 0) {
    const planExenta = params.imprimirLegal === 1
      ? configAsientoVenta.con_exenta 
      : configAsientoVentaComun.con_exenta;

    await db.sql(`
      INSERT INTO detalle_asiento_contable (
        dac_asiento,
        dac_plan,
        dac_debe,
        dac_haber,
        dac_concepto
      ) VALUES (
        ${idAsiento},
        ${planExenta},
        0,
        ${quitarComas(totalExentas.toString())},
        '${conceptoAsiento}'
      )
    `);
  }

  };

  module.exports = {
    guardarAsientoAutomatico
  }
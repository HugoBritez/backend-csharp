const db = require('../../../DB/mysql');
const { obtenerConfigAsiento } = require('../utils/obtenerConfigAsiento');
const { quitarComas } = require('../utils/quitarComas');
const { generarNumeroAsiento } = require('../utils/generarNroAsiento');

const guardarAsientoCosto = async (
  params,
) => {
  if (!params.automatico) return;

  // Obtenemos las configuraciones
  const configAsientoCosto = await obtenerConfigAsiento(6); // Para costos
  const configAsientoComun = await obtenerConfigAsiento(8); // Para notas comunes

  // Obtenemos el número de asiento
  const numeroAsiento = await generarNumeroAsiento();

  // Ajustar montos si la moneda es dólares
  let costoTotalExenta = params.costoTotalExenta;
  let costoTotalCinco = params.costoTotalCinco;
  let costoTotalDiez = params.costoTotalDiez;
  let totalDebe = quitarComas((costoTotalCinco + costoTotalDiez + costoTotalExenta).toString());
  let cotizacion = 0;

  if (params.moneda === 2) {
    cotizacion = quitarComas(params.monedaDolar.toString());
    costoTotalExenta *= cotizacion;
    costoTotalCinco *= cotizacion;
    costoTotalDiez *= cotizacion;
    totalDebe = quitarComas((costoTotalCinco + costoTotalDiez + costoTotalExenta).toString());
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
      ${numeroAsiento},
      '${params.fecha}',
      '${params.fecha}',
      ${totalDebe},
      ${totalDebe},
      ${cotizacion},
      ${params.referencia || ''},
      16
    )
  `;

  const [result] = await db.sql(queryCabecera);
  const idAsiento = result.insertId;

  // Generar concepto del asiento
  const conceptoAsiento = params.factura 
    ? `${configAsientoCosto.con_concepto.trim()} ${params.factura.trim()}`
    : `Costo de Nota Interna N°: ${idAsiento}`;

  // Insertar detalles de gravadas
  const totalSumaGrav = costoTotalCinco + costoTotalDiez;
  if (totalSumaGrav > 0) {
    const planCuenta = params.imprimirLegal 
      ? configAsientoCosto.con_gravada 
      : configAsientoComun.con_contado;
    
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
        ${quitarComas((costoTotalCinco + costoTotalDiez).toString())},
        0,
        '${conceptoAsiento}'
      )
    `);
  }

  // Insertar detalles de exentas
  if (costoTotalExenta > 0) {
    const planCuenta = params.imprimirLegal 
      ? configAsientoCosto.con_gravada10 
      : configAsientoComun.con_gravada10;

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
        ${quitarComas(costoTotalExenta.toString())},
        0,
        '${conceptoAsiento}'
      )
    `);
  }

  // Insertar detalles de IVA 5%
  if (costoTotalCinco > 0) {
    await db.sql(`
      INSERT INTO detalle_asiento_contable (
        dac_asiento,
        dac_plan,
        dac_debe,
        dac_haber,
        dac_concepto
      ) VALUES (
        ${idAsiento},
        ${configAsientoCosto.con_iva5},
        0,
        ${quitarComas(costoTotalCinco.toString())},
        '${conceptoAsiento}'
      )
    `);
  }

  // Insertar detalles de IVA 10%
  if (costoTotalDiez > 0) {
    await db.sql(`
      INSERT INTO detalle_asiento_contable (
        dac_asiento,
        dac_plan,
        dac_debe,
        dac_haber,
        dac_concepto
      ) VALUES (
        ${idAsiento},
        ${configAsientoCosto.con_iva10},
        0,
        ${quitarComas(costoTotalDiez.toString())},
        '${conceptoAsiento}'
      )
    `);
  }

  // Insertar detalles de exentas
  if (costoTotalExenta > 0) {
    await db.sql(`
      INSERT INTO detalle_asiento_contable (
        dac_asiento,
        dac_plan,
        dac_debe,
        dac_haber,
        dac_concepto
      ) VALUES (
        ${idAsiento},
        ${configAsientoCosto.con_exenta},
        0,
        ${quitarComas(costoTotalExenta.toString())},
        '${conceptoAsiento}'
      )
    `);
  }

  return idAsiento;
};

module.exports = {
  guardarAsientoCosto
}
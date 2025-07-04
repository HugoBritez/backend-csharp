"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.guardarAsientoAutomatico = void 0;
const db = require("../../../DB/mysql");
const obtenerConfigAsiento_1 = require("../utils/obtenerConfigAsiento");
const quitarComas_1 = require("../utils/quitarComas");
const redondearNumero_1 = require("../utils/redondearNumero");
const buscarCodigoPlanCuentaDef_1 = require("../utils/buscarCodigoPlanCuentaDef");
const guardarAsientoAutomatico = (params) => __awaiter(void 0, void 0, void 0, function* () {
    if (!params.automatico)
        return;
    // Obtenemos las configuraciones
    const configAsientoVenta = yield (0, obtenerConfigAsiento_1.obtenerConfigAsiento)(1); // Ventas S/Factura N°
    const configAsientoVentaComun = yield (0, obtenerConfigAsiento_1.obtenerConfigAsiento)(8); // VENTAS S/ NOTA COMUN NRO.
    // Ajustar montos si la moneda es dólares
    let totalExentas = params.totalExentas;
    let total5 = params.total5;
    let total10 = params.total10;
    let totalDebe = params.totalPagar;
    if (params.moneda === 2) {
        totalExentas *= params.cotizacion;
        total5 *= params.cotizacion;
        total10 *= params.cotizacion;
        totalDebe = (0, quitarComas_1.quitarComas)((params.totalPagar * params.cotizacion).toString());
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
    const [result] = yield db.sql(queryCabecera);
    const idAsiento = result.insertId;
    // Generar concepto del asiento
    const conceptoAsiento = params.factura
        ? `${configAsientoVenta.con_concepto.trim()} ${params.factura.trim()}`
        : `Nota Interna N°: ${idAsiento}`;
    // Obtener plan de cuenta según tipo de venta
    let planCuenta;
    if (params.tipoVenta === 0) {
        if (params.cajaDefinicion) {
            planCuenta = yield (0, buscarCodigoPlanCuentaDef_1.buscarCodigoPlanCuentaCajaDef)(params.cajaDefinicion);
        }
        else {
            planCuenta = configAsientoVenta.con_contado;
        }
    }
    else {
        planCuenta = params.moneda === 1
            ? configAsientoVenta.con_credito
            : configAsientoVenta.con_creditod;
    }
    // Insertar detalle de caja/crédito
    const montoDebeCaja = (0, quitarComas_1.quitarComas)((totalExentas + total5 + total10).toString());
    yield db.sql(`
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
        const planGravada5 = params.imprimirLegal
            ? configAsientoVenta.con_gravada
            : configAsientoVentaComun.con_gravada;
        const montoGravada5 = (0, quitarComas_1.quitarComas)((total5 - (0, redondearNumero_1.redondear)(total5 / 21)).toString());
        yield db.sql(`
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
        const planIva5 = params.imprimirLegal
            ? configAsientoVenta.con_iva5
            : configAsientoVentaComun.con_iva5;
        const montoIva5 = (0, quitarComas_1.quitarComas)((0, redondearNumero_1.redondear)(total5 / 21).toString());
        yield db.sql(`
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
        const planGravada10 = params.imprimirLegal
            ? configAsientoVenta.con_gravada10
            : configAsientoVentaComun.con_gravada10;
        const montoGravada10 = (0, quitarComas_1.quitarComas)((total10 - (0, redondearNumero_1.redondear)(total10 / 11)).toString());
        yield db.sql(`
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
        const planIva10 = params.imprimirLegal
            ? configAsientoVenta.con_iva10
            : configAsientoVentaComun.con_iva10;
        const montoIva10 = (0, quitarComas_1.quitarComas)((0, redondearNumero_1.redondear)(total10 / 11).toString());
        yield db.sql(`
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
        const planExenta = params.imprimirLegal
            ? configAsientoVenta.con_exenta
            : configAsientoVentaComun.con_exenta;
        yield db.sql(`
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
        ${(0, quitarComas_1.quitarComas)(totalExentas.toString())},
        '${conceptoAsiento}'
      )
    `);
    }
    return idAsiento;
});
exports.guardarAsientoAutomatico = guardarAsientoAutomatico;

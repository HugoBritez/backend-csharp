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
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.guardarAsientoCosto = void 0;
const mysql_js_1 = __importDefault(require("../../../DB/mysql.js"));
const obtenerConfigAsiento_1 = require("../utils/obtenerConfigAsiento");
const quitarComas_1 = require("../utils/quitarComas");
const generarNumeroAsiento_1 = require("../utils/generarNumeroAsiento");
const guardarAsientoCosto = (params) => __awaiter(void 0, void 0, void 0, function* () {
    if (!params.automatico)
        return;
    // Obtenemos las configuraciones
    const configAsientoCosto = yield (0, obtenerConfigAsiento_1.obtenerConfigAsiento)(6); // Para costos
    const configAsientoComun = yield (0, obtenerConfigAsiento_1.obtenerConfigAsiento)(8); // Para notas comunes
    // Obtenemos el número de asiento
    const numeroAsiento = yield (0, generarNumeroAsiento_1.generarNumeroAsiento)();
    // Ajustar montos si la moneda es dólares
    let costoTotalExenta = params.costoTotalExenta;
    let costoTotalCinco = params.costoTotalCinco;
    let costoTotalDiez = params.costoTotalDiez;
    let totalDebe = (0, quitarComas_1.quitarComas)((costoTotalCinco + costoTotalDiez + costoTotalExenta).toString());
    let cotizacion = 0;
    if (params.moneda === 2) {
        cotizacion = (0, quitarComas_1.quitarComas)(params.monedaDolar.toString());
        costoTotalExenta *= cotizacion;
        costoTotalCinco *= cotizacion;
        costoTotalDiez *= cotizacion;
        totalDebe = (0, quitarComas_1.quitarComas)((costoTotalCinco + costoTotalDiez + costoTotalExenta).toString());
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
    const [result] = yield mysql_js_1.default.sql(queryCabecera);
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
        yield mysql_js_1.default.sql(`
      INSERT INTO detalle_asiento_contable (
        dac_asiento,
        dac_plan,
        dac_debe,
        dac_haber,
        dac_concepto
      ) VALUES (
        ${idAsiento},
        ${planCuenta},
        ${(0, quitarComas_1.quitarComas)((costoTotalCinco + costoTotalDiez).toString())},
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
        yield mysql_js_1.default.sql(`
      INSERT INTO detalle_asiento_contable (
        dac_asiento,
        dac_plan,
        dac_debe,
        dac_haber,
        dac_concepto
      ) VALUES (
        ${idAsiento},
        ${planCuenta},
        ${(0, quitarComas_1.quitarComas)(costoTotalExenta.toString())},
        0,
        '${conceptoAsiento}'
      )
    `);
    }
    // Insertar detalles de IVA 5%
    if (costoTotalCinco > 0) {
        yield mysql_js_1.default.sql(`
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
        ${(0, quitarComas_1.quitarComas)(costoTotalCinco.toString())},
        '${conceptoAsiento}'
      )
    `);
    }
    // Insertar detalles de IVA 10%
    if (costoTotalDiez > 0) {
        yield mysql_js_1.default.sql(`
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
        ${(0, quitarComas_1.quitarComas)(costoTotalDiez.toString())},
        '${conceptoAsiento}'
      )
    `);
    }
    // Insertar detalles de exentas
    if (costoTotalExenta > 0) {
        yield mysql_js_1.default.sql(`
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
        ${(0, quitarComas_1.quitarComas)(costoTotalExenta.toString())},
        '${conceptoAsiento}'
      )
    `);
    }
    return idAsiento;
});
exports.guardarAsientoCosto = guardarAsientoCosto;

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
const TABLA = "caja";
const TABLA_DETALLE = "detalle_caja";
const TABLA_OPERACIONES = "operacion_caja";
module.exports = function (dbInyectada) {
    let db = dbInyectada;
    if (!db) {
        db = require("../../../DB/mysql");
    }
    function traerCajas() {
        return __awaiter(this, void 0, void 0, function* () {
            const cajas = yield db.sql(`SELECT * FROM cajadef WHERE cd_estado = 1`);
            return cajas;
        });
    }
    function CerrarCaja(req) {
        return __awaiter(this, void 0, void 0, function* () {
            const { ca_codigo, ca_fecha_cierre, ca_hora_cierre, ca_saldo_final } = req.body;
            if (!ca_codigo) {
                throw new Error("Invalid request body: ca_codigo is required");
            }
            console.log("cerrando caja con los siguientes datos: ", req.body);
            try {
                const cajaResult = yield db.actualizar(TABLA, {
                    ca_fecha_cierre: ca_fecha_cierre,
                    ca_saldofin: ca_saldo_final,
                    ca_horafin: ca_hora_cierre,
                    ca_estado: 1,
                }, ca_codigo, "ca_codigo");
                return cajaResult;
            }
            catch (err) {
                throw new Error(err.message);
            }
        });
    }
    function IniciarCaja(req) {
        return __awaiter(this, void 0, void 0, function* () {
            const { caja } = req.body;
            console.log("iniciando caja con los siguientes datos: ", caja);
            try {
                const cajaResult = yield db.agregar(TABLA, {
                    ca_fecha: caja.fecha,
                    ca_fecha_cierre: null,
                    ca_operador: caja.operador,
                    ca_definicion: caja.definicion,
                    ca_saldoini: caja.saldo_inicial,
                    ca_saldofin: null,
                    ca_horaini: caja.hora_inicio,
                    ca_horafin: null,
                    ca_estado: 0,
                    ca_situacion: 1,
                    ca_sucursal: caja.sucursal,
                    ca_area: caja.area,
                    ca_moneda: caja.moneda,
                    ca_tipo_caja: caja.tipo_caja,
                    ca_prioridad: caja.prioridad,
                    ca_turno: caja.turno,
                    ca_clinica: 0,
                    ca_transferido: 0,
                    ca_plan: 0,
                }, 0, "ca_codigo");
                return cajaResult;
            }
            catch (err) {
                throw new Error(err.message);
            }
        });
    }
    function VerificarCajaAbierta(operadorId) {
        return __awaiter(this, void 0, void 0, function* () {
            const query = `SELECT * FROM ${TABLA} WHERE ca_operador = ${operadorId} AND ca_estado = 0  AND ca_fecha_cierre IS NULL order by ca_fecha desc limit 1`;
            try {
                const cajaAbierta = yield db.sql(query);
                return cajaAbierta;
            }
            catch (err) {
                throw new Error(err.message);
            }
        });
    }
    function insertarOperacion(datos) {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                const operacionResult = yield db.agregar(TABLA_OPERACIONES, {
                    oc_caja: datos.caja,
                    oc_cuenta: datos.cuenta,
                    oc_fecha: datos.fecha,
                    oc_obs: datos.observacion,
                    oc_recibo: datos.recibo,
                    oc_documento: datos.documento,
                    oc_operador: datos.operador,
                    oc_redondeo: datos.redondeo,
                }, 0, "op_codigo");
                const operacionId = operacionResult.insertId;
                const detalleResult = yield db.agregar(TABLA_DETALLE, {
                    deca_operacion: operacionId,
                    deca_monto: datos.monto,
                    deca_mora: datos.mora,
                    deca_punitorio: datos.punitorio,
                    deca_descuento: datos.descuento,
                    deca_estado: datos.estado,
                    deca_cod_retencion: datos.cod_retencion,
                }, 0, "cd_codigo");
                const detalleId = detalleResult.insertId;
                yield db.sql(`INSERT INTO detalle_caja_cobro (deco_detalleCaja, deco_venta, deco_cuota) VALUES (${detalleId}, ${datos.ventaId}, 0)`);
                yield db.sql(`UPDATE ventas SET  ve_saldo = 0 WHERE ve_codigo = ${datos.ventaId};`);
                ///for empieza aqui
                yield db.sql(`INSERT INTO detalle_caja_metodo (dcm_deca, dcm_metodo, monto, dcm_cambio, dcm_cotizacion, dcm_cambio_m, referencia, obs, tercero, dcm_monto_cambio)
        VALUES (${detalleId}, ${datos.metodo}, ${datos.monto}, 0.00, 0.00, 0, ${detalleId}, '', 0, 0.00)
        `);
                if ([3, 8, 6, 9].includes(datos.metodo)) {
                    const obsMovCuenta = datos.metodo === 3 ? 'Ingreso por TC' : datos.metodo === 8 ? 'Ingreso por TD' : datos.metodo === 6 ? 'Ingreso por Cheque' : 'Ingreso por transferencia';
                    //mirar configuraciones id 17
                    yield db.sql(`INSERT INTO movcuenta (mc_operador, mc_fecha, mc_tipo, mc_cuenta, mc_comprobante, mc_efectivo, mc_estado, mc_obs, mc_cartera) VALUES
          (${datos.operador}, '${datos.fecha}', ${datos.tipomovimiento}, ${datos.cuenta_bancaria}, ${datos.ventaId}, 0.00, 1, '${obsMovCuenta}', 0)`);
                    const movCuentaId = yield db.sql(`SELECT LAST_INSERT_ID() AS id`);
                    const movCuentaCodigo = movCuentaId[0].id;
                    yield db.sql(`INSERT INTO venta_movcuenta (venta, movcuenta) VALUES (${datos.ventaId}, ${movCuentaId[0].id})`);
                    yield db.sql(`INSERT INTO movimientoscuentabco (mc_movimiento, mc_debe, mc_haber, mc_saldo, mc_conciliado, mc_cod_ref_cheque) VALUES (${movCuentaId[0].id},${datos.monto},
          0.00, 0.00, 0, 0)`);
                    const movimientosctabcoResult = yield db.sql(`SELECT * FROM movimientoscuentabco ORDER BY mc_codigo DESC LIMIT 1`);
                    const movimientosctabcoId = movimientosctabcoResult[0].mc_codigo;
                    if (datos.metodo === 3 || datos.metodo === 8) {
                        yield db.sql(`INSERT INTO cobro_tarjetas ( c_fecha, c_cuenta, c_banco, c_tarjeta, c_titular, c_importe, c_nro_autorizacion, c_autorizacion, c_moneda, c_tipo_tarjeta, c_conciliado, c_estado) VALUES
            ('${datos.fecha}', 1, ${datos.banco}, ${datos.tarjeta}, ${datos.titular}, ${datos.monto}, ${datos.nro_autorizacion}, ${datos.nro_autorizacion}, ${datos.moneda}, ${datos.tarjeta}, 0, 1)`);
                        const cobroTarjetaResult = yield db.sql(`SELECT * FROM cobro_tarjetas ORDER BY c_codigo DESC LIMIT 1`);
                        const codigoTarjeta = cobroTarjetaResult[0].c_codigo;
                        yield db.sql(`INSERT INTO movbancotarjeta (mt_movcuenta, mt_cod_tarjeta, mt_tipo_tarjeta, mt_tipo_sistema) VALUES (${movimientosctabcoId}, ${codigoTarjeta}, ${datos.tipotarjeta}, 1 )`);
                        yield db.sql(`UPDATE detalle_caja_metodo SET referencia = ${codigoTarjeta} WHERE dcm_deca = ${detalleId}`);
                    }
                    if (datos.metodo === 9) {
                        yield db.sql(`
            UPDATE detalle_caja_metodo SET referencia = ${movCuentaCodigo} WHERE dcm_deca = ${detalleId}`);
                    }
                }
                return operacionResult;
            }
            catch (err) {
                throw err;
            }
        });
    }
    return {
        IniciarCaja,
        CerrarCaja,
        VerificarCajaAbierta,
        insertarOperacion,
        traerCajas
    };
};

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
const express = require('express');
const seguridad = require('../../../middleware/seguridad');
const router = express.Router();
const respuesta = require('../../../red/respuestas.js');
const controlador = require('./index.js');
const auth = require('../../../auth/index.js');
router.post('/agregar', seguridad(), agregar);
router.get('/detalles', seguridad(), detalles);
router.post('/consultas', seguridad(), consultas);
router.get('/', seguridad(), uno);
router.post('/agregarPresupuesto', seguridad(), controlador.agregarPresupuesto);
router.post('/confirmarPresupuesto', seguridad(), confirmarPresupuesto);
router.post('/actualizarParcial', seguridad(), actualizarPresupuestoParcial);
router.get('/obtener', seguridad(), obtenerPresupuestosParaVenta);
router.post('/insertar-presupuesto', seguridad(), insertarPresupuesto);
router.get('/recuperar-presupuesto', seguridad(), recuperarPresupuesto);
router.get('/imprimir-presupuesto', seguridad(), imprimirPresupuesto);
function imprimirPresupuesto(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const result = yield controlador.imprimirPresupuesto(req.query.id);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function recuperarPresupuesto(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const result = yield controlador.recuperarPresupuesto(req.query.id);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function insertarPresupuesto(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const presupuesto = req.body.presupuesto;
            const detalle_presupuesto = req.body.detallesPresupuesto;
            const result = yield controlador.insertarPresupuesto(presupuesto, detalle_presupuesto);
            respuesta.success(req, res, result, 200);
            console.log(result);
        }
        catch (err) {
            next(err);
        }
    });
}
function obtenerPresupuestosParaVenta(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const result = yield controlador.obtenerPresupuestosParaVenta(req.query.id);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            console.log(err);
            next(err);
        }
    });
}
function actualizarPresupuestoParcial(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const { codigo, items } = req.body;
            const result = yield controlador.actualizarPresupuestoParcial(codigo, items);
            if (result) {
                respuesta.success(req, res, result, 200);
            }
            else {
                respuesta.error(req, res, 'Error al actualizar presupuesto', 500);
            }
        }
        catch (err) {
            next(err);
        }
    });
}
function confirmarPresupuesto(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const presupuestoId = req.body.id;
            const result = yield controlador.confirmarPresupuesto(presupuestoId);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function uno(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const item = yield controlador.uno(req.query.cod);
            respuesta.success(req, res, item, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function consultas(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log(req.body);
            const bod = req.body;
            const items = yield controlador.getCabeceras(bod.fecha_desde, bod.fecha_hasta, bod.sucursal, bod.cliente, bod.vendedor, bod.articulo, bod.moneda, bod.estado, bod.busqueda);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function detalles(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.getDetalles(req.query.cod);
            respuesta.success(req, res, items, 200);
            console.log(items);
        }
        catch (err) {
            next(err);
        }
    });
}
function agregar(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            //Carga cabecera de presupuesto - Tabla "presupuesto"
            const presupuesto = req.body.presupuesto;
            const total_presupuesto = presupuesto.pre_total;
            delete presupuesto.pre_total;
            const articulos = req.body.tabla;
            //Carga detalle - Para luego cargar tablas "detalle_presupuesto"
            let detalles = [];
            let detalle = null;
            const porc_descuento = ((100 * presupuesto.pre_descuento) / (total_presupuesto + presupuesto.pre_descuento)) / 100;
            let subtotal_detalle = 0;
            if (presupuesto.pre_codigo === 0) { //Es una carga nueva
                let presupuesto_nuevo = yield controlador.agregarCabecera(presupuesto);
                for (i = 0; i < articulos.length; i++) {
                    let descuento_calculado = Math.round((articulos[i].tabla_precio_venta - articulos[i].tabla_descuento) * porc_descuento);
                    detalle = {
                        depre_codigo: 0,
                        depre_presupuesto: presupuesto_nuevo.insertId,
                        depre_articulo: articulos[i].tabla_ar_codigo,
                        depre_cantidad: articulos[i].tabla_cantidad,
                        depre_precio: articulos[i].tabla_precio_venta - articulos[i].tabla_descuento - descuento_calculado,
                        depre_descuento: articulos[i].tabla_descuento + descuento_calculado,
                        depre_exentas: 0,
                        depre_cinco: 0,
                        depre_diez: 0,
                        depre_porcentaje: 0,
                        depre_altura: 0,
                        depre_largura: 0,
                        depre_mts2: 0,
                        depre_listaprecio: articulos[i].tabla_listaprecio,
                        depre_talle: "",
                        depre_codlote: articulos[i].tabla_al_codigo,
                        depre_lote: articulos[i].tabla_lote,
                        depre_vence: articulos[i].tabla_vence,
                        depre_descripcio_art: '',
                        depre_obs: articulos[i].tabla_obs,
                        depre_procesado: 0,
                    };
                    subtotal_detalle = articulos[i].tabla_cantidad * (articulos[i].tabla_precio_venta - articulos[i].tabla_descuento - descuento_calculado);
                    switch (articulos[i].tabla_iva) {
                        case 1: //Exentas
                            detalle.depre_exentas = subtotal_detalle;
                            break;
                        case 2: //10%
                            detalle.depre_diez = subtotal_detalle;
                            break;
                        case 3: //5%
                            detalle.depre_cinco = subtotal_detalle;
                            break;
                    }
                    detalles.push(detalle);
                }
                for (d = 0; d < detalles.length; d++) {
                    nuevo_detalle = yield controlador.agregarDetalle(detalles[d]);
                }
                respuesta.success(req, res, presupuesto_nuevo.insertId, 200);
            }
            else { //Es una modificación
                yield controlador.agregarCabecera(presupuesto); //Actualizar cabecera
                const para_eliminar = yield controlador.getDetalles(presupuesto.pre_codigo); //Traemos los detalles existentes para comparar eliminados
                for (i = 0; i < articulos.length; i++) {
                    let descuento_calculado = Math.round((articulos[i].tabla_precio_venta - articulos[i].tabla_descuento) * porc_descuento);
                    for (c = 0; c < para_eliminar.length; c++) {
                        if (articulos[i].tabla_codigo === para_eliminar[c].det_codigo) {
                            para_eliminar.splice(c, 1); //Sacamos de la cola a eliminar si existe aún
                            break;
                        }
                    }
                    detalle = {
                        depre_codigo: articulos[i].tabla_codigo,
                        depre_presupuesto: presupuesto.pre_codigo,
                        depre_articulo: articulos[i].tabla_ar_codigo,
                        depre_cantidad: articulos[i].tabla_cantidad,
                        depre_precio: articulos[i].tabla_precio_venta - articulos[i].tabla_descuento - descuento_calculado,
                        depre_descuento: articulos[i].tabla_descuento + descuento_calculado,
                        depre_exentas: 0,
                        depre_cinco: 0,
                        depre_diez: 0,
                        depre_porcentaje: 0,
                        depre_altura: 0,
                        depre_largura: 0,
                        depre_mts2: 0,
                        depre_listaprecio: articulos[i].tabla_listaprecio,
                        depre_talle: "",
                        depre_codlote: articulos[i].tabla_al_codigo,
                        depre_lote: articulos[i].tabla_lote,
                        depre_vence: articulos[i].tabla_vence,
                        depre_descripcio_art: '',
                        depre_obs: articulos[i].tabla_obs,
                        depre_procesado: 0,
                    };
                    subtotal_detalle = articulos[i].tabla_cantidad * (articulos[i].tabla_precio_venta - articulos[i].tabla_descuento - descuento_calculado);
                    switch (articulos[i].tabla_iva) {
                        case 1: //Exentas
                            detalle.depre_exentas = subtotal_detalle;
                            break;
                        case 2: //10%
                            detalle.depre_diez = subtotal_detalle;
                            break;
                        case 3: //5%
                            detalle.depre_cinco = subtotal_detalle;
                            break;
                    }
                    detalles.push(detalle);
                }
                for (d = 0; d < detalles.length; d++) {
                    yield controlador.agregarDetalle(detalles[d]);
                }
                for (p = 0; p < para_eliminar.length; p++) {
                    yield controlador.eliminarDetalle(para_eliminar[p].det_codigo);
                }
                respuesta.success(req, res, presupuesto.pre_codigo, 200);
            }
        }
        catch (err) {
            next(err);
        }
    });
}
module.exports = router;

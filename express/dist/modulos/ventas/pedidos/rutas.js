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
const express = require("express");
const seguridad = require("../../../middleware/seguridad");
const router = express.Router();
const respuesta = require("../../../red/respuestas.js");
const controlador = require("./index.js");
const auth = require("../../../auth/index.js");
router.get("/", seguridad(), uno);
router.post("/agregar", seguridad(), agregar);
router.get("/detalles", seguridad(), detalles);
router.get("/pedido-estados", seguridad(), pedido_estados);
router.post("/consultas", seguridad(), consultas);
router.post("/autorizar", seguridad(), autorizar);
router.post("/agregar-pedido", controlador.agregarPedido);
router.post('/confirmarPedido', seguridad(), confirmarPedido);
router.post('/actualizarParcial', seguridad(), actualizarPedidoParcial);
router.get('/consulta-pedidos', seguridad(), pedidosNuevo);
router.post('/update-observacion-pedido', seguridad(), updateObservacionPedido);
router.get('/preparacion-pedido', seguridad(), pedidosPreparar);
router.post('/iniciar-preparacion-pedido', seguridad(), iniciarPreparacionPedido);
router.get('/traer-pedidos-disponibles', seguridad(), traerPedidosAPreparar);
router.get('/traer-items-por-pedido', seguridad(), traerItemsPorPedido);
router.post('/cargar-pedido-preparado', seguridad(), cargarPedidoPreparado);
router.post('/registrar-cajas', seguridad(), registrarCajas);
router.get('/numero-cajas', seguridad(), numeroCajas);
router.get('/obtener', seguridad(), obtenerPedidosParaVenta);
router.get('/reporte-de-preparacion', seguridad(), reportePreparacionPedidos);
router.get('/pedidos-agenda', seguridad(), pedidosAgenda);
router.get('/pedidos-faltantes', seguridad(), pedidosFaltantes);
router.post('/insertar-detalle-faltante', seguridad(), insertarDetalleFaltante);
router.post('/reprocesar-pedido', seguridad(), reprocesarPedido);
function reprocesarPedido(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log('llego');
            console.log(req.body);
            const { id_pedido, detalle } = req.body.datos;
            console.log('id', id_pedido);
            console.log('detalle', detalle);
            const result = yield controlador.rehacerPedidoConFaltantes({
                pedido_id: id_pedido,
                detalles: detalle
            });
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function pedidosFaltantes(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const result = yield controlador.getPedidosFaltantes(req.query);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function insertarDetalleFaltante(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const result = yield controlador.insertarDetalleFaltante(req.body);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function reportePreparacionPedidos(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const result = yield controlador.reportePreparacionPedidos(req.query.fecha_desde, req.query.fecha_hasta);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function obtenerPedidosParaVenta(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const result = yield controlador.obtenerPedidosParaVenta(req.query.id);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function numeroCajas(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const result = yield controlador.getNumeroCajas(req.query.id);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function registrarCajas(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const { pedidoId, numeroCajas, verificadoPor } = req.body;
            const result = yield controlador.insertarCantidadDeCajas(pedidoId, numeroCajas, verificadoPor);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function traerItemsPorPedido(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const result = yield controlador.traerItemsPorPedido(req.query.id, req.query.buscar);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function traerPedidosAPreparar(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log("llego");
            console.log(req.query);
            const result = yield controlador.traerPedidosAPreparar(req.query.deposito_id);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function iniciarPreparacionPedido(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            // Ahora esperamos un array de IDs
            console.log(req.body);
            const pedidoIds = req.body.pedido_ids;
            const preparadoPor = req.body.preparado_por;
            const result = yield controlador.iniciarPreparacionPedido(pedidoIds, preparadoPor);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function pedidosPreparar(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log(req.query);
            const result = yield controlador.prepararPedido(req.query.id, req.query.consolidar, req.query.cliente, req.query.fecha_desde, req.query.fecha_hasta, req.query.estado);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function pedidosNuevo(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const fecha_desde = req.query.fecha_desde;
            const fecha_hasta = req.query.fecha_hasta;
            const nro_pedido = req.query.nro_pedido;
            const articulo = req.query.articulo;
            const clientes = req.query.clientes;
            const vendedores = req.query.vendedores;
            const sucursales = req.query.sucursales;
            const estado = req.query.estado;
            const moneda = req.query.moneda;
            const factura = req.query.factura;
            const result = yield controlador.getPedidosNuevo(fecha_desde, fecha_hasta, nro_pedido, articulo, clientes, vendedores, sucursales, estado, moneda, factura);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function actualizarPedidoParcial(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const { codigo, items } = req.body;
            const result = yield controlador.actualizarPedidoParcial(codigo, items);
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
function confirmarPedido(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const pedidoId = req.body.id;
            const result = yield controlador.confirmarPedido(pedidoId);
            respuesta.success(req, res, result, 204);
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
            const bod = req.body;
            const items = yield controlador.getCabeceras(bod.fecha_desde, bod.fecha_hasta, bod.sucursal, bod.cliente, bod.vendedor, bod.articulo, bod.moneda, bod.factura, bod.limit);
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
        }
        catch (err) {
            next(err);
        }
    });
}
function agregar(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            //Carga cabecera de pedido - Tabla "pedidos"
            const pedido = req.body.pedido;
            const total_pedido = pedido.p_total;
            delete pedido.p_total;
            const articulos = req.body.tabla;
            //Carga detalle - Para luego cargar tablas "detalle_pedido"
            let detalles = [];
            let detalle = null;
            const porc_descuento = (100 * pedido.p_descuento) / (total_pedido + pedido.p_descuento) / 100;
            let subtotal_detalle = 0;
            if (pedido.p_codigo === 0) {
                //Es una carga nueva
                const primera_sec = yield controlador.primeraSecuenciaArea();
                if (primera_sec.length > 0)
                    pedido.p_area = primera_sec[0].ac_area;
                let pedido_nuevo = yield controlador.agregarCabecera(pedido);
                for (i = 0; i < articulos.length; i++) {
                    let descuento_calculado = Math.round((articulos[i].tabla_precio_venta - articulos[i].tabla_descuento) *
                        porc_descuento);
                    let bonif_num = 0;
                    if (articulos[i].tabla_venta_bonif === "B")
                        bonif_num = 1;
                    detalle = {
                        dp_codigo: 0,
                        dp_pedido: pedido_nuevo.insertId,
                        dp_articulo: articulos[i].tabla_ar_codigo,
                        dp_cantidad: articulos[i].tabla_cantidad,
                        dp_precio: articulos[i].tabla_precio_venta -
                            articulos[i].tabla_descuento -
                            descuento_calculado,
                        dp_descuento: articulos[i].tabla_descuento + descuento_calculado,
                        dp_exentas: 0,
                        dp_cinco: 0,
                        dp_diez: 0,
                        dp_lote: articulos[i].tabla_lote,
                        dp_vence: articulos[i].tabla_vence,
                        dp_vendedor: pedido.p_operador,
                        dp_codigolote: articulos[i].tabla_al_codigo,
                        dp_porcomision: 0,
                        dp_actorizado: 0,
                        dp_bonif: bonif_num,
                        dp_facturado: 0,
                    };
                    subtotal_detalle =
                        articulos[i].tabla_cantidad *
                            (articulos[i].tabla_precio_venta -
                                articulos[i].tabla_descuento -
                                descuento_calculado);
                    switch (articulos[i].tabla_iva) {
                        case 1: //Exentas
                            detalle.dp_exentas = subtotal_detalle;
                            break;
                        case 2: //10%
                            detalle.dp_diez = subtotal_detalle;
                            break;
                        case 3: //5%
                            detalle.dp_cinco = subtotal_detalle;
                            break;
                    }
                    detalles.push(detalle);
                }
                for (d = 0; d < detalles.length; d++) {
                    nuevo_detalle = yield controlador.agregarDetalle(detalles[d]);
                }
                respuesta.success(req, res, pedido_nuevo.insertId, 200);
            }
            else {
                //Es una modificación
                yield controlador.agregarCabecera(pedido); //Actualizar cabecera
                const para_eliminar = yield controlador.getDetalles(pedido.p_codigo); //Traemos los detalles existentes para comparar eliminados
                for (i = 0; i < articulos.length; i++) {
                    let descuento_calculado = Math.round((articulos[i].tabla_precio_venta - articulos[i].tabla_descuento) *
                        porc_descuento);
                    let bonif_num = 0;
                    if (articulos[i].tabla_venta_bonif === "B")
                        bonif_num = 1;
                    for (c = 0; c < para_eliminar.length; c++) {
                        if (articulos[i].tabla_codigo === para_eliminar[c].det_codigo) {
                            para_eliminar.splice(c, 1); //Sacamos de la cola a eliminar si existe aún
                            break;
                        }
                    }
                    detalle = {
                        dp_codigo: articulos[i].tabla_codigo,
                        dp_pedido: pedido.p_codigo,
                        dp_articulo: articulos[i].tabla_ar_codigo,
                        dp_cantidad: articulos[i].tabla_cantidad,
                        dp_precio: articulos[i].tabla_precio_venta -
                            articulos[i].tabla_descuento -
                            descuento_calculado,
                        dp_descuento: articulos[i].tabla_descuento + descuento_calculado,
                        dp_exentas: 0,
                        dp_cinco: 0,
                        dp_diez: 0,
                        dp_lote: articulos[i].tabla_lote,
                        dp_vence: articulos[i].tabla_vence,
                        dp_vendedor: pedido.p_operador,
                        dp_codigolote: articulos[i].tabla_al_codigo,
                        dp_porcomision: 0,
                        dp_actorizado: 0,
                        dp_bonif: bonif_num,
                        dp_facturado: 0,
                    };
                    subtotal_detalle =
                        articulos[i].tabla_cantidad *
                            (articulos[i].tabla_precio_venta -
                                articulos[i].tabla_descuento -
                                descuento_calculado);
                    switch (articulos[i].tabla_iva) {
                        case 1: //Exentas
                            detalle.dp_exentas = subtotal_detalle;
                            break;
                        case 2: //10%
                            detalle.dp_diez = subtotal_detalle;
                            break;
                        case 3: //5%
                            detalle.dp_cinco = subtotal_detalle;
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
                respuesta.success(req, res, pedido.p_codigo, 200);
            }
        }
        catch (err) {
            next(err);
        }
    });
}
function autorizar(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const item = yield controlador.autorizar(req.body.pedido, req.body.user, req.body.username, req.body.password);
            respuesta.success(req, res, item, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function pedido_estados(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const item = yield controlador.pedido_estados(req.query.cod);
            respuesta.success(req, res, item, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function updateObservacionPedido(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const { pedidoId, observacion } = req.body;
            const result = yield controlador.updateObservacionPedido(pedidoId, observacion);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function cargarPedidoPreparado(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const { pedidoId, cantidad } = req.body;
            const result = yield controlador.cargarPedidoPreparado(pedidoId, cantidad);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function pedidosAgenda(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const result = yield controlador.getPedidosParaAgenda(req.query.vendedor, req.query.cliente, req.query.busqueda);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
module.exports = router;

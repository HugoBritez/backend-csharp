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
router.post("/insertar", seguridad(), insertarReparto);
router.get("/listar", seguridad(), listarReparto);
router.get("/listar-detalle", seguridad(), listarDetalleReparto);
router.get("/listar-rutas", seguridad(), listarRutas);
router.get("/fetch-ventas", seguridad(), fetchVentas);
router.get("/fetch-pedidos", seguridad(), fetchPedidos);
router.get("/detalle-ventas", seguridad(), fetchDetalleVentas);
router.get("/detalle-pedidos", seguridad(), fetchDetallePedidos);
router.get("/marcar-salida-ruta", seguridad(), marcarSalidaRuta);
router.get("/marcar-llegada-ruta", seguridad(), marcarLlegadaRuta);
router.get("/marcar-llegada-entrega", seguridad(), marcarLlegadaEntrega);
router.get("/marcar-salida-entrega", seguridad(), marcarSalidaEntrega);
router.get("/camiones", seguridad(), fetchCamiones);
router.get("/choferes", seguridad(), fetchChoferes);
router.get("/resumen-repartos", seguridad(), resumenRepartos);
function resumenRepartos(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log(req.query);
            const fecha_desde = req.query.fecha_desde;
            const fecha_hasta = req.query.fecha_hasta;
            const sucursales = req.query.sucursales || null;
            const choferes = req.query.choferes || null;
            const camiones = req.query.camiones || null;
            const tipos = req.query.tipos || null;
            const id_entrega = req.query.id_entrega || null;
            const result = yield controlador.resumenRepartos(fecha_desde, fecha_hasta, sucursales, choferes, camiones, tipos, id_entrega);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function fetchCamiones(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const query = req.query;
            const result = yield controlador.fetchCamiones(query);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function fetchChoferes(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const query = req.query;
            const result = yield controlador.fetchChoferes(query);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function marcarSalidaRuta(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const query = req.query;
            const result = yield controlador.marcarSalidaRuta(query);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function marcarLlegadaRuta(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const query = req.query;
            const result = yield controlador.marcarLlegadaRuta(query);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function marcarLlegadaEntrega(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log(req.query);
            const id = req.query.id;
            // Manejar chat_id como array si viene como chat_id[]
            const chat_id = Array.isArray(req.query.chat_id)
                ? req.query.chat_id
                : req.query.chat_id;
            const latitud = req.query.latitud;
            const longitud = req.query.longitud;
            const result = yield controlador.marcarLlegadaEntrega(id, chat_id, latitud, longitud);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function marcarSalidaEntrega(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log(req.query);
            const id = req.query.id;
            // Manejar chat_id como array si viene como chat_id[]
            const chat_id = Array.isArray(req.query.chat_id)
                ? req.query.chat_id
                : req.query.chat_id;
            const latitud = req.query.latitud;
            const longitud = req.query.longitud;
            const result = yield controlador.marcarSalidaEntrega(id, chat_id, latitud, longitud);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function fetchVentas(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const query = req.query;
            const result = yield controlador.fetchVentas(query);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function fetchPedidos(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const query = req.query;
            const result = yield controlador.fetchPedidos(query);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function fetchDetallePedidos(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const id = req.query.id;
            const result = yield controlador.fetchDetallePedidos(id);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function fetchDetalleVentas(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const id = req.query.id;
            const result = yield controlador.fetchDetalleVentas(id);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function listarRutas(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log(req.query);
            const query = req.query;
            const result = yield controlador.listarRutas(query);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function insertarReparto(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log(req.body);
            const datos = req.body;
            const result = yield controlador.insertarReparto(datos);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function listarReparto(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log(req.query);
            const id = req.query.id;
            const result = yield controlador.listarReparto(id);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function listarDetalleReparto(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log(req.query);
            const query = req.query;
            const result = yield controlador.listarDetalleReparto(query);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
module.exports = router;

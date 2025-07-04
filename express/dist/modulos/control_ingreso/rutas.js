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
const seguridad = require("../../middleware/seguridad");
const router = express.Router();
const respuesta = require("../../red/respuestas.js");
const controlador = require("./index.js");
const auth = require("../../auth/index.js");
router.get("/", seguridad(), getFacturas);
function getFacturas(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const { deposito, sucursal, nro_proveedor, fecha_desde, fecha_hasta, nro_factura, verificado, } = req.query;
            const response = yield controlador.getFacturas(deposito, sucursal, nro_proveedor, fecha_desde, fecha_hasta, nro_factura, verificado);
            respuesta.success(req, res, response, 200);
        }
        catch (error) {
            next(error);
        }
    });
}
router.get("/items", seguridad(), getItems);
function getItems(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const { id_ingreso } = req.query;
            const response = yield controlador.getItems(id_ingreso);
            respuesta.success(req, res, response, 200);
        }
        catch (error) {
            next(error);
            return respuesta.error(req, res, error, 500);
        }
    });
}
router.post("/verificar", seguridad(), verificarCompra);
function verificarCompra(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const { id_compra, user_id } = req.body;
            const response = yield controlador.verificarCompra(id_compra, user_id);
            respuesta.success(req, res, response, 200);
        }
        catch (error) {
            next(error);
            return respuesta.error(req, res, error, 500);
        }
    });
}
router.post("/verificar-item", seguridad(), verificarItem);
function verificarItem(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const { id_detalle, cantidad } = req.body;
            if (!id_detalle) {
                return respuesta.error(res, "El id del detalle es requerido", 500);
            }
            if (!cantidad) {
                return respuesta.error(res, "La cantidad es requerida", 500);
            }
            const response = yield controlador.verificarItem(id_detalle, cantidad);
            respuesta.success(req, res, response, 200);
        }
        catch (error) {
            next(error);
            return respuesta.error(req, res, error, 500);
        }
    });
}
router.post("/confirmar", seguridad(), confirmarVerificacion);
function confirmarVerificacion(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log('AQUI ESTAN LOS BODY', req.body);
            const { id_compra, factura_compra, deposito_transitorio, deposito_destino, items, user_id, operador_id } = req.body;
            if (!id_compra) {
                return respuesta.error(res, "El id de la compra es requerido", 500);
            }
            const response = yield controlador.confirmarVerificacion(id_compra, factura_compra, deposito_transitorio, deposito_destino, items, user_id, operador_id);
            respuesta.success(req, res, response, 200);
        }
        catch (error) {
            next(error);
            return respuesta.error(req, res, error, 500);
        }
    });
}
router.get("/items-a-escanear", seguridad(), getItemsAEscanear);
function getItemsAEscanear(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const { id_compra, busqueda } = req.query;
            const response = yield controlador.getItemsAEscanear(id_compra, busqueda);
            respuesta.success(req, res, response, 200);
        }
        catch (error) {
            next(error);
            return respuesta.error(req, res, error, 500);
        }
    });
}
router.get('/reporte-ingresos', seguridad(), reporteIngresos);
function reporteIngresos(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const { deposito, sucursal, nro_proveedor, fecha_desde, fecha_hasta, nro_factura, verificado } = req.query;
            const response = yield controlador.reporteIngresos(deposito, sucursal, nro_proveedor, fecha_desde, fecha_hasta, nro_factura, verificado);
            respuesta.success(req, res, response, 200);
        }
        catch (error) {
            next(error);
            return respuesta.error(req, res, error, 500);
        }
    });
}
module.exports = router;

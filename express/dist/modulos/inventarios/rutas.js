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
const respuesta = require("../../red/respuestas");
const controlador = require("./index.js");
const auth = require("../../auth/index.js");
router.get("/", seguridad(), get_inventario);
function get_inventario(req, res) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const { inventario_id, nro_inventario, deposito, sucursal } = req.query;
            const resultado = yield controlador.get_inventario(inventario_id, nro_inventario, deposito, sucursal);
            // Llamar a success con req como primer parámetro
            respuesta.success(req, res, resultado, 200);
        }
        catch (error) {
            console.error("Error en get_inventario:", error);
            respuesta.error(req, res, "Error interno del servidor", 500);
        }
    });
}
router.get("/all", seguridad(), get_inventarios);
function get_inventarios(req, res) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const { estado, deposito, sucursal, nro_inventario } = req.query;
            const resultado = yield controlador.get_inventarios(estado, deposito, sucursal, nro_inventario);
            // Llamar a success con req como primer parámetro
            respuesta.success(req, res, resultado, 200);
        }
        catch (error) {
            console.error("Error en get_inventario:", error);
            respuesta.error(req, res, "Error interno del servidor", 500);
        }
    });
}
router.post("/", seguridad(), crear_inventario);
function crear_inventario(req, res) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const { inventario } = req.body;
            console.log(inventario);
            const resultado = yield controlador.crear_inventario(inventario);
            respuesta.success(req, res, resultado, 200);
        }
        catch (error) {
            console.error("Error en crear_inventario:", error);
            respuesta.error(req, res, "Error interno del servidor", 500);
        }
    });
}
router.post("/items", seguridad(), insertar_items_inventario);
function insertar_items_inventario(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log("req.body", req.body);
            const { inventario_id, filtros, deposito } = req.body;
            // Validar que los parámetros requeridos existan
            if (!inventario_id || !deposito) {
                return respuesta.error(req, res, "El ID del inventario y el depósito son obligatorios", 400);
            }
            // Asegurarse de que filtros sea un objeto válido
            const filtrosNormalizados = {
                categorias: (filtros === null || filtros === void 0 ? void 0 : filtros.categorias) || [],
                marcas: (filtros === null || filtros === void 0 ? void 0 : filtros.marcas) || [],
                secciones: (filtros === null || filtros === void 0 ? void 0 : filtros.secciones) || [],
                articulos: (filtros === null || filtros === void 0 ? void 0 : filtros.articulos) || [],
            };
            const resultado = yield controlador.insertar_items_inventario(inventario_id, deposito, filtrosNormalizados);
            // Si hay error en el resultado, enviar como error
            if (resultado.error) {
                return respuesta.error(req, res, resultado.mensaje, 400);
            }
            return respuesta.success(req, res, resultado, 200);
        }
        catch (error) {
            // Usar next(error) para consistencia con el resto del codebase
            next(error);
        }
    });
}
router.post("/items/escanear", seguridad(), escanear_item_inventario);
function escanear_item_inventario(req, res) {
    return __awaiter(this, void 0, void 0, function* () {
        console.log("req.body", req.body);
        const { id_articulo, id_lote, cantidad, lote, talle_id, color_id, vencimiento, codigo_barras, id_inventario, ubicacion_id, sub_ubicacion_id, } = req.body;
        const resultado = yield controlador.scannear_item_inventario(id_articulo, id_lote, cantidad, lote, talle_id, color_id, vencimiento, codigo_barras, id_inventario, ubicacion_id, sub_ubicacion_id);
        respuesta.success(req, res, resultado, 200);
    });
}
router.post("/cerrar", seguridad(), cerrar_inventario);
function cerrar_inventario(req, res) {
    return __awaiter(this, void 0, void 0, function* () {
        const { id } = req.body;
        const resultado = yield controlador.cerrar_inventario(id);
        respuesta.success(req, res, resultado, 200);
    });
}
router.post("/autorizar", seguridad(), autorizar_inventario);
function autorizar_inventario(req, res) {
    return __awaiter(this, void 0, void 0, function* () {
        const { id, operador, sucursal, deposito, nro_inventario } = req.body;
        const resultado = yield controlador.autorizar_inventario(id, operador, sucursal, deposito, nro_inventario);
        respuesta.success(req, res, resultado, 200);
    });
}
router.get("/items", seguridad(), get_items_inventario);
function get_items_inventario(req, res) {
    return __awaiter(this, void 0, void 0, function* () {
        const { nro_inventario, scanneado, deposito, sucursal, buscar, id_inventario, } = req.query;
        const resultado = yield controlador.get_items_inventario(nro_inventario, scanneado, deposito, sucursal, buscar, id_inventario);
        respuesta.success(req, res, resultado, 200);
    });
}
router.get("/escanear", seguridad(), escanear_inventario);
function escanear_inventario(req, res) {
    return __awaiter(this, void 0, void 0, function* () {
        console.log("req.query", req.query);
        const { nro_inventario, id_inventario, busqueda, deposito } = req.query;
        const resultado = yield controlador.get_items_a_escanear(nro_inventario, id_inventario, busqueda, deposito);
        respuesta.success(req, res, resultado, 200);
    });
}
router.get("/disponibles", seguridad(), get_disponibles);
function get_disponibles(req, res) {
    return __awaiter(this, void 0, void 0, function* () {
        const { estado, deposito } = req.query;
        const resultado = yield controlador.inventariosDisponibles(estado, deposito);
        respuesta.success(req, res, resultado, 200);
    });
}
router.get("/anomalias", seguridad(), get_anomalias);
function get_anomalias(req, res) {
    return __awaiter(this, void 0, void 0, function* () {
        const { nro_inventario, sucursal, deposito } = req.query;
        const resultado = yield controlador.reporteDeAnomalias(nro_inventario, sucursal, deposito);
        respuesta.success(req, res, resultado, 200);
    });
}
router.get("/reporte", seguridad(), get_reporte);
function get_reporte(req, res) {
    return __awaiter(this, void 0, void 0, function* () {
        const { id_inventario, categorias, incluir_sin_cambios, fecha_inicio, fecha_fin, deposito } = req.query;
        const resultado = yield controlador.reporte_inventario(id_inventario, categorias, incluir_sin_cambios, fecha_inicio, fecha_fin, deposito);
        respuesta.success(req, res, resultado, 200);
    });
}
router.post("/actualizar-cantidad-inicial", seguridad(), actualizar_cantidad_inicial);
function actualizar_cantidad_inicial(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const { id_inventario, id_articulo, id_lote, cantidad } = req.body;
            // Agregar logs de depuración
            console.log('Datos recibidos:', {
                id_inventario,
                id_articulo,
                id_lote,
                cantidad,
                bodyCompleto: req.body
            });
            const resultado = yield controlador.actualizar_cantidad_inicial(id_inventario, id_articulo, id_lote, cantidad);
            if (resultado.error) {
                return respuesta.error(req, res, resultado.mensaje, 400);
            }
            return respuesta.success(req, res, resultado);
        }
        catch (error) {
            console.error("Error detallado en actualizar_cantidad_inicial:", error);
            return respuesta.error(req, res, "Error interno del servidor", 500);
        }
    });
}
router.post("/anular", seguridad(), anular_inventario);
function anular_inventario(req, res) {
    return __awaiter(this, void 0, void 0, function* () {
        const { id } = req.body;
        const resultado = yield controlador.anular_inventario(id);
        respuesta.success(req, res, resultado, 200);
    });
}
module.exports = router;

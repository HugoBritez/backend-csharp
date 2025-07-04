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
router.get('/', seguridad(), buscarCabeceras);
router.get('/detalles', seguridad(), getDetalles);
router.post('/agregar', seguridad(), agregar);
router.put('/', seguridad(), eliminar);
router.get('/datos/imprimir', seguridad(), imprimir);
function buscarCabeceras(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            let fecha_desde = req.query.desde;
            let fecha_hasta = req.query.hasta;
            let paciente = req.query.paciente;
            let prestadores = req.query.prest;
            let items = yield controlador.buscarCabeceras(fecha_desde, fecha_hasta, paciente, prestadores);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function getDetalles(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            let codigo = req.query.codigo;
            const consultas = yield controlador.getDetallesConsultas(codigo);
            const procedimientos = yield controlador.getDetallesProcedimientos(codigo);
            let items = [...consultas, ...procedimientos];
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
            let cabecera = req.body.visacion;
            let detalles = req.body.visacion_detalles;
            let visacion_nueva = yield controlador.agregarCabecera(cabecera);
            controlador.updateNumeroCabecera(visacion_nueva.insertId);
            for (d in detalles) {
                detalles[d].svd_visacion = visacion_nueva.insertId;
                yield controlador.agregarDetalle(detalles[d]);
            }
            respuesta.success(req, res, visacion_nueva.insertId, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function eliminar(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            yield controlador.eliminarCabecera(req.query.id);
            // await controlador.eliminarDetalles(req.query.id); //Aparentemente no se cambia el estado de los detalles al anular la cabecera
            respuesta.success(req, res, 'Item eliminado satisfactoriamente!', 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function imprimir(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const configuraciones = yield controlador.getConfiguraciones();
            const cabecera = {
                empresa: configuraciones[0].valor,
                fecha: configuraciones[0].fecha,
                hora: configuraciones[0].hora,
                ruc: configuraciones[30].valor,
            };
            const visaciones = yield controlador.uno(req.query.id);
            const visacion = visaciones[0];
            const consultas = yield controlador.getDetallesConsultas(req.query.id);
            const procedimientos = yield controlador.getDetallesProcedimientos(req.query.id);
            const detalles = [...consultas, ...procedimientos];
            const datos = { cabecera, visacion, detalles };
            respuesta.success(req, res, datos, 200);
        }
        catch (error) {
            next(error);
        }
    });
}
module.exports = router;

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
router.get('/por_id/', por_id);
router.get('/', todos);
router.get('/todos', todosSinQuery);
router.post('/', seguridad(), modificar);
router.get('/cabecera', seguridad(), cabecera_impresion);
router.get('/get-configuraciones', getConfiguraciones);
function todos(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.todos(req.query.buscar);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function todosSinQuery(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.getConfiguraciones();
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
router.get('/por_id', por_id);
function por_id(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.por_id(req.query.ids);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function cabecera_impresion(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const configuraciones = yield controlador.getConfiguraciones();
            const cabecera = {
                empresa: configuraciones[0].valor,
                fecha: configuraciones[0].fecha,
                hora: configuraciones[0].hora,
                ruc: configuraciones[30].valor,
                telef: configuraciones[2].valor,
            };
            respuesta.success(req, res, cabecera, 201);
        }
        catch (error) {
            next(error);
        }
    });
}
function modificar(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            yield controlador.modificar(req.query.id, req.query.valor);
            let message = '';
            message = 'Guardado con éxito';
            respuesta.success(req, res, message, 201);
        }
        catch (error) {
            next(error);
        }
    });
}
function getConfiguraciones(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const configuraciones = yield controlador.getConfiguraciones();
            respuesta.success(req, res, configuraciones, 200);
        }
        catch (error) {
            next(error);
        }
    });
}
module.exports = router;

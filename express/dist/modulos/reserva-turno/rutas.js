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
const seguridad = require('../../middleware/seguridad');
const router = express.Router();
const respuesta = require('../../red/respuestas.js');
const controlador = require('./index.js');
const auth = require('../../auth/index.js');
router.get('/', todos);
router.post('/actualizar-confirmacion', actualizar_confirmacion);
router.post('/reagendar', seguridad(), reagendar);
router.post('/anular', seguridad(), anular);
function todos(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const q = req.query;
            const items = yield controlador.todos(q.fecha_desde, q.fecha_hasta, q.suc, q.pac, q.doc);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function actualizar_confirmacion(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const q = req.query;
            const items = yield controlador.actualizar_confirmacion(q.codigo, q.tipo, q.etapa);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function reagendar(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.reagendar(req.query.cod, req.query.fch, req.query.hora, req.query.doc, req.query.con, req.query.dis);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function anular(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.anular(req.query.cod);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
module.exports = router;

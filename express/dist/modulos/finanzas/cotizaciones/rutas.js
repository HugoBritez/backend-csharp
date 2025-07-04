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
router.get('/', seguridad(), listar);
router.post('/insert', seguridad(), agregarCotizaciones);
router.post('/update', seguridad(), modificarCotizaciones);
function listar(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const cotizacion = yield controlador.listar();
            respuesta.success(req, res, cotizacion, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function agregarCotizaciones(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            let message = 'Procesado';
            const cotizaciones = req.body;
            for (c in cotizaciones) {
                yield controlador.agregarCotizacion(cotizaciones[c]);
            }
            respuesta.success(req, res, message, 201);
        }
        catch (err) {
            next(err);
        }
    });
}
function modificarCotizaciones(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            let message = 'Procesado';
            const cotizaciones = req.body;
            for (c in cotizaciones) {
                yield controlador.modificarCotizacion(cotizaciones[c]);
            }
            respuesta.success(req, res, message, 201);
        }
        catch (err) {
            next(err);
        }
    });
}
module.exports = router;

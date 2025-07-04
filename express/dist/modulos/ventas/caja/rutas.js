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
const respuesta = require('../../../red/respuestas.js');
const router = express.Router();
const controlador = require('./index.js');
const seguridad = require('../../../middleware/seguridad');
router.get('/traer-cajas', seguridad(), traerCajas);
router.post('/iniciar', seguridad(), iniciar);
router.post('/cerrar', seguridad(), cerrar);
router.get('/verificar/:id', seguridad(), VerificarCajaAbierta);
router.post('/insertar-operacion', seguridad(), insertarOperacion);
router.post('insertar-inventario', seguridad(), insertarInventario);
function traerCajas(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const cajas = yield controlador.traerCajas();
            respuesta.success(req, res, cajas, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function insertarInventario(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log('Insertando inventario con los siguientes datos: ', req.body);
            const result = yield controlador.insertarInventario(req.body);
            respuesta.success(req, res, result, 201);
        }
        catch (err) {
            next(err);
        }
    });
}
function insertarOperacion(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log('Insertando operacion con los siguientes datos: ', req.body);
            const result = yield controlador.insertarOperacion(req.body);
            respuesta.success(req, res, result, 201);
        }
        catch (err) {
            next(err);
        }
    });
}
function iniciar(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const result = yield controlador.IniciarCaja(req);
            respuesta.success(req, res, result, 201);
        }
        catch (err) {
            next(err);
        }
    });
}
function cerrar(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log('Iniciando caja con los siguientes datos: ', req.body);
            const result = yield controlador.CerrarCaja(req);
            respuesta.success(req, res, result, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function VerificarCajaAbierta(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const result = yield controlador.VerificarCajaAbierta(req.params.id);
            respuesta.success(req, res, result, 200);
            console.log('Caja abierta: ', result);
        }
        catch (err) {
            next(err);
        }
    });
}
module.exports = router;

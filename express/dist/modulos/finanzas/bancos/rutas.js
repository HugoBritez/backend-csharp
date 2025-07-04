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
router.get('/todos', seguridad(), listarBancos);
router.get('/cuentas', seguridad(), listarCuentasBancarias);
router.get('/tarjetas', seguridad(), listarTarjetas);
function listarTarjetas(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const tarjetas = yield controlador.tarjetas();
            respuesta.success(req, res, tarjetas, 200);
        }
        catch (error) {
            respuesta.error(req, res, error, 500);
        }
    });
}
function listarCuentasBancarias(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const cuentas = yield controlador.listarCuentasBancarias();
            respuesta.success(req, res, cuentas, 200);
        }
        catch (error) {
            respuesta.error(req, res, error, 500);
        }
    });
}
function listarBancos(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const bancos = yield controlador.listarBancos();
            respuesta.success(req, res, bancos, 200);
        }
        catch (error) {
            respuesta.error(req, res, error, 500);
        }
    });
}
module.exports = router;

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
//router.put('/',seguridad(),getVentas)
router.get('/beneficiario', seguridad(), getBeneficiario);
router.get('/adherente', seguridad(), getAdherente);
function getBeneficiario(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const buscar = req.query.string_busqueda;
            const item = yield controlador.getBeneficiario(buscar);
            if (item.length > 0) {
                item[0].preexistencia = yield controlador.verificarPreexistencia(item[0].paccod);
            }
            respuesta.success(req, res, item, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function getAdherente(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const buscar = req.query.string_busqueda;
            const item = yield controlador.getAdherente(buscar);
            if (item.length > 0) {
                item[0].preexistencia = yield controlador.verificarPreexistencia(item[0].paccod);
            }
            respuesta.success(req, res, item, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
module.exports = router;

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
const respuesta = require('../../red/respuestas');
const controlador = require('./index.js');
const auth = require('../../auth/index');
router.get('/', get_configuraciones);
router.get('/fotos-nota-comun', get_configuraciones_fotos_nota_comun);
router.post('/fotos-nota-comun', update_configuraciones_fotos_nota_comun);
router.get('/fotos-factura', get_configuraciones_fotos_factura);
router.post('/fotos-factura', update_configuraciones_fotos_factura);
router.get('/factura', get_configuraciones_factura);
router.post('/factura', update_configuraciones_factura);
function get_configuraciones(req, res) {
    return __awaiter(this, void 0, void 0, function* () {
        const configuraciones = yield controlador.get_configuraciones();
        respuesta.success(req, res, configuraciones, 200);
    });
}
function get_configuraciones_fotos_nota_comun(req, res) {
    return __awaiter(this, void 0, void 0, function* () {
        const configuraciones = yield controlador.get_configuraciones_fotos_nota_comun();
        respuesta.success(req, res, configuraciones, 200);
    });
}
function update_configuraciones_fotos_nota_comun(req, res) {
    return __awaiter(this, void 0, void 0, function* () {
        console.log(req.body);
        const configuraciones = yield controlador.update_configuraciones_fotos_nota_comun(req.body);
        respuesta.success(req, res, configuraciones, 200);
    });
}
function get_configuraciones_fotos_factura(req, res) {
    return __awaiter(this, void 0, void 0, function* () {
        const configuraciones = yield controlador.get_configuraciones_fotos_factura();
        respuesta.success(req, res, configuraciones, 200);
    });
}
function update_configuraciones_fotos_factura(req, res) {
    return __awaiter(this, void 0, void 0, function* () {
        const configuraciones = yield controlador.update_configuraciones_fotos_factura(req.body);
        respuesta.success(req, res, configuraciones, 200);
    });
}
function get_configuraciones_factura(req, res) {
    return __awaiter(this, void 0, void 0, function* () {
        const configuraciones = yield controlador.get_configuraciones_factura();
        respuesta.success(req, res, configuraciones, 200);
    });
}
function update_configuraciones_factura(req, res) {
    return __awaiter(this, void 0, void 0, function* () {
        const configuraciones = yield controlador.update_configuraciones_factura(req.body);
        respuesta.success(req, res, configuraciones, 200);
    });
}
module.exports = router;

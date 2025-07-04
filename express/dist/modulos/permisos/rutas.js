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
router.post('/', seguridad(), todos);
router.post('/acceso', traerPermisosDeAcceso);
router.post('/permitir', permitirAcceso);
function traerPermisosDeAcceso(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const operador = yield controlador.traerPermisosDeAcceso(req.body.user);
            respuesta.success(req, res, operador, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function permitirAcceso(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const permiso = yield controlador.permitirAcceso(req.body.userId, req.body.menuId);
            respuesta.success(req, res, permiso, 200);
            console.log(permiso);
        }
        catch (err) {
            next(err);
        }
    });
}
function todos(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            let items = yield controlador.todos(req.query.user, req.query.orden);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
module.exports = router;

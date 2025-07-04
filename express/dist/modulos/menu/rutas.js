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
//router.get('/', todos)
router.get('/rol', seguridad(), rol);
router.post('/', seguridad(), todos);
function todos(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            let items = yield controlador.todos(req.query.user, req.query.grupo, req.query.orden);
            for (i in items) {
                if (items[i].m_descripcion.substring(0, 2) === "w.") {
                    items[i].m_descripcion = items[i].m_descripcion.substring(2);
                }
            }
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function rol(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            let items = yield controlador.rol(req.query.user);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
module.exports = router;

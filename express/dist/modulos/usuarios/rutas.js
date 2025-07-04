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
router.post('/login', login);
router.get('/', seguridad(), todos);
router.get("/vendedores", seguridad(), vendedores);
router.get('/:id', seguridad(), uno);
router.post('/', seguridad(), agregar);
router.put('/:id', seguridad(), eliminar);
router.get('/verificar/:user', verificarUsuario);
function vendedores(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const vendedores = yield controlador.vendedores(req.query.buscar, req.query.id_vendedor);
            respuesta.success(req, res, vendedores, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function verificarUsuario(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.verificarUsuario(req.params.user);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function login(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const data = yield controlador.login(req.body.user, req.body.pass);
            const token = auth.asignarToken(Object.assign({}, data));
            const datos = { usuario: data, token: token };
            respuesta.success(req, res, datos, 200);
        }
        catch (err) {
            /*respuesta.error(req, res, err, 500)*/
            next(err);
        }
    });
}
function todos(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const busqueda = req.query.buscar;
            const items = yield controlador.todos(busqueda);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function uno(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.uno(req.params.id);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            /*respuesta.error(req, res, err, 500)*/
            next(err);
        }
    });
}
function agregar(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            yield controlador.agregar(req.body);
            let message = '';
            if (req.body.id == 0) {
                message = 'Guardado con éxito';
            }
            else {
                message = 'Item no guardado';
            }
            respuesta.success(req, res, message, 201);
        }
        catch (error) {
            next(error);
        }
    });
}
function eliminar(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            yield controlador.eliminar(req.params.id);
            respuesta.success(req, res, 'Item eliminado satisfactoriamente!', 200);
        }
        catch (err) {
            next(err);
        }
    });
}
module.exports = router;

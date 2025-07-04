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
const router = express.Router();
const respuesta = require('../../red/respuestas.js');
const controlador = require('./index.js');
const auth = require('../../auth/index.js');
router.post('/', todos);
router.post('/doctores', get_doctores);
router.post('/doctores_horarios', get_doctores_horarios);
router.post('/especialidades', get_especialidades);
router.post('/turnos', get_turnos);
function todos(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.todos(req.query.p_fecha);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function get_doctores(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.get_doctores(req.query.p_fecha, req.query.p_especialidad);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function get_doctores_horarios(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.get_doctores_horarios(req.query.p_fecha, req.query.p_doctor);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function get_especialidades(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.get_especialidades(req.query.p_fecha);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function get_turnos(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            //SE VERIFICA SI YA HAY UNA RESERVA EN CUALQUIERA DE LAS ESPECIALIDADES DEL DOCTOR
            const reservas = yield controlador.get_turnos_reservas(req.query.p_fecha, req.query.p_hora, req.query.p_doctor);
            if (reservas.length > 0) {
                respuesta.success(req, res, reservas, 200);
            }
            else {
                //SE VERIFICA SI YA HAY UNA CONSULTA AGENDADA EN CUALQUIERA DE LAS ESPECIALIDADES DEL DOCTOR
                const items = yield controlador.get_turnos_consultas(req.query.p_fecha, req.query.p_hora, req.query.p_doctor);
                if (items.length > 0) {
                    respuesta.success(req, res, items, 200);
                }
                else { //SI NO EXISTE UNA CONSULTA AGENDADA SE REVISA SI NO SE AGENDO UN ESTUDIO O LABORATORIO AL MISMO DOCTOR EN LAS MISMA HORA
                    const items_est = yield controlador.get_turnos_estudios(req.query.p_fecha, req.query.p_hora, req.query.p_doctor);
                    respuesta.success(req, res, items_est, 200);
                }
            }
        }
        catch (err) {
            next(err);
        }
    });
}
module.exports = router;

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
const getDirecciones = (req, res, next) => __awaiter(void 0, void 0, void 0, function* () {
    try {
        const items = yield controlador.getDirecciones(req.query.busqueda, req.query.zona);
        respuesta.success(req, res, items, 200);
    }
    catch (err) {
        next(err);
    }
});
const crearDirecciones = (req, res, next) => __awaiter(void 0, void 0, void 0, function* () {
    try {
        console.log(req.body);
        const items = yield controlador.postDirecciones(req.body);
        respuesta.success(req, res, items, 200);
    }
    catch (err) {
        next(err);
    }
});
const eliminarDirecciones = (req, res, next) => __awaiter(void 0, void 0, void 0, function* () {
    try {
        console.log(req.query);
        const items = yield controlador.eliminarDirecciones(req.query.rango);
        respuesta.success(req, res, items, 200);
    }
    catch (err) {
        next(err);
    }
});
const crearAgrupacionesEnSecuencia = (req, res, next) => __awaiter(void 0, void 0, void 0, function* () {
    try {
        console.log(req.body);
        const { rango, zona } = req.body;
        const items = yield controlador.agruparDireccionesEnSecuencia(rango, zona);
        respuesta.success(req, res, items, 200);
    }
    catch (err) {
        next(err);
    }
});
const getArticulosDirecciones = (req, res, next) => __awaiter(void 0, void 0, void 0, function* () {
    try {
        const items = yield controlador.getArticulosDirecciones(req.query.busqueda, req.query.rango);
        respuesta.success(req, res, items, 200);
    }
    catch (err) {
        next(err);
    }
});
const crearArticuloDireccion = (req, res, next) => __awaiter(void 0, void 0, void 0, function* () {
    try {
        console.log(req.body);
        const items = yield controlador.crearArticuloDireccion(req.body);
        respuesta.success(req, res, items, 200);
    }
    catch (err) {
        next(err);
    }
});
const eliminarArticuloDireccion = (req, res, next) => __awaiter(void 0, void 0, void 0, function* () {
    try {
        console.log(req.query);
        const items = yield controlador.eliminarArticuloDireccion(req.query.rango, req.query.articulo);
        respuesta.success(req, res, items, 200);
    }
    catch (err) {
        next(err);
    }
});
const generarRotulos = (req, res, next) => __awaiter(void 0, void 0, void 0, function* () {
    try {
        console.log('Query para generar rotulos:', req.query);
        const items = yield controlador.generarRotulos(req.query.rango);
        respuesta.success(req, res, items, 200);
    }
    catch (err) {
        next(err);
    }
});
router.get('/', seguridad(), getDirecciones);
router.post('/', seguridad(), crearDirecciones);
router.delete('/', seguridad(), eliminarDirecciones);
router.post('/agrupar', seguridad(), crearAgrupacionesEnSecuencia);
router.get('/articulos', seguridad(), getArticulosDirecciones);
router.post('/articulo', seguridad(), crearArticuloDireccion);
router.delete('/articulo', seguridad(), eliminarArticuloDireccion);
router.get('/rotulos', seguridad(), generarRotulos);
module.exports = router;

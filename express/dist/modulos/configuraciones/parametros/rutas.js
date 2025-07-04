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
router.get('/get-configuraciones', getConfiguraciones);
function getConfiguraciones(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const configuraciones = yield controlador.getConfiguraciones();
            respuesta.success(req, res, configuraciones, 200);
        }
        catch (error) {
            next(error);
        }
    });
}
module.exports = router;

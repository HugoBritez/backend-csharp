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
const { networkInterfaces } = require('os');
const nets = networkInterfaces();
router.post('/', seguridad(), agregar);
/*Acciones: 1-
            2-
            3- Eliminar
            4-
*/
function agregar(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const datos = req.body;
            let ip = '';
            for (const name of Object.keys(nets)) { //networkInterfaces devuelve varios objetos, revisamos cada uno
                for (const net of nets[name]) {
                    const familyV4Value = typeof net.family === 'string' ? 'IPv4' : 4; //Solo nos importa el IPv4
                    if (net.family === familyV4Value && !net.internal) { //Filtramos que no nos muestre el IP interno, 127.0.0.1 etc.
                        ip = net.address;
                    }
                }
            }
            datos.usuario += '@' + ip;
            yield controlador.agregar(datos);
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
module.exports = router;

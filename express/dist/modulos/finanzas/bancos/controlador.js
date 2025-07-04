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
const TABLA = "cotizaciones";
module.exports = function (dbInyectada) {
    let db = dbInyectada;
    if (!db) {
        db = require("../../../DB/mysql.js");
    }
    function listarBancos() {
        return __awaiter(this, void 0, void 0, function* () {
            return db.sql(`SELECT * FROM bancos where ba_estado = 1`);
        });
    }
    function listarCuentasBancarias() {
        return __awaiter(this, void 0, void 0, function* () {
            return db.sql(`SELECT * FROM cuentasbco where cb_estado = 1`);
        });
    }
    function tarjetas() {
        return __awaiter(this, void 0, void 0, function* () {
            return db.sql(`SELECT * FROM tarjetas where t_estado = 1`);
        });
    }
    return {
        listarBancos,
        listarCuentasBancarias,
        tarjetas
    };
};

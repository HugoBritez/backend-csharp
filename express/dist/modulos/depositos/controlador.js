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
const TABLA = `depositos de
                INNER JOIN operador_depositos od ON od.ode_deposito = de.dep_codigo `;
module.exports = function (dbInyectada) {
    let db = dbInyectada;
    if (!db) {
        db = require('../../DB/mysql.js');
    }
    function todos_filtro(id_sucursal, operador) {
        return __awaiter(this, void 0, void 0, function* () {
            let query = `SELECT de.dep_codigo, de.dep_descripcion
      FROM depositos de
      INNER JOIN operador_depositos od ON od.ode_deposito = de.dep_codigo
      WHERE de.dep_estado = 1 AND od.ode_operador = ${operador}`;
            let resultado = yield db.sql(query);
            if (resultado.length <= 0) {
                query = `SELECT de.dep_codigo, de.dep_descripcion FROM depositos de WHERE de.dep_estado = 1`;
                resultado = yield db.sql(query);
            }
            return resultado;
        });
    }
    function todos_sucursal(sucursales) {
        const query = `SELECT de.dep_codigo, de.dep_descripcion FROM depositos de WHERE de.dep_estado = 1 and de.dep_sucursal IN (${sucursales})`;
        return db.sql(query);
    }
    function todos() {
        const query = "SELECT dep_codigo, dep_descripcion, dep_principal FROM depositos WHERE dep_estado = 1";
        return db.sql(query);
    }
    return {
        todos_filtro, todos_sucursal, todos
    };
};

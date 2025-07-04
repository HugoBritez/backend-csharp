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
const TABLA = 'operadores';
/*const auth = require('../../auth/index');*/
module.exports = function (dbInyectada) {
    let db = dbInyectada;
    if (!db) {
        db = require('../../DB/mysql.js');
    }
    function login(user, pass) {
        return __awaiter(this, void 0, void 0, function* () {
            const data = yield db.login(TABLA, user, pass);
            return data;
        });
    }
    function vendedores(busqueda, id_vendedor) {
        return __awaiter(this, void 0, void 0, function* () {
            let where = "op_estado = 1";
            if (busqueda && busqueda.trim() !== '' && busqueda.length > 2) {
                where += ` AND (op_nombre LIKE '%${busqueda}%' OR op_documento LIKE '%${busqueda}%')`;
            }
            if (id_vendedor) {
                where += ` AND op_codigo = ${id_vendedor}`;
            }
            const query = `
        SELECT 
        op.op_codigo, 
        op.op_nombre, 
        op.op_documento,
        rol.rol_descripcion as op_rol
        FROM operadores op
        INNER JOIN operador_roles oprol ON op.op_codigo = oprol.or_operador
        INNER JOIN roles rol ON oprol.or_rol = rol.rol_codigo
        WHERE ${where} LIMIT 5`;
            console.log(query);
            return yield db.sql(query);
        });
    }
    function todos(busqueda) {
        let where = "op_estado = 1";
        if (busqueda && busqueda.trim() !== '' && busqueda.length > 2) {
            where += ` AND (op_nombre LIKE '%${busqueda}%' OR op_documento LIKE '%${busqueda}%')`;
        }
        const query = `SELECT * FROM operadores WHERE ${where}`;
        return db.sql(query);
    }
    function uno(id) {
        const primary_key = `op_codigo = ${id} `;
        const campos = " * ";
        return db.uno(TABLA, primary_key, campos);
    }
    function agregar(id) {
        return db.agregar(TABLA, id);
    }
    function eliminar(body) {
        return db.eliminar(TABLA, body);
    }
    return {
        login,
        todos,
        uno,
        agregar,
        eliminar,
        vendedores
    };
};

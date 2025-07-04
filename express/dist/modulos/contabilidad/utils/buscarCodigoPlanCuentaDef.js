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
Object.defineProperty(exports, "__esModule", { value: true });
exports.buscarCodigoPlanCuentaCajaDef = void 0;
const db = require("../../../DB/mysql");
// Función para buscar el plan de cuenta de caja
const buscarCodigoPlanCuentaCajaDef = (codigoDefCaja) => __awaiter(void 0, void 0, void 0, function* () {
    const query = `
      SELECT 
        cp.plan
      FROM 
        cajadef_plancuentas cp 
        INNER JOIN cajadef cf ON cp.cajad = cf.cd_codigo
      WHERE 
        cp.cajad = ${codigoDefCaja} 
        AND cf.cd_estado = 1
    `;
    const [result] = yield db.sql(query);
    if (result && result.plan) {
        return result.plan;
    }
    return 0;
});
exports.buscarCodigoPlanCuentaCajaDef = buscarCodigoPlanCuentaCajaDef;

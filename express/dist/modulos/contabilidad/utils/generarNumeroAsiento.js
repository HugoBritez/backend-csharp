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
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.generarNumeroAsiento = void 0;
const mysql_js_1 = __importDefault(require("../../../DB/mysql.js"));
const generarNumeroAsiento = () => __awaiter(void 0, void 0, void 0, function* () {
    const query = `
    SELECT
      CAST(IFNULL(MAX(ac.ac_numero), '0') as char) as numero 
    FROM
      asiento_contable ac
    WHERE 
      ac.ac_cierre_asiento = 0
  `;
    const [result] = yield mysql_js_1.default.sql(query);
    if (result && result.numero) {
        return parseInt(result.numero) + 1;
    }
    return 1;
});
exports.generarNumeroAsiento = generarNumeroAsiento;

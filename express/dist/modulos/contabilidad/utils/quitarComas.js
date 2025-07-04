"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.quitarComas = void 0;
// Función para quitar comas de números
const quitarComas = (valor) => {
    return parseFloat(valor.replace(/,/g, ''));
};
exports.quitarComas = quitarComas;

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
module.exports = function (dbInyectada) {
    let db = dbInyectada;
    if (!db) {
        db = require("../../../DB/mysql.js");
    }
    function get_configuraciones() {
        return __awaiter(this, void 0, void 0, function* () {
            const query = `SELECT * FROM configuraciones_web`;
            return yield db.sql(query);
        });
    }
    function get_configuraciones_fotos_nota_comun() {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                // Consulta optimizada que solo selecciona el campo valor
                const query = `SELECT valor FROM configuraciones_web WHERE descripcion = 'Configuraciones Nota Comun'`;
                const resultado = yield db.sql(query);
                // Verificar si se encontraron resultados
                if (!resultado || resultado.length === 0) {
                    return {
                        success: false,
                        message: "No se encontraron configuraciones",
                        body: null,
                    };
                }
                // Obtener el valor y parsearlo directamente
                let valorConfig = null;
                try {
                    valorConfig = JSON.parse(resultado[0].valor);
                }
                catch (jsonError) {
                    console.error("Error al parsear JSON:", jsonError);
                    valorConfig = resultado[0].valor; // Devolver el valor sin parsear como fallback
                }
                return {
                    success: true,
                    message: "Configuración obtenida correctamente",
                    body: valorConfig, // Devolver directamente el objeto parseado
                };
            }
            catch (error) {
                console.error("Error al obtener configuración:", error);
                return {
                    success: false,
                    message: "Error al obtener configuración",
                    error: error.message,
                    body: null,
                };
            }
        });
    }
    function update_configuraciones_fotos_nota_comun(updateData) {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                // 1. Primero obtenemos el valor actual
                const querySelect = `SELECT valor FROM configuraciones_web WHERE descripcion = 'Configuraciones Nota Comun'`;
                const resultado = yield db.sql(querySelect);
                if (!resultado || resultado.length === 0) {
                    return {
                        success: false,
                        message: "No se encontró la configuración para actualizar",
                        body: null,
                    };
                }
                // 2. Parseamos el JSON actual
                let configuracionActual;
                try {
                    // Si el valor está vacío, inicializamos con un objeto vacío
                    configuracionActual = resultado[0].valor ? JSON.parse(resultado[0].valor) : {};
                }
                catch (jsonError) {
                    return {
                        success: false,
                        message: "Error al parsear la configuración existente",
                        error: jsonError.message,
                        body: null,
                    };
                }
                // 3. Aplicamos las actualizaciones de manera recursiva
                const actualizarObjeto = (original, actualizaciones) => {
                    for (const [key, value] of Object.entries(actualizaciones)) {
                        if (typeof value === "object" &&
                            value !== null &&
                            !Array.isArray(value) &&
                            original[key] &&
                            typeof original[key] === "object") {
                            // Si ambos son objetos, actualizar recursivamente
                            actualizarObjeto(original[key], value);
                        }
                        else {
                            // Caso base: sobrescribir o añadir valor
                            original[key] = value;
                        }
                    }
                };
                // Aplicar las actualizaciones
                actualizarObjeto(configuracionActual, updateData);
                // 4. Guardar el objeto actualizado
                const nuevoValorJSON = JSON.stringify(configuracionActual);
                const queryUpdate = `UPDATE configuraciones_web SET valor = '${nuevoValorJSON}' WHERE descripcion = 'Configuraciones Nota Comun'`;
                yield db.sql(queryUpdate);
                return {
                    success: true,
                    message: "Configuración actualizada correctamente",
                    body: configuracionActual,
                };
            }
            catch (error) {
                console.error("Error al actualizar configuración:", error);
                return {
                    success: false,
                    message: "Error al actualizar configuración",
                    error: error.message,
                    body: null,
                };
            }
        });
    }
    function get_configuraciones_fotos_factura() {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                // Consulta optimizada que solo selecciona el campo valor
                const query = `SELECT valor FROM configuraciones_web WHERE descripcion = 'Ajustes Foto de Cabecera Factura'`;
                const resultado = yield db.sql(query);
                // Verificar si se encontraron resultados
                if (!resultado || resultado.length === 0) {
                    return {
                        success: false,
                        message: "No se encontraron configuraciones",
                        body: null,
                    };
                }
                // Obtener el valor y parsearlo directamente
                let valorConfig = null;
                try {
                    valorConfig = JSON.parse(resultado[0].valor);
                }
                catch (jsonError) {
                    console.error("Error al parsear JSON:", jsonError);
                    valorConfig = resultado[0].valor; // Devolver el valor sin parsear como fallback
                }
                return {
                    success: true,
                    message: "Configuración obtenida correctamente",
                    body: valorConfig, // Devolver directamente el objeto parseado
                };
            }
            catch (error) {
                console.error("Error al obtener configuración:", error);
                return {
                    success: false,
                    message: "Error al obtener configuración",
                    error: error.message,
                    body: null,
                };
            }
        });
    }
    function update_configuraciones_fotos_factura(updateData) {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                // 1. Primero obtenemos el valor actual
                console.log(updateData);
                const querySelect = `SELECT valor FROM configuraciones_web WHERE descripcion = 'Ajustes Foto de Cabecera Factura'`;
                const resultado = yield db.sql(querySelect);
                console.log(resultado);
                // 2. Parseamos el JSON actual
                let configuracionActual;
                try {
                    // Si el valor está vacío, inicializamos con un objeto vacío
                    configuracionActual = resultado[0].valor ? JSON.parse(resultado[0].valor) : {};
                }
                catch (jsonError) {
                    console.log(jsonError);
                    return {
                        success: false,
                        message: "Error al parsear la configuración existente",
                        error: jsonError.message,
                        body: null,
                    };
                }
                console.log(configuracionActual);
                // 3. Aplicamos las actualizaciones de manera recursiva
                const actualizarObjeto = (original, actualizaciones) => {
                    for (const [key, value] of Object.entries(actualizaciones)) {
                        if (typeof value === "object" &&
                            value !== null &&
                            !Array.isArray(value) &&
                            original[key] &&
                            typeof original[key] === "object") {
                            // Si ambos son objetos, actualizar recursivamente
                            actualizarObjeto(original[key], value);
                        }
                        else {
                            // Caso base: sobrescribir o añadir valor
                            original[key] = value;
                        }
                    }
                };
                // Aplicar las actualizaciones
                actualizarObjeto(configuracionActual, updateData);
                // 4. Guardar el objeto actualizado
                const nuevoValorJSON = JSON.stringify(configuracionActual);
                const queryUpdate = `UPDATE configuraciones_web SET valor = '${nuevoValorJSON}' WHERE descripcion = 'Ajustes Foto de Cabecera Factura'`;
                console.log(queryUpdate);
                yield db.sql(queryUpdate);
                return {
                    success: true,
                    message: "Configuración actualizada correctamente",
                    body: configuracionActual,
                };
            }
            catch (error) {
                console.error("Error al actualizar configuración:", error);
                return {
                    success: false,
                    message: "Error al actualizar configuración",
                    error: error.message,
                    body: null,
                };
            }
        });
    }
    function get_configuraciones_factura() {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                // Consulta optimizada que solo selecciona el campo valor
                const query = `SELECT valor FROM configuraciones_web WHERE descripcion = 'Ajustes Factura'`;
                const resultado = yield db.sql(query);
                // Verificar si se encontraron resultados
                if (!resultado || resultado.length === 0) {
                    return {
                        success: false,
                        message: "No se encontraron configuraciones",
                        body: null,
                    };
                }
                // Obtener el valor y parsearlo directamente
                let valorConfig = null;
                try {
                    valorConfig = JSON.parse(resultado[0].valor);
                }
                catch (jsonError) {
                    console.error("Error al parsear JSON:", jsonError);
                    valorConfig = resultado[0].valor; // Devolver el valor sin parsear como fallback
                }
                return {
                    success: true,
                    message: "Configuración obtenida correctamente",
                    body: valorConfig, // Devolver directamente el objeto parseado
                };
            }
            catch (error) {
                console.error("Error al obtener configuración:", error);
                return {
                    success: false,
                    message: "Error al obtener configuración",
                    error: error.message,
                    body: null,
                };
            }
        });
    }
    function update_configuraciones_factura(updateData) {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                // 1. Primero obtenemos el valor actual
                const querySelect = `SELECT valor FROM configuraciones_web WHERE descripcion = 'Ajustes Factura'`;
                const resultado = yield db.sql(querySelect);
                if (!resultado || resultado.length === 0) {
                    return {
                        success: false,
                        message: "No se encontró la configuración para actualizar",
                        body: null,
                    };
                }
                // 2. Parseamos el JSON actual
                let configuracionActual;
                try {
                    // Si el valor está vacío, inicializamos con un objeto vacío
                    configuracionActual = resultado[0].valor ? JSON.parse(resultado[0].valor) : {};
                }
                catch (jsonError) {
                    return {
                        success: false,
                        message: "Error al parsear la configuración existente",
                        error: jsonError.message,
                        body: null,
                    };
                }
                // 3. Aplicamos las actualizaciones de manera recursiva
                const actualizarObjeto = (original, actualizaciones) => {
                    for (const [key, value] of Object.entries(actualizaciones)) {
                        if (typeof value === "object" &&
                            value !== null &&
                            !Array.isArray(value) &&
                            original[key] &&
                            typeof original[key] === "object") {
                            // Si ambos son objetos, actualizar recursivamente
                            actualizarObjeto(original[key], value);
                        }
                        else {
                            // Caso base: sobrescribir o añadir valor
                            original[key] = value;
                        }
                    }
                };
                // Aplicar las actualizaciones
                actualizarObjeto(configuracionActual, updateData);
                // 4. Guardar el objeto actualizado
                const nuevoValorJSON = JSON.stringify(configuracionActual);
                const queryUpdate = `UPDATE configuraciones_web SET valor = '${nuevoValorJSON}' WHERE descripcion = 'Ajustes Factura'`;
                yield db.sql(queryUpdate);
                return {
                    success: true,
                    message: "Configuración actualizada correctamente",
                    body: configuracionActual,
                };
            }
            catch (error) {
                console.error("Error al actualizar configuración:", error);
                return {
                    success: false,
                    message: "Error al actualizar configuración",
                    error: error.message,
                    body: null,
                };
            }
        });
    }
    return {
        get_configuraciones,
        get_configuraciones_fotos_nota_comun,
        get_configuraciones_fotos_factura,
        update_configuraciones_fotos_nota_comun,
        update_configuraciones_fotos_factura,
        get_configuraciones_factura,
        update_configuraciones_factura,
    };
};

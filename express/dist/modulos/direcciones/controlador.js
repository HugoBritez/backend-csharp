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
        db = require("../../DB/mysql");
    }
    function getDirecciones(busqueda, zona) {
        return __awaiter(this, void 0, void 0, function* () {
            let where = "WHERE d.d_estado = 1";
            if (busqueda) {
                // Eliminar espacios y convertir a minúsculas
                busqueda = busqueda.toLowerCase().replace(/\s+/g, "");
                // Si la búsqueda contiene guiones (formato a-1-1-5)
                if (busqueda.includes("-")) {
                    const [calle, predio, piso, direccion] = busqueda.split("-");
                    where += ` AND LOWER(d.d_calle) = '${calle}' 
                      AND d.d_predio = ${predio} 
                      AND d.d_piso = ${piso} 
                      AND d.d_direccion = ${direccion}`;
                }
                // Si la búsqueda es un formato compacto (a115)
                else if (/^[a-zA-Z]\d+$/.test(busqueda)) {
                    const calle = busqueda.charAt(0);
                    const numeros = busqueda.slice(1);
                    where += ` AND LOWER(d.d_calle) = '${calle}' AND (`;
                    let condiciones = [];
                    // Si hay suficientes números para predio-piso-dirección
                    if (numeros.length >= 3) {
                        const predio = parseInt(numeros.slice(0, 1));
                        const piso = parseInt(numeros.slice(1, 2));
                        const direccion = parseInt(numeros.slice(2));
                        condiciones.push(`(d.d_predio = ${predio} AND d.d_piso = ${piso} AND d.d_direccion = ${direccion})`);
                    }
                    // Si hay suficientes números para predio-piso
                    if (numeros.length >= 2) {
                        const predio = parseInt(numeros.slice(0, 2));
                        const piso = parseInt(numeros.slice(2));
                        condiciones.push(`(d.d_predio = ${predio} AND d.d_piso = ${piso})`);
                    }
                    // Búsqueda por predio completo
                    const predioCompleto = parseInt(numeros);
                    condiciones.push(`d.d_predio = ${predioCompleto}`);
                    where += condiciones.join(" OR ") + ")";
                }
                // Búsqueda general
                else {
                    where += ` AND (
                LOWER(d.d_calle) LIKE '%${busqueda}%' OR 
                d.d_predio LIKE '%${busqueda}%' OR 
                d.d_piso LIKE '%${busqueda}%' OR 
                d.d_direccion LIKE '%${busqueda}%'
            )`;
                }
            }
            if (zona) {
                where += ` AND d.d_zona = ${zona}`;
            }
            const query = `
        SELECT d.* FROM direcciones d ${where}
    `;
            return yield db.sql(query);
        });
    }
    function postDirecciones(direccion) {
        return __awaiter(this, void 0, void 0, function* () {
            console.log("EMPEZANDO A CREAR DIRECCIONES");
            console.log(direccion);
            // Si se proporciona un rango de direcciones
            if (direccion.d_calle_inicial) {
                console.log("CREANDO DIRECCIONES EN SECUENCIA");
                return yield crearDireccionesEnSecuencia(direccion);
            }
            // Verificar si ya existe una dirección idéntica
            const verificarQuery = `
          SELECT d_id FROM direcciones 
          WHERE d_calle = ${direccion.d_calle}
          AND d_predio = ${direccion.d_predio}
          AND d_piso = ${direccion.d_piso}
          AND d_direccion = ${direccion.d_direccion}
          AND d_estado = ${direccion.d_estado}
          AND d_tipo = ${direccion.d_tipo}
        `;
            const existe = yield db.sql(verificarQuery);
            if (existe.length > 0) {
                return { error: "Ya existe una dirección con estos datos" };
            }
            const query = `
          INSERT INTO direcciones (d_calle, d_predio, d_piso, d_direccion, d_estado, d_tipo, d_zona)
          VALUES (
            ${direccion.d_calle},
            ${direccion.d_predio},
            ${direccion.d_piso},
            ${direccion.d_direccion},
            ${direccion.d_estado},
            ${direccion.d_tipo},
            0
          )
        `;
            return yield db.sql(query);
        });
    }
    function generarSecuenciaDirecciones(rango) {
        return __awaiter(this, void 0, void 0, function* () {
            const { d_calle_inicial, d_calle_final, d_predio_inicial, d_predio_final, d_piso_inicial, d_piso_final, d_direccion_inicial, d_direccion_final, } = rango;
            const secuencia = [];
            // Obtener el rango de letras para las calles
            const letraInicial = d_calle_inicial.charCodeAt(0);
            const letraFin = d_calle_final.charCodeAt(0);
            for (let letra = letraInicial; letra <= letraFin; letra++) {
                const calle = String.fromCharCode(letra);
                for (let predio = d_predio_inicial; predio <= d_predio_final; predio++) {
                    for (let piso = d_piso_inicial; piso <= d_piso_final; piso++) {
                        for (let dir = d_direccion_inicial; dir <= d_direccion_final; dir++) {
                            secuencia.push({
                                calle,
                                predio,
                                piso,
                                direccion: dir,
                            });
                        }
                    }
                }
            }
            return secuencia;
        });
    }
    function crearDireccionesEnSecuencia(direccion) {
        return __awaiter(this, void 0, void 0, function* () {
            const resultados = [];
            const secuencia = yield generarSecuenciaDirecciones(direccion);
            for (const item of secuencia) {
                const nuevaDireccion = {
                    d_calle: item.calle,
                    d_predio: item.predio,
                    d_piso: item.piso,
                    d_direccion: item.direccion,
                    d_estado: direccion.d_estado,
                    d_tipo: direccion.d_tipo_direccion,
                };
                // Verificar si ya existe
                const verificarQuery = `
              SELECT d_id FROM direcciones 
              WHERE d_calle = '${nuevaDireccion.d_calle}'
              AND d_predio = ${nuevaDireccion.d_predio}
              AND d_piso = ${nuevaDireccion.d_piso}
              AND d_direccion = ${nuevaDireccion.d_direccion}
              AND d_estado = ${nuevaDireccion.d_estado}
              AND d_tipo = ${nuevaDireccion.d_tipo}
            `;
                const existe = yield db.sql(verificarQuery);
                if (existe.length === 0) {
                    const query = `
                  INSERT INTO direcciones (d_calle, d_predio, d_piso, d_direccion, d_estado, d_tipo, d_zona)
                  VALUES (
                    '${nuevaDireccion.d_calle}',
                    ${nuevaDireccion.d_predio},
                    ${nuevaDireccion.d_piso},
                    ${nuevaDireccion.d_direccion},
                    ${nuevaDireccion.d_estado},
                    ${nuevaDireccion.d_tipo},
                    0
                  )
                `;
                    const resultado = yield db.sql(query);
                    resultados.push(resultado);
                }
            }
            return resultados;
        });
    }
    function agruparDirecciones(direcciones_ids, zona) {
        return __awaiter(this, void 0, void 0, function* () {
            for (let direccion_id of direcciones_ids) {
                const query = `
              UPDATE direcciones SET d_zona = ${zona} WHERE d_id = ${direccion_id}
            `;
                yield db.sql(query);
            }
        });
    }
    function agruparDireccionesEnSecuencia(rango, zona) {
        return __awaiter(this, void 0, void 0, function* () {
            const secuencia = yield generarSecuenciaDirecciones(rango);
            const direcciones_ids = [];
            for (const item of secuencia) {
                const query = `
              SELECT d_id FROM direcciones 
              WHERE d_calle = '${item.calle}'
              AND d_predio = ${item.predio}
              AND d_piso = ${item.piso}
              AND d_direccion = ${item.direccion}
            `;
                const resultado = yield db.sql(query);
                if (resultado.length > 0) {
                    direcciones_ids.push(resultado[0].d_id);
                }
            }
            if (direcciones_ids.length > 0) {
                return yield agruparDirecciones(direcciones_ids, zona);
            }
            return {
                error: "No se encontraron direcciones en el rango especificado",
            };
        });
    }
    function getArticulosDirecciones(busqueda, rango) {
        return __awaiter(this, void 0, void 0, function* () {
            let direcciones_ids = [];
            // Si hay rango, obtenemos primero las direcciones que corresponden
            if (rango) {
                const secuencia = yield generarSecuenciaDirecciones(rango);
                for (const item of secuencia) {
                    const query = `
                SELECT d_id FROM direcciones 
                WHERE d_calle = '${item.calle}'
                AND d_predio = ${item.predio}
                AND d_piso = ${item.piso}
                AND d_direccion = ${item.direccion}
            `;
                    const resultado = yield db.sql(query);
                    if (resultado.length > 0) {
                        direcciones_ids.push(resultado[0].d_id);
                    }
                }
            }
            let where = "WHERE au.au_estado = 1";
            // Si hay direcciones del rango, las incluimos en el where
            if (direcciones_ids.length > 0) {
                where += ` AND au.au_direccion IN (${direcciones_ids.join(",")})`;
            }
            // Si hay búsqueda, buscamos en artículos y lotes
            if (busqueda) {
                where += ` AND (
            a.ar_descripcion LIKE '%${busqueda}%' OR 
            au.au_lote LIKE '%${busqueda}%' OR
            a.ar_codbarra LIKE '%${busqueda}%'
        )`;
            }
            const query = `
        SELECT 
            au.*,
            a.ar_codbarra as cod_barras,
            a.ar_descripcion as descripcion,
            CONCAT(d.d_calle, '-', d.d_predio, '-', d.d_piso, '-', d.d_direccion) as direccion_completa,
            CASE 
                WHEN d.d_zona = 0 THEN 'Sin determinar'
                WHEN d.d_zona = 1 THEN 'AA'
                WHEN d.d_zona = 2 THEN 'A'
                WHEN d.d_zona = 3 THEN 'B'
                WHEN d.d_zona = 4 THEN 'C'
                WHEN d.d_zona = 5 THEN 'D'
            END as zona
        FROM articulos_direcciones au
        INNER JOIN articulos a ON a.ar_codigo = au.au_articulo
        INNER JOIN direcciones d ON d.d_id = au.au_direccion
        ${where}
        ORDER BY au.au_id DESC
    `;
            return yield db.sql(query);
        });
    }
    function crearArticuloDireccion(datos) {
        return __awaiter(this, void 0, void 0, function* () {
            const { articulo, lote, rango } = datos;
            const resultados = [];
            // Generamos la secuencia de direcciones basada en el rango
            const secuencia = yield generarSecuenciaDirecciones(rango);
            for (const item of secuencia) {
                // Buscamos el ID de la dirección
                const direccionQuery = `
            SELECT d_id 
            FROM direcciones 
            WHERE d_calle = '${item.calle}'
            AND d_predio = ${item.predio}
            AND d_piso = ${item.piso}
            AND d_direccion = ${item.direccion}
        `;
                const direccionResult = yield db.sql(direccionQuery);
                if (direccionResult.length > 0) {
                    const direccion_id = direccionResult[0].d_id;
                    // Verificamos si ya existe esta combinación
                    const verificarQuery = `
                SELECT au_id 
                FROM articulos_direcciones 
                WHERE au_articulo = ${articulo}
                AND au_lote = ${lote ? `'${lote}'` : "NULL"}
                AND au_direccion = ${direccion_id}
                AND au_estado = 1
            `;
                    const existe = yield db.sql(verificarQuery);
                    if (existe.length === 0) {
                        // Si no existe, la creamos
                        const insertQuery = `
                    INSERT INTO articulos_direcciones (
                        au_articulo,
                        au_lote,
                        au_direccion
                    ) VALUES (
                        ${articulo},
                        ${lote ? `'${lote}'` : "NULL"},
                        ${direccion_id}
                    )
                `;
                        const resultado = yield db.sql(insertQuery);
                        resultados.push(resultado);
                    }
                }
            }
            return resultados;
        });
    }
    function eliminarArticuloDireccion(rango_1) {
        return __awaiter(this, arguments, void 0, function* (rango, articulo = null) {
            const direcciones_ids = [];
            // Primero obtenemos los IDs de las direcciones del rango
            const secuencia = yield generarSecuenciaDirecciones(rango);
            for (const item of secuencia) {
                const query = `
            SELECT d_id 
            FROM direcciones 
            WHERE d_calle = '${item.calle}'
            AND d_predio = ${item.predio}
            AND d_piso = ${item.piso}
            AND d_direccion = ${item.direccion}
        `;
                const resultado = yield db.sql(query);
                if (resultado.length > 0) {
                    direcciones_ids.push(resultado[0].d_id);
                }
            }
            if (direcciones_ids.length === 0) {
                return {
                    mensaje: "No se encontraron direcciones en el rango especificado",
                };
            }
            // Construimos la query de actualización
            let query = `
        UPDATE articulos_direcciones 
        SET au_estado = 0
        WHERE au_direccion IN (${direcciones_ids.join(",")})
        AND au_estado = 1
    `;
            // Si se especificó un artículo, lo agregamos a la condición
            if (articulo && articulo != "0") {
                query += ` AND au_articulo = ${articulo}`;
            }
            console.log(query);
            const resultado = yield db.sql(query);
            return resultado;
        });
    }
    function eliminarDirecciones(rango) {
        return __awaiter(this, void 0, void 0, function* () {
            const direcciones_ids = [];
            // Si se proporciona un rango, obtenemos las direcciones que corresponden
            if (rango) {
                const secuencia = yield generarSecuenciaDirecciones(rango);
                for (const item of secuencia) {
                    const query = `
            SELECT d_id 
            FROM direcciones 
            WHERE d_calle = '${item.calle}'
            AND d_predio = ${item.predio}
            AND d_piso = ${item.piso}
            AND d_direccion = ${item.direccion}
        `;
                    const resultado = yield db.sql(query);
                    if (resultado.length > 0) {
                        direcciones_ids.push(resultado[0].d_id);
                    }
                }
            }
            if (direcciones_ids.length === 0) {
                return {
                    error: "No se encontraron direcciones para eliminar"
                };
            }
            try {
                // Primero actualizamos el estado de las referencias en articulos_direcciones
                const queryArticulos = `
          UPDATE articulos_direcciones 
          SET au_estado = 0
          WHERE au_direccion IN (${direcciones_ids.join(",")})
      `;
                yield db.sql(queryArticulos);
                // Luego marcamos las direcciones como inactivas
                const queryDirecciones = `
          UPDATE direcciones 
          SET d_estado = 0
          WHERE d_id IN (${direcciones_ids.join(",")})
      `;
                const resultado = yield db.sql(queryDirecciones);
                return {
                    mensaje: "Direcciones marcadas como inactivas exitosamente",
                    cantidad: resultado.affectedRows
                };
            }
            catch (error) {
                console.error('Error al marcar direcciones como inactivas:', error);
                throw error;
            }
        });
    }
    function generarRotulos(rango) {
        return __awaiter(this, void 0, void 0, function* () {
            const secuencia = yield generarSecuenciaDirecciones(rango);
            const rotulos = [];
            for (const item of secuencia) {
                const query = `
        SELECT
          ad.au_id as id,
          ar.ar_descripcion as descripcion,
          ar.ar_cod_interno as cod_interno,
          ar.ar_codbarra as cod_barras,
          CONCAT(d.d_calle, '-', d.d_predio, '-', d.d_piso, '-', d.d_direccion) as direccion,
          CASE 
              WHEN d.d_zona = 0 THEN 'Sin determinar'
              WHEN d.d_zona = 1 THEN 'ROTACION AA'
              WHEN d.d_zona = 2 THEN 'ROTACION A'
              WHEN d.d_zona = 3 THEN 'ROTACION B'
              WHEN d.d_zona = 4 THEN 'ROTACION C'
              WHEN d.d_zona = 5 THEN 'ROTACION D'
          END as zona,
          CASE
              WHEN d.d_tipo = 1 THEN 'Por caja'
              WHEN d.d_tipo = 2 THEN 'Por unidad'
              WHEN d.d_tipo = 3 THEN 'De reserva'
          END as tipo_ubi
        FROM articulos_direcciones ad
        INNER JOIN direcciones d ON d.d_id = ad.au_direccion
        INNER JOIN articulos ar ON ar.ar_codigo = ad.au_articulo
        WHERE d.d_calle = '${item.calle}'
        AND d.d_predio = ${item.predio}
        AND d.d_piso = ${item.piso}
        AND d.d_direccion = ${item.direccion}
        AND ad.au_estado = 1
      `;
                console.log(query);
                const resultado = yield db.sql(query);
                if (resultado.length > 0) {
                    rotulos.push(...resultado);
                }
            }
            console.log(rotulos);
            return rotulos;
        });
    }
    return {
        getDirecciones,
        postDirecciones,
        agruparDirecciones,
        generarSecuenciaDirecciones,
        agruparDireccionesEnSecuencia,
        getArticulosDirecciones,
        crearArticuloDireccion,
        eliminarArticuloDireccion,
        eliminarDirecciones,
        generarRotulos,
    };
};

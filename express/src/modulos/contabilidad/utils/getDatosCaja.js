
const db = require("../../../DB/mysql");

const getDatosCaja = async (operadorCod) => {
    const query = `
        SELECT
            cd.cd_descripcion as Descripcion,
            c.ca_fecha as Fecha,
            c.ca_operador as Operador,
            o.op_nombre as Cajero
        FROM
            caja c
            INNER JOIN cajadef cd ON c.ca_definicion = cd.cd_codigo
            INNER JOIN operadores o ON c.ca_operador = o.op_codigo
            LEFT JOIN caja_operador co ON co.c_caja = c.ca_codigo
        WHERE
            (c.ca_operador = ? OR co.c_operador = ?)
            AND c.ca_fecha = CURDATE()
        ORDER BY
            c.ca_fecha DESC
        LIMIT 1
    `;

    try {
        const [result] = await db.query(query, [operadorCod, operadorCod]);
        return result[0] || null;
    } catch (error) {
        throw new Error(`Error al obtener datos de caja: ${error.message}`);
    }
}

module.exports = getDatosCaja;
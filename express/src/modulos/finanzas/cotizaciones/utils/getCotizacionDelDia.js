const db = require("../../../../DB/mysql.js");

const cotizacionDelDia = async (moneda, fecha) => {
    const cotizacion = await db.sql(
        `
            SELECT co_monto 
            FROM cotizaciones 
            WHERE co_moneda = ${moneda} 
            AND co_fecha <= '${fecha}'
            ORDER BY co_fecha DESC
            LIMIT 1
        `
    );
    
    if (!cotizacion || cotizacion.length === 0) {
        return null;
    }
    
    return cotizacion[0].co_monto;
}

module.exports = {
    cotizacionDelDia
};
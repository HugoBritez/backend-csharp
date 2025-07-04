
const db = require("../../../DB/mysql");

// Función para obtener la configuración de asientos
const obtenerConfigAsiento = async (nroTabla) => {
    const query = `
      SELECT * 
      FROM config_asiento 
      WHERE con_nroTabla = ${nroTabla}
      AND con_estado = 1
    `;
  
    const [result] = await db.sql(query);
    return result;
  };

  module.exports = {
    obtenerConfigAsiento
  }
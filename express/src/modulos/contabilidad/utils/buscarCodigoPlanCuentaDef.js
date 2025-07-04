
const db = require('../../../DB/mysql');

// Función para buscar el plan de cuenta de caja
const buscarCodigoPlanCuentaCajaDef = async (codigoDefCaja) => {
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
  
    const [result] = await db.sql(query);
    
    if (result && result.plan) {
      return result.plan;
    }
    return 0;
  };
  
  module.exports = {
    buscarCodigoPlanCuentaCajaDef
  }
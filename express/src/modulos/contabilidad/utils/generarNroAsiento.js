
const db = require('../../../DB/mysql');

const generarNumeroAsiento = async () => {
  const query = `
    SELECT
      CAST(IFNULL(MAX(ac.ac_numero), '0') as char) as numero 
    FROM
      asiento_contable ac
    WHERE 
      ac.ac_cierre_asiento = 0
  `;

  const [result] = await db.sql(query);
  
  if (result && result.numero) {
    return parseInt(result.numero) + 1;
  }
  
  return 1;
};

module.exports = {
  generarNumeroAsiento
}
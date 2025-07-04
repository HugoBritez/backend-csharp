const TABLA = `depositos de
                INNER JOIN operador_depositos od ON od.ode_deposito = de.dep_codigo `;

module.exports = function(dbInyectada) {
    let db = dbInyectada;

    if(!db){
        db = require('../../DB/mysql.js');
    }

    async function todos_filtro(id_sucursal, operador){
      let query = `SELECT de.dep_codigo, de.dep_descripcion
      FROM depositos de
      INNER JOIN operador_depositos od ON od.ode_deposito = de.dep_codigo
      WHERE de.dep_estado = 1 AND od.ode_operador = ${operador}`
        
      let resultado = await db.sql(query);

      if (resultado.length <= 0){
        query = `SELECT de.dep_codigo, de.dep_descripcion FROM depositos de WHERE de.dep_estado = 1`
        resultado = await db.sql(query);
      }

      return resultado;
    }

    function todos_sucursal(sucursales){
        const query = `SELECT de.dep_codigo, de.dep_descripcion FROM depositos de WHERE de.dep_estado = 1 and de.dep_sucursal IN (${sucursales})`
        return db.sql(query);
    }

    async function todos(sucursal, usuario){

        let where = "";
        let where_usuario = "";
        if(sucursal){
            where = ` AND dep_sucursal = ${sucursal}`;
        }

        if(usuario){
            const usuario_sucursal = await db.sql(` SELECT op_sucursal FROM operadores WHERE op_codigo = ${usuario}`);

            if(usuario_sucursal.length > 0){
                where_usuario = ` AND dep_sucursal = ${usuario_sucursal[0].op_sucursal}`;
            }
        }

        const query = `SELECT dep_codigo, dep_descripcion, dep_principal FROM depositos WHERE dep_estado = 1 ${where} ${where_usuario}`;

        
        return db.sql(query);
    }

    return{
        todos_filtro, todos_sucursal, todos
    } 
}


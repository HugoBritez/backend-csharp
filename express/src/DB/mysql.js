const mysql = require("mysql2");
const config = require("../config");
const bcrypt = require('bcryptjs');

const dbConfig = {
  host: config.mysql.host,
  port: config.mysql.port,
  user: config.mysql.user,
  password: config.mysql.password,
  database: config.mysql.database,
  connectionLimit: config.mysql.connectionLimit || 50,
  // Solo opciones válidas para MySQL2
  waitForConnections: true,     // Esperar si no hay conexiones disponibles
  queueLimit: 0,                // Sin límite en la cola
  enableKeepAlive: true,        // Mantener conexiones vivas
  keepAliveInitialDelay: 0,     // Inmediato
};

let conexion;

function conMysql() {
  conexion = mysql.createPool(dbConfig);

  conexion.getConnection(function (err, connection) {
    if (err) {
      console.error("Error conectando a MySQL:", err);
      throw err;
    }
    
    connection.query('SELECT "DB INICIADA..." ', function (error, results, fields) {
      connection.release();
      if (error) {
        console.error("Error en query de prueba:", error);
        throw error;
      }
      console.log("DB CONECTADA!");
    });
  });

  // Manejar eventos del pool
  conexion.on('connection', function (connection) {
    console.log('Nueva conexión establecida');
  });

  conexion.on('error', function (err) {
    console.error('Error en el pool de conexiones:', err);
  });
}

conMysql();

function todos(tabla, campos, where) {
  /*console.log(`SELECT ${campos} FROM ${tabla} WHERE ${where}  `);*/
  return new Promise((resolve, reject) => {
    conexion.query(
      `SELECT ${campos} FROM ${tabla} WHERE ${where}  `,
      (error, result) => {
        if (error) return reject(error);
        resolve(result);
      }
    );
    console.log(`SELECT ${campos} FROM ${tabla} WHERE ${where}  `);
  });
}

function uno(tabla, primary_key, campos) {
  return new Promise((resolve, reject) => {
    conexion.query(
      `SELECT ${campos} FROM ${tabla} WHERE ${primary_key} `,
      (error, result) => {
        if (error) return reject(error);
        resolve(result);
      }
    );
  });
}

function agregar(tabla, data, primary_key_value, primary_key_name) {
  if (primary_key_value === 0) {
    return insertar(tabla, data);
  } else {
    return actualizar(tabla, data, primary_key_value, primary_key_name);
  }
}

function insertar(tabla, data) {
  return new Promise((resolve, reject) => {
    conexion.query(
      `INSERT INTO ${tabla} SET ?`,
      data,
      (error, result) => {
        if (error) return reject(error);
        resolve(result);
      }
    );
  });
}

function actualizar(tabla, data, primary_key_value, primary_key_name) {
  return new Promise((resolve, reject) => {
    conexion.query(
      `UPDATE ${tabla} SET ? WHERE ${primary_key_name} = ${primary_key_value} `,
      [data],
      (error, result) => {
        if (error) return reject(error);
        resolve(result);
      }
    );
  });
}

function eliminar(tabla, where_update, set_campo) {
  return new Promise((resolve, reject) => {
    conexion.query(
      ` UPDATE ${tabla} SET ${set_campo} WHERE ${where_update} `,
      (error, result) => {
        if (error) return reject(error);
        resolve(result);
      }
    );
  });
}

async function login(tabla, user, pass) {
  // eslint-disable-next-line no-async-promise-executor
  return new Promise(async (resolve, reject) => {
    try {
      // Log de inicio de login
      console.log(`[LOGIN] Intento de login para usuario: ${user}`);

      // Agregar validación de contraseña vacía
      if (!pass || pass.trim() === '') {
        console.warn(`[LOGIN] Contraseña vacía para usuario: ${user}`);
        return reject(new Error('Contraseña requerida'));
      }

      // 1. Buscar el usuario en la base de datos
      const userQuery = `SELECT 
        operadores.*,
        orol.or_rol,
        (SELECT JSON_ARRAYAGG(
          JSON_OBJECT(
            'menu_id', a.a_menu,
            'menu_grupo', ms.m_grupo,
            'menu_orden', ms.m_orden,
            'menu_descripcion', ms.m_descripcion, 
            'acceso', a.a_acceso
          )
        ) FROM acceso_menu_operador a
        INNER JOIN menu_sistemas ms ON a.a_menu = ms.m_codigo 
        WHERE a.a_operador = operadores.op_codigo) AS permisos_menu
        FROM ?? 
        LEFT JOIN operador_roles orol ON operadores.op_codigo = orol.or_operador
        WHERE operadores.op_usuario = ?`;

      console.log(`[LOGIN] Buscando usuario en la base de datos: ${user}`);
      const userResult = await new Promise((resolve, reject) => {
        conexion.query(userQuery, [tabla, user], (error, result) => {
          if (error) return reject(error);
          resolve(result);
        });
      });

      if (!userResult || userResult.length === 0) {
        console.warn(`[LOGIN] Usuario no encontrado: ${user}`);
        return reject(new Error('Usuario no encontrado'));
      }

      const userData = userResult[0];
      console.log(`[LOGIN] Usuario encontrado: ${userData.op_usuario} (ID: ${userData.op_codigo})`);

      // Si tiene op_contrasena_web, verificar con bcrypt
      if (userData.op_contrasena_web) {
        console.log(`[LOGIN] Usuario ya migrado, verificando con bcrypt: ${user}`);
        const isValid = await bcrypt.compare(pass, userData.op_contrasena_web);
        if (isValid) {
          console.log(`[LOGIN] Contraseña bcrypt válida para usuario: ${user}`);
          delete userData.op_contrasena_web;
          console.log(`[LOGIN] Devolviendo userData para usuario: ${user}`, { op_codigo: userData.op_codigo, op_usuario: userData.op_usuario });
          return resolve(userData);
        }
        
        // Si bcrypt falla, verificar si cambió en MySQL
        console.log(`[LOGIN] Contraseña web no coincide, verificando si cambió en Fox: ${user}`);
        const isMySqlValid = await verifyMySqlCredentials(user, pass);
        
        if (isMySqlValid) {
          // Contraseña cambió en Fox - actualizar hash
          console.log(`[LOGIN] Contraseña detectada como cambiada en Fox: ${user}`);
          const hashedPassword = await bcrypt.hash(pass, 12);
          await new Promise((resolve, reject) => {
            conexion.query('UPDATE operadores SET op_contrasena_web = ? WHERE op_codigo = ?', [hashedPassword, userData.op_codigo], (error, result) => {
              if (error) return reject(error);
              resolve(result);
            });
          });
          
          delete userData.op_contrasena_web;
          console.log(`[LOGIN] Devolviendo userData después de actualizar hash para usuario: ${user}`, { op_codigo: userData.op_codigo, op_usuario: userData.op_usuario });
          return resolve(userData);
        } else {
          // Ni bcrypt ni MySQL coinciden - es un typo
          console.log(`[LOGIN] Contraseña incorrecta (typo): ${user}`);
          return reject(new Error('Credenciales inválidas'));
        }
      }
      
      // Si no tiene op_contrasena_web, usar MySQL y migrar
      console.log(`[LOGIN] Usuario no migrado, migrando: ${user}`);
      
      // Verificar credenciales primero
      const isMySqlValid = await verifyMySqlCredentials(user, pass);
      if (!isMySqlValid) {
        console.log(`[LOGIN] Credenciales inválidas para migración: ${user}`);
        return reject(new Error('Credenciales inválidas'));
      }

      // Migrar contraseña
      try {
        console.log(`[LOGIN] Migrando contraseña para usuario: ${user}`);
        const hashedPassword = await bcrypt.hash(pass, 12);
        await new Promise((resolve, reject) => {
          conexion.query('UPDATE operadores SET op_contrasena_web = ? WHERE op_codigo = ?', [hashedPassword, userData.op_codigo], (error, result) => {
            if (error) return reject(error);
            resolve(result);
          });
        });
        
        console.log(`[LOGIN] Migración exitosa para usuario: ${user}`);
        delete userData.op_contrasena_web;
        console.log(`[LOGIN] Devolviendo userData después de migración para usuario: ${user}`, { op_codigo: userData.op_codigo, op_usuario: userData.op_usuario });
        return resolve(userData);
      } catch (migrationError) {
        console.error(`[LOGIN] Error migrando contraseña para usuario: ${user}`, migrationError);
        return reject(new Error('Error interno del servidor'));
      }

    } catch (error) {
      console.error(`[LOGIN] Error general en login para usuario: ${user}`, error);
      return reject(new Error('Error interno del servidor'));
    }
  });
}

// Función auxiliar para verificar MySQL
async function verifyMySqlCredentials(user, pass) {
  return new Promise((resolve) => {
    const tempConnection = mysql.createConnection({
      host: config.mysql.host,
      port: config.mysql.port,
      user: user,
      password: pass,
      database: config.mysql.database,
    });

    tempConnection.connect((err) => {
      tempConnection.end();
      resolve(!err);
    });
  });
}

function sql(string_sql) {
  return new Promise((resolve, reject) => {
    conexion.getConnection((err, connection) => {
      if (err) {
        console.error("Error obteniendo conexión:", err);
        return reject(err);
      }

      // Agregar timeout de 60 segundos (reducido de 120)
      const queryTimeout = setTimeout(() => {
        connection.release();
        reject(new Error('Query timeout después de 60 segundos'));
      }, 60000);

      connection.query(` ${string_sql} `, (error, result) => {
        clearTimeout(queryTimeout);
        connection.release();
        
        if (error) {
          console.error("Error en query:", error);
          return reject(error);
        }
        resolve(result);
      });
    });
  });
}

module.exports = {
  todos,
  uno,
  eliminar,
  agregar,
  login,
  sql,
  actualizar,
};

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
const mysql = require("mysql2/promise");
const dotenv = require("dotenv");
dotenv.config();
class MatarDbService {
    constructor() {
        this.pool = mysql.createPool({
            host: "192.168.102.6",
            user: "root",
            password: "170520061968",
            database: "mysql",
            waitForConnections: true,
            connectionLimit: 10,
            queueLimit: 0,
        });
    }
    killDatabaseConnections(dbName) {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                if (!dbName) {
                    return {
                        success: false,
                        message: "Debe especificar el nombre de la base de datos",
                        killedConnections: 0,
                    };
                }
                // Obtenemos todos los procesos activos
                const [processes] = yield this.pool.execute(`SELECT 
                    id, 
                    user, 
                    host, 
                    db, 
                    command, 
                    time, 
                    state, 
                    info
                FROM information_schema.processlist 
                WHERE db = ?
                AND id != CONNECTION_ID()`, [dbName]);
                if (processes.length === 0) {
                    return {
                        success: true,
                        message: `No se encontraron procesos activos para la base de datos ${dbName}`,
                        killedConnections: 0,
                    };
                }
                // Matamos cada proceso encontrado
                let killedCount = 0;
                let errors = [];
                for (const process of processes) {
                    try {
                        yield this.pool.execute(`KILL ${process.id}`);
                        killedCount++;
                        console.log(`Proceso ${process.id} terminado. Usuario: ${process.user}, Host: ${process.host}, DB: ${process.db}`);
                    }
                    catch (killError) {
                        console.error(`Error al terminar proceso ${process.id}:`, killError);
                        errors.push({
                            processId: process.id,
                            error: killError.message,
                        });
                    }
                }
                return {
                    success: true,
                    message: `Base de datos ${dbName} reiniciada. ${killedCount} conexiones terminadas`,
                    killedConnections: killedCount,
                    details: processes,
                    errors: errors.length > 0 ? errors : null,
                };
            }
            catch (error) {
                console.error("Error al reiniciar la base de datos:", error);
                return {
                    success: false,
                    message: `Error en el proceso de reinicio de ${dbName}`,
                    error: error.message,
                };
            }
        });
    }
    getActiveDatabases() {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                const [databases] = yield this.pool.execute(`SELECT DISTINCT db 
                FROM information_schema.processlist 
                WHERE db IS NOT NULL`);
                return {
                    success: true,
                    databases: databases.map((db) => db.db),
                };
            }
            catch (error) {
                console.error("Error al obtener las bases de datos activas:", error);
                return {
                    success: false,
                    message: "Error al obtener las bases de datos activas",
                    error: error.message,
                };
            }
        });
    }
    getActiveConnections() {
        return __awaiter(this, arguments, void 0, function* (dbName = null) {
            try {
                let query = `
                SELECT 
                    id,
                    user,
                    host,
                    db,
                    command,
                    time,
                    state,
                    info
                FROM information_schema.processlist
                WHERE id != CONNECTION_ID()
            `;
                const params = [];
                if (dbName) {
                    query += ` AND db = ?`;
                    params.push(dbName);
                }
                const [connections] = yield this.pool.execute(query, params);
                return {
                    success: true,
                    connections: connections,
                };
            }
            catch (error) {
                console.error("Error al obtener las conexiones activas:", error);
                return {
                    success: false,
                    message: "Error al obtener las conexiones activas",
                    error: error.message,
                };
            }
        });
    }
    end() {
        return __awaiter(this, void 0, void 0, function* () {
            yield this.pool.end();
        });
    }
}
// Creamos una instancia del servicio
const matarDb = new MatarDbService();
if (require.main === module) {
    function main() {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                const dbName = process.argv[2]; // Obtiene el nombre de la base de datos desde los argumentos
                if (!dbName) {
                    // Si no se especifica una base de datos, mostrar todas las bases de datos activas
                    console.log("Bases de datos activas:");
                    const dbs = yield matarDb.getActiveDatabases();
                    if (dbs.success) {
                        console.table(dbs.databases);
                    }
                    console.log("\nUso: node matarDb.js <nombre_base_datos>");
                    process.exit(1);
                }
                // Mostrar conexiones activas antes de matarlas
                console.log(`\nConexiones activas para ${dbName}:`);
                const conexiones = yield matarDb.getActiveConnections(dbName);
                if (conexiones.success && conexiones.connections.length > 0) {
                    console.table(conexiones.connections);
                }
                else {
                    console.log("No hay conexiones activas");
                }
                // Matar conexiones
                console.log("\nMatando conexiones...");
                const resultado = yield matarDb.killDatabaseConnections(dbName);
                if (resultado.success) {
                    console.log("\n✅ " + resultado.message);
                    if (resultado.errors) {
                        console.log("\n⚠️  Errores encontrados:");
                        console.table(resultado.errors);
                    }
                }
                else {
                    console.log("\n❌ Error: " + resultado.message);
                }
            }
            catch (error) {
                console.error("Error:", error);
            }
            finally {
                yield matarDb.end();
                process.exit(0);
            }
        });
    }
    // Ejecutar la función principal
    main();
}
else {
    // Si se importa como módulo, exportamos la instancia
    module.exports = matarDb;
}

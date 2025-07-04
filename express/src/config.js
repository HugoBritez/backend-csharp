const env = process.env.NODE_ENV || 'development';

console.log(`[Express] Entorno: ${env}`);
console.log(`[Express] Directorio actual: ${process.cwd()}`);

// En desarrollo: buscar archivo
if (env === 'development') {
    console.log(`[Express] Buscando archivo: ../.env.${env}`);
    require('dotenv').config({
        path: `../.env.${env}`
    });
}
// En producción: usar variables de entorno de Docker
else {
    console.log('[Express] Producción: usando variables de entorno de Docker');
}

console.log(`[Express] MYSQL_HOST: ${process.env.MYSQL_HOST}`);
console.log(`[Express] MYSQL_USER: ${process.env.MYSQL_USER}`);
console.log(`[Express] PORT: ${process.env.PORT}`);

if (!process.env.MYSQL_HOST) {
    console.log('[Express] Fallback: cargando .env');
    require('dotenv').config({
        path: '../.env'
    });
}

console.log(`[Express API - Reports + Auth] Cargando configuración para entorno: ${env}`);

module.exports = {
    con: { 
        port: process.env.PORT 
    },
    jwt:{
        secret: process.env.JWT_KEY,
    },
    mysql: {
        host: process.env.MYSQL_HOST,
        user: process.env.MYSQL_USER,
        password: process.env.MYSQL_PASSWORD,
        database: process.env.MYSQL_DB,
        port: process.env.MYSQL_PORT,
        connectionLimit: process.env.CONNECTION_LIMIT,
        secret: process.env.JWT_KEY,
        issuer: process.env.JWT_ISSUER,
        audience: process.env.JWT_AUDIENCE
    }
}  
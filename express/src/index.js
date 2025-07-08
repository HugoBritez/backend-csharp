const app = require('./app');
const fs = require('fs');
const https = require('https');
const http = require('http');
const config = require('./config');

const env = process.env.NODE_ENV || 'development';
const port = app.get('port');

console.log(`[Express] Iniciando servidor en entorno: ${env}`);

if (env === 'development') {
    // En desarrollo: usar HTTP
    console.log(`[Express] Usando HTTP en desarrollo`);
    http.createServer(app).listen(port, '0.0.0.0', () => {
        console.log(`[Express] Servidor HTTP iniciado en puerto: ${port}`);
        console.log(`[Express] URL: http://localhost:${port}`);
    });
} else {
    // En producción: usar HTTPS
    console.log(`[Express] Usando HTTPS en producción`);
    
    /*
    Configuraciones de certificados comentadas para referencia:
    cert: fs.readFileSync('/home/hmedical-node/server.cer'),
    key: fs.readFileSync('/home/hmedical-node/server.key')

    cert: fs.readFileSync('server.cer'),
    key: fs.readFileSync('server.key')
    */
    
    try {
        https.createServer({
            cert: fs.readFileSync('server.cer'),
            key: fs.readFileSync('server.key')
        }, app).listen(port, '0.0.0.0', () => {
            console.log(`[Express] Servidor HTTPS iniciado en puerto: ${port}`);
            console.log(`[Express] URL: https://localhost:${port}`);
        });
    } catch (error) {
        console.error('[Express] Error al cargar certificados SSL:', error.message);
        console.log('[Express] Fallback a HTTP por error en certificados');
        http.createServer(app).listen(port, '0.0.0.0', () => {
            console.log(`[Express] Servidor HTTP iniciado en puerto: ${port} (fallback)`);
            console.log(`[Express] URL: http://localhost:${port}`);
        });
    }
}


const jwt = require('jsonwebtoken')
const config = require('../config')
const secret = config.jwt.secret
const error = require('../middleware/errors')

// Función para verificar token generado por .NET
function verificarToken(token){
    try {
        const decoded = jwt.verify(token, secret, {
            issuer: config.jwt.issuer || 'SofmarAPI',
            audience: config.jwt.audience || 'WebStock'
        });
        return decoded;
    } catch (err) {
        console.log('Error verificando token:', err.message);
        throw error('Token inválido o expirado', 401);
    }
}

const chequearToquen = {
    confirmarToken: function(req){
        return decodificarCabecera(req);
    }
}

function decodificarCabecera(req){
    try {
        const autorizacion = req.headers.authorization || '';
        const token = obtenerToken(autorizacion);
        const decodificado = verificarToken(token);
        req.user = decodificado; // Ahora puedes acceder a req.user en tus rutas
        return decodificado;
    } catch (err) {
        console.log('Error decodificando cabecera:', err.message);
        throw err;
    }
}

function obtenerToken(autorizacion){
    if(!autorizacion){
        throw error('No se mando correctamente el token', 401);
    }

    if(autorizacion.indexOf('Bearer') === -1){
        throw error('Formato Inválido', 401);
    }

    let token = autorizacion.replace('Bearer ','');
    return token;
}

// Función para generar token (mantener compatibilidad si es necesario)
function asignarToken(data){
    const payload = {
        op_codigo: data.op_codigo,
        op_nombre: data.op_nombre,
        op_sucursal: data.op_sucursal,
        or_rol: data.or_rol,
        // Agregar otros campos que necesites
    };
    
    return jwt.sign(payload, secret, {
        issuer: config.jwt.issuer || 'SofmarAPI',
        audience: config.jwt.audience || 'WebStock',
        expiresIn: '8h'
    });
}

module.exports = {
    asignarToken, 
    chequearToquen,
    verificarToken
}
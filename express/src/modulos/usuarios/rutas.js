const express = require('express');
const seguridad = require('../../middleware/seguridad')
const router = express.Router();
const respuesta = require('../../red/respuestas.js') 
const controlador = require('./index.js')
const auth = require('../../auth/index.js')

router.post('/login', login)
router.get('/',seguridad(), todos)
router.get("/vendedores", seguridad(), vendedores);
router.get('/:id',seguridad(), uno)
router.post('/',seguridad(), agregar)
router.put('/:id',seguridad(), eliminar)
router.get('/verificar/:user', verificarUsuario)


async function vendedores(req, res, next){
    try {
        const vendedores = await controlador.vendedores(req.query.buscar , req.query.id_vendedor);
        respuesta.success(req, res, vendedores, 200);

    } catch (err) {
        next(err);
    }
}


async function verificarUsuario(req, res, next){
    try {
        const items = await controlador.verificarUsuario(req.params.user);
        respuesta.success(req, res, items,200);  
    } catch (err) {
        next(err);
    }
}


async function login(req, res, next){
    try {
        console.log(`[RUTA] Iniciando login para usuario: ${req.body.user}`);
        const data = await controlador.login(req.body.user, req.body.pass);
        console.log(`[RUTA] Controlador devolvió datos para usuario: ${req.body.user}`, { 
            tieneData: !!data, 
            op_codigo: data?.op_codigo, 
            op_usuario: data?.op_usuario 
        });
        
        // Validar que data no sea undefined y tenga las propiedades necesarias
        if (!data || !data.op_codigo) {
            console.error(`[RUTA] Datos inválidos para usuario: ${req.body.user}`, data);
            return respuesta.error(req, res, 'Datos de usuario inválidos', 500);
        }
        
        console.log(`[RUTA] Generando token para usuario: ${req.body.user}`);
        console.log(`[RUTA] Datos que se pasan a asignarToken:`, data);
        const token = auth.asignarToken(data);
        const datos = {usuario: data, token: token };
        console.log(`[RUTA] Login exitoso para usuario: ${req.body.user}`);
        respuesta.success(req, res, datos, 200); 
    } catch (err) {
        console.error(`[RUTA] Error en login para usuario: ${req.body.user}`, err);
        /*respuesta.error(req, res, err, 500)*/
        next(err);
    }
}

async function todos (req, res, next){
    try {
        const busqueda = req.query.buscar;
        const items = await controlador.todos(busqueda);
        respuesta.success(req, res, items,200); 
    } catch (err) {
        next(err);
    }

}

async function uno (req, res, next){
    try {
        const items = await controlador.uno(req.params.id);
        respuesta.success(req, res, items,200); 
    } catch (err) {
        /*respuesta.error(req, res, err, 500)*/
        next(err);
    }
}

async function agregar (req, res, next){
    try {
        await controlador.agregar(req.body);
        let message = '';
        if(req.body.id == 0)
        {
            message = 'Guardado con éxito';
        }else{
            message = 'Item no guardado';
        }
        respuesta.success(req, res, message, 201);
    } catch (error) {
        next(error);
    }
}

async function eliminar (req, res, next){
    try {
        await controlador.eliminar(req.params.id);
        respuesta.success(req, res, 'Item eliminado satisfactoriamente!',200); 
    } catch (err) {
        next(err);
    }
}

module.exports = router;
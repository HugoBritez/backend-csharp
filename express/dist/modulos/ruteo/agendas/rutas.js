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
const express = require("express");
const seguridad = require("../../../middleware/seguridad");
const router = express.Router();
const respuesta = require("../../../red/respuestas.js");
const controlador = require("./index.js");
const auth = require("../../../auth/index.js");
router.post("/", todos);
router.get("/notas/:id", seguridad(), notas);
router.get("/localizaciones/:id", seguridad(), localizaciones);
router.get("/graficos", seguridad(), graficos);
router.post("/agregar", seguridad(), agregar);
router.post("/nueva-nota", seguridad(), agregarNota);
router.post("/registrar-llegada", registrarLlegada);
router.post("/registrar-salida", seguridad(), registrarSalida);
router.post("/finalizar-visita", seguridad(), finalizarVisita);
router.post("/reagendar-visita", seguridad(), reagendarVisita);
router.post("/anular-visita", seguridad(), anularVisita);
router.get("/:id", seguridad(), uno);
router.put("/:id", seguridad(), eliminar);
router.post("/contarvisitas", seguridad(), contarVisitas);
router.post("/tiempopromedio", seguridad(), tiempoPromedio);
router.post("/top-vendedores", seguridad(), topVendedores);
router.post("/top-clientes", seguridad(), topClientes);
router.post("/grafico-general", seguridad(), graficoGeneral);
router.post('/grafico-por-vendedor', seguridad(), graficoPorVendedor);
router.post('/consultar-ventas-y-detalles', seguridad(), consultarVentasYDetalles);
router.post('/consultar-pedidos-y-detalles', seguridad(), consultarPedidosYDetalles);
function consultarPedidosYDetalles(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.consultarPedidosYDetalles(req.body.cliente);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function consultarVentasYDetalles(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.consultarVentasYDetalles(req.body.cliente);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function graficoPorVendedor(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.obtenerDatosGraficoPorVendedor(req.body.desde, req.body.hasta, req.body.vendedor);
            respuesta.success(req, res, items, 200);
            console.log(items);
        }
        catch (err) {
            next(err);
        }
    });
}
function graficoGeneral(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log(req.body);
            const items = yield controlador.obtenerDatosGrafico(req.body.desde, req.body.hasta);
            respuesta.success(req, res, items, 200);
            console.log(items);
        }
        catch (err) {
            next(err);
        }
    });
}
function topClientes(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.topClientes(req.body.desde, req.body.hasta);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function topVendedores(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.topVendedores(req.body.desde, req.body.hasta);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function contarVisitas(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.contarAgendamientos(req.body.desde, req.body.hasta, req.body.vendedor);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function tiempoPromedio(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.tiempoPromedioVisitas(req.body.desde, req.body.hasta, req.body.vendedor);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function uno(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const item = yield controlador.uno(req.params.id);
            respuesta.success(req, res, item, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function notas(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.notas(req.params.id);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function localizaciones(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.localizaciones(req.params.id);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function graficos(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const hoy = new Date();
            const mes_actual_date = new Date(hoy.getFullYear(), hoy.getMonth(), 1);
            const mes_anterior_date = new Date(hoy.getFullYear(), hoy.getMonth() - 1, 1);
            const mes_actual_inicio = `${mes_actual_date.getFullYear()}-${("0" +
                (mes_actual_date.getMonth() + 1))
                .toString()
                .slice(-2)}-${("0" + mes_actual_date.getDate()).toString().slice(-2)}`;
            const mes_anterior_inicio = `${mes_anterior_date.getFullYear()}-${("0" +
                (mes_anterior_date.getMonth() + 1))
                .toString()
                .slice(-2)}-${("0" + mes_anterior_date.getDate()).toString().slice(-2)}`;
            const agendamientos = yield controlador.agendamientos(req.query.desde, req.query.hasta, req.query.user, mes_actual_inicio, mes_anterior_inicio);
            const clientes = yield controlador.clientes(mes_actual_inicio);
            const ruteos = yield controlador.ruteos(req.query.desde, req.query.hasta, req.query.user, mes_actual_inicio, mes_anterior_inicio);
            const ruteo_por_vendedor = yield controlador.ruteoPorVendedor(req.query.desde, req.query.hasta);
            const planificaciones_por_mes = yield controlador.planificacionesPorMes(`${hoy.getFullYear()}-01-01`, `${hoy.getFullYear()}-12-31`);
            const planificaciones_por_vend = yield controlador.planificacionesPorVend(`${hoy.getFullYear()}-01-01`, `${hoy.getFullYear()}-12-31`);
            const items = [
                Object.assign({}, agendamientos[0]),
                Object.assign({}, clientes[0]),
                Object.assign({}, ruteos[0]),
                [...ruteo_por_vendedor],
                Object.assign({}, planificaciones_por_mes),
                Object.assign({}, planificaciones_por_vend),
            ];
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function todos(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log(req.body);
            const bod = req.body;
            const validarVendedor = (vendedor) => {
                if (Array.isArray(vendedor)) {
                    return vendedor.length > 0 ? vendedor : null;
                }
                return vendedor > 0 ? vendedor : null;
            };
            const items = yield controlador.todos(bod.fecha_desde, bod.fecha_hasta, bod.cliente && bod.cliente.length > 0 ? bod.cliente : null, validarVendedor(bod.vendedor), bod.visitado, bod.estado, bod.planificacion, bod.notas, bod.orden);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function agregar(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            yield controlador.agregar(req.body);
            let message = "";
            if (req.body.a_codigo == 0) {
                message = "Guardado con éxito";
            }
            else {
                message = "Item no guardado";
            }
            respuesta.success(req, res, message, 201);
        }
        catch (error) {
            next(error);
        }
    });
}
function agregarNota(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            yield controlador.agregarNota(req.body);
            let message = "";
            if (req.body.an_codigo == 0) {
                message = "Guardado con éxito";
            }
            else {
                message = "Item no guardado";
            }
            respuesta.success(req, res, message, 201);
        }
        catch (error) {
            next(error);
        }
    });
}
function registrarLlegada(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log(req.body);
            yield controlador.agregarLocalizacion(req.body);
            let message = "";
            if (req.body.l_codigo == 0) {
                message = "Guardado con éxito";
            }
            else {
                message = "Item no guardado";
            }
            respuesta.success(req, res, message, 201);
        }
        catch (error) {
            next(error);
        }
    });
}
function registrarSalida(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            yield controlador.registrarSalida(req.body.l_agenda, req.body.l_hora_fin);
            respuesta.success(req, res, "Salida registrada con éxito", 201);
        }
        catch (error) {
            next(error);
        }
    });
}
function reagendarVisita(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            yield controlador.reagendarVisita(req.body.a_codigo, req.body.a_prox_llamada, req.body.a_hora_prox);
            respuesta.success(req, res, "Reagendado con éxito", 201);
        }
        catch (error) {
            next(error);
        }
    });
}
function anularVisita(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            yield controlador.anularVisita(req.body.a_codigo);
            respuesta.success(req, res, "Anulado con éxito", 201);
        }
        catch (error) {
            next(error);
        }
    });
}
function finalizarVisita(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            yield controlador.finalizarVisita(req.body.a_codigo, req.body.a_latitud, req.body.a_longitud);
            let message = "";
            if (req.body.l_codigo == 0) {
                message = "Guardado con éxito";
            }
            else {
                message = "Item no guardado";
            }
            respuesta.success(req, res, message, 201);
        }
        catch (error) {
            next(error);
        }
    });
}
function eliminar(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            yield controlador.eliminar(req.params.id);
            respuesta.success(req, res, "Item eliminado satisfactoriamente!", 200);
        }
        catch (err) {
            next(err);
        }
    });
}
router.post('/subvisitas', seguridad(), crearSubvisita);
router.get('/subvisitas/todos', seguridad(), getSubvisitas);
router.post('/subvisitas/actualizar', seguridad(), actualizarSubvisita);
function actualizarSubvisita(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log(req.body);
            yield controlador.actualizarSubvisita(req.body);
            respuesta.success(req, res, "Subvisita actualizada con éxito", 201);
        }
        catch (error) {
            next(error);
        }
    });
}
function crearSubvisita(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            yield controlador.crearSubvisita(req.body);
            respuesta.success(req, res, "Subvisita creada con éxito", 201);
        }
        catch (error) {
            next(error);
        }
    });
}
function getSubvisitas(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.getSubvisitas(req.query.id_agenda);
            respuesta.success(req, res, items, 200);
        }
        catch (error) {
            next(error);
        }
    });
}
module.exports = router;

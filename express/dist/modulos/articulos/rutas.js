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
const seguridad = require("../../middleware/seguridad");
const router = express.Router();
const respuesta = require("../../red/respuestas.js");
const controlador = require("./index.js");
const auth = require("../../auth/index.js");
router.get("/", seguridad(), todos);
router.get("/lista-barra", seguridad(), listar_por_barra);
router.get("/directa", seguridad(), todosDirecta);
router.get("/uno", seguridad(), uno);
router.get("/barra", seguridad(), barra);
router.get("/pedido-remision", seguridad(), enPedidoRemision);
router.post("/informe", seguridad(), informe_stock);
router.post("/resumen-comprasventas", seguridad(), resumen_comprasventas);
router.post("/ver_lotes_talle", seguridad(), ver_lotes_talle);
router.post("/agregar-inventario", agregarInventario);
router.post("/agregar-item-inventario", agregarItemInventario);
router.post("/agregar-item-inventario-con-vencimiento", agregarItemInventarioConVencimiento);
router.post("/agregar-item", agregarItem);
router.get("/ultimo-nro-inventario", seguridad(), ultimoNroInventario);
router.post("/insertar-reconteo", seguridad(), insertarReconteo);
router.get("/reporte-reconteo", seguridad(), reporte_reconteo);
router.get("/todos", seguridad(), todosNuevo);
router.get("/categorias-articulos", seguridad(), categoriasArticulos);
router.get("/marcas-articulos", seguridad(), marcasArticulos);
router.get("/secciones-articulos", seguridad(), seccionesArticulos);
router.get("/toma-inventario-items", seguridad(), tomaInventario);
router.get("/toma-inventario-scanner", seguridad(), tomaInventarioScanner);
router.post("/insertar-item-conteo-scanner", seguridad(), insertarItemConteoScanner);
router.post("/insertar-inventario-auxiliar", seguridad(), insertarInventarioAuxiliar);
router.post("/insertar-inventario-auxiliar-items", seguridad(), insertarInventarioAuxiliarItems);
router.post("/insertar-conteo-scanner", seguridad(), insertarConteoScanner);
router.get("/ultimo-inventario-auxiliar", seguridad(), ultimoInventarioAuxiliar);
router.post("/cerrar-inventario-auxiliar", seguridad(), cerrarInventarioAuxiliar);
router.get("/mostrar-items-inventario-auxiliar", seguridad(), mostrarItemsInventarioAuxiliar);
router.get("/mostrar-items-inventario-auxiliar-principal", seguridad(), mostrarItemsInventarioAuxiliarPrincipal);
router.post("/scannear-item-inventario-auxiliar", seguridad(), scannearItemInventarioAuxiliar);
router.get("/reporte-anomalias", seguridad(), reporteAnomalias);
router.get("/consulta-articulos", seguridad(), consultaArticulosSimplificado);
router.get("/inventarios-disponibles", seguridad(), inventariosDisponibles);
router.get("/anular-inventario-auxiliar", seguridad(), anularInventarioAuxiliar);
router.get("/reporte-inventario", seguridad(), reporte_inventario);
router.get("/buscar-articulos", seguridad(), buscarArticulos);
router.get('/pedido', seguridad(), pedidosArticulo);
function pedidosArticulo(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.getArticulosEnPedidos(req.query.articulo_id, req.query.id_lote);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function anularInventarioAuxiliar(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.anularInventarioAuxiliar(req.query.id);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function inventariosDisponibles(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log(req.query);
            const items = yield controlador.inventariosDisponibles(req.query.estado, req.query.deposito, req.query.sucursal);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function consultaArticulosSimplificado(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log(req.query);
            const items = yield controlador.consultaArticulosSimplificado(req.query.articulo_id, req.query.busqueda, req.query.codigo_barra, req.query.moneda, req.query.stock, req.query.deposito, req.query.marca, req.query.categoria, req.query.ubicacion, req.query.proveedor, req.query.cod_interno);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function buscarArticulos(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.buscarArticulos(req.query.articulo_id, req.query.busqueda, req.query.codigo_barra, req.query.moneda, req.query.stock, req.query.deposito, req.query.marca, req.query.categoria, req.query.ubicacion, req.query.proveedor, req.query.cod_interno, req.query.lote, req.query.negativo);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function reporteAnomalias(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.reporteDeAnomalias(req.query.nro_inventario, req.query.sucursal, req.query.deposito);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function scannearItemInventarioAuxiliar(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log(req.body);
            const items = yield controlador.scannearItemInventarioAuxiliar(req.body.id_articulo, req.body.id_lote, req.body.cantidad, req.body.lote, req.body.codigo_barras, req.body.id_inventario);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function mostrarItemsInventarioAuxiliar(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.mostrarItemsDelInventarioAuxiliar(req.query.id, req.query.id_inventario, req.query.buscar, req.query.deposito);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function mostrarItemsInventarioAuxiliarPrincipal(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log(req.query);
            const items = yield controlador.mostrarItemsDelInventarioAuxiliarPrincipal(req.query.id, req.query.scanneado, req.query.deposito, req.query.sucursal, req.query.buscar, req.query.id_inventario);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function cerrarInventarioAuxiliar(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log(req.body);
            const items = yield controlador.cerrarInventarioAuxiliar(req.body.id, req.body.operador, req.body.sucursal, req.body.deposito, req.body.nro_inventario, req.body.autorizado);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function ultimoInventarioAuxiliar(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log(req.query);
            const items = yield controlador.ultimoInventarioAuxiliar(req.query.deposito, req.query.sucursal, req.query.nro_inventario);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function insertarItemConteoScanner(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.insertarItemConteoScanner(req.body);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function insertarInventarioAuxiliar(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log(req.body);
            const inventario = req.body;
            const inventario_items = req.body.inventario_items;
            const items = yield controlador.insertarInventarioAuxiliar(inventario, inventario_items);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function insertarInventarioAuxiliarItems(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log(req.body);
            const items = yield controlador.insertarInventarioAuxiliarItems(req.body.inventario_items, req.body.inventario_id);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function insertarConteoScanner(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const { cantidad, id_articulo, id_lote, id_inventario } = req.body;
            const items = yield controlador.insertarConteoScanner(cantidad, id_articulo, id_lote, id_inventario);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function tomaInventarioScanner(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const { deposito_id, articulo_id, ubicacion, sub_ubicacion, categorias } = req.query;
            const items = yield controlador.itemTomaInventarioScanner(deposito_id, articulo_id, ubicacion, sub_ubicacion, categorias);
            respuesta.success(req, res, items, 200);
        }
        catch (error) {
            next(error);
        }
    });
}
function tomaInventario(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log(req.query);
            const { deposito_id, articulo_id, ubicacion, sub_ubicacion, categorias, marcas, } = req.query;
            const items = yield controlador.itemTomaInventario(deposito_id, articulo_id, ubicacion, sub_ubicacion, categorias, marcas);
            respuesta.success(req, res, items, 200);
        }
        catch (error) {
            next(error);
        }
    });
}
function categoriasArticulos(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.categoriasArticulos();
            respuesta.success(req, res, items, 200);
        }
        catch (error) {
            next(error);
        }
    });
}
function marcasArticulos(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.marcasArticulos();
            respuesta.success(req, res, items, 200);
        }
        catch (error) {
            next(error);
        }
    });
}
function seccionesArticulos(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.seccionesArticulos();
            respuesta.success(req, res, items, 200);
        }
        catch (error) {
            next(error);
        }
    });
}
function todosNuevo(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log("###Esta es la query ###", req.query);
            const busqueda = req.query.busqueda;
            const deposito = req.query.deposito;
            const stock = req.query.stock;
            const marca = req.query.marca;
            const categoria = req.query.categoria;
            const subcategoria = req.query.subcategoria;
            const proveedor = req.query.proveedor;
            const ubicacion = req.query.ubicacion;
            const servicio = req.query.servicio;
            const moneda = req.query.moneda;
            const unidadMedida = req.query.unidadMedida;
            const pagina = parseInt(req.query.pagina) || 1;
            const limite = parseInt(req.query.limite) || 50;
            const tipoValorizacionCosto = req.query.tipoValorizacionCosto;
            const items = yield controlador.todosNuevo(busqueda, deposito, stock, marca, categoria, subcategoria, proveedor, ubicacion, servicio, moneda, unidadMedida, pagina, limite, tipoValorizacionCosto);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function reporte_reconteo(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log(req.query);
            const { marca, deposito, categoria, proveedor } = req.query;
            const items = yield controlador.reporte_reconteo({
                marca,
                deposito,
                categoria,
                proveedor,
            });
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
router.get("/traer-todos-los-articulos", (req, res) => __awaiter(void 0, void 0, void 0, function* () {
    try {
        const pagina = parseInt(req.query.pagina) || 1;
        const limite = parseInt(req.query.limite) || 50;
        const articulos = yield controlador.traerTodosLosArticulos(pagina, limite, req.query);
        res.json(articulos);
    }
    catch (error) {
        res.status(500).json({ error: error.message });
    }
}));
function ultimoNroInventario(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.nroUltimoInventario();
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function agregarItem(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const datos = req.body;
            const items = yield controlador.insertarArticulo(datos);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function insertarReconteo(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log(req.body);
            const item = req.body;
            const items = yield controlador.insertar_reconteo(item);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function agregarInventario(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log(req.body);
            const consulta = req.body;
            const items = yield controlador.insertarInventario(consulta);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function agregarItemInventario(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const consulta = req.body;
            const items = yield controlador.insertarItemInventario(consulta);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function agregarItemInventarioConVencimiento(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const consulta = req.body;
            const items = yield controlador.insertarItemInventarioConVencimiento(consulta);
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
            console.log(req.query);
            const items = yield controlador.todos(req.query.buscar, req.query.id_deposito, req.query.stock);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function resumen_comprasventas(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const consulta = req.body;
            const fecha_desde = consulta.fecha_desde;
            const fecha_hasta = consulta.fecha_hasta;
            const depositos = consulta.codigos.depositos.toString();
            const articulos = consulta.codigos.articulos.toString();
            const marcas = consulta.codigos.marcas.toString();
            const categorias = consulta.codigos.categorias.toString();
            const subcategorias = consulta.codigos.subcategorias.toString();
            const proveedores = consulta.codigos.proveedores.toString();
            const moneda = consulta.codigos.moneda;
            const tipo_valorizacion = consulta.codigos.tipo_valorizacion;
            const talles = consulta.codigos.talles_ropa.toString() +
                consulta.codigos.talles_calzado.toString();
            const colores = consulta.codigos.colores.toString();
            const items = yield controlador.resumen_comprasventas(fecha_desde, fecha_hasta, depositos, articulos, marcas, categorias, subcategorias, proveedores, moneda, tipo_valorizacion, talles, colores);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function informe_stock(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const consulta = req.body;
            const depositos = consulta.depositos.toString();
            const articulos = consulta.articulos.toString();
            const ubicaciones = consulta.ubicaciones.toString();
            const sububicaciones = consulta.sububicaciones.toString();
            const categorias = consulta.categorias.toString();
            const subcategorias = consulta.subcategorias.toString();
            const marcas = consulta.marcas.toString();
            const presentaciones = consulta.presentaciones.toString();
            const proveedores = consulta.proveedores.toString();
            const lineas = consulta.lineas.toString();
            const bloques = consulta.bloques.toString();
            const moneda = consulta.moneda;
            const est_stock = consulta.est_stock;
            const tipo_valorizacion = consulta.tipo_valorizacion;
            const talles = consulta.talles_ropa.toString() + consulta.talles_calzado.toString();
            const colores = consulta.colores.toString();
            const items = yield controlador.informe_stock(depositos, articulos, ubicaciones, sububicaciones, categorias, subcategorias, marcas, presentaciones, proveedores, lineas, bloques, moneda, est_stock, tipo_valorizacion, talles, colores);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function listar_por_barra(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.listar_por_barra();
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function todosDirecta(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            console.log(req.query);
            const items = yield controlador.todosDirecta(req.query.busqueda, req.query.deposito, req.query.stock);
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
            const items = yield controlador.uno(req.query.articulo, req.query.deposito, req.query.lote);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function barra(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.barra(req.query.articulo, req.query.deposito, req.query.lote);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function enPedidoRemision(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.enPedidoRemision(req.query.articulo, req.query.lote);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function ver_lotes_talle(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const articulos_codigos = req.body;
            const items = yield controlador.ver_lotes_talle(articulos_codigos);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
function reporte_inventario(req, res, next) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const items = yield controlador.reporte_inventario(req.query);
            respuesta.success(req, res, items, 200);
        }
        catch (err) {
            next(err);
        }
    });
}
module.exports = router;

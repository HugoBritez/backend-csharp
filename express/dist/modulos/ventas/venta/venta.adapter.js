"use strict";
// src/modulos/ventas/venta/adapters/venta.adapter.ts
Object.defineProperty(exports, "__esModule", { value: true });
exports.VentaAdapter = void 0;
class VentaAdapter {
    static adaptarVentaDTO(ventaDTO) {
        // Formatear la fecha al formato YYYY-MM-DD
        const fecha = new Date(ventaDTO.ve_fecha);
        const fechaFormateada = fecha.toISOString().split('T')[0];
        return {
            ventaId: ventaDTO.ve_codigo || 0,
            cliente: ventaDTO.ve_cliente,
            operador: ventaDTO.ve_vendedor,
            deposito: ventaDTO.ve_deposito,
            moneda: ventaDTO.ve_moneda,
            fecha: fechaFormateada,
            factura: ventaDTO.ve_factura || '',
            credito: ventaDTO.ve_credito,
            saldo: ventaDTO.ve_saldo,
            total: ventaDTO.ve_total,
            devolucion: ventaDTO.ve_devolucion || 0,
            procesado: ventaDTO.ve_procesado || 0,
            descuento: ventaDTO.ve_descuento,
            cuotas: ventaDTO.ve_cuotas || 0,
            cantCuotas: ventaDTO.ve_cantCuotas || 0,
            obs: ventaDTO.ve_obs || '',
            vendedor: ventaDTO.ve_vendedor,
            sucursal: ventaDTO.ve_sucursal,
            metodo: ventaDTO.ve_metodo || 1,
            aplicar_a: ventaDTO.ve_aplicar_a || 0,
            retencion: ventaDTO.ve_retencion || 0,
            timbrado: ventaDTO.ve_timbrado || '',
            codeudor: ventaDTO.ve_codeudor || 0,
            pedido: ventaDTO.ve_pedido || 0,
            hora: ventaDTO.ve_hora,
            userpc: ventaDTO.ve_userpc,
            situacion: ventaDTO.ve_situacion || 1,
            chofer: ventaDTO.ve_chofer || 0,
            ve_cdc: ventaDTO.ve_cdc || '',
            ve_qr: ventaDTO.ve_qr || '',
            km_actual: ventaDTO.ve_km_actual || 0,
            vehiculo: ventaDTO.ve_vehiculo || 0,
            desc_trabajo: ventaDTO.ve_desc_trabajo || '',
            kilometraje: ventaDTO.ve_kilometraje || 0,
            mecanico: ventaDTO.ve_mecanico || 0,
            servicio: ventaDTO.ve_servicio || 0,
            siniestro: ventaDTO.ve_siniestro || 0
        };
    }
    static adaptarDetalleVentaDTO(detalleDTO) {
        return {
            deve_venta: detalleDTO.deve_venta,
            deve_articulo: detalleDTO.deve_articulo,
            deve_cantidad: detalleDTO.deve_cantidad,
            deve_precio: detalleDTO.deve_precio,
            deve_descuento: detalleDTO.deve_descuento || 0,
            deve_exentas: detalleDTO.deve_exentas,
            deve_cinco: detalleDTO.deve_cinco,
            deve_diez: detalleDTO.deve_diez,
            deve_devolucion: detalleDTO.deve_devolucion || 0,
            deve_vendedor: detalleDTO.deve_vendedor,
            deve_color: detalleDTO.deve_color || '',
            deve_bonificacion: detalleDTO.deve_bonificacion || 0,
            deve_talle: detalleDTO.deve_talle || '',
            deve_codioot: detalleDTO.deve_codioot || 0,
            deve_costo: detalleDTO.deve_costo || 0,
            deve_costo_art: detalleDTO.deve_costo_art || 0,
            deve_cinco_x: detalleDTO.deve_cinco_x || 0,
            deve_diez_x: detalleDTO.deve_diez_x || 0,
            lote: detalleDTO.lote || '',
            loteid: detalleDTO.lote_id || 0,
            articulo_editado: detalleDTO.articulo_editado || false,
            deve_descripcion_editada: detalleDTO.deve_descripcion_editada || ''
        };
    }
}
exports.VentaAdapter = VentaAdapter;

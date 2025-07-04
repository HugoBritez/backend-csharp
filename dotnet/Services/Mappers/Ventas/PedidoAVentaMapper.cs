using Api.Models.Dtos;
using Api.Models.Entities;

namespace Api.Services.Mappers
{
    public static class PedidoAVentasMapper
    {
        public static Venta? toVenta(this Pedido pedido)
        {
            if (pedido is null)
            {
                return null;
            }

            return new Venta
            {
                Codigo = 0,
                Cliente = pedido.Cliente,
                Operador = pedido.Operador,
                Deposito = pedido.Deposito,
                Moneda = pedido.Moneda,
                Fecha = pedido.Fecha,
                Factura = "",
                Credito = pedido.Credito,
                Saldo = 0,
                Total = 0,
                Vencimiento = DateTime.MinValue,
                Estado = (uint)pedido.Estado,
                Devolucion = 0,
                Procesado = 0,
                Descuento = pedido.Descuento,
                Cuotas = (uint)(pedido.CantCuotas > 0 ? 1 : 0),
                CantCuotas = pedido.CantCuotas,
                Obs = pedido.Observacion,
                Vendedor = pedido.Vendedor,
                Sucursal = pedido.Sucursal,
                Metodo = 1,
                AplicarA = 0,
                Retencion = 0,
                Timbrado = "",
                Codeudor = pedido.Vendedor,
                Hora = "",
                UserPc = "",
                Situacion = 0,
                Chofer = 0,
                Cdc = "",
                Qr = "",
                KmActual = 0,
                Vehiculo = 0,
                DescTrabajo = "",
                Kilometraje = 0,
                Servicio = 0,
                Siniestro = ""
            };
        }

        public static DetalleVentaDTO? toDetalleVenta(this DetallePedido detallePedido, Articulo articulo)
        {
            if (detallePedido is null)
            {
                return null;
            }

            return new DetalleVentaDTO
            {
                DeveVenta = 0,
                DeveArticulo = detallePedido.Articulo,
                DeveCantidad = detallePedido.Cantidad,
                DevePrecio = detallePedido.Precio,
                DeveDescuento = detallePedido.Descuento,
                DeveExentas = detallePedido.Exentas,
                DeveCinco = detallePedido.Cinco,
                DeveDiez = detallePedido.Diez,
                DeveDevolucion = 0,
                DeveVendedor = detallePedido.Vendedor,
                DeveColor = "",
                DeveBonificacion = detallePedido.Bonificacion,
                DeveTalle = "",
                DeveCodioot = 0,
                DeveCosto = articulo.ArPrecioCompraGuaranies,
                DeveCostoArt = articulo.ArPrecioCompraGuaranies,
                DeveCincoX = Math.Round(detallePedido.Cinco / 21, 2),
                DeveDiezX = Math.Round(detallePedido.Diez / 11, 2),
                Lote = detallePedido.Lote,
                LoteId = (int)detallePedido.CodigoLote,
                ArticuloEditado = false,
                DeveDescripcionEditada = "",
            };
        }
    }
}
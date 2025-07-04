using Api.Models.Entities;
using Api.Models.Dtos;

namespace Api.Services.Mappers
{
    public static class DetalleVentasMapper
    {

        public static DetalleVenta? toDetalleVenta(this DetalleVentaDTO detalleVenta)
        {
            if (detalleVenta == null)
            {
                return null;
            }

            return new DetalleVenta
            {
                Venta = detalleVenta.DeveVenta,
                Articulo = detalleVenta.DeveArticulo,
                Cantidad = detalleVenta.DeveCantidad,
                Precio = detalleVenta.DevePrecio,
                Descuento = detalleVenta.DeveDescuento,
                Exentas = detalleVenta.DeveExentas,
                Cinco = detalleVenta.DeveCinco,
                Diez = detalleVenta.DeveDiez,
                Devolucion = detalleVenta.DeveDevolucion,
                Vendedor = detalleVenta.DeveVendedor,
                Bonificacion = detalleVenta.DeveBonificacion,
                Talle = detalleVenta.DeveTalle ?? string.Empty,
                CodigoOT = detalleVenta.DeveCodioot,
                Costo = detalleVenta.DeveCosto,
                CostoArt = detalleVenta.DeveCostoArt,
                CincoX = detalleVenta.DeveCincoX,
                DiezX = detalleVenta.DeveDiezX
            };
        }
        public static DetalleVentaBonificacion? ToBonificacion(this DetalleVentaDTO detalleVenta, int insertId)
        {
            if (detalleVenta == null)
            {
                return null;
            }

            if (detalleVenta.DeveBonificacion == 0)
            {
                return null;
            }

            return new DetalleVentaBonificacion
            {
                DetalleVenta = (uint)insertId,
                Cantidad = detalleVenta.DeveCantidad,
                Precio = detalleVenta.DevePrecio,
                Exentas = detalleVenta.DeveExentas,
                Cinco = detalleVenta.DeveCinco,
                Diez = detalleVenta.DeveDiez
            };
        }

        public static DetalleArticulosEditado? ToDetalleArticulosEditado(this DetalleVentaDTO detalleVenta, int insertId)
        {
            if (detalleVenta == null)
            {
                return null;
            }

            if (detalleVenta.DeveBonificacion == 1)
            {
                return null;
            }

            return new DetalleArticulosEditado
            {
                Codigo = 0,
                DetalleVenta = (uint)insertId,
                Descripcion = detalleVenta.DeveDescripcionEditada ?? string.Empty,
            };
        }

        public static DetalleVentaVencimiento? ToDetalleVencimiento(this DetalleVentaDTO detalleVenta, int insertId)
        {

            Console.WriteLine("LoteId del detalleDTO: " + detalleVenta.LoteId);
            Console.WriteLine("Id del detalleVenta: " + insertId);

            if (detalleVenta == null)
            {
                return null;
            }

            if (detalleVenta.DeveBonificacion == 1)
            {
                return null;
            }

            return new DetalleVentaVencimiento
            {
                DetalleVenta = (uint)insertId,
                Lote = detalleVenta.Lote ?? string.Empty,
                LoteId = detalleVenta.LoteId
            };
        }
    }
}
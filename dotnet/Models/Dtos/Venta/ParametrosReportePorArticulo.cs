namespace Api.Models.Dtos
{
    public class ParametrosReporte
    {
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int AnioInicio { get; set; }
        public int CantidadAnios { get; set;}
        public int? VendedorId { get; set; }
        public int? CategoriaId { get; set; }
        public int? ClienteId { get; set; }
        public int? MarcaId { get; set; }
        public int? ArticuloId { get; set; }
        public int? CiudadId { get; set; }
        public int? SucursalId { get; set; }
        public int? DepositoId { get; set; }
        public int? MonedaId { get; set; }
        public int? ProveedorId { get; set; }
        public bool VerUtilidadYCosto { get; set; }
        public bool MovimientoPorFecha { get; set; }
    }

    public class ReporteVentaAnual
    {
        public List<DetalleVentaAnual> Detalles { get; set; } = new();
        public TotalesVentaAnual Totales { get; set; } = new();
    }

    public class DetalleVentaAnual
    {
        public int CodigoArticulo { get; set; }
        public string? Descripcion { get; set; }
        public decimal Stock { get; set; }
        public decimal Costo { get; set; }
        public decimal PrecioVenta1 { get; set; }
        public decimal PrecioVenta2 { get; set; }
        public decimal PrecioVenta3 { get; set; }
        public decimal CantidadAnio1 { get; set; }
        public decimal ImporteAnio1 { get; set; }
        public decimal CantidadAnio2 { get; set; }
        public decimal ImporteAnio2 { get; set; }
        public decimal CantidadAnio3 { get; set; }
        public decimal ImporteAnio3 { get; set; }
        public decimal CantidadAnio4 { get; set; }
        public decimal ImporteAnio4 { get; set; }
        public decimal CantidadAnio5 { get; set; }
        public decimal ImporteAnio5 { get; set; }
        public decimal Cantidad { get; set; }  // Agregamos esta propiedad
        public decimal Importe { get; set; }
        public decimal DemandaPromedio { get; set;} // promedio de items de los anios delimitados
        public decimal MetaAcordada { get; set; } // meta de ventas acordada (esperar a implementar CRUD )
        public decimal PorcentajeUtilidad { get; set; } // porcentaje de beneficio basado en el precio de venta y el costo
        public decimal VentaTotal { get; set;} // total importe basado en el precio del producto y la meta acordada
        public decimal UnidadesVendidas { get;set;} // total de items vendidos
        public decimal ImporteTotal { get; set; } // sumatoria de los importes de los anios delimitados
    }

    public class TotalesVentaAnual
    {
        public decimal TotalCantidadAnio1 { get; set; }
        public decimal TotalImporteAnio1 { get; set; }
        public decimal TotalCantidadAnio2 { get; set; }
        public decimal TotalImporteAnio2 { get; set; }
        public decimal TotalCantidadAnio3 { get; set; }
        public decimal TotalImporteAnio3 { get; set; }
        public decimal TotalCantidadAnio4 { get; set; }
        public decimal TotalImporteAnio4 { get; set; }
        public decimal TotalCantidadAnio5 { get; set; }
        public decimal TotalImporteAnio5 { get; set; }
        public decimal TotalNotasCreditoAnio1 { get; set; }
        public decimal TotalNotasCreditoAnio2 { get; set; }
        public decimal TotalNotasCreditoAnio3 { get; set; }
        public decimal TotalNotasCreditoAnio4 { get; set; }
        public decimal TotalNotasCreditoAnio5 { get; set; }
    }
}
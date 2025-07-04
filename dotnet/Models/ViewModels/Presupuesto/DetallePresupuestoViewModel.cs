namespace Api.Models.ViewModels
{
    public class DetallePresupuestoViewModel
    {
        public uint det_codigo { get; set; }
        public uint art_codigo { get; set; }
        public string codbarra { get; set; } = string.Empty;
        public string descripcion { get; set; } = string.Empty;
        public int ar_editar_desc { get; set; }
        public decimal cantidad { get; set; }
        public decimal precio { get; set; }
        public decimal descuento { get; set; }
        public decimal exentas { get; set; }
        public decimal cinco { get; set; }
        public decimal diez { get; set; }
        public uint codlote { get; set; }
        public string lote { get; set; } = string.Empty;
        public decimal largura { get; set; }
        public decimal altura { get; set; }
        public decimal mts { get; set; }
        public string descripcion_editada { get; set; } = string.Empty;
        public uint listaprecio { get; set; }
        public string vence { get; set; } = string.Empty;
        public string depre_obs { get; set; } = string.Empty;
        public uint iva { get; set; }
        public decimal kilos { get; set; }
    }
}
namespace Api.Models.ViewModels
{
    public class DetalleInventarioViewModel
    {
        public uint articulo_id { get; set; }
        public string cod_interno { get; set; } = string.Empty;
        public uint lote_id { get; set; }
        public string descripcion { get; set; } = string.Empty;
        public string ubicacion { get; set; } = string.Empty;
        public uint control_vencimiento { get; set; }
        public string vencimiento { get; set; } = string.Empty;
        public string sub_ubicacion { get; set; } = string.Empty;
        public string lote { get; set; } = string.Empty;
        public string cod_barra_articulo { get; set; } = string.Empty;
        public string cod_barra_lote { get; set; } = string.Empty;
        public decimal cantidad_inicial { get; set; }
        public decimal cantidad_final { get; set; }
        public decimal cantidad_actual { get; set; }
    }
}
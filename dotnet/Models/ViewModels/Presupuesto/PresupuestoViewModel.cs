namespace Api.Models.ViewModels
{
    public class PresupuestoViewModel
    {
        public uint codigo { get; set; }
        public uint codcliente { get; set; }
        public string cliente { get; set; } = string.Empty;
        public string moneda { get; set; } = string.Empty;
        public string fecha { get; set; } = string.Empty;
        public uint codsucursal { get; set; }
        public string sucursal { get; set; } = string.Empty;
        public string vendedor { get; set; } = string.Empty;
        public string operador { get; set; } = string.Empty;
        public string total { get; set; } = string.Empty;
        public decimal descuento { get; set; }
        public decimal saldo { get; set; }
        public string condicion { get; set; } = string.Empty;
        public string vencimiento { get; set; } = string.Empty;
        public string factura { get; set; } = string.Empty;
        public string obs { get; set; } = string.Empty;
        public int estado { get; set; }
        public string estado_desc { get; set; } = string.Empty;
        public string pre_condicion { get; set; } = string.Empty;
        public string pre_plazo { get; set; } = string.Empty;
        public string pre_flete { get; set; } = string.Empty;
    }
}
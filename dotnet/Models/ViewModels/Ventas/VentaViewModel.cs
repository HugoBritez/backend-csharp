namespace Api.Models.ViewModels
{
    public class VentaViewModel
    {
        public uint codigo { get; set; }
        public uint codcliente { get; set; }
        public string cliente { get; set; } = string.Empty;
        public uint moneda_id { get; set; }
        public string moneda { get; set; } = string.Empty;
        public string fecha { get; set; } = string.Empty;
        public uint codsucursal { get; set; }
        public string sucursal { get; set; } = string.Empty;
        public string vendedor { get; set; } = string.Empty;
        public string operador { get; set; } = string.Empty;
        public string total { get; set; } = string.Empty;
        public string descuento { get; set; } = string.Empty;
        public string saldo { get; set; } = string.Empty;
        public string condicion { get; set; } = string.Empty;
        public string vencimiento { get; set; } = string.Empty;
        public string factura { get; set; } = string.Empty;
        public string obs { get; set; } = string.Empty;
        public uint estado { get; set; }
        public string estado_desc { get; set; } = string.Empty;
        public string obs_anulacion { get; set; } = string.Empty;
        public string terminal { get; set; } = string.Empty;
        public string exentas_total { get; set; } = string.Empty;
        public string descuento_total { get; set; } = string.Empty;
        public string iva5_total { get; set; } = string.Empty;
        public string iva10_total { get; set; } = string.Empty;
        public string sub_total { get; set; } = string.Empty;
        public string total_articulos { get; set; } = string.Empty;
        public string total_neto { get; set; } = string.Empty;
        public string ve_cdc { get; set; } = string.Empty;
        public uint tipo_documento { get; set; }
        public string cliente_descripcion { get; set; } = string.Empty;
        public string cliente_direccion { get; set; } = string.Empty;
        public string cliente_ciudad { get; set; } = string.Empty;
        public uint ciudad_id { get; set; }
        public string ciudad_descripcion { get; set; } = string.Empty;
        public uint distrito_id { get; set; }
        public string distrito_descripcion { get; set; } = string.Empty;
        public uint departamento_id { get; set; }
        public string departamento_descripcion { get; set; } = string.Empty;
        public string cliente_telefono { get; set; } = string.Empty;
        public string cliente_email { get; set; } = string.Empty;
        public uint cliente_codigo_interno { get; set; }
        public string operador_nombre { get; set; } = string.Empty;
        public string operador_documento { get; set; } = string.Empty;
        public string establecimiento { get; set; } = string.Empty;
        public string punto_emision { get; set; } = string.Empty;
        public string numero_factura { get; set; } = string.Empty;
        public string cliente_ruc { get; set; } = string.Empty;
        public int cant_cuotas { get; set; }
        public decimal entrega_inicial { get; set; }
        
    }

}
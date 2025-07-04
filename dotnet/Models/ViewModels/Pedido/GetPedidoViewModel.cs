using System.Text.Json.Serialization;

namespace Api.Models.ViewModels
{

    public class PedidoDetalladoViewModel : PedidoViewModel
    {
        [JsonPropertyName("detalles")]
        public List<PedidoDetalleViewModel> Detalles { get; set; } = new();
    }

    public class PedidoViewModel
    {
        [JsonPropertyName("pedido_id")]
        public int PedidoId { get; set; }

        [JsonPropertyName("cliente")]
        public string Cliente { get; set; } = string.Empty;

        [JsonPropertyName("moneda")]
        public string Moneda { get; set; } = string.Empty;

        [JsonPropertyName("fecha")]
        public DateTime Fecha { get; set; }

        [JsonPropertyName("factura")]
        public string Factura { get; set; } = string.Empty;

        [JsonPropertyName("area")]
        public string Area { get; set; } = string.Empty;

        [JsonPropertyName("siguiente_area")]
        public string SiguienteArea { get; set; } = string.Empty;

        [JsonPropertyName("estado")]
        public string Estado { get; set; } = string.Empty;

        [JsonPropertyName("estado_num")]
        public int EstadoNum { get; set; }

        [JsonPropertyName("condicion")]
        public string Condicion { get; set; } = string.Empty;

        [JsonPropertyName("operador")]
        public string Operador { get; set; } = string.Empty;

        [JsonPropertyName("vendedor")]
        public string Vendedor { get; set; } = string.Empty;

        [JsonPropertyName("deposito")]
        public string Deposito { get; set; } = string.Empty;

        [JsonPropertyName("p_cantcuotas")]
        public int CantidadCuotas { get; set; }

        [JsonPropertyName("p_entrega")]
        public decimal Entrega { get; set; }

        [JsonPropertyName("p_autorizar_a_contado")]
        public bool AutorizarAContado { get; set; }

        [JsonPropertyName("imprimir")]
        public bool Imprimir { get; set; }

        [JsonPropertyName("imprimir_preparacion")]
        public bool ImprimirPreparacion { get; set; }

        [JsonPropertyName("cliente_id")]
        public int ClienteId { get; set; }

        [JsonPropertyName("cantidad_cajas")]
        public int CantidadCajas { get; set; }

        [JsonPropertyName("obs")]
        public string Observaciones { get; set; } = string.Empty;

        [JsonPropertyName("total")]
        public string Total { get; set; } = string.Empty;

        [JsonPropertyName("acuerdo")]
        public string Acuerdo { get; set; } = string.Empty;
    }

    public class PedidoDetalleViewModel
    {
        [JsonPropertyName("codigo")]
        public int Codigo { get; set; }

        [JsonPropertyName("descripcion_articulo")]
        public string DescripcionArticulo { get; set; } = string.Empty;

        [JsonPropertyName("cantidad_vendida")]
        public decimal CantidadVendida { get; set; }

        [JsonPropertyName("bonificacion")]
        public string Bonificacion { get; set; } = string.Empty;

        [JsonPropertyName("d_cantidad")]
        public decimal CantidadFaltante { get; set; }

        [JsonPropertyName("precio")]
        public string Precio { get; set; } = string.Empty;

        [JsonPropertyName("ultimo_precio")]
        public string UltimoPrecio { get; set; } = string.Empty;

        [JsonPropertyName("porc_costo")]
        public decimal PorcentajeCosto { get; set; }

        [JsonPropertyName("porcentaje")]
        public decimal Porcentaje { get; set; }

        [JsonPropertyName("descuento")]
        public string Descuento { get; set; } = string.Empty;

        [JsonPropertyName("exentas")]
        public string Exentas { get; set; } = string.Empty;

        [JsonPropertyName("cinco")]
        public string Cinco { get; set; } = string.Empty;

        [JsonPropertyName("diez")]
        public string Diez { get; set; } = string.Empty;

        [JsonPropertyName("dp_lote")]
        public string Lote { get; set; } = string.Empty;

        [JsonPropertyName("vencimiento")]
        public string Vencimiento { get; set; } = string.Empty;

        [JsonPropertyName("comision")]
        public decimal Comision { get; set; }

        [JsonPropertyName("actorizado")]
        public bool Autorizado { get; set; }

        [JsonPropertyName("obs")]
        public string Observaciones { get; set; } = string.Empty;

        [JsonPropertyName("cant_stock")]
        public decimal CantidadStock { get; set; }

        [JsonPropertyName("dp_codigolote")]
        public int CodigoLote { get; set; }

        [JsonPropertyName("cant_pendiente")]
        public decimal CantidadPendiente { get; set; }

        [JsonPropertyName("cantidad_verificada")]
        public decimal CantidadVerificada { get; set; }
    }
}
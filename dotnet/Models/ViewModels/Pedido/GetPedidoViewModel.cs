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
        public uint PedidoId { get; set; }

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


    public class PedidoDetalle
    {
        public uint det_codigo { get; set; }
        public uint art_codigo { get; set; }
        public string codbarra { get; set; } = string.Empty;
        public string descripcion { get; set; } = string.Empty;
        public decimal costo { get; set; }
        public decimal cantidad { get; set; }
        public decimal precio { get; set; }
        public decimal descuento { get; set; }
        public decimal exentas { get; set; }
        public decimal cinco { get; set; }
        public decimal diez { get; set;}
        public uint codlote { get; set; }
        public string lote { get; set; } = string.Empty;
        public bool ar_editar_desc { get; set; }
        public decimal bonificacion { get; set; }
        public string largura { get; set; } = string.Empty;
        public string altura { get; set; } = string.Empty;
        public string mt2 { get; set; } = string.Empty;
        public decimal precio_compra { get; set; }
        public decimal porcentaje_utilidad { get; set; }
    }

    public class PedidoDetalleViewModel
    {
        [JsonPropertyName("det_codigo")]
        public int DetCodigo { get; set; }

        [JsonPropertyName("art_codigo")]
        public int ArtCodigo { get; set; }

        [JsonPropertyName("codbarra")]
        public string CodBarra { get; set; } = string.Empty;

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [JsonPropertyName("costo")]
        public decimal Costo { get; set; }

        [JsonPropertyName("cantidad")]
        public decimal Cantidad { get; set; }

        [JsonPropertyName("precio")]
        public decimal Precio { get; set; }

        [JsonPropertyName("descuento")]
        public decimal Descuento { get; set; }

        [JsonPropertyName("exentas")]
        public decimal Exentas { get; set; }

        [JsonPropertyName("cinco")]
        public decimal Cinco { get; set; }

        [JsonPropertyName("diez")]
        public decimal Diez { get; set; }

        [JsonPropertyName("codlote")]
        public int CodLote { get; set; }

        [JsonPropertyName("lote")]
        public string Lote { get; set; } = string.Empty;

        [JsonPropertyName("ar_editar_desc")]
        public bool EditarDesc { get; set; }

        [JsonPropertyName("bonificacion")]
        public decimal Bonificacion { get; set; }

        [JsonPropertyName("largura")]
        public string Largura { get; set; } = string.Empty;

        [JsonPropertyName("altura")]
        public string Altura { get; set; } = string.Empty;

        [JsonPropertyName("mt2")]
        public string Mt2 { get; set; } = string.Empty;

        [JsonPropertyName("precio_compra")]
        public decimal? PrecioCompra { get; set; }

        [JsonPropertyName("porcentaje_utilidad")]
        public decimal PorcentajeUtilidad { get; set; }
    }
}
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Api.Models.Dtos.Articulo
{
    public class ArticuloLoteDTO
    {
        public uint id_articulo { get; set; }
        public string codigo_barra { get; set; } = string.Empty;
        public string codigo_barra_lote { get; set; } = string.Empty;
        public string descripcion { get; set; } = string.Empty;
        public string codigo_interno { get; set; } = string.Empty;
        public decimal stock { get; set; }
        [JsonIgnore]
        public string lotes_json { get; set; } = string.Empty;
        [JsonIgnore]
        public string depositos_json { get; set; } = string.Empty;
        public decimal precio_costo { get; set; }
        public decimal precio_venta { get; set; }
        public decimal precio_credito { get; set; }
        public decimal precio_mostrador { get; set; }
        public decimal precio_4 { get; set; }
        public string ubicacion { get; set; } = string.Empty;
        public string sub_ubicacion { get; set; } = string.Empty;
        public string marca { get; set; } = string.Empty;
        public string talle { get; set; } = string.Empty;
        public string color { get; set; } = string.Empty;
        public string categoria { get; set; } = string.Empty;
        public string subcategoria { get; set; } = string.Empty;
        public string proveedor_razon { get; set; } = string.Empty;
        public string fecha_ultima_compra { get; set; } = string.Empty;
        public string fecha_ultima_venta { get; set; } = string.Empty;

        [NotMapped]
        public List<LoteDTO> lotes =>
            string.IsNullOrWhiteSpace(lotes_json)
                ? []
                : JsonConvert.DeserializeObject<List<LoteDTO>>(lotes_json)
                    ?? [];

        [NotMapped]
        public List<DepositoLoteDTO> depositos =>
            string.IsNullOrWhiteSpace(depositos_json)
                ? []
                : JsonConvert.DeserializeObject<List<DepositoLoteDTO>>(depositos_json)
                    ?? [];
    }

    public class LoteDTO
    {
        public uint id { get; set; }
        public string lote { get; set; } = string.Empty;
        public decimal cantidad { get; set; }
        public string vencimiento { get; set; } = string.Empty;
    }

    public class DepositoLoteDTO
    {
        public uint codigo { get; set; }
        public string descripcion { get; set; } = string.Empty;
        public decimal stock { get; set; }
    }
}
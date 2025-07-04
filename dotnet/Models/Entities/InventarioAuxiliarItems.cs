using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.Models.Entities
{
    [Table("inventario_auxiliar_items")]
    public class InventarioAuxiliarItems
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("id_articulo")]
        public uint IdArticulo { get; set; }
        [Column("id_lote")]
        public uint IdLote { get; set; }
        [Column("id_inventario")]
        public uint IdInventario { get; set; }
        [JsonIgnore]
        public virtual InventarioAuxiliar? Inventario { get; set; }
        [Column("lote")]
        public string Lote { get; set; } = string.Empty;
        [Column("fecha_vencimiento")]
        public DateTime FechaVencimientoItem { get; set; }
        [Column("cantidad_inicial")]
        public decimal CantidadInicial { get; set; }
        [Column("cantidad_scanner")]
        public decimal CantidadFinal { get; set; }
    }
}
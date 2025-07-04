using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Api.Models.Entities
{
    [Table("detalle_articulos_editado")]
    public class DetalleArticulosEditado
    {
        [Key]
        [Column("a_codigo")]
        public int Codigo { get; set; }

        [Column("a_detalle_venta")]
        public uint DetalleVenta { get; set; }

        [Column("a_descripcion")]
        public string Descripcion { get; set; } = string.Empty;
    }
}
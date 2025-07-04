using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Api.Models.Entities
{
    [Table("config_asiento")]
    public class ConfiguracionAsiento
    {
        [Key]
        [Column("con_codigo")]
        public uint Codigo { get; set; }

        [Column("con_nroTabla")]
        public uint NroTabla { get; set; }

        [Column("con_iva5")]
        public uint Iva5 { get; set; }

        [Column("con_iva10")]
        public uint Iva10 { get; set; }

        [Column("con_exenta")]
        public uint Exenta { get; set; }

        [Column("con_gravada")]
        public uint Gravada { get; set; }

        [Column("con_gravada10")]
        public uint Gravada10 { get; set; }

        [Column("con_contado")]
        public uint Contado { get; set; }

        [Column("con_cont_prov")]
        public uint ContProv { get; set; }

        [Column("con_credito")]
        public uint Credito { get; set; }

        [Column("con_credito_extranjero")]
        public uint CreditoExtranjero { get; set; }

        [Column("con_creditod")]
        public uint CreditoD { get; set; }

        [Column("con_devolucion_art")]
        public uint DevolucionArt { get; set; }

        [Column("con_descuento")]
        public uint Descuento { get; set; }

        [Column("con_materia_prima")]
        public uint MateriaPrima { get; set; }

        [Column("con_producto")]
        public uint Producto { get; set; }

        [Column("con_bienes")]
        public uint Bienes { get; set; }

        [Column("con_concepto")]
        public string Concepto { get; set; } = string.Empty;

        [Column("con_anticipo")]
        public uint Anticipo { get; set; }

        [Column("con_anticipo_extranjero")]
        public uint AnticipoExtranjero { get; set; }

        [Column("con_anticipo_caja")]
        public uint AnticipoCaja { get; set; }

        [Column("con_anticipo_banco")]
        public uint AnticipoBanco { get; set; }

        [Column("con_anticipo_diferido")]
        public uint AnticipoDiferido { get; set; }

        [Column("con_anticipod")]
        public uint AnticipoD { get; set; }

        [Column("con_anticipo_cajad")]
        public uint AnticipoCajaD { get; set; }

        [Column("con_anticipo_bancod")]
        public uint AnticipoBancoD { get; set; }

        [Column("con_anticipo_diferidod")]
        public uint AnticipoDiferidoD { get; set; }

        [Column("con_automatico")]
        public uint Automatico { get; set; }

        [Column("con_estado")]
        public uint Estado { get; set; }
    }
}
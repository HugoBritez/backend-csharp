using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities
{
    [Table("personas")]
    public class Persona
    {
        [Key]
        [Column("pe_codigo")]
        public uint Codigo { get; set; }
        [Column("pe_interno")]
        public string CodigoInterno { get; set; } = "";
        [Column("pe_razon")]
        public string RazonSocial { get; set; } = ""; //es el nombre que figura en la subsecretaria
        [Column("pe_nombre_fantasia")]
        public string NombreFantasia { get; set; } = ""; // es el nombre por el que se conoce el establecimiento/cliente
        [Column("pe_ruc")]
        public string Ruc { get; set; } = "";
        [Column("pe_ci")]
        public string Ci { get; set; } = "";
        [Column("pe_tipo_doc")]
        public uint TipoDocumento { get; set; }
        [Column("pe_departamento")]
        public uint Departamento { get; set; }
        [Column("pe_ciudad")]
        public uint Ciudad { get; set; }
        [Column("pe_direccion")]
        public string Direccion { get; set; } = "";
        [Column("pe_barrio")]
        public string Barrio { get; set; } = "";
        [Column("pe_zona")]
        public uint Zona { get; set; }
        [Column("pe_moneda")]
        public uint Moneda { get; set; }
        [Column("pe_observacion")]
        public string Observacion { get; set; } = "";
        [Column("pe_email")]
        public string Email { get; set; } = "";
        [Column("pe_telefono")]
        public string Telefono { get; set; } = "";
        [Column("pe_estado")]
        public int Estado { get; set; } = 1;
        [Column("pe_fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        [Column("pe_fecha_modificacion")]
        public DateTime FechaModificacion { get; set; } = DateTime.Now;
    }
}
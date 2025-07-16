using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Storage;

namespace Api.Models.Entities
{
    [Table("contactos_crm")]
    public class ContactoCRM
    {
        [Key]
        [Column("co_codigo")]
        public uint Codigo { get; set;}

        [Column("co_nombre")]
        public string? Nombre { get; set;}
        [Column("co_mail")]
        public string? EMail { get; set; }
        [Column("co_telefono")]
        public string? Telefono { get; set; }
        [Column("co_ruc")]
        public string? Ruc { get; set; }
        [Column("co_notas")]
        public string? Notas { get; set; }
        [Column("co_empresa")]
        public string? Empresa { get; set; }
        [Column("co_cargo")]
        public string? Cargo { get; set; }
        [Column("co_fecha_contacto")]
        public DateTime FechaContacto { get; set; }
        [Column("co_es_cliente")]
        public uint EsCliente { get; set; }
        [Column("co_departamento")]
        public uint Departamento { get; set; }
        [Column("co_ciudad")]
        public uint Ciudad { get; set; }
        [Column("co_zona")]
        public uint Zona { get; set; }
        [Column("co_direccion")]
        public string? Direccion { get; set; }
        [Column("co_estado")]
        public int Estado { get; set; }

        [Column("co_general")]
        public int General { get; set; }
        [Column("co_operador")]
        public uint Operador { get; set; }
    }
}
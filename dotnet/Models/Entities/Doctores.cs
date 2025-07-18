using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities
{
    [Table("doctores")]
    public class Doctor
    {
        [Key]
        [Column("doc_codigo")]
        public uint Codigo { get; set;}
        [Column("doc_documento")]
        public string? Documento { get; set; }
        [Column("doc_fecha")]
        public DateOnly Fecha { get; set;}
        [Column("doc_matricula")]
        public string? Matricula { get; set;}
        [Column("doc_apellidos")]
        public string? Apellidos { get; set; }
        [Column("doc_nombres")]
        public string? Nombres { get; set; }
        [Column("doc_estado_civil")]
        public int EstadoCivil { get; set;}
        [Column("doc_nacionalidad")]
        public uint Nacionalidad { get; set; }
        [Column("doc_sexo")]
        public int Sexo { get; set; }
        [Column("doc_direccion")]
        public string? Direccion { get; set; }
        [Column("doc_correo")]
        public string? Correo { get; set;}
        [Column("doc_cargo")]
        public uint Cargo { get; set;}
        [Column("doc_fijo")]
        public string? Fijo { get; set; }
        [Column("doc_celular")]
        public string? Celular { get; set;}
        [Column("doc_estado")]
        public int Estado { get; set;}
    }
}
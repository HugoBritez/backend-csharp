using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities
{
    [Table("pacientes")]
    public class Paciente
    {
        [Key]
        [Column("pac_codigo")]
        public uint Codigo { get; set; }
        [Column("pac_apellidos")]
        public string? Apellidos { get; set;}
        [Column("pac_nombres")]
        public string? Nombres { get; set; }
        [Column("pac_ciudad")]
        public uint Ciudad { get; set;}
        [Column("pac_tipo_documento")]
        public uint TipoDocumento { get; set; }
        [Column("pac_documento")]
        public string? Documento { get; set; }
        [Column("pac_fecha")]
        public DateTime Fecha { get; set;}
        [Column("pac_hora")]
        public string? Hora { get; set; }
        [Column("pac_ingreso")]
        public DateTime Ingreso { get; set;}
        [Column("pac_edad")]
        public uint Edad { get; set;}
        [Column("pac_tipo_edad")]
        public uint TipoEdad { get; set; }
        [Column("pac_pais")]
        public uint Pais { get; set; }
        [Column("pac_departamento")]
        public uint Departamento { get; set;}
        [Column("pac_nacionalidad")]
        public uint Nacionalidad { get; set; }
        [Column("pac_resciudad")]
        public uint ResCiudad { get; set; }
        [Column("pac_resdepartamento")]
        public uint ResDepartamento { get; set; }
        [Column("pac_barrio")]
        public uint Barrio { get; set; }
        [Column("pac_direccion")]
        public string? Direccion { get; set; }
        [Column("pac_fijo")]
        public string? Fijo { get; set; }
        [Column("pac_celular")]
        public string? Celular { get; set;}
        [Column("pac_correo")]
        public string? Correo { get; set;}
        [Column("pac_seguro")]
        public uint Seguro { get; set;}
        [Column("pac_ocupacion")]
        public uint Ocupacion { get; set; }
        [Column("pac_estado_civil")]
        public uint EstadoCivil { get; set;}
        [Column("pac_sexo")]
        public uint Sexo { get; set; }
        [Column("pac_estado")]
        public int Estado { get; set;}
        [Column("pac_carnet")]
        public string? Carnet { get; set; }
        [Column("pac_grupo")]
        public string? Grupo { get; set; }
        [Column("pac_foto")]
        public string? Foto { get; set;}
        [Column("pac_ruc")]
        public string? Ruc { get; set; }
        [Column("pac_limite")]
        public decimal Limite { get; set;}
        [Column("pac_ficha")]
        public string? Ficha { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities
{
    [Table("proyectos_colaboradores_crm")]
    public class ProyectosColaboradoresCRM
    {
        [Key]
        [Column("pc_codigo")]
        public uint Codigo { get; set;}
        [Column("pc_proyecto")]
        public uint Proyecto { get; set; }
        [Column("pc_colaborador")]
        public uint Colaborador { get; set;}
        [Column("pc_estado")]
        public int Estado { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Api.Models.Entities
{
    [Table("oportunidades_pestanas_crm")]
    [PrimaryKey(nameof(Pestana), nameof(Oportunidad))]
    public class OportunidadPestanaCRM
    {
        [Column("op_pestana")]
        public uint Pestana { get; set; }
        
        [Column("op_oportunidad")]
        public uint Oportunidad { get; set; }
    }
}
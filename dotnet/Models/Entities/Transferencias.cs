using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Entities
{
    [Table("transferencias")]
    public class Transferencia
    {
        [Key]
        [Column("tr_id")]
        public uint Id { get; set; }

        [Column("tr_fecha")]
        public DateTime Fecha { get; set; }

        [Column("tr_operador")]
        public uint Operador { get; set; }

        [Column("tr_origen")]
        public uint Origen { get; set; }

        [Column("tr_destino")]
        public uint Destino { get; set; }

        [Column("tr_comprobante")]
        [StringLength(45)]
        public string Comprobante { get; set; } = string.Empty;

        [Column("tr_estado")]
        public byte Estado { get; set; } = 1;

        [Column("tr_motivo")]
        [StringLength(100)]
        public string Motivo { get; set; } = string.Empty;

        [Column("tr_fechaOP")]
        public DateTime FechaOperacion { get; set; }

        [Column("tr_idmaestro")]
        public uint IdMaestro { get; set; } = 0;

        [Column("tr_estado_transf")]
        public byte EstadoTransferencia { get; set; } = 1;

        [Column("tr_user_autorizador")]
        public uint UserAutorizador { get; set; } = 0;

        [Column("tr_talle")]
        public uint Talle { get; set; } = 0;

        [Column("tr_solicitud")]
        public uint Solicitud { get; set; } = 0;
        
        public virtual ICollection<TransferenciaItem>? Items { get; set; }

    }
}
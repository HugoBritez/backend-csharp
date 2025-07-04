using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Api.Models.Dtos
{
    public class DetallePedidoFaltante
    {
        public decimal? Cantidad { get; set; }
        public uint? Situacion { get; set; }
        public string? Observacion { get; set; } = string.Empty;        
    }
}
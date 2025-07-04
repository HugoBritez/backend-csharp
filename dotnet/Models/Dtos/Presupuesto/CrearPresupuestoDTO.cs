using Api.Models.Entities;

namespace Api.Models.Dtos
{
    public class CrearPresupuestoDTO
    {
        public required Presupuesto Presupuesto { get; set; }
        public required PresupuestoObservacion Observacion { get; set; }
        public required IEnumerable<DetallePresupuesto> DetallePresupuesto { get; set; }
    }
}
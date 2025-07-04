using Api.Models.Entities;
namespace Api.Models.ViewModels
{
    public class RecuperarPresupuestoViewModel
    {
        public required Presupuesto Presupuesto { get; set; }
        public required IEnumerable<DetallePresupuesto> Detalles { get; set;  }
    }
}
namespace Api.Models.ViewModels
{
    public class ConsultaMovimientosResponse
    {
        public IEnumerable<MovimientoBancarioViewModel> Movimientos { get; set; } = [];
        public decimal SaldoAnterior { get; set; }
        public decimal TotalCredito { get; set; }
        public decimal TotalDebito { get; set; }
        public decimal Total { get; set; }
        public decimal SaldoActual { get; set; }
        public decimal AConHaber { get; set; }
    }
}
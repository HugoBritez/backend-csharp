namespace Api.Models.ViewModels
{
    public class ReportePedidosFacturadosViewModel
    {
        public string? vendedor { get;set;}
        public uint? codCliente { get; set; }
        public string? nombreCliente { get; set; }
        public uint? nroPedido { get; set; }
        public string? fechaPedido { get; set; }
        public uint? codProducto { get; set; }
        public string? producto { get; set; }
        public string? marca { get; set; }
        public decimal? cantidadPedido { get; set; }
        public decimal? cantidadFacturada { get; set; }
        public decimal? cantidadFaltante { get; set; }
        public decimal? totalPedido { get; set; }
        public decimal? totalVenta { get;set;}
        public decimal? diferenciaTotal { get; set; }
    }
}
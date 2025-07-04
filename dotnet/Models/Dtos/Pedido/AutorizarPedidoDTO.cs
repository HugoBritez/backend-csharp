namespace Api.Models.Dtos
{
    public class AutorizarPedidoDTO
    {
        public uint idPedido { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
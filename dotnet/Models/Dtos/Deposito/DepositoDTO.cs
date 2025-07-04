namespace Api.Models.Dtos.Deposito
{
    public class DepositoDTO
    {
        public uint dep_codigo { get; set;}
        public string dep_descripcion { get; set;} = string.Empty;
        public uint dep_principal { get; set;}
    }
}

using Api.Models.Entities;

namespace Api.Models.ViewModels
{
    public class CuentaBancariaViewModel : CuentaBancaria
    {
        public string? BancoDescripcion { get; set;}
        public string? TitularDescripcion { get; set;}
        public string? MonedaDescripcion { get; set;}
        public string? CuentaNombre { get; set;}
    }
}
namespace Api.Models.ViewModels
{
    public class DatosCajaViewModel
    {
        public string Descripcion { get; set; } = string.Empty;
        public DateOnly Fecha { get; set; }

        public uint Operador { get; set; }

        public string Cajero { get; set; } = string.Empty;
    }
} 
namespace Api.Models.ViewModels
{
    public class ProveedorViewModel
    {
        public uint ProCodigo { get; set; }
        public string ProRazon { get; set; } = string.Empty;
        public string? ProZona { get; set; } = "Sin Determinar";
    }
}
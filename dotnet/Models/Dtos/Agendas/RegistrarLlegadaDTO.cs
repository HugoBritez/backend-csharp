namespace Api.Models.Dtos
{
    public class RegistrarLlegadaDTO
    {
        public uint AgendaId { get; set; }
        public uint OperadorId { get; set; }
        public string? Longitud { get; set; }
        public string? Latitud { get; set; }
    }
}
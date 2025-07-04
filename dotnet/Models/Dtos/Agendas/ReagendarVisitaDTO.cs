namespace Api.Models.Dtos
{
    public class ReagendarVisitaDTO
    {
        public uint IdAgenda { get; set; }
        public DateTime ProximaFecha { get; set; }
        public string? ProximaHora { get; set; }
    }
}
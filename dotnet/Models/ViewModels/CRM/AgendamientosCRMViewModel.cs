using Api.Models.Entities;

namespace Api.Models.ViewModels
{
    public class AgendamientosCRMViewModel : AgendamientoCRM
    {
        public string? PacienteNombre { get; set; }
        public string? DoctorNombre { get; set; }
    }
}
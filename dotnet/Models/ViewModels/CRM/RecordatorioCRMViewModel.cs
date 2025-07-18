using Api.Models.Entities;

namespace Api.Models.ViewModels
{
    public class RecordatorioCRMViewModel : RecordatorioCRM
    {
        public string? OperadorNombre { get; set; }
        public string? ClienteNombre { get; set;}
    }
}
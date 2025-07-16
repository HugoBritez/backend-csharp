using Api.Models.Entities;

namespace Api.Models.ViewModels
{
    public class OportunidadViewModel : OportunidadCRM
    {
        public string? ClienteNombre { get; set; }
        public string? OperadorNombre { get; set; }
        public string? EstadoDescripcion { get; set; }
    }
}
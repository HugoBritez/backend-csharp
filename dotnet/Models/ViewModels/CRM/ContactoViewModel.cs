using Api.Models.Entities;

namespace Api.Models.ViewModels
{
    public class ContactoViewModel : ContactoCRM
    {
        public string? DepartamentoDescripcion { get; set; }
        public string? CiudadDescripcion { get; set; }
        public string? ZonaDescripcion { get; set; }
        public string? OperadorNombre { get; set; }
    }
}
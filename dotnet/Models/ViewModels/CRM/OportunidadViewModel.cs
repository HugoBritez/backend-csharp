using Api.Models.Entities;
using Api.Models.ViewModels.CRM;

namespace Api.Models.ViewModels
{
    public class OportunidadViewModel : OportunidadCRM
    {
        public string? ClienteNombre { get; set; }
        public string? OperadorNombre { get; set; }
        public string? OperadorCargo { get; set; }
        public string? EstadoDescripcion { get; set; }
        public string? ClienteRuc { get; set; }
        public IEnumerable<ColaboradoresViewModel> Colaboradores { get; set; } = [];
        public string? AutorizadoPorNombre { get; set; }
        public string? AutorizadoPorCargo { get; set; }
    }
}
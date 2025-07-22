using Api.Models.Entities;

namespace Api.Models.Dtos.CRM
{
    public class CrearOportunidadDTO : OportunidadCRM
    {
        public IEnumerable<uint> Colaboradores { get; set; } = [];
    }
}
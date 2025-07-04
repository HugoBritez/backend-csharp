using Api.Models.Entities;

namespace Api.Models.ViewModels
{
    public class EditarPersonaViewModel
    {
        public Persona? persona { get;set;}
        public Proveedor? proveedor { get; set; }
        public Cliente? cliente  { get; set; }
        public IEnumerable<ListaPrecio>? precios { get;set;}
        public IEnumerable<int> Tipo { get; set; } = [];

    }
}
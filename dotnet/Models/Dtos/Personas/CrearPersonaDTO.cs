using Api.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Dtos
{
    public class CrearPersonaDTO
    {
        [Required(ErrorMessage = "La información de la persona es requerida")]
        public Persona? persona { get; set; }
        
        public Operador? Operador { get; set; }
        public Proveedor? Proveedor { get; set; }
        public Cliente? Cliente { get; set; }
        
        [Required(ErrorMessage = "Debe especificar al menos un tipo de persona")]
        [MinLength(1, ErrorMessage = "Debe especificar al menos un tipo de persona")]
        public IEnumerable<int> Tipo { get; set; } = []; // 0- cliente, 1- proveedor, 2- operador
        
        public IEnumerable<int> Precios { get; set; } = [];
    } 
}
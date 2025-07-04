namespace Api.Models.Dtos.Sucursal
{
    public class SucursalDTO
    {
        public uint id { get; set; }
        public string descripcion { get; set; } = string.Empty;

        public string direccion { get; set; } = string.Empty;
        public string nombre_emp { get; set; } = string.Empty;

        public string ruc_emp { get; set; } = string.Empty;

        public uint matriz { get; set; }

        
    }
}

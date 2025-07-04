namespace Api.Models.Dtos
{
    public class UsuarioViewModel
    {
        public uint op_codigo { get; set; }
        public string op_nombre { get; set; } = string.Empty;
        public string op_documento { get; set; } = string.Empty;
        public string op_rol { get; set; } = string.Empty;
    }
}
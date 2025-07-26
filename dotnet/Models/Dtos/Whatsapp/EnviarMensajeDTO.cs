namespace Api.Models.Dtos.Whatsapp
{
    public class EnviarMensajeDTO
    {
        public string? Numero { get; set;}
        public string? Mensaje { get; set; }
        public Stream? Archivo { get; set; }
    }
}
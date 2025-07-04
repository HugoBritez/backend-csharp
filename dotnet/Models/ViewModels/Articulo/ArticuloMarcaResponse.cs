namespace Api.Models.Dtos.Articulo
{
    public class ArticuloMarcaResponse
    {
        public uint id { get; set;}
        public string nombre { get; set;} = string.Empty;

        public int cantidad_articulos { get; set;}
    }
}

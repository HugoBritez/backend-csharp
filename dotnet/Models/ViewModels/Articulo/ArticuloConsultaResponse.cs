namespace Api.Models.Dtos.Articulo
{
    public class ArticuloConsultaResponse
    {
        public IEnumerable<ConsultaArticulosDto> Datos { get; set; }= [];
        public PaginacionDTO Paginacion { get; set; }  = new PaginacionDTO();
    }
}
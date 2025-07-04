using Api.Models.Dtos.Articulo;
using Api.Models.Entities;

namespace Api.Repositories.Interfaces
{
    
    public interface IArticuloRepository
    {

        Task<ArticuloBusquedaDTO?> BuscarArticuloPorCodigo(
            string codigoBarra,
            uint deposito,
            bool? stock
        );

        Task<IEnumerable<ArticuloBusquedaDTO>> BuscarArticulos(
            uint? articuloId = null,
            string? busqueda = null,
            string? codigoBarra = null,
            uint moneda = 1,
            bool? stock = null,
            uint? deposito = null,
            uint? marca = null,
            uint? categoria = null,
            uint? ubicacion = null,
            uint? proveedor = null,
            string? codInterno = null,
            string? lote = null,
            bool? negativo = null
        );

        Task<ArticuloConsultaResponse> ConsultarArticulos(
        string? busqueda = null,
        string? deposito = null,
        string? stock = null,
        string? marca = null,
        string? categoria = null,
        string? subcategoria = null,
        string? proveedor = null,
        string? ubicacion = null,
        bool? servicio = null,
        uint? moneda = null,
        string? unidadMedida = null,
        int pagina = 1,
        int limite = 50,
        string? tipoValorizacionCosto = null);

        Task<IEnumerable<ArticuloLoteDTO>> ConsultarArticuloSimple(
            uint? articulo_id,
            string? busqueda,
            string? codigo_barra,
            uint? moneda = 1,
            bool? stock = null,
            uint? deposito = null,
            uint? marca = null,
            uint? categoria = null,
            uint? ubicacion = null,
            uint? proveedor = null,
            string? cod_interno = null
        );

        Task<IEnumerable<ArticuloCategoriaResponse>> ArticulosPorCategoria();

        Task<IEnumerable<ArticuloMarcaResponse>> ArticulosPorMarca();

        Task<IEnumerable<ArticuloSeccionResponse>> ArticulosPorSeccion();

        Task<IEnumerable<ArticuloEnPedidoResponse>> ArticulosEnPedido(int articulo_id, int id_lote);
        Task<Articulo> GetById(uint id);
        Task<Articulo?> Update(Articulo articulo);

        Task<Articulo> Create(Articulo articulo);

    }
}

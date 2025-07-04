using Microsoft.AspNetCore.Mvc;
using Api.Repositories.Interfaces;
using Api.Models.Dtos.Articulo;
using Microsoft.AspNetCore.Authorization;
using Api.Models.Dtos;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    
    public class ArticuloController : ControllerBase
    {
    private readonly IArticuloRepository _articuloRepository;

    public ArticuloController(IArticuloRepository articuloRepository)
    {
        _articuloRepository = articuloRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArticuloLoteDTO>>> ConsultarArticuloSimple(
        [FromQuery] uint? articulo_id = null,
        [FromQuery] string? busqueda = null,
        [FromQuery] string? codigo_barra = null,
        [FromQuery] uint? moneda = 1,
        [FromQuery] bool? stock = null,
        [FromQuery] uint? deposito = null,
        [FromQuery] uint? marca = null,
        [FromQuery] uint? categoria = null,
        [FromQuery] uint? ubicacion = null,
        [FromQuery] uint? proveedor = null,
        [FromQuery] string? cod_interno = null
    )
    {
        var articulos = await _articuloRepository.ConsultarArticuloSimple(
                articulo_id, busqueda, codigo_barra, moneda, stock, deposito, marca, categoria, ubicacion, proveedor, cod_interno);

        return Ok(articulos);
    }

    [HttpGet("buscar")]
    
    public async Task<ActionResult<IEnumerable<ArticuloBusquedaDTO>>> BuscarArticulos(
        [FromQuery] uint? articuloId = null,
        [FromQuery] string? busqueda = null,
        [FromQuery] string? codigoBarra = null,
        [FromQuery] uint moneda = 1,
        [FromQuery] bool? stock = null,
        [FromQuery] uint? deposito = null,
        [FromQuery] uint? marca = null,
        [FromQuery] uint? categoria = null,
        [FromQuery] uint? ubicacion = null,
        [FromQuery] uint? proveedor = null,
        [FromQuery] string? codInterno = null,
        [FromQuery] string? lote = null,
        [FromQuery] bool? negativo = null)
    {
        var articulos = await _articuloRepository.BuscarArticulos(
            articuloId, busqueda, codigoBarra, moneda, stock, deposito,
            marca, categoria, ubicacion, proveedor, codInterno, lote, negativo);

        return Ok(articulos);
    }



    [HttpGet("consultar")]
    public async Task<ActionResult<ArticuloConsultaResponse>> ConsultarArticulos(
        [FromQuery] string? busqueda = null,
        [FromQuery] string? deposito = null,
        [FromQuery] string? stock = null,
        [FromQuery] string? marca = null,
        [FromQuery] string? categoria = null,
        [FromQuery] string? subcategoria = null,
        [FromQuery] string? proveedor = null,
        [FromQuery] string? ubicacion = null,
        [FromQuery] bool? servicio = null,
        [FromQuery] uint? moneda = null,
        [FromQuery] string? unidadMedida = null,
        [FromQuery] int pagina = 1,
        [FromQuery] int limite = 50,
        [FromQuery] string? tipoValorizacionCosto = null

    )
    {
        var articulos = await _articuloRepository.ConsultarArticulos(
            busqueda, deposito, stock, marca, categoria, subcategoria, proveedor, ubicacion, servicio, moneda, unidadMedida, pagina, limite, tipoValorizacionCosto);

        return Ok(articulos);
    }

    [HttpGet("categorias")]
    public async Task<ActionResult<IEnumerable<ArticuloCategoriaResponse>>> ArticulosPorCategoria()
    {
        var categorias = await _articuloRepository.ArticulosPorCategoria();

        return Ok(categorias);
    }

    [HttpGet("marcas")]
    public async Task<ActionResult<IEnumerable<ArticuloMarcaResponse>>> ArticulosPorMarca()
    {
        var marcas = await _articuloRepository.ArticulosPorMarca();

        return Ok(marcas);
    }

    [HttpGet("secciones")]
    public async Task<ActionResult<IEnumerable<ArticuloSeccionResponse>>> ArticulosPorSeccion()
    {
        var secciones = await _articuloRepository.ArticulosPorSeccion();

        return Ok(secciones);
    }
    
    [HttpGet("pedido")]
    public async Task<ActionResult<IEnumerable<ArticuloEnPedidoResponse>>> ArticulosEnPedido(
        [FromQuery] int articulo_id,
        [FromQuery] int id_lote
    )
    {
        var articulos = await _articuloRepository.ArticulosEnPedido(articulo_id, id_lote);

        return Ok(articulos);
    }

    [HttpGet("codigo")]
    public async Task<ActionResult<ArticuloBusquedaDTO>> BuscarArticuloPorCodigo(
        [FromQuery] string codigoBarra,
        [FromQuery] uint deposito,
        [FromQuery] bool? stock
    )
    {
        var articulo = await _articuloRepository.BuscarArticuloPorCodigo(codigoBarra, deposito, stock);

        return Ok(articulo);
    }
}
}

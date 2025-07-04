using Microsoft.AspNetCore.Mvc;
using Api.Repositories.Interfaces;
using Api.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Api.Models.Entities;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/lista-precios")]
    // [Authorize]
    public class ListaPreciosController : ControllerBase
    {
        private readonly IListaPrecioRepository _listaPrecioRepository;

        public ListaPreciosController(IListaPrecioRepository listaPrecioRepository)
        {
            _listaPrecioRepository = listaPrecioRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.ViewModels.ListaPrecio>>> GetListaPrecios()
        {
            var listaPrecios = await _listaPrecioRepository.GetAll();
            return Ok(listaPrecios);
        }

        [HttpGet("cliente")]
        public async Task<ActionResult<IEnumerable<Models.ViewModels.ListaPrecio>>> GetListaPreciosByCliente([FromQuery] uint clienteId)
        {
            if (clienteId == 0)
            {
                return BadRequest("El ID del cliente no puede ser cero.");
            }

            var listaPrecios = await _listaPrecioRepository.GetByCliente(clienteId);
            if (listaPrecios == null || !listaPrecios.Any())
            {
                return NotFound($"No se encontraron listas de precios para el cliente con ID {clienteId}.");
            }

            return Ok(listaPrecios);
        }

        [HttpPost("cliente")]
        public async Task<ActionResult<Models.Entities.ListaPrecio>> InsertarListaPrecioPorCliente([FromBody] ClientesListaDePrecio data)
        {
            if (data == null || data.ClienteId == 0 || data.ListaDePrecioId == 0)
            {
                return BadRequest("Los datos proporcionados son inválidos.");
            }

            var nuevaListaPrecio = await _listaPrecioRepository.InsertarPorCliente(data);
            return CreatedAtAction(nameof(GetListaPreciosByCliente), new { clienteId = data.ClienteId }, nuevaListaPrecio);
        }

    }
}

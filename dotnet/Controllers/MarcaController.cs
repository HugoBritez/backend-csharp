using Microsoft.AspNetCore.Mvc;
using Api.Repositories.Interfaces;
using Api.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Api.Models.Entities;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/marcas")]
    // [Authorize]
    public class MarcaController : ControllerBase
    {
        private readonly IMarcaRepository _marcaRepository;

        public MarcaController(IMarcaRepository marcaRepository)
        {
            _marcaRepository = marcaRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Marca>>> GetCategorias(
            [FromQuery] string? busqueda
        )
        {
            var marcas = await _marcaRepository.GetMarcas(busqueda);
            return Ok(marcas);
        }

    }
}

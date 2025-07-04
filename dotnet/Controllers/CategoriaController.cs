using Microsoft.AspNetCore.Mvc;
using Api.Repositories.Interfaces;
using Api.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Api.Models.Entities;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/categorias")]
    // [Authorize]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaRepository _categoriasRepository;

        public CategoriaController(ICategoriaRepository categoriaRepository)
        {
            _categoriasRepository = categoriaRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias(
            [FromQuery] string? busqueda
        )
        {
            var categorias = await _categoriasRepository.GetCategorias(busqueda);
            return Ok(categorias);
        }

    }
}

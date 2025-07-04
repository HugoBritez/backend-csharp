using Microsoft.AspNetCore.Mvc;
using Api.Repositories.Interfaces;
using Api.Models.Dtos;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    // [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioViewModel>>> GetUsuarios(
            [FromQuery] string? busqueda,
            [FromQuery] uint? id,
            [FromQuery] uint? rol   
        )
        {
            var usuarios = await _usuarioRepository.GetUsuarios(busqueda, id, rol);
            return Ok(usuarios);
        }

    }
}

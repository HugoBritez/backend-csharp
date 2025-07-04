using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/tipo-plazo")]
    public class TipoPlazoController(ITipoPlazoRepository tipoPlazoRepository) : ControllerBase
    {
        public readonly ITipoPlazoRepository _tipoPlazoRepository = tipoPlazoRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoPlazo>>> GetAll()
        {
            var res = await _tipoPlazoRepository.GetAll();
            return Ok(res);
        }

    }
}
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoDocumentoController : ControllerBase
    {
        private readonly ITipoDocumentoRepository _tipoDocumentoRepository;

        public TipoDocumentoController(ITipoDocumentoRepository tipoDocumentoRepository)
        {
            _tipoDocumentoRepository = tipoDocumentoRepository;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<TipoDocumento>>> GetAll()
        {
            var res = await _tipoDocumentoRepository.GetAll();
            if (res == null || !res.Any())
            {
                return NotFound("No se encontraron tipos de documentos.");
            }
            return Ok(res);
        }
    }
}
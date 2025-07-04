using Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class GrupoClienteController : ControllerBase
    {
        private readonly IClienteGruposRepository _grupoClienteRepository;

        public GrupoClienteController(IClienteGruposRepository grupoClienteRepository)
        {
            _grupoClienteRepository = grupoClienteRepository;
        }

        [HttpGet("api/grupo-clientes")]
        public async Task<IActionResult> GetAll()
        {
                var gruposClientes = await _grupoClienteRepository.GetAll();
                return Ok(gruposClientes);
        }
    }
}
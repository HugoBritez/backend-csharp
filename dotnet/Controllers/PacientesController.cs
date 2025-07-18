using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/pacientes")]
    // [Authorize]
    public class PacientesController(IPacientesRepository pacientesRepository) : ControllerBase
    {
        private readonly IPacientesRepository _pacientesRepository = pacientesRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Paciente>>> GetAll()
        {
            var pacientes = await _pacientesRepository.GetAll();
            return Ok(pacientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Paciente>> GetById(uint id)
        {
            var paciente = await _pacientesRepository.GetById(id);
            return Ok(paciente);
        }
    }
}
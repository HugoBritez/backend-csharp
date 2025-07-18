using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/doctores")]
    // [Authorize]
    public class DoctoresController(IDoctoresRepository doctoresRepository) : ControllerBase
    {
        private readonly IDoctoresRepository _doctoresRepository = doctoresRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetAll()
        {
            var doctores = await _doctoresRepository.GetAll();
            return Ok(doctores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetById(uint id)
        {
            var doctor = await _doctoresRepository.GetById(id);
            if (doctor == null)
                return NotFound();
            return Ok(doctor);
        }
    }
}
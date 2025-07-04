using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class TallesController(ITallesRepository tallesRepository) : ControllerBase
    {
        private readonly ITallesRepository _tallesRepository = tallesRepository;

        [HttpGet("api/talles")]
        public async Task<ActionResult<IEnumerable<Talle>>> GetAll()
        {
            var talles = await _tallesRepository.GetAll();
            return Ok(talles);
        }   
    }
}
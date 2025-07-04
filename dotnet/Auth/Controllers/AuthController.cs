using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Auth.Services;
using Api.Auth.Models;

namespace Api.Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            Console.WriteLine("Login");
            Console.WriteLine(request.Usuario);
            Console.WriteLine(request.Password);
            var response = await _authService.Login(request.Usuario , request.Password);
            if (response == null)
            {
                return Unauthorized();
            }

            return Ok(response);
        }
    }
}

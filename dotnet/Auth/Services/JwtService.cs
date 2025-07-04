using Api.Auth.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotNetEnv;

namespace Api.Auth.Services
{
    public interface IJwtService
    {
        string GenerateToken(UsuarioResponse user);
    }

    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(UsuarioResponse user)
        {
            var jwtKey = Env.GetString("JWT_KEY");
            var jwtIssuer = Env.GetString("JWT_ISSUER");
            var jwtAudience = Env.GetString("JWT_AUDIENCE");

            Console.WriteLine($"JWT Key: {jwtKey}");
            Console.WriteLine($"JWT Issuer: {jwtIssuer}");
            Console.WriteLine($"JWT Audience: {jwtAudience}");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("op_codigo", user.OpCodigo.ToString()),
                new Claim("op_nombre", user.OpNombre),
                new Claim("or_rol", user.OrRol.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

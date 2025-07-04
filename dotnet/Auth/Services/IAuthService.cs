using Api.Auth.Models;

namespace Api.Auth.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> Login( string usuario, string  password);
    }
}
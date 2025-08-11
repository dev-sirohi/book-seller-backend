using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto);
        Task<RegisterResponseDto?> RegisterAsync(RegisterRequestDto dto);
        Task<UserDto?> GetProfileAsync(string email);
        Task LogoutAsync(string token);
    }
}
using System.Threading.Tasks;
using Domain.DTO;
using Common;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<ResultWrapper> LoginAsync(LoginRequestDTO dto);
        Task<ResultWrapper> RegisterAsync(RegisterRequestDTO dto);
        Task<ResultWrapper> GetProfileAsync(string email);
        Task LogoutAsync(string token);
    }
}
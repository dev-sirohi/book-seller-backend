using BSB.src.Common;
using BSB.src.Common.Database.DBInterfaces;
using BSB.src.Domain.DTO;

namespace BSB.src.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ResultWrapper> LoginAsync(LoginRequestDto dto, IDBConnection connection, IDBTransaction transaction);
        Task<ResultWrapper> RegisterAsync(RegisterRequestDto dto, IDBConnection connection, IDBTransaction transaction);
        Task<ResultWrapper> GetProfileAsync(string email, IDBConnection connection, IDBTransaction transaction);
        Task LogoutAsync(string token, IDBConnection connection, IDBTransaction transaction);
    }
}
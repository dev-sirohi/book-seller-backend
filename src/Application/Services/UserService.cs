using Application.DTOs;
using Application.Interfaces;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        public Task<UserDto?> GetByIdAsync(Guid id) => Task.FromResult<UserDto?>(null);
    }
}
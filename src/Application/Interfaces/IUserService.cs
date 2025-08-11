using Application.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> GetByIdAsync(Guid id);
        Task<UserDto?> GetByEmailAsync(string email);
        // Add more methods as needed
    }
}
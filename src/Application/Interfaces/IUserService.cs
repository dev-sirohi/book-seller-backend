using Application.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;
using Common;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<ResultWrapper> GetByIdAsync(Guid id);
        Task<ResultWrapper> GetByEmailAsync(string email);
        // Add more methods as needed
    }
}
using System.Threading.Tasks;
using System.Collections.Generic;
using BSB.src.Common;
using BSB.src.Common.Database.DBInterfaces;

namespace BSB.src.Application.Interfaces
{
    public interface IUserService
    {
        Task<ResultWrapper> GetByUserIdAsync(Guid id, IDBConnection connection, IDBTransaction transaction);
        Task<ResultWrapper> GetByEmailAsync(string email, IDBConnection connection, IDBTransaction transaction);
        Task<ResultWrapper> VerifyUserPassword(Guid userId, string password, IDBConnection connection, IDBTransaction transaction);
    }
}
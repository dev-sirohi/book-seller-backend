using Application.Interfaces;
using Common;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        public async Task<ResultWrapper> GetByIdAsync(Guid id)
        {
            ResultWrapper rw = new ResultWrapper();
            return rw;
        }
        public async Task<ResultWrapper> GetByEmailAsync(string email)
        {
            ResultWrapper rw = new ResultWrapper();

            try
            {

            }
            catch (Exception ex)
            {

            }

            return rw;
        }
    }
}
using BSB.src.Application.Interfaces;
using BSB.src.Common;
using BSB.src.Common.Database.DBInterfaces;
using BSB.src.Common.DBServices;
using BSB.src.Domain.Entities;

namespace BSB.src.Application.Services
{
    public class UserService : IUserService
    {
        public async Task<ResultWrapper> GetByUserIdAsync(Guid userId, IDBConnection connection, IDBTransaction transaction)
        {
            ResultWrapper rw = new ResultWrapper();

            try
            {
                User? user = (User?)await new DBCommandFactory
                    .CommandBuilder()
                    .SetQuery(@"SELECT TOP 1 * FROM User WHERE UserId = @param_USERID")
                    .SetParams(new { userId })
                    .SetDBCommandExecuterType(Domain.Enums.Database.DBCommandExecutorTypes.READER)
                    .GetFirstOrDefault()
                    .Build()
                    .ExecuteAsync<User>(connection, transaction);

                if (user is null)
                {
                    throw new Exception("User does not exist");
                }

                rw.Success = true;
                rw.Data = user;
            }
            catch (Exception ex)
            {
                rw.Success = false;
                rw.Data = new { };
                rw.Message = ex.Message;
            }

            return rw;
        }
        public async Task<ResultWrapper> GetByEmailAsync(string email, IDBConnection connection, IDBTransaction transaction)
        {
            ResultWrapper rw = new ResultWrapper();

            try
            {
                User? user = (User?)await new DBCommandFactory
                    .CommandBuilder()
                    .SetQuery(@"SELECT TOP 1 * FROM User WHERE Email = @param_EMAIL")
                    .SetParams(new { email })
                    .SetDBCommandExecuterType(Domain.Enums.Database.DBCommandExecutorTypes.READER)
                    .GetFirstOrDefault()
                    .Build()
                    .ExecuteAsync<User>(connection, transaction);

                if (user is null)
                {
                    throw new Exception("User not found");
                }

                rw.Success = true;
                rw.Data = user;
            }
            catch (Exception ex)
            {
                rw.Success = false;
                rw.Data = new { };
                rw.Message = ex.Message;
            }

            return rw;
        }

        public async Task<ResultWrapper> VerifyUserPassword(Guid userId, string password, IDBConnection connection, IDBTransaction transaction)
        {
            ResultWrapper rw = new ResultWrapper();

            try
            {
                UserAuth? userAuth = (UserAuth?)await new DBCommandFactory
                    .CommandBuilder()
                    .SetQuery(@"SELECT TOP 1 * FROM UserAuth WHERE UserId = @param_USERID AND ExpiredFlag <> 1")
                    .SetParams(new { userId })
                    .SetDBCommandExecuterType(Domain.Enums.Database.DBCommandExecutorTypes.READER)
                    .GetFirstOrDefault()
                    .Build()
                    .ExecuteAsync<UserAuth>(connection, transaction);

                if (userAuth is null)
                {
                    throw new Exception("Could not retrieve authentication data");
                }

                if (!userAuth.Password.Equals(Utils.GenerateHash(password)))
                {
                    throw new Exception("Invalid password");
                }

                rw.Success = true;
            }
            catch (Exception ex)
            {
                rw.Success = false;
                rw.Data = new { };
                rw.Message = ex.Message;
            }

            return rw;
        }
    }
}
using BSB.src.Application.Interfaces;
using BSB.src.Common;
using BSB.src.Common.Database.DBInterfaces;
using BSB.src.Common.DBServices;
using BSB.src.Domain.Entities;

namespace BSB.src.Application.Services
{
    public class UserService : IUserService
    {
        public async Task<ResultWrapper> GetByUserIdAsync(Guid userId, IDBConnection cn, IDBTransaction tx)
        {
            ResultWrapper rw = new ResultWrapper();

            try
            {
                var user = await new DBCommandFactory
                    .CommandBuilder(new DBCommand
                    {
                        TableQuery = "TUser",
                        WhereClauseList = new List<string>() { "UserId = @param_USERID" },
                        Params = new { userId },
                        Connection = cn,
                        Transaction = tx
                    }).ExecuteAsync<User>(true);

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
        public async Task<ResultWrapper> GetByEmailAsync(string email, IDBConnection cn, IDBTransaction tx)
        {
            ResultWrapper rw = new ResultWrapper();

            try
            {
                var user = await new DBCommandFactory
                    .CommandBuilder(new DBCommand
                    {
                        TableQuery = @"TUser",
                        WhereClauseList = new List<string>() { "Email = @param_EMAIL", "IsActive = 1" },
                        Params = new { email },
                        Connection = cn,
                        Transaction = tx
                    }).ExecuteAsync<User>(true);

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

        public async Task<ResultWrapper> VerifyUserPassword(Guid userId, string password, IDBConnection cn, IDBTransaction tx)
        {
            ResultWrapper rw = new ResultWrapper();

            try
            {
                var userAuthResult = await new DBCommandFactory
                    .CommandBuilder(new DBCommand
                    {
                        TableQuery = "TUserAuth",
                        Columns = new List<string>() { "1" },
                        WhereClauseList = new List<string>() { "UserId = @param_USERID", "Password = @param_PASSWORD" },
                        Params = new { userId, password = Utils.GenerateHash(password) },
                        CommandExecType = Domain.Enums.Database.DBCommandExecutorTypes.SCALAR,
                        Connection = cn,
                        Transaction = tx
                    }).ExecuteAsync<int>(true);

                if (Convert.ToInt32(userAuthResult) == 0)
                {
                    throw new Exception("Could not retrieve authentication data");
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
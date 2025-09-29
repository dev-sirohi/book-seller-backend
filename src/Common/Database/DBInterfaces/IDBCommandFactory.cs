using Microsoft.Data.SqlClient;

namespace BSB.src.Common.Database.DBInterfaces
{
    public interface IDBCommandFactory
    {
        public IDBCommand CreateCommand(string query, SqlParameter[]? parameters, Domain.Enums.Database.DBCommandExecutorTypes dbCommandExecutorType);
    }
}

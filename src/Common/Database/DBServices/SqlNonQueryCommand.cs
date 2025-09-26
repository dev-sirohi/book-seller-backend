using BSB.src.Common.Database.DBInterfaces;
using Microsoft.Data.SqlClient;

namespace BSB.src.Common.Database.DBServices
{
    public class SqlNonQueryCommand: IDBCommand
    {
        private readonly string _query;
        private readonly SqlParameter[]? _parameters;

        public SqlNonQueryCommand(string query, SqlParameter[]? parameters = null)
        {
            _query = query;
            _parameters = parameters ?? new SqlParameter[0];
        }

        public async Task<object?> ExecuteAsync(
            IDBConnection connection,
            IDBTransaction transaction)
        {
            using (SqlCommand cmd = new SqlCommand(_query, (SqlConnection)connection.GetConnection(), (SqlTransaction)transaction.GetTransaction()))
            {
                cmd.Parameters.AddRange(_parameters);
                await cmd.ExecuteNonQueryAsync();
                return null;
            }
        }

        public Task<object?> ExecuteAsync<T>(IDBConnection dbConnection, IDBTransaction dbTransaction)
        {
            throw new NotImplementedException();
        }
    }
}

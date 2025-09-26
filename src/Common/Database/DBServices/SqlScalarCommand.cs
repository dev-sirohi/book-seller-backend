using BSB.src.Common.Database.DBInterfaces;
using Microsoft.Data.SqlClient;

namespace BSB.src.Common.Database.DBServices
{
    public class SqlScalarCommand: IDBCommand
    {
        private readonly string _query;
        private readonly SqlParameter[]? _parameters;

        public SqlScalarCommand(string query, SqlParameter[]? parameters = null)
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
                return await cmd.ExecuteScalarAsync();
            }
        }

        public async Task<object?> ExecuteAsync<T>(
            IDBConnection connection,
            IDBTransaction transaction)
        {
            using (SqlCommand cmd = new SqlCommand(_query, (SqlConnection)connection.GetConnection(), (SqlTransaction)transaction.GetTransaction()))
            {
                cmd.Parameters.AddRange(_parameters);
                return Utils.TransformTo<T>(await cmd.ExecuteScalarAsync());
            }
        }
    }
}

using BSB.src.Common.Database.DBInterfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BSB.src.Common.Database.DBServices
{
    public class SqlNonQueryCommand: IDBCommand
    {
        private readonly string _query;
        private readonly SqlParameter[]? _parameters;

        public SqlNonQueryCommand(string query, SqlParameter[]? parameters)
        {
            _query = query;
            _parameters = parameters;
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

        public Task<List<T>> ExecuteAsync<T>(IDBConnection dbConnection, IDBTransaction dbTransaction)
        {
            throw new NotImplementedException();
        }

        public Task<T?> ExecuteAsync<T>(IDBConnection dbConnection, IDBTransaction dbTransaction, bool getFirstOrDefault)
        {
            throw new NotImplementedException();
        }

        public Task<DataSet?> ExecuteAsync(IDBConnection dbConnection, IDBTransaction dbTransaction, Dictionary<string, int> dataSetCounterDict)
        {
            throw new NotImplementedException();
        }
    }
}

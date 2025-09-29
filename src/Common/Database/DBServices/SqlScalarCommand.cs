using BSB.src.Common.Database.DBInterfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BSB.src.Common.Database.DBServices
{
    public class SqlScalarCommand: IDBCommand
    {
        private readonly string _query;
        private readonly SqlParameter[]? _parameters;

        public SqlScalarCommand(string query, SqlParameter[]? parameters)
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
                return await cmd.ExecuteScalarAsync();
            }
        }

        public async Task<List<T>> ExecuteAsync<T>(
            IDBConnection connection,
            IDBTransaction transaction)
        {
            throw new NotImplementedException();
        }

        public async Task<T?> ExecuteAsync<T>(
            IDBConnection connection,
            IDBTransaction transaction,
            bool getFirstOrDefault)
        {
            using (SqlCommand cmd = new SqlCommand(_query, (SqlConnection)connection.GetConnection(), (SqlTransaction)transaction.GetTransaction()))
            {
                cmd.Parameters.AddRange(_parameters);
                return Utils.TransformTo<T>(await cmd.ExecuteScalarAsync());
            }
        }

        public Task<DataSet?> ExecuteAsync(IDBConnection dbConnection, IDBTransaction dbTransaction, Dictionary<string, int> dataSetCounterDict)
        {
            throw new NotImplementedException();
        }
    }
}

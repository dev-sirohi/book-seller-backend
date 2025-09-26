using BSB.src.Common.Database.DBInterfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BSB.src.Common.Database.DBServices
{
    public class SqlReaderCommand: IDBCommand
    {
        private readonly string _query;
        private readonly SqlParameter[]? _parameters;
        private readonly bool _getFirstOrDefault = false;

        public SqlReaderCommand(string query, SqlParameter[]? parameters = null, bool getFirstOrDefault = false)
        {
            _query = query;
            _parameters = parameters ?? new SqlParameter[0];
            _getFirstOrDefault = getFirstOrDefault;
        }

        public async Task<object?> ExecuteAsync(
            IDBConnection connection,
            IDBTransaction transaction)
        {
            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand(_query, (SqlConnection)connection.GetConnection(), (SqlTransaction)transaction.GetTransaction()))
            {
                cmd.Parameters.AddRange(_parameters);
                
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    dt.Load(reader);
                }

                if (_getFirstOrDefault)
                {
                    return dt;
                }

                return dt;
            }
        }

        public async Task<object?> ExecuteAsync<T>(
            IDBConnection connection,
            IDBTransaction transaction)
        {
            using (SqlCommand cmd = new SqlCommand(_query, (SqlConnection)connection.GetConnection(), (SqlTransaction)transaction.GetTransaction()))
            {
                cmd.Parameters.AddRange(_parameters);

                if (_getFirstOrDefault)
                {
                    return (Common.Database.DBUtils.DataTableToObjectList<T>((DataTable?)await ExecuteAsync(connection, transaction)) ?? new List<T>()).FirstOrDefault();
                }

                return Common.Database.DBUtils.DataTableToObjectList<T>((DataTable?)await ExecuteAsync(connection, transaction));
            }
        }
    }
}

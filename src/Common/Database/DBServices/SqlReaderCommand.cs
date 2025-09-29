using BSB.src.Common.Database.DBInterfaces;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace BSB.src.Common.Database.DBServices
{
    public class SqlReaderCommand: IDBCommand
    {
        private readonly string _query;
        private readonly SqlParameter[]? _parameters;

        public string Query { get { return _query; } }

        public SqlReaderCommand(string query, SqlParameter[]? parameters)
        {
            _query = query;
            _parameters = parameters;
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

                return dt;
            }
        }

        public async Task<DataSet?> ExecuteAsync(
            IDBConnection connection,
            IDBTransaction transaction,
            Dictionary<string, int> dataSetCounterDict)
        {
            DataSet ds = new DataSet();

            using (SqlCommand cmd = new SqlCommand(_query, (SqlConnection)connection.GetConnection(), (SqlTransaction)transaction.GetTransaction()))
            {
                cmd.Parameters.AddRange(_parameters);

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    int tableIndex = 0;

                    do
                    {
                        var dt = new DataTable();
                        dt.Load(reader);
                        foreach (KeyValuePair<string, int> entry in dataSetCounterDict)
                        {
                            if (entry.Value == tableIndex)
                            {
                                dt.TableName = entry.Key;
                                tableIndex++;
                                break;
                            }
                        }
                        ds.Tables.Add(dt);
                    } while (await reader.NextResultAsync());
                }

                return ds;
            }
        }

        public async Task<List<T>> ExecuteAsync<T>(
            IDBConnection connection,
            IDBTransaction transaction)
        {
            List<T>? result = Common.Database.DBUtils.DataTableToObjectList<T>((DataTable?)await ExecuteAsync(connection, transaction));

            if (result != null)
            {
                return result;
            }

            return new List<T>();
        }

        public async Task<T?> ExecuteAsync<T>(
            IDBConnection connection,
            IDBTransaction transaction,
            bool getFirstOrDefault)
        {
            List<T>? result = Common.Database.DBUtils.DataTableToObjectList<T>((DataTable?)await ExecuteAsync(connection, transaction));

            if (result == null)
            {
                result = new List<T>();
            }

            return result.FirstOrDefault();
        }
    }
}

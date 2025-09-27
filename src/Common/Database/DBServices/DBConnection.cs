using BSB.src.Common.Database.DBInterfaces;
using Microsoft.Data.SqlClient;

namespace BSB.src.Common.Database.DBServices
{
    public class DBConnection: IDBConnection
    {
        private readonly string _connectionString;
        private readonly SqlConnection _connection;
        public DBConnection(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SqlConnection(connectionString);
        }
        public async Task OpenAsync()
        {
            await _connection.OpenAsync();
        }

        public void Open()
        {
            _connection.Open();
        }

        public async Task CloseAsync()
        {
            await _connection.CloseAsync();
        }

        public IDBTransaction BeginTransaction()
        {
            return new DBTransaction(_connection.BeginTransaction());
        }

        public object GetConnection() => _connection;

        public async void Dispose()
        {
            await _connection.DisposeAsync();
        }
    }
}

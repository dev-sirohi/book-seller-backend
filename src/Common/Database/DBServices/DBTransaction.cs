using BSB.src.Common.Database.DBInterfaces;
using Microsoft.Data.SqlClient;

namespace BSB.src.Common.Database.DBServices
{
    public class DBTransaction: IDBTransaction
    {
        private readonly SqlTransaction _transaction;
        public DBTransaction(SqlTransaction transaction)
        {
            _transaction = transaction;
        }

        public async Task CommitAsync()
        {
            // check for XAdmin
            if (true)
            {
                await _transaction.CommitAsync();
            }
        }

        public object GetTransaction() => _transaction;

        public async Task RollbackAsync(bool dispose = false)
        {
            await _transaction.RollbackAsync();
            if (dispose)
            {
                Dispose();
            }
        }

        public async void Dispose()
        {
            await _transaction.DisposeAsync();
        }

        public async Task CreateSavepointAsync(string savepointName)
        {
            if (string.IsNullOrWhiteSpace(savepointName))
            {
                throw new ArgumentException("Savepoint name cannot be null or whitespace!");
            }

            var command = _transaction.Connection!.CreateCommand();
            command.Transaction = _transaction;
            command.CommandText = " SAVE TRANSACTION " + savepointName;
            await command.ExecuteNonQueryAsync();
        }

        public async Task RollbackToSavepointAsync(string savepointName)
        {
            if (string.IsNullOrWhiteSpace(savepointName))
            {
                throw new ArgumentException("Savepoint name cannot be null or whitespace!");
            }

            var command = _transaction.Connection!.CreateCommand();
            command.Transaction = _transaction;
            command.CommandText = " ROLLBACK TRANSACTION " + savepointName;
            await command.ExecuteNonQueryAsync();
        }
    }
}

using System.Data;

namespace BSB.src.Common.Database.DBInterfaces
{
    public interface IDBCommand
    {
        Task<object?> ExecuteAsync(IDBConnection dbConnection, IDBTransaction dbTransaction);
        Task<List<T>> ExecuteAsync<T>(IDBConnection dbConnection, IDBTransaction dbTransaction);
        Task<T?> ExecuteAsync<T>(IDBConnection dbConnection, IDBTransaction dbTransaction, bool getFirstOrDefault);
        Task<DataSet?> ExecuteAsync(IDBConnection dbConnection, IDBTransaction dbTransaction, Dictionary<string, int> dataSetCounterDict);
    }
}

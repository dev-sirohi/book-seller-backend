namespace BSB.src.Common.Database.DBInterfaces
{
    public interface IDBCommand
    {
        Task<object?> ExecuteAsync(IDBConnection dbConnection, IDBTransaction dbTransaction);
        Task<object?> ExecuteAsync<T>(IDBConnection dbConnection, IDBTransaction dbTransaction);
    }
}

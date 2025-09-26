namespace BSB.src.Common.Database.DBInterfaces
{
    public interface IDBTransaction: IDisposable
    {
        public Task CommitAsync();
        public Task RollbackAsync(bool dispose = false);
        public object GetTransaction();
        public Task CreateSavepointAsync(string savepointName);
        public Task RollbackToSavepointAsync(string savepointName);
    }
}

namespace BSB.src.Common.Database.DBInterfaces
{
    public interface IDBConnection: IDisposable
    {
        public Task OpenAsync();
        public void Open();
        public Task CloseAsync();
        public IDBTransaction BeginTransaction();
        public object GetConnection();
    }
}

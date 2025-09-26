using System;
using System.Data;
using BSB.src.Common;
using BSB.src.Domain;

namespace BSB.src.Common.Database.DBInterfaces
{
    public interface IDBCommandFactory
    {
        public IDBCommand CreateCommand(string query, Dictionary<string, object?>? parameters, Domain.Enums.Database.DBCommandExecutorTypes dbCommandExecutorType, bool getFirstOrDefault);
    }
}

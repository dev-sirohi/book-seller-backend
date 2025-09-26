using BSB.src.Common.Database;
using BSB.src.Common.Database.DBInterfaces;
using BSB.src.Common.Database.DBServices;

namespace BSB.src.Common.DBServices
{
    public class DBCommandFactory: IDBCommandFactory
    {
        private DBCommandFactory() { }
        public IDBCommand CreateCommand(
            string query,
            Dictionary<string, object?>? parameters,
            Domain.Enums.Database.DBCommandExecutorTypes dbCommandExecutorType,
            bool getFirstOrDefault = false)
        {
            switch (dbCommandExecutorType)
            {
                case Domain.Enums.Database.DBCommandExecutorTypes.NONQUERY:
                    return new SqlNonQueryCommand(query, DBUtils.ConvertDictToSqlParameter(parameters));
                case Domain.Enums.Database.DBCommandExecutorTypes.SCALAR:
                    return new SqlScalarCommand(query, DBUtils.ConvertDictToSqlParameter(parameters));
                case Domain.Enums.Database.DBCommandExecutorTypes.READER:
                    return new SqlReaderCommand(query, DBUtils.ConvertDictToSqlParameter(parameters));
                default:
                    return new SqlNonQueryCommand(query, DBUtils.ConvertDictToSqlParameter(parameters));
            }
        }

        public class CommandBuilder
        {
            private string _query = string.Empty;
            private Dictionary<string, object?>? _parameters;
            private Domain.Enums.Database.DBCommandExecutorTypes _dbCommandExecutorType = Domain.Enums.Database.DBCommandExecutorTypes.READER;
            private bool _getFirstOrDefault = false;

            public CommandBuilder SetQuery(string query)
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    throw new ArgumentNullException("Query cannot be empty!");
                }

                _query = query;
                return this;
            }

            public CommandBuilder SetParams(object? parameters)
            {
                if (parameters is not null)
                {
                    Dictionary<string, object?> paramDict = new Dictionary<string, object?>();

                    foreach (var prop in parameters.GetType().GetProperties())
                    {
                        string key = $"@param_{prop.Name.ToUpperInvariant}";
                        object? value = prop.GetValue(parameters, null);

                        paramDict.Add(key, value);
                    }
                }                

                return this;
            }

            public CommandBuilder SetDBCommandExecuterType(Domain.Enums.Database.DBCommandExecutorTypes dBCommandExecutorType)
            {
                _dbCommandExecutorType = dBCommandExecutorType;
                return this;
            }

            public CommandBuilder GetFirstOrDefault()
            {
                _getFirstOrDefault = true;
                return this;
            }

            public IDBCommand Build()
            {
                return new DBCommandFactory().CreateCommand(
                    _query,
                    _parameters,
                    _dbCommandExecutorType,
                    _getFirstOrDefault);
            }

            public IDBCommand Build(string query)
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    throw new ArgumentNullException("Query cannot be empty!");
                }

                return new DBCommandFactory().CreateCommand(
                    _query,
                    _parameters,
                    _dbCommandExecutorType);
            }
        }
    }
}

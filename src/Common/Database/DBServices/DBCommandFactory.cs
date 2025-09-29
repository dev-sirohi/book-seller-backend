using BSB.src.Common.Database;
using BSB.src.Common.Database.DBInterfaces;
using BSB.src.Common.Database.DBServices;
using BSB.src.Domain.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;
using static BSB.src.Domain.Enums.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BSB.src.Common.DBServices
{
    public class DBCommand
    {
        public string TableQuery { get; set; } = string.Empty;
        public object? Params { get; set; }
        public List<string>? Columns { get; set; }
        public List<string>? WhereClauseList { get; set; }
        public Domain.Enums.Database.DBCommandExecutorTypes CommandExecType { get; set; } = Domain.Enums.Database.DBCommandExecutorTypes.READER;
        public IDBConnection? Connection { get; set; }
        public IDBTransaction? Transaction { get; set; }
        public bool IsQuery { get; set; }

        public string Identifier { get; set; } = string.Empty;
    }
    public class DBCommandFactory: IDBCommandFactory
    {
        private DBCommandFactory() { }
        public IDBCommand CreateCommand(
            string query,
            SqlParameter[]? parameters,
            Domain.Enums.Database.DBCommandExecutorTypes dbCommandExecutorType)
        {
            switch (dbCommandExecutorType)
            {
                case Domain.Enums.Database.DBCommandExecutorTypes.NONQUERY:
                    return new SqlNonQueryCommand(query, parameters);
                case Domain.Enums.Database.DBCommandExecutorTypes.SCALAR:
                    return new SqlScalarCommand(query, parameters);
                case Domain.Enums.Database.DBCommandExecutorTypes.READER:
                    return new SqlReaderCommand(query, parameters);
                default:
                    return new SqlReaderCommand(query, parameters);
            }
        }

        public class CommandBuilder
        {
            private string _tableQuery = string.Empty;
            private List<string>? _whereClauseList;
            private List<string>? _columnList;
            private bool _isQuery;
            private Dictionary<string, object>? _parameters;
            private Domain.Enums.Database.DBCommandExecutorTypes _dbCommandExecutorType;
            private readonly IDBConnection? _connection;
            private readonly IDBTransaction? _transaction;
            private readonly IConfiguration _configuration = ConfigurationServices.GetConfiguration();

            public CommandBuilder(IDBConnection? connection, IDBTransaction? transaction)
            {
                if ((connection is null && transaction is not null) || (transaction is null && connection is not null))
                {
                    throw new InvalidOperationException("Either both connection/transaction should be null or neither");
                }

                this._connection = connection;
                this._transaction = transaction;
            }
            public CommandBuilder(DBCommand command)
            {
                this.SetTableQuery(command.TableQuery)
                    .SetParams(command.Params)
                    .SetColumns(command.Columns)
                    .SetWhereClause(command.WhereClauseList);
                this._dbCommandExecutorType = command.CommandExecType;

                if ((command.Connection is null && command.Transaction is not null) || (command.Transaction is null && command.Connection is not null))
                {
                    throw new InvalidOperationException("Either both connection/transaction should be null or neither");
                }

                this._connection = command.Connection;
                this._transaction = command.Transaction;
            }

            public CommandBuilder SetTableQuery(string query)
            {
                if (!string.IsNullOrWhiteSpace(query))
                {
                    _tableQuery = query;
                }
                                
                return this;
            }

            public CommandBuilder SetParams(object? parameters)
            {
                if (parameters is not null)
                {
                    if (parameters is not null)
                    {
                        foreach (var prop in parameters.GetType().GetProperties())
                        {
                            string key = $"@param_{prop.Name.ToUpperInvariant()}";
                            object? value = prop.GetValue(parameters, null);

                            if (value != null)
                            {
                                if (this._parameters is null)
                                {
                                    this._parameters = new Dictionary<string, object>();
                                }
                                this._parameters.Add(key, value);
                            }
                        }
                    }
                }

                return this;
            }

            public CommandBuilder SetColumns(List<string>? columnList)
            {
                if (columnList is not null && columnList.Count() > 0)
                {
                    _columnList = columnList;
                }

                return this;
            }

            public CommandBuilder SetWhereClause(List<string>? whereClauseList)
            {
                if (whereClauseList is not null)
                {
                    _whereClauseList = whereClauseList;
                }

                return this;
            }

            public IDBCommand Build(Domain.Enums.Database.DBCommandExecutorTypes dBCommandExecutorType, bool IsQuery)
            {
                _dbCommandExecutorType = dBCommandExecutorType;
                _isQuery = IsQuery;

                return this.Build();
            }

            private IDBCommand Build()
            {
                _columnList = _columnList?.Where(col => !string.IsNullOrWhiteSpace(col)).ToList();
                if (_columnList is null || _columnList.Count() == 0)
                {
                    _columnList = new List<string>() { "*" };
                }

                _whereClauseList = _whereClauseList?.Where(wc => !string.IsNullOrWhiteSpace(wc)).ToList();
                if (_whereClauseList is null || _whereClauseList.Count() == 0)
                {
                    _whereClauseList = new List<string>() { "1 = 1" };
                }

                if (_isQuery)
                {
                    _tableQuery = $" ({_tableQuery}) AS INNERQUERY ";
                }

                _tableQuery = $@" SELECT {string.Join(", ", _columnList)} FROM {_tableQuery} WHERE {string.Join(" AND ", _whereClauseList)} ; ";

                if (_tableQuery.Contains("@param_") && (_parameters is null || _parameters.Count() == 0))
                {
                    throw new InvalidDataException("No parameters provided in a parameterized query");
                }

                _tableQuery = SanitizeTableQuery(_tableQuery);

                return new DBCommandFactory().CreateCommand(
                    _tableQuery,
                    DBUtils.ConvertDictToSqlParameter(_parameters),
                    _dbCommandExecutorType);
            }

            public static string SanitizeTableQuery(string query)
            {
                if (string.IsNullOrEmpty(query)) return query;

                string _query = query;

                _query = Regex.Replace(_query, @"--.*?$", "", RegexOptions.Multiline);

                _query = Regex.Replace(_query, @"/\*.*?\*/", "", RegexOptions.Singleline);

                new List<string>()
                {
                    "DROP",
                    "ALTER",
                    "TRUNCATE",
                    "CREATE",
                    "BACKUP",
                    "RESTORE",
                    "SHUTDOWN",
                    "EXEC",
                    "EXECUTE",
                    "OPENDATASOURCE",
                    "OPENROWSET",
                    "BULK",
                    "xp_cmdshell"
                }.ForEach(invalidKeyword => _query.Replace(invalidKeyword, ""));

                return _query;
            }

            public async Task<object?> ExecuteAsync()
            {
                if (_connection is null)
                {
                    using (IDBConnection cn = new DBConnection(ConfigurationServices.GetConnectionString(Global.Constants.DEFAULT_CONNECTION) ?? throw new ArgumentNullException()))
                    {
                        await cn.OpenAsync();
                        using (IDBTransaction tx = cn.BeginTransaction())
                        {
                            return await this.Build(_dbCommandExecutorType, _isQuery).ExecuteAsync(cn, tx);
                        }
                    }
                }
                else
                {
                    return await this.Build(_dbCommandExecutorType, _isQuery).ExecuteAsync(_connection, _transaction);
                }
            }

            public async Task<List<T>> ExecuteAsync<T>()
            {
                if (_connection is null)
                {
                    using (IDBConnection cn = new DBConnection(ConfigurationServices.GetConnectionString(Global.Constants.DEFAULT_CONNECTION) ?? throw new ArgumentNullException()))
                    {
                        await cn.OpenAsync();
                        using (IDBTransaction tx = cn.BeginTransaction())
                        {
                            return await this.Build(_dbCommandExecutorType, _isQuery).ExecuteAsync<T>(cn, tx);
                        }
                    }
                }
                else
                {
                    return await this.Build(_dbCommandExecutorType, _isQuery).ExecuteAsync<T>(_connection, _transaction);
                }
            }

            public async Task<T?> ExecuteAsync<T>(bool firstOrDefault)
            {
                if (_connection is null)
                {
                    using (IDBConnection cn = new DBConnection(ConfigurationServices.GetConnectionString(Global.Constants.DEFAULT_CONNECTION) ?? throw new ArgumentNullException()))
                    {
                        await cn.OpenAsync();
                        using (IDBTransaction tx = cn.BeginTransaction())
                        {
                            return await this.Build(_dbCommandExecutorType, _isQuery).ExecuteAsync<T>(cn, tx, true);
                        }
                    }
                }
                else
                {
                    return await this.Build(_dbCommandExecutorType, _isQuery).ExecuteAsync<T>(_connection, _transaction, true);
                }                    
            }
        }

        public class BatchCommandBuilder
        {
            private List<DBCommand> _commandList = new List<DBCommand>();
            private readonly IDBConnection? _connection;
            private readonly IDBTransaction? _transaction;
            private readonly IConfiguration _configuration = ConfigurationServices.GetConfiguration();
            private string _finalTableQuery = string.Empty;
            private Dictionary<string, object>? _parameters;
            private readonly Domain.Enums.Database.DBCommandExecutorTypes _defaultDBCommandExecutorType = Domain.Enums.Database.DBCommandExecutorTypes.READER;
            private Dictionary<string, int>? _parametersCounter;
            private Dictionary<string, int> _dataSetCounterDict = new Dictionary<string, int>();
            private Dictionary<string, object?>? _result;
            private DataSet? _multiCommandDataSet;

            public BatchCommandBuilder(IDBConnection? connection, IDBTransaction? transaction)
            {
                this._connection = connection;
                this._transaction = transaction;
            }

            public BatchCommandBuilder(List<DBCommand> commandList, IDBConnection? connection, IDBTransaction? transaction)
            {
                if ((connection is null && transaction is not null) || (transaction is null && connection is not null))
                {
                    throw new InvalidOperationException("Either both connection/transaction should be null or neither");
                }

                this._commandList = commandList;
                this._connection = connection;
                this._transaction = transaction;
            }

            public BatchCommandBuilder AddCommand(string identifier, DBCommand command)
            {
                _commandList.Add(command);
                return this;
            }

            private void SetParams(DBCommand command)
            {
                if (command.Params is not null)
                {
                    foreach (var prop in command.Params .GetType().GetProperties())
                    {
                        string key = $"@param_{prop.Name.ToUpperInvariant()}";
                        object? value = prop.GetValue(command.Params , null);

                        if (value != null)
                        {
                            if (this._parameters is null)
                            {
                                this._parameters = new Dictionary<string, object>();
                            }
                            
                            if (!_parameters.ContainsKey(key))
                            {
                                this._parameters.Add(key, value);
                            }
                            else
                            {
                                if (!object.Equals(_parameters[key], value))
                                {
                                    if (_parametersCounter is not null && _parametersCounter.ContainsKey(key))
                                    {
                                        _parametersCounter[key]++;
                                    }
                                    else
                                    {
                                        if (_parametersCounter is null)
                                        {
                                            _parametersCounter = new Dictionary<string, int>();
                                        }
                                        _parametersCounter.Add(key, 0);
                                    }

                                    string newKey = key + "_" + _parametersCounter[key];
                                    command.TableQuery.Replace(key, newKey);
                                    _parameters.Add(newKey, value);
                                }
                            }
                        }
                    }
                }
            }

            private IDBCommand Build()
            {
                int dataSetCounter = 0;
                _commandList.ForEach(command =>
                {
                    command.Connection = null;
                    command.Transaction = null;

                    if (string.IsNullOrWhiteSpace(command.Identifier))
                    {
                        throw new InvalidOperationException("Identifier not provided for multi-command query");
                    }

                    if (command.CommandExecType != _defaultDBCommandExecutorType)
                    {
                        throw new InvalidOperationException("Only READER commands are accepted in multi-command builder");
                    }

                    if (!command.IsQuery)
                    {
                        throw new InvalidOperationException("Only queries are accepted in multi-command builder");
                    }

                    if (command.Params is not null)
                    {
                        this.SetParams(command);
                    }

                    CommandBuilder cd = new CommandBuilder(command);
                    _finalTableQuery += ((SqlReaderCommand)cd.Build(_defaultDBCommandExecutorType, true)).Query;

                    dataSetCounter++;

                    if (_dataSetCounterDict.ContainsKey(command.Identifier))
                    {
                        throw new InvalidOperationException("Duplicate key found");
                    }

                    _dataSetCounterDict.Add(command.Identifier, dataSetCounter);
                });

                return new DBCommandFactory().CreateCommand(
                    _finalTableQuery,
                    DBUtils.ConvertDictToSqlParameter(_parameters),
                    _defaultDBCommandExecutorType);
            }

            public async Task<BatchCommandBuilder> ExecuteAsync()
            {
                DataSet? result = new DataSet();

                using (IDBConnection cn = _connection ?? new DBConnection(ConfigurationServices.GetConnectionString(Global.Constants.DEFAULT_CONNECTION) ?? throw new ArgumentNullException()))
                {
                    await cn.OpenAsync();
                    using (IDBTransaction tx = _transaction ?? cn.BeginTransaction())
                    {
                        result = await this.Build().ExecuteAsync(cn, tx, _dataSetCounterDict);
                    }
                }

                this._multiCommandDataSet = result;

                return this;
            }

            public object? GetData(string commandId)
            {
                return Common.Database.DBUtils.DataTableToObjectList(_multiCommandDataSet?.Tables[commandId]);
            }

            public List<T> GetData<T>(string commandId)
            {
                List<T>? result = Common.Database.DBUtils.DataTableToObjectList<T>(_multiCommandDataSet?.Tables[commandId]);

                if (result != null)
                {
                    return result;
                }

                return new List<T>();
            }
        }
    }
}

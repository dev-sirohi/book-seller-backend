using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Dynamic;

namespace BSB.src.Common.Database
{
    public class DBUtils
    {
        public static SqlParameter[] ConvertDictToSqlParameter(Dictionary<string, object?>? dict)
        {
            if (dict == null || dict.Count == 0)
            {
                return Array.Empty<SqlParameter>();
            }

            return dict.Select(x => new SqlParameter(x.Key, Common.Database.DBUtils.DBNullToNull(x.Value))).ToArray();
        }

        public static object? DBNullToNull(object? obj)
        {
            if (obj is DBNull)
            {
                return null;
            }

            return obj;
        }

        public static object? NullToDBNull(object? obj)
        {
            if (obj is null)
            {
                return DBNull.Value;
            }

            return obj;
        }

        public static List<dynamic> DataTableToObjectList(DataTable? table)
        {
            var result = new List<dynamic>();

            if (table is not null)
            {
                foreach (DataRow row in table.Rows)
                {
                    dynamic obj = new ExpandoObject();
                    foreach (DataColumn col in table.Columns)
                    {
                        obj[col.ColumnName] = Common.Database.DBUtils.DBNullToNull(row[col]);
                    }

                    result.Add(obj);
                }
            }

            return result;
        }

        public static List<T>? DataTableToObjectList<T>(DataTable? table)
        {
            return Common.Utils.TransformTo<List<T>>(DataTableToObjectList(table));
        }
    }
}

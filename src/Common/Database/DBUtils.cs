using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Dynamic;

namespace BSB.src.Common.Database
{
    public class DBUtils
    {
        public static SqlParameter[] ConvertDictToSqlParameter(Dictionary<string, object>? dict)
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
            if (table is null) return result;

            var columns = table.Columns.Cast<DataColumn>()
                               .Select((c, i) => (Name: c.ColumnName, Index: i))
                               .ToArray();

            foreach (DataRow row in table.Rows)
            {
                var exp = new ExpandoObject();
                var dict = (IDictionary<string, object?>)exp;
                var items = row.ItemArray;

                for (int i = 0; i < columns.Length; i++)
                    dict[columns[i].Name] = Common.Database.DBUtils.DBNullToNull(items[columns[i].Index]);

                result.Add(exp);
            }

            return result;
        }


        public static List<T>? DataTableToObjectList<T>(DataTable? table)
        {
            return Common.Utils.TransformTo<List<T>>(DataTableToObjectList(table));
        }
    }
}

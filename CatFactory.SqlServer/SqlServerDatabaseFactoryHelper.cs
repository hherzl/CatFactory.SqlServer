using System.Collections.Generic;
using System.Data.Common;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// 
    /// </summary>
    public static class SqlServerDatabaseFactoryHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetNames(DbDataReader dataReader)
        {
            if (dataReader.HasRows)
            {
                for (var i = 0; i < dataReader.FieldCount; i++)
                    yield return dataReader.GetName(i);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static Column GetColumn(IDictionary<string, object> dictionary)
        {
            var column = new Column
            {
                Name = string.Concat(dictionary["Column_name"])
            };

            column.Type = string.Concat(dictionary["Type"]);
            column.Length = int.Parse(string.Concat(dictionary["Length"]));
            column.Prec = string.Concat(dictionary["Prec"]).Trim().Length == 0 ? default(short) : short.Parse(string.Concat(dictionary["Prec"]));
            column.Scale = string.Concat(dictionary["Scale"]).Trim().Length == 0 ? default(short) : short.Parse(string.Concat(dictionary["Scale"]));
            column.Nullable = string.Compare(string.Concat(dictionary["Nullable"]), "yes", true) == 0 ? true : false;
            column.Collation = string.Concat(dictionary["Collation"]);

            return column;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static Parameter GetParameter(IDictionary<string, object> dictionary)
        {
            var parameter = new Parameter
            {
                Name = string.Concat(dictionary["Parameter_name"])
            };

            parameter.Type = string.Concat(dictionary["Type"]);
            parameter.Length = short.Parse(string.Concat(dictionary["Length"]));
            parameter.Prec = string.Concat(dictionary["Prec"]).Trim().Length == 0 ? default(int) : int.Parse(string.Concat(dictionary["Prec"]));
            parameter.ParamOrder = string.Concat(dictionary["Param_order"]).Trim().Length == 0 ? default(int) : int.Parse(string.Concat(dictionary["Param_order"]));
            parameter.Collation = string.Concat(dictionary["Collation"]);

            return parameter;
        }
    }
}

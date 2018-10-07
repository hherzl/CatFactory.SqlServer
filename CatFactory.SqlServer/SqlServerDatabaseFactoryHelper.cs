using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
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
        /// <param name="database"></param>
        /// <param name="connection"></param>
        public static void AddUserDefinedDataTypes(Database database, DbConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.Connection = connection;
                command.CommandText = " select name, system_type_id, user_type_id, collation_name, is_nullable, is_user_defined from sys.types ";

                var types = new[]
                {
                    new
                    {
                        Name = string.Empty,
                        SystemTypeId = default(byte),
                        UserTypeId = 0,
                        CollationName = string.Empty,
                        IsNullable = false,
                        IsUserDefined = false
                    }
                }.ToList();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        types.Add(new
                        {
                            Name = reader.GetString(0),
                            SystemTypeId = reader.GetByte(1),
                            UserTypeId = reader.GetInt32(2),
                            CollationName = reader[3] is DBNull ? null : reader.GetString(3),
                            IsNullable = reader.GetBoolean(4),
                            IsUserDefined = reader.GetBoolean(5)
                        });
                    }
                }

                foreach (var type in types)
                {
                    if (type.IsUserDefined)
                    {
                        var parent = types.FirstOrDefault(item => !item.IsUserDefined && item.SystemTypeId == type.SystemTypeId);

                        if (parent != null)
                            database.DatabaseTypeMaps.Add(new DatabaseTypeMap
                            {
                                DatabaseType = type.Name,
                                Collation = type.CollationName,
                                IsUserDefined = type.IsUserDefined,
                                ParentDatabaseType = parent.Name
                            });
                    }
                }
            }
        }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static Index GetIndex(IDictionary<string, object> dictionary)
            => new Index
            {
                IndexName = string.Concat(dictionary["index_name"]),
                IndexDescription = string.Concat(dictionary["index_description"]),
                IndexKeys = string.Concat(dictionary["index_keys"])
            };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static ConstraintDetail GetConstraintDetail(IDictionary<string, object> dictionary)
            => new ConstraintDetail
            {
                ConstraintType = string.Concat(dictionary["constraint_type"]),
                ConstraintName = string.Concat(dictionary["constraint_name"]),
                DeleteAction = string.Concat(dictionary["delete_action"]),
                UpdateAction = string.Concat(dictionary["update_action"]),
                StatusEnabled = string.Concat(dictionary["status_enabled"]),
                StatusForReplication = string.Concat(dictionary["status_for_replication"]),
                ConstraintKeys = string.Concat(dictionary["constraint_keys"])
            };
    }
}

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// Contains helper methods to import feature
    /// </summary>
    public static class SqlServerDatabaseFactoryHelper
    {
        /// <summary>
        /// Add user defined data types for database
        /// </summary>
        /// <param name="database">Instance of <see cref="Database"/> class</param>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
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

                using (var dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        types.Add(new
                        {
                            Name = dataReader.GetString(0),
                            SystemTypeId = dataReader.GetByte(1),
                            UserTypeId = dataReader.GetInt32(2),
                            CollationName = dataReader[3] is DBNull ? null : dataReader.GetString(3),
                            IsNullable = dataReader.GetBoolean(4),
                            IsUserDefined = dataReader.GetBoolean(5)
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
        /// Gets the column names from data reader
        /// </summary>
        /// <param name="dataReader">Instance of <see cref="DbDataReader"/> class</param>
        /// <returns>A sequence of <see cref="string"/> that contains column names</returns>
        public static IEnumerable<string> GetNames(DbDataReader dataReader)
        {
            if (dataReader.HasRows)
            {
                for (var i = 0; i < dataReader.FieldCount; i++)
                    yield return dataReader.GetName(i);
            }
        }

        /// <summary>
        /// Gets a column from row dictionary
        /// </summary>
        /// <param name="dictionary">Dictionary from data reader</param>
        /// <returns>An instance of <see cref="Column"/> class</returns>
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
        /// Gets a parameter from row dictionary
        /// </summary>
        /// <param name="dictionary">Dictionary from data reader</param>
        /// <returns>An instance of <see cref="Parameter"/> class</returns>
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
        /// Gets an index from row dictionary
        /// </summary>
        /// <param name="dictionary">Dictionary from data reader</param>
        /// <returns>An instance of <see cref="Index"/> class</returns>
        public static Index GetIndex(IDictionary<string, object> dictionary)
            => new Index
            {
                IndexName = string.Concat(dictionary["index_name"]),
                IndexDescription = string.Concat(dictionary["index_description"]),
                IndexKeys = string.Concat(dictionary["index_keys"])
            };

        /// <summary>
        /// Gets a constraint detail from row dictionary
        /// </summary>
        /// <param name="dictionary">Dictionary from data reader</param>
        /// <returns>An instance of <see cref="ConstraintDetail"/> class</returns>
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

        /// <summary>
        /// Gets the first result sets for stored procedure
        /// </summary>
        /// <param name="storedProcedure">Instance of <see cref="StoredProcedure"/> class</param>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <returns></returns>
        public static IEnumerable<FirstResultSetForObject> GetFirstResultSetForObject(StoredProcedure storedProcedure, DbConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.Connection = connection;
                command.CommandText = string.Format(" select[column_ordinal], [name], [is_nullable], [system_type_name] from [sys].[dm_exec_describe_first_result_set_for_object] (object_id('{0}'), null) ", storedProcedure.FullName);

                using (var dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        yield return new FirstResultSetForObject
                        {
                            ColumnOrdinal = dataReader.GetInt32(0),
                            Name = dataReader.GetString(1),
                            IsNullable = dataReader.GetBoolean(2),
                            SystemTypeName = dataReader.GetString(3)
                        };
                    }
                }
            }
        }
    }
}

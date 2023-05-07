using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using CatFactory.ObjectRelationalMapping;
using CatFactory.ObjectRelationalMapping.Programmability;
using CatFactory.SqlServer.DatabaseObjectModel;
using CatFactory.SqlServer.DatabaseObjectModel.Queries;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// Contains helper methods for import feature
    /// </summary>
    public static class SqlServerDatabaseFactoryHelper
    {
        /// <summary>
        /// Adds user defined data types for database
        /// </summary>
        /// <param name="database">Instance of <see cref="Database"/> class</param>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        public static async Task AddUserDefinedDataTypesAsync(Database database, DbConnection connection)
        {
            var sysTypes = await connection.GetSysTypesAsync();

            foreach (var type in sysTypes)
            {
                if (type.IsUserDefined == false)
                    continue;

                var parent = sysTypes
                    .FirstOrDefault(item => item.IsUserDefined == false && item.SystemTypeId == type.SystemTypeId);

                if (parent == null)
                    continue;

                database.DatabaseTypeMaps.Add(new DatabaseTypeMap
                {
                    DatabaseType = type.Name,
                    IsUserDefined = (bool)type.IsUserDefined,
                    ParentDatabaseType = parent.Name,
                    Collation = type.CollationName,
                });
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
                {
                    yield return dataReader.GetName(i);
                }
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
                Name = string.Concat(dictionary["Column_name"]),
                Type = string.Concat(dictionary["Type"]),
                Computed = string.Concat(dictionary["Computed"]),
                Length = int.Parse(string.Concat(dictionary["Length"]))
            };

            column.Prec = string.Concat(dictionary["Prec"]).Trim().Length == 0 ? default : short.Parse(string.Concat(dictionary["Prec"]));
            column.Scale = string.Concat(dictionary["Scale"]).Trim().Length == 0 ? default : short.Parse(string.Concat(dictionary["Scale"]));
            column.Nullable = string.Compare(string.Concat(dictionary["Nullable"]), "yes", true) == 0 ? true : false;
            column.Collation = string.Concat(dictionary["Collation"]);

            column.ImportBag.TrimTrailingBlanks = string.Concat(dictionary["TrimTrailingBlanks"]);
            column.ImportBag.FixedLenNullInSource = string.Concat(dictionary["FixedLenNullInSource"]);

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
                Name = string.Concat(dictionary["Parameter_name"]),
                Type = string.Concat(dictionary["Type"]),
                Length = short.Parse(string.Concat(dictionary["Length"]))
            };

            parameter.Prec = string.Concat(dictionary["Prec"]).Trim().Length == 0 ? default : int.Parse(string.Concat(dictionary["Prec"]));
            parameter.Order = string.Concat(dictionary["Param_order"]).Trim().Length == 0 ? default : int.Parse(string.Concat(dictionary["Param_order"]));
            parameter.Collation = string.Concat(dictionary["Collation"]);

            return parameter;
        }

        /// <summary>
        /// Gets an index from row dictionary
        /// </summary>
        /// <param name="dictionary">Dictionary from data reader</param>
        /// <returns>An instance of <see cref="Index"/> class</returns>
        public static Index GetIndex(IDictionary<string, object> dictionary)
            => new()
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
            => new()
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
        /// <returns>A sequence of <see cref="FirstResultSetForObject"/> class</returns>
        public static async Task<ICollection<FirstResultSetForObject>> GetFirstResultSetForObjectAsync(StoredProcedure storedProcedure, DbConnection connection)
        {
            var collection = new Collection<FirstResultSetForObject>();

            foreach (var item in await connection.DmExecDescribeFirstResultSetForObjectAsync(storedProcedure.FullName))
            {
                collection.Add(new FirstResultSetForObject
                {
                    ColumnOrdinal = item.ColumnOrdinal,
                    Name = item.Name,
                    IsNullable = item.IsNullable,
                    SystemTypeName = item.SystemTypeName
                });
            }

            return collection;
        }

        /// <summary>
        /// Gets the result sets for stored procedure
        /// </summary>
        /// <param name="storedProcedure">Instance of <see cref="StoredProcedure"/> class</param>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <returns>A sequence of <see cref="ResultSet"/> class</returns>
        public static async Task<ICollection<ResultSet>> GetResultSetsAsync(StoredProcedure storedProcedure, DbConnection connection)
        {
            var collection = new Collection<ResultSet>();

            foreach (var item in await connection.DmExecDescribeFirstResultSetForObjectAsync(storedProcedure.FullName))
            {
                collection.Add(new ResultSet
                {
                    Name = item.Name,
                    Nullable = item.IsNullable,
                    Length = item.MaxLength,
                    Prec = item.Precision,
                    Scale = item.Scale,
                    Collation = item.CollationName,
                    Type = item.SystemTypeName
                });
            }

            return collection;
        }
    }
}

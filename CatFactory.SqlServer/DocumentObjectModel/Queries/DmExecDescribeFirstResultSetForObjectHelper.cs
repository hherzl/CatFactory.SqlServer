using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace CatFactory.SqlServer.DocumentObjectModel.Queries
{
    /// <summary>
    /// 
    /// </summary>
    public static class DmExecDescribeFirstResultSetForObjectHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="objectId"></param>
        /// <param name="browseInformationMode"></param>
        /// <returns></returns>
        public static IEnumerable<DmExecDescribeFirstResultSetForObject> DmExecDescribeFirstResultSetForObject(this DbConnection connection, string objectId, byte? browseInformationMode = null)
        {
            using (var command = connection.CreateCommand())
            {
                var cmdText = new StringBuilder();

                cmdText.Append(" select");
                cmdText.Append("  [column_ordinal] ColumnOrdinal, ");
                cmdText.Append("  [name] Name, ");
                cmdText.Append("  [is_nullable] IsNullable, ");
                cmdText.Append("  [system_type_name] SystemTypeName");
                cmdText.Append(" from ");
                cmdText.Append("  [sys].[dm_exec_describe_first_result_set_for_object] ");
                cmdText.AppendFormat("  (object_id('{0}'), {1}) ", objectId, browseInformationMode.HasValue ? browseInformationMode.Value.ToString() : "null");

                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = cmdText.ToString();

                using (var dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        yield return new DmExecDescribeFirstResultSetForObject
                        {
                            ColumnOrdinal = dataReader.GetInt32(0),
                            Name = dataReader[1] is DBNull ? string.Empty : dataReader.GetString(1),
                            IsNullable = dataReader[2] is DBNull ? false : dataReader.GetBoolean(2),
                            SystemTypeName = dataReader[3] is DBNull ? string.Empty : dataReader.GetString(3)
                        };
                    }
                }
            }
        }
    }
}

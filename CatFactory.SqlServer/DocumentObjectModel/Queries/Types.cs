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
    public static class Types
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static IEnumerable<SysType> GetSysTypes(this DbConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                var cmdText = new StringBuilder();

                cmdText.Append(" select ");
                cmdText.Append("  [name] Name, ");
                cmdText.Append("  [system_type_id] SystemTypeId, ");
                cmdText.Append("  [user_type_id] UserTypeId, ");
                cmdText.Append("  [schema_id] SchemaId, ");
                cmdText.Append("  [principal_id] PrincipalId, ");
                cmdText.Append("  [max_length] MaxLength, ");
                cmdText.Append("  [precision] Precision, ");
                cmdText.Append("  [scale] Scale, ");
                cmdText.Append("  [collation_name] CollationName, ");
                cmdText.Append("  [is_nullable] IsNullable, ");
                cmdText.Append("  [is_user_defined] IsUserDefined, ");
                cmdText.Append("  [is_assembly_type] IsAssemblyType, ");
                cmdText.Append("  [default_object_id] DefaultObjectId, ");
                cmdText.Append("  [rule_object_id] RuleObjectId, ");
                cmdText.Append("  [is_table_type] IsTableType ");
                cmdText.Append(" from ");
                cmdText.Append("  [sys].[types] ");

                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = cmdText.ToString();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return new SysType
                        {
                            Name = reader.GetString(0),
                            SystemTypeId = reader.GetByte(1),
                            UserTypeId = reader.GetInt32(2),
                            SchemaId = reader.GetInt32(3),
                            PrincipalId = reader[4] is DBNull ? default(int?) : reader.GetInt32(4),
                            MaxLength = reader.GetInt16(5),
                            Precision = reader.GetByte(6),
                            Scale = reader.GetByte(7),
                            CollationName = reader[8] is DBNull ? string.Empty : reader.GetString(8),
                            IsNullable = reader.GetBoolean(9),
                            IsUserDefined = reader.GetBoolean(10),
                            IsAssemblyType = reader.GetBoolean(11),
                            DefaultObjectId = reader.GetInt32(12),
                            RuleObjectId = reader.GetInt32(13),
                            IsTableType = reader.GetBoolean(14)
                        };
                    }
                }
            }
        }
    }
}

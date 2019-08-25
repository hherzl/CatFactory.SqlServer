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
    public static class SysQueries
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static IEnumerable<SysSchema> GetSysSchemas(this DbConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                var cmdText = new StringBuilder();

                cmdText.Append(" select ");
                cmdText.Append("  [name] Name, ");
                cmdText.Append("  [principal_id] PrincipalId, ");
                cmdText.Append("  [schema_id] SchemaId ");
                cmdText.Append(" from ");
                cmdText.Append("  [sys].[schemas] ");

                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = cmdText.ToString();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return new SysSchema
                        {
                            Name = reader.GetString(0),
                            PrincipalId = reader.GetInt32(1),
                            SchemaId = reader.GetInt32(2)
                        };
                    }
                }
            }
        }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static IEnumerable<SysSequence> GetSysSequences(this DbConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                var cmdText = new StringBuilder();

                cmdText.Append(" select ");
                cmdText.Append("  [name] Name, ");
                cmdText.Append("  [object_id] ObjectId, ");
                cmdText.Append("  [principal_id] PrincipalId, ");
                cmdText.Append("  [schema_id] SchemaId, ");
                cmdText.Append("  [parent_object_id] ParentObjectId, ");
                cmdText.Append("  [type] Type, ");
                cmdText.Append("  [type_desc] TypeDesc, ");
                cmdText.Append("  [create_date] CreateDate, ");
                cmdText.Append("  [modify_date] ModifyDate, ");
                cmdText.Append("  [is_ms_shipped] IsMsShipped, ");
                cmdText.Append("  [is_published] IsPublished, ");
                cmdText.Append("  [is_schema_published] IsSchemaPublished, ");
                cmdText.Append("  [start_value] StartValue, ");
                cmdText.Append("  [increment] Increment, ");
                cmdText.Append("  [minimum_value] MinimumValue, ");
                cmdText.Append("  [maximum_value] MaximumValue, ");
                cmdText.Append("  [is_cycling] IsCycling, ");
                cmdText.Append("  [is_cached] IsCached, ");
                cmdText.Append("  [cache_size] CacheSize, ");
                cmdText.Append("  [system_type_id] SystemTypeId, ");
                cmdText.Append("  [user_type_id] UserTypeId, ");
                cmdText.Append("  [precision] Precision, ");
                cmdText.Append("  [scale] Scale, ");
                cmdText.Append("  [current_value] CurrentValue, ");
                cmdText.Append("  [is_exhausted] IsExhausted, ");
                cmdText.Append("  [last_used_value] LastUsedValue ");
                cmdText.Append(" from ");
                cmdText.Append("  [sys].[sequences] ");

                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = cmdText.ToString();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return new SysSequence
                        {
                            Name = reader.GetString(0),
                            ObjectId = reader.GetInt32(1),
                            PrincipalId = reader[2] is DBNull ? default(int?) : reader.GetInt32(2),
                            SchemaId = reader.GetInt32(3),
                            ParentObjectId = reader.GetInt32(4),
                            Type = reader.GetString(5),
                            TypeDesc = reader.GetString(6),
                            CreateDate = reader.GetDateTime(7),
                            ModifyDate = reader.GetDateTime(8),
                            IsMsShipped = reader.GetBoolean(9),
                            IsPublished = reader.GetBoolean(10),
                            IsSchemaPublished = reader.GetBoolean(11),
                            StartValue = reader[12],
                            Increment = reader[13],
                            MinimumValue = reader[14],
                            MaximumValue = reader[15],
                            IsCycling = reader.GetBoolean(16),
                            IsCached = reader.GetBoolean(17),
                            CacheSize = reader[18] is DBNull ? default(int?) : reader.GetInt32(18),
                            SystemTypeId = reader.GetByte(19),
                            UserTypeId = reader.GetInt32(20),
                            Precision = reader.GetByte(21),
                            Scale = reader.GetByte(22),
                            CurrentValue = reader[23],
                            IsExhausted = reader.GetBoolean(24),
                            LastUsedValue = reader[25] is DBNull ? default(int?) : reader.GetInt32(25),
                        };
                    }
                }
            }
        }
    }
}

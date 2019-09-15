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
        public static IEnumerable<SysTable> GetSysTables(this DbConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                // Create string builder for query
                var cmdText = new StringBuilder();

                // Create sql statement
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
                cmdText.Append("  [lob_data_space_id] LobDataSpaceId, ");
                cmdText.Append("  [filestream_data_space_id] FilestreamDataSpaceId, ");
                cmdText.Append("  [max_column_id_used] MaxColumnIdUsed, ");
                cmdText.Append("  [lock_on_bulk_load] LockOnBulkLoad, ");
                cmdText.Append("  [uses_ansi_nulls] UsesAnsiNulls, ");
                cmdText.Append("  [is_replicated] IsReplicated, ");
                cmdText.Append("  [has_replication_filter] HasReplicationFilter, ");
                cmdText.Append("  [is_merge_published] IsMergePublished, ");
                cmdText.Append("  [is_sync_tran_subscribed] IsSyncTranSubscribed, ");
                cmdText.Append("  [has_unchecked_assembly_data] HasUncheckedAssemblyData, ");
                cmdText.Append("  [text_in_row_limit] TextInRowLimit, ");
                cmdText.Append("  [large_value_types_out_of_row] LargeValueTypesOutOfRow, ");
                cmdText.Append("  [is_tracked_by_cdc] IsTrackedByCdc, ");
                cmdText.Append("  [lock_escalation] LockEscalation, ");
                cmdText.Append("  [lock_escalation_desc] LockEscalationDesc, ");
                cmdText.Append("  [is_filetable] IsFiletable, ");
                cmdText.Append("  [is_memory_optimized] IsMemoryOptimized, ");
                cmdText.Append("  [durability] Durability, ");
                cmdText.Append("  [durability_desc] DurabilityDesc, ");
                cmdText.Append("  [temporal_type] TemporalType, ");
                cmdText.Append("  [temporal_type_desc] TemporalTypeDesc, ");
                cmdText.Append("  [history_table_id] HistoryTableId, ");
                cmdText.Append("  [is_remote_data_archive_enabled] IsRemoteDataArchiveEnabled, ");
                cmdText.Append("  [is_external] IsExternal, ");
                cmdText.Append("  [history_retention_period] HistoryRetentionPeriod, ");
                cmdText.Append("  [history_retention_period_unit] HistoryRetentionPeriodUnit, ");
                cmdText.Append("  [history_retention_period_unit_desc] HistoryRetentionPeriodUnitDesc, ");
                cmdText.Append("  [is_node] IsNode, ");
                cmdText.Append("  [is_edge] IsEdge ");
                cmdText.Append(" from ");
                cmdText.Append("  [sys].[tables] ");

                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = cmdText.ToString();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return new SysTable
                        {
                            Name = reader.GetString(0),
                            ObjectId = reader.GetInt32(1),
                            PrincipalId = reader[2] is DBNull ? 0 : reader.GetInt32(2),
                            SchemaId = reader.GetInt32(3),
                            ParentObjectId = reader.GetInt32(4),
                            Type = reader.GetString(5),
                            TypeDesc = reader.GetString(6),
                            CreateDate = reader.GetDateTime(7),
                            ModifyDate = reader.GetDateTime(8),
                            IsMsShipped = reader.GetBoolean(9),
                            IsPublished = reader.GetBoolean(10),
                            IsSchemaPublished = reader.GetBoolean(11),
                            LobDataSpaceId = reader.GetInt32(12),
                            FilestreamDataSpaceId = reader[13] is DBNull ? 0 : reader.GetInt32(13),
                            MaxColumnIdUsed = reader[14] is DBNull ? 0 : reader.GetInt32(14),
                            LockOnBulkLoad = reader.GetBoolean(15),
                            UsesAnsiNulls = reader.GetBoolean(16),
                            IsReplicated = reader.GetBoolean(17),
                            HasReplicationFilter = reader.GetBoolean(18),
                            IsMergePublished = reader.GetBoolean(19),
                            IsSyncTranSubscribed = reader.GetBoolean(20),
                            HasUncheckedAssemblyData = reader.GetBoolean(21),
                            TextInRowLimit = reader.GetInt32(22),
                            LargeValueTypesOutOfRow = reader.GetBoolean(23),
                            IsTrackedByCdc = reader.GetBoolean(24),
                            LockEscalation = reader.GetByte(25),
                            LockEscalationDesc = reader.GetString(26),
                            IsFiletable = reader.GetBoolean(27),
                            IsMemoryOptimized = reader.GetBoolean(28),
                            Durability = reader.GetByte(29),
                            DurabilityDesc = reader.GetString(30),
                            TemporalType = reader.GetByte(31),
                            TemporalTypeDesc = reader.GetString(32),
                            HistoryTableId = reader[33] is DBNull ? 0 : reader.GetInt32(33),
                            IsRemoteDataArchiveEnabled = reader.GetBoolean(34),
                            IsExternal = reader.GetBoolean(35),
                            HistoryRetentionPeriod = reader[36] is DBNull ? 0 : reader.GetInt32(36),
                            HistoryRetentionPeriodUnit = reader[37] is DBNull ? 0 : reader.GetInt32(37),
                            HistoryRetentionPeriodUnitDesc = reader[38] is DBNull ? string.Empty : reader.GetString(38),
                            IsNode = reader.GetBoolean(39),
                            IsEdge = reader.GetBoolean(40)
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
        public static IEnumerable<SysView> GetSysViews(this DbConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                // Create string builder for query
                var cmdText = new StringBuilder();

                // Create sql statement
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
                cmdText.Append("  [is_replicated] IsReplicated, ");
                cmdText.Append("  [has_replication_filter] HasReplicationFilter, ");
                cmdText.Append("  [has_opaque_metadata] HasOpaqueMetadata, ");
                cmdText.Append("  [has_unchecked_assembly_data] HasUncheckedAssemblyData, ");
                cmdText.Append("  [with_check_option] WithCheckOption, ");
                cmdText.Append("  [is_date_correlation_view] IsDateCorrelationView, ");
                cmdText.Append("  [is_tracked_by_cdc] IsTrackedByCdc ");
                cmdText.Append(" from ");
                cmdText.Append("  [sys].[views] ");

                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = cmdText.ToString();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return new SysView
                        {
                            Name = reader.GetString(0),
                            ObjectId = reader.GetInt32(1),
                            PrincipalId = reader[2] is DBNull ? 0 : reader.GetInt32(2),
                            SchemaId = reader.GetInt32(3),
                            ParentObjectId = reader.GetInt32(4),
                            Type = reader[5] is DBNull ? string.Empty : reader.GetString(5),
                            TypeDesc = reader[6] is DBNull ? string.Empty : reader.GetString(6),
                            CreateDate = reader.GetDateTime(7),
                            ModifyDate = reader.GetDateTime(8),
                            IsMsShipped = reader.GetBoolean(9),
                            IsPublished = reader.GetBoolean(10),
                            IsSchemaPublished = reader.GetBoolean(11),
                            IsReplicated = reader[12] is DBNull ? false : reader.GetBoolean(12),
                            HasReplicationFilter = reader[13] is DBNull ? false : reader.GetBoolean(13),
                            HasOpaqueMetadata = reader.GetBoolean(14),
                            HasUncheckedAssemblyData = reader.GetBoolean(15),
                            WithCheckOption = reader.GetBoolean(16),
                            IsDateCorrelationView = reader.GetBoolean(17),
                            IsTrackedByCdc = reader[18] is DBNull ? false : reader.GetBoolean(18),
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
        public static IEnumerable<SysColumn> GetSysColumns(this DbConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                // Create string builder for query
                var cmdText = new StringBuilder();

                // Create sql statement
                cmdText.Append(" select ");
                cmdText.Append("  [object_id] ObjectId, ");
                cmdText.Append("  [name] Name, ");
                cmdText.Append("  [column_id] ColumnId, ");
                cmdText.Append("  [system_type_id] SystemTypeId, ");
                cmdText.Append("  [user_type_id] UserTypeId, ");
                cmdText.Append("  [max_length] MaxLength, ");
                cmdText.Append("  [precision] Precision, ");
                cmdText.Append("  [scale] Scale, ");
                cmdText.Append("  [collation_name] CollationName, ");
                cmdText.Append("  [is_nullable] IsNullable, ");
                cmdText.Append("  [is_ansi_padded] IsAnsiPadded, ");
                cmdText.Append("  [is_rowguidcol] IsRowguidcol, ");
                cmdText.Append("  [is_identity] IsIdentity, ");
                cmdText.Append("  [is_computed] IsComputed, ");
                cmdText.Append("  [is_filestream] IsFilestream, ");
                cmdText.Append("  [is_replicated] IsReplicated, ");
                cmdText.Append("  [is_non_sql_subscribed] IsNonSqlSubscribed, ");
                cmdText.Append("  [is_merge_published] IsMergePublished, ");
                cmdText.Append("  [is_dts_replicated] IsDtsReplicated, ");
                cmdText.Append("  [is_xml_document] IsXmlDocument, ");
                cmdText.Append("  [xml_collection_id] XmlCollectionId, ");
                cmdText.Append("  [default_object_id] DefaultObjectId, ");
                cmdText.Append("  [rule_object_id] RuleObjectId, ");
                cmdText.Append("  [is_sparse] IsSparse, ");
                cmdText.Append("  [is_column_set] IsColumnSet, ");
                cmdText.Append("  [generated_always_type] GeneratedAlwaysType, ");
                cmdText.Append("  [generated_always_type_desc] GeneratedAlwaysTypeDesc, ");
                cmdText.Append("  [encryption_type] EncryptionType, ");
                cmdText.Append("  [encryption_type_desc] EncryptionTypeDesc, ");
                cmdText.Append("  [encryption_algorithm_name] EncryptionAlgorithmName, ");
                cmdText.Append("  [column_encryption_key_id] ColumnEncryptionKeyId, ");
                cmdText.Append("  [column_encryption_key_database_name] ColumnEncryptionKeyDatabaseName, ");
                cmdText.Append("  [is_hidden] IsHidden, ");
                cmdText.Append("  [is_masked] IsMasked, ");
                cmdText.Append("  [graph_type] GraphType, ");
                cmdText.Append("  [graph_type_desc] GraphTypeDesc ");
                cmdText.Append(" from ");
                cmdText.Append("  [sys].[columns] ");

                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = cmdText.ToString();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return new SysColumn
                        {
                            ObjectId = reader.GetInt32(0),
                            Name = reader[1] is DBNull ? string.Empty : reader.GetString(1),
                            ColumnId = reader.GetInt32(2),
                            SystemTypeId = reader.GetByte(3),
                            UserTypeId = reader.GetInt32(4),
                            MaxLength = reader.GetInt16(5),
                            Precision = reader.GetByte(6),
                            Scale = reader.GetByte(7),
                            CollationName = reader[8] is DBNull ? string.Empty : reader.GetString(8),
                            IsNullable = reader[9] is DBNull ? false : reader.GetBoolean(9),
                            IsAnsiPadded = reader.GetBoolean(10),
                            IsRowguidcol = reader.GetBoolean(11),
                            IsIdentity = reader.GetBoolean(12),
                            IsComputed = reader.GetBoolean(13),
                            IsFilestream = reader.GetBoolean(14),
                            IsReplicated = reader[15] is DBNull ? false : reader.GetBoolean(15),
                            IsNonSqlSubscribed = reader[16] is DBNull ? false : reader.GetBoolean(16),
                            IsMergePublished = reader[17] is DBNull ? false : reader.GetBoolean(17),
                            IsDtsReplicated = reader[18] is DBNull ? false : reader.GetBoolean(18),
                            IsXmlDocument = reader.GetBoolean(19),
                            XmlCollectionId = reader.GetInt32(20),
                            DefaultObjectId = reader.GetInt32(21),
                            RuleObjectId = reader.GetInt32(22),
                            IsSparse = reader[23] is DBNull ? false : reader.GetBoolean(23),
                            IsColumnSet = reader[24] is DBNull ? false : reader.GetBoolean(24),
                            GeneratedAlwaysType = reader[25] is DBNull ? (byte)0 : reader.GetByte(25),
                            GeneratedAlwaysTypeDesc = reader[26] is DBNull ? string.Empty : reader.GetString(26),
                            EncryptionType = reader[27] is DBNull ? 0 : reader.GetInt32(27),
                            EncryptionTypeDesc = reader[28] is DBNull ? string.Empty : reader.GetString(28),
                            EncryptionAlgorithmName = reader[29] is DBNull ? string.Empty : reader.GetString(29),
                            ColumnEncryptionKeyId = reader[30] is DBNull ? 0 : reader.GetInt32(30),
                            ColumnEncryptionKeyDatabaseName = reader[31] is DBNull ? string.Empty : reader.GetString(31),
                            IsHidden = reader[32] is DBNull ? false : reader.GetBoolean(32),
                            IsMasked = reader[33] is DBNull ? false : reader.GetBoolean(33),
                            GraphType = reader[34] is DBNull ? 0 : reader.GetInt32(34),
                            GraphTypeDesc = reader[35] is DBNull ? string.Empty : reader.GetString(35)
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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace CatFactory.SqlServer.DatabaseObjectModel.Queries
{
    /// <summary>
    /// Provides extension methods to read results from sys views (e.g. sys.*)
    /// </summary>
    public static class SysQueriesHelper
    {
        /// <summary>
        /// Gets an enumerator for 'sys.schemas' view result
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <returns>An enumerator of <see cref="SysSchema"/> that contains all schemas in database</returns>
        public static async Task<ICollection<SysSchema>> GetSysSchemasAsync(this DbConnection connection)
        {
            using var command = connection.CreateCommand();

            var cmdText = new StringBuilder();

            cmdText.Append(" SELECT ");
            cmdText.Append("  [name], ");
            cmdText.Append("  [principal_id], ");
            cmdText.Append("  [schema_id] ");
            cmdText.Append(" FROM ");
            cmdText.Append("  [sys].[schemas] ");

            command.Connection = connection;
            command.CommandType = CommandType.Text;
            command.CommandText = cmdText.ToString();

            var collection = new Collection<SysSchema>();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                collection.Add(new SysSchema
                {
                    Name = reader.GetString(0),
                    PrincipalId = reader.GetInt32(1),
                    SchemaId = reader.GetInt32(2)
                });
            }

            return collection;
        }

        /// <summary>
        /// Gets an enumerator for 'sys.types' view result
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="name">Name for sys.type</param>
        /// <param name="schemaId">Schema ID</param>
        /// <param name="isUserDefined">Is user defined</param>
        /// <returns>An enumerator of <see cref="SysType"/> that contains all types in database</returns>
        public static async Task<ICollection<SysType>> GetSysTypesAsync(this DbConnection connection, string name = "", int? schemaId = null, bool? isUserDefined = null)
        {
            using var command = connection.CreateCommand();

            var cmdText = new StringBuilder();

            cmdText.Append(" SELECT ");
            cmdText.Append("  [name], ");
            cmdText.Append("  [system_type_id], ");
            cmdText.Append("  [user_type_id], ");
            cmdText.Append("  [schema_id], ");
            cmdText.Append("  [principal_id], ");
            cmdText.Append("  [max_length], ");
            cmdText.Append("  [precision], ");
            cmdText.Append("  [scale], ");
            cmdText.Append("  [collation_name], ");
            cmdText.Append("  [is_nullable], ");
            cmdText.Append("  [is_user_defined], ");
            cmdText.Append("  [is_assembly_type], ");
            cmdText.Append("  [default_object_id], ");
            cmdText.Append("  [rule_object_id], ");
            cmdText.Append("  [is_table_type] ");
            cmdText.Append(" FROM ");
            cmdText.Append("  [sys].[types] ");
            cmdText.Append(" WHERE ");
            cmdText.Append("  (@name IS null OR [sys].[types].[name] like @name) ");
            cmdText.Append("  AND (@schemaId IS null OR [sys].[types].[schema_id] = @schemaId) ");
            cmdText.Append("  AND (@isUserDefined IS null OR [sys].[types].[is_user_defined] = @isUserDefined) ");

            command.Connection = connection;
            command.CommandType = CommandType.Text;
            command.CommandText = cmdText.ToString();
            command.Parameters.Add(new SqlParameter("@name", string.IsNullOrEmpty(name) ? (object)DBNull.Value : name));
            command.Parameters.Add(new SqlParameter("@schemaId", schemaId ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@isUserDefined", isUserDefined ?? (object)DBNull.Value));

            var collection = new Collection<SysType>();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                collection.Add(new SysType
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
                });
            }

            return collection;
        }

        /// <summary>
        /// Gets an enumerator for 'sys.tables' view result
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <returns>An enumerator of <see cref="SysTable"/> that contains all tables in database</returns>
        public static async Task<ICollection<SysTable>> GetSysTablesAsync(this DbConnection connection)
        {
            using var command = connection.CreateCommand();

            var cmdText = new StringBuilder();

            cmdText.Append(" SELECT ");
            cmdText.Append("  [name], ");
            cmdText.Append("  [object_id], ");
            cmdText.Append("  [principal_id], ");
            cmdText.Append("  [schema_id], ");
            cmdText.Append("  [parent_object_id], ");
            cmdText.Append("  [type] Type, ");
            cmdText.Append("  [type_desc], ");
            cmdText.Append("  [create_date], ");
            cmdText.Append("  [modify_date], ");
            cmdText.Append("  [is_ms_shipped], ");
            cmdText.Append("  [is_published], ");
            cmdText.Append("  [is_schema_published], ");
            cmdText.Append("  [lob_data_space_id], ");
            cmdText.Append("  [filestream_data_space_id], ");
            cmdText.Append("  [max_column_id_used], ");
            cmdText.Append("  [lock_on_bulk_load], ");
            cmdText.Append("  [uses_ansi_nulls], ");
            cmdText.Append("  [is_replicated], ");
            cmdText.Append("  [has_replication_filter], ");
            cmdText.Append("  [is_merge_published], ");
            cmdText.Append("  [is_sync_tran_subscribed], ");
            cmdText.Append("  [has_unchecked_assembly_data], ");
            cmdText.Append("  [text_in_row_limit], ");
            cmdText.Append("  [large_value_types_out_of_row], ");
            cmdText.Append("  [is_tracked_by_cdc], ");
            cmdText.Append("  [lock_escalation], ");
            cmdText.Append("  [lock_escalation_desc], ");
            cmdText.Append("  [is_filetable], ");
            cmdText.Append("  [is_memory_optimized], ");
            cmdText.Append("  [durability], ");
            cmdText.Append("  [durability_desc], ");
            cmdText.Append("  [temporal_type], ");
            cmdText.Append("  [temporal_type_desc], ");
            cmdText.Append("  [history_table_id], ");
            cmdText.Append("  [is_remote_data_archive_enabled], ");
            cmdText.Append("  [is_external], ");
            cmdText.Append("  [history_retention_period], ");
            cmdText.Append("  [history_retention_period_unit], ");
            cmdText.Append("  [history_retention_period_unit_desc], ");
            cmdText.Append("  [is_node], ");
            cmdText.Append("  [is_edge] ");
            cmdText.Append(" FROM ");
            cmdText.Append("  [sys].[tables] ");

            command.Connection = connection;
            command.CommandType = CommandType.Text;
            command.CommandText = cmdText.ToString();

            var collection = new Collection<SysTable>();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                collection.Add(new SysTable
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
                });
            }

            return collection;
        }

        /// <summary>
        /// Gets an enumerator for 'sys.views' view result
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <returns>An enumerator of <see cref="SysView"/> that contains all views in database</returns>
        public static async Task<ICollection<SysView>> GetSysViewsAsync(this DbConnection connection)
        {
            using var command = connection.CreateCommand();

            var cmdText = new StringBuilder();

            cmdText.Append(" SELECT ");
            cmdText.Append("  [name], ");
            cmdText.Append("  [object_id], ");
            cmdText.Append("  [principal_id], ");
            cmdText.Append("  [schema_id], ");
            cmdText.Append("  [parent_object_id], ");
            cmdText.Append("  [type], ");
            cmdText.Append("  [type_desc], ");
            cmdText.Append("  [create_date], ");
            cmdText.Append("  [modify_date], ");
            cmdText.Append("  [is_ms_shipped], ");
            cmdText.Append("  [is_published], ");
            cmdText.Append("  [is_schema_published], ");
            cmdText.Append("  [is_replicated], ");
            cmdText.Append("  [has_replication_filter], ");
            cmdText.Append("  [has_opaque_metadata], ");
            cmdText.Append("  [has_unchecked_assembly_data], ");
            cmdText.Append("  [with_check_option], ");
            cmdText.Append("  [is_date_correlation_view], ");
            cmdText.Append("  [is_tracked_by_cdc] ");
            cmdText.Append(" FROM ");
            cmdText.Append("  [sys].[views] ");

            command.Connection = connection;
            command.CommandType = CommandType.Text;
            command.CommandText = cmdText.ToString();

            var collection = new Collection<SysView>();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                collection.Add(new SysView
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
                    IsTrackedByCdc = reader[18] is DBNull ? false : reader.GetBoolean(18)
                });
            }

            return collection;
        }

        /// <summary>
        /// Gets an enumerator for 'sys.columns' view result
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <returns>An enumerator of <see cref="SysColumn"/> that contains all columns in database</returns>
        public static async Task<ICollection<SysColumn>> GetSysColumnsAsync(this DbConnection connection)
        {
            using var command = connection.CreateCommand();

            var cmdText = new StringBuilder();

            cmdText.Append(" SELECT ");
            cmdText.Append("  [object_id], ");
            cmdText.Append("  [name], ");
            cmdText.Append("  [column_id], ");
            cmdText.Append("  [system_type_id], ");
            cmdText.Append("  [user_type_id], ");
            cmdText.Append("  [max_length], ");
            cmdText.Append("  [precision], ");
            cmdText.Append("  [scale], ");
            cmdText.Append("  [collation_name], ");
            cmdText.Append("  [is_nullable], ");
            cmdText.Append("  [is_ansi_padded], ");
            cmdText.Append("  [is_rowguidcol], ");
            cmdText.Append("  [is_identity], ");
            cmdText.Append("  [is_computed], ");
            cmdText.Append("  [is_filestream], ");
            cmdText.Append("  [is_replicated], ");
            cmdText.Append("  [is_non_sql_subscribed], ");
            cmdText.Append("  [is_merge_published], ");
            cmdText.Append("  [is_dts_replicated], ");
            cmdText.Append("  [is_xml_document], ");
            cmdText.Append("  [xml_collection_id], ");
            cmdText.Append("  [default_object_id], ");
            cmdText.Append("  [rule_object_id], ");
            cmdText.Append("  [is_sparse], ");
            cmdText.Append("  [is_column_set], ");
            cmdText.Append("  [generated_always_type], ");
            cmdText.Append("  [generated_always_type_desc], ");
            cmdText.Append("  [encryption_type], ");
            cmdText.Append("  [encryption_type_desc], ");
            cmdText.Append("  [encryption_algorithm_name], ");
            cmdText.Append("  [column_encryption_key_id], ");
            cmdText.Append("  [column_encryption_key_database_name], ");
            cmdText.Append("  [is_hidden], ");
            cmdText.Append("  [is_masked], ");
            cmdText.Append("  [graph_type], ");
            cmdText.Append("  [graph_type_desc] ");
            cmdText.Append(" FROM ");
            cmdText.Append("  [sys].[columns] ");

            command.Connection = connection;
            command.CommandType = CommandType.Text;
            command.CommandText = cmdText.ToString();

            var collection = new Collection<SysColumn>();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                collection.Add(new SysColumn
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
                });
            }

            return collection;
        }

        /// <summary>
        /// Gets an enumerator for 'sys.sequences' view result
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <returns>An enumerator of <see cref="SysSequence"/> that contains all sequences in database</returns>
        public static async Task<ICollection<SysSequence>> GetSysSequencesAsync(this DbConnection connection)
        {
            using var command = connection.CreateCommand();

            var cmdText = new StringBuilder();

            cmdText.Append(" SELECT ");
            cmdText.Append("  [name], ");
            cmdText.Append("  [object_id], ");
            cmdText.Append("  [principal_id], ");
            cmdText.Append("  [schema_id], ");
            cmdText.Append("  [parent_object_id], ");
            cmdText.Append("  [type], ");
            cmdText.Append("  [type_desc], ");
            cmdText.Append("  [create_date], ");
            cmdText.Append("  [modify_date], ");
            cmdText.Append("  [is_ms_shipped], ");
            cmdText.Append("  [is_published], ");
            cmdText.Append("  [is_schema_published], ");
            cmdText.Append("  [start_value], ");
            cmdText.Append("  [increment], ");
            cmdText.Append("  [minimum_value], ");
            cmdText.Append("  [maximum_value], ");
            cmdText.Append("  [is_cycling], ");
            cmdText.Append("  [is_cached], ");
            cmdText.Append("  [cache_size], ");
            cmdText.Append("  [system_type_id], ");
            cmdText.Append("  [user_type_id], ");
            cmdText.Append("  [precision], ");
            cmdText.Append("  [scale], ");
            cmdText.Append("  [current_value], ");
            cmdText.Append("  [is_exhausted], ");
            cmdText.Append("  [last_used_value] ");
            cmdText.Append(" FROM ");
            cmdText.Append("  [sys].[sequences] ");

            command.Connection = connection;
            command.CommandType = CommandType.Text;
            command.CommandText = cmdText.ToString();

            var collection = new Collection<SysSequence>();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                collection.Add(new SysSequence
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
                    LastUsedValue = reader[25] is DBNull ? default(int?) : reader.GetInt32(25)
                });
            }

            return collection;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace CatFactory.SqlServer.DatabaseObjectModel.Queries
{
    /// <summary>
    /// Contains extension methods for sys.dm_exec_describe_first_result_set_for_object execution
    /// </summary>
    public static class DmExecDescribeFirstResultSetForObjectHelper
    {
        /// <summary>
        /// Gets the result for execution of sys.dm_exec_describe_first_result_set_for_object function
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="objectName">Object name</param>
        /// <param name="browseInformationMode">Browse information mode</param>
        /// <returns>An enumerator of <see cref="DmExecDescribeFirstResultSetForObject"/> class</returns>
        public static async Task<ICollection<DmExecDescribeFirstResultSetForObject>> DmExecDescribeFirstResultSetForObjectAsync(this DbConnection connection, string objectName, byte? browseInformationMode = null)
        {
            using var command = connection.CreateCommand();

            var cmdText = new StringBuilder();

            cmdText.Append(" select");
            cmdText.Append("  [is_hidden], ");
            cmdText.Append("  [column_ordinal], ");
            cmdText.Append("  [name], ");
            cmdText.Append("  [is_nullable], ");
            cmdText.Append("  [system_type_id], ");
            cmdText.Append("  [system_type_name], ");
            cmdText.Append("  [max_length], ");
            cmdText.Append("  [precision], ");
            cmdText.Append("  [scale], ");
            cmdText.Append("  [collation_name], ");
            cmdText.Append("  [user_type_id], ");
            cmdText.Append("  [user_type_database], ");
            cmdText.Append("  [user_type_schema], ");
            cmdText.Append("  [user_type_name], ");
            cmdText.Append("  [assembly_qualified_type_name], ");
            cmdText.Append("  [xml_collection_id], ");
            cmdText.Append("  [xml_collection_database], ");
            cmdText.Append("  [xml_collection_schema], ");
            cmdText.Append("  [xml_collection_name], ");
            cmdText.Append("  [is_xml_document], ");
            cmdText.Append("  [is_case_sensitive], ");
            cmdText.Append("  [is_fixed_length_clr_type], ");
            cmdText.Append("  [source_server], ");
            cmdText.Append("  [source_database], ");
            cmdText.Append("  [source_schema], ");
            cmdText.Append("  [source_table], ");
            cmdText.Append("  [source_column], ");
            cmdText.Append("  [is_identity_column], ");
            cmdText.Append("  [is_part_of_unique_key], ");
            cmdText.Append("  [is_updateable], ");
            cmdText.Append("  [is_computed_column], ");
            cmdText.Append("  [is_sparse_column_set], ");
            cmdText.Append("  [ordinal_in_order_by_list], ");
            cmdText.Append("  [order_by_is_descending], ");
            cmdText.Append("  [order_by_list_length], ");
            cmdText.Append("  [error_number], ");
            cmdText.Append("  [error_severity], ");
            cmdText.Append("  [error_state], ");
            cmdText.Append("  [error_message], ");
            cmdText.Append("  [error_type], ");
            cmdText.Append("  [error_type_desc] ");
            cmdText.Append(" from ");
            cmdText.Append("  [sys].[dm_exec_describe_first_result_set_for_object] ");
            cmdText.AppendFormat(" (object_id('{0}'), {1}) ", objectName, browseInformationMode.HasValue ? browseInformationMode.Value.ToString() : "null");

            command.Connection = connection;
            command.CommandType = CommandType.Text;
            command.CommandText = cmdText.ToString();

            var collection = new Collection<DmExecDescribeFirstResultSetForObject>();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                collection.Add(new DmExecDescribeFirstResultSetForObject
                {
                    IsHidden = reader[0] is DBNull ? false : reader.GetBoolean(0),
                    ColumnOrdinal = reader[1] is DBNull ? 0 : reader.GetInt32(1),
                    Name = reader[2] is DBNull ? string.Empty : reader.GetString(2),
                    IsNullable = reader[3] is DBNull ? false : reader.GetBoolean(3),
                    SystemTypeId = reader[4] is DBNull ? 0 : reader.GetInt32(4),
                    SystemTypeName = reader[5] is DBNull ? string.Empty : reader.GetString(5),
                    MaxLength = reader[6] is DBNull ? (short)0 : reader.GetInt16(6),
                    Precision = reader[7] is DBNull ? (byte)0 : reader.GetByte(7),
                    Scale = reader[8] is DBNull ? (byte)0 : reader.GetByte(8),
                    CollationName = reader[9] is DBNull ? string.Empty : reader.GetString(9),
                    UserTypeId = reader[10] is DBNull ? 0 : reader.GetInt32(10),
                    UserTypeDatabase = reader[11] is DBNull ? string.Empty : reader.GetString(11),
                    UserTypeSchema = reader[12] is DBNull ? string.Empty : reader.GetString(12),
                    UserTypeName = reader[13] is DBNull ? string.Empty : reader.GetString(13),
                    AssemblyQualifiedTypeName = reader[14] is DBNull ? string.Empty : reader.GetString(14),
                    XmlCollectionId = reader[15] is DBNull ? 0 : reader.GetInt32(15),
                    XmlCollectionDatabase = reader[16] is DBNull ? string.Empty : reader.GetString(16),
                    XmlCollectionSchema = reader[17] is DBNull ? string.Empty : reader.GetString(17),
                    XmlCollectionName = reader[18] is DBNull ? string.Empty : reader.GetString(18),
                    IsXmlDocument = reader[19] is DBNull ? false : reader.GetBoolean(19),
                    IsCaseSensitive = reader[20] is DBNull ? false : reader.GetBoolean(20),
                    IsFixedLengthClrType = reader[21] is DBNull ? false : reader.GetBoolean(21),
                    SourceServer = reader[22] is DBNull ? string.Empty : reader.GetString(22),
                    SourceDatabase = reader[23] is DBNull ? string.Empty : reader.GetString(23),
                    SourceSchema = reader[24] is DBNull ? string.Empty : reader.GetString(24),
                    SourceTable = reader[25] is DBNull ? string.Empty : reader.GetString(25),
                    SourceColumn = reader[26] is DBNull ? string.Empty : reader.GetString(26),
                    IsIdentityColumn = reader[27] is DBNull ? false : reader.GetBoolean(27),
                    IsPartOfUniqueKey = reader[28] is DBNull ? false : reader.GetBoolean(28),
                    IsUpdateable = reader[29] is DBNull ? false : reader.GetBoolean(29),
                    IsComputedColumn = reader[30] is DBNull ? false : reader.GetBoolean(30),
                    IsSparseColumnSet = reader[31] is DBNull ? false : reader.GetBoolean(31),
                    OrdinalInOrderByList = reader[32] is DBNull ? (short)0 : reader.GetInt16(32),
                    OrderByIsDescending = reader[33] is DBNull ? false : reader.GetBoolean(33),
                    OrderByListLength = reader[34] is DBNull ? (short)0 : reader.GetInt16(34),
                    ErrorNumber = reader[35] is DBNull ? 0 : reader.GetInt32(35),
                    ErrorSeverity = reader[36] is DBNull ? 0 : reader.GetInt32(36),
                    ErrorState = reader[37] is DBNull ? 0 : reader.GetInt32(37),
                    ErrorMessage = reader[38] is DBNull ? string.Empty : reader.GetString(38),
                    ErrorType = reader[39] is DBNull ? 0 : reader.GetInt32(39),
                    ErrorTypeDesc = reader[40] is DBNull ? string.Empty : reader.GetString(40)
                });
            }

            return collection;
        }
    }
}

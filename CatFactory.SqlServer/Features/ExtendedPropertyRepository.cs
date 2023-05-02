using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CatFactory.SqlServer.DatabaseObjectModel;

namespace CatFactory.SqlServer.Features
{
    /// <summary>
    /// Implements operations to read and write extended properties
    /// </summary>
    public class ExtendedPropertyRepository : IExtendedPropertyRepository
    {
        private const string LEVEL_0_TYPE = "@level0type";
        private const string LEVEL_0_NAME = "@level0name";
        private const string LEVEL_1_TYPE = "@level1type";
        private const string LEVEL_1_NAME = "@level1name";
        private const string LEVEL_2_TYPE = "@level2type";
        private const string LEVEL_2_NAME = "@level2name";

        private static SqlParameter GetParameter(string name, SqlDbType sqlDbType, string value)
        {
            var parameter = new SqlParameter(name, sqlDbType);

            if (string.IsNullOrEmpty(value))
                parameter.Value = DBNull.Value;
            else
                parameter.Value = value;

            return parameter;
        }

        private readonly SqlConnection _connection;

        /// <summary>
        /// Initializes a new instance of <see cref="ExtendedPropertyRepository"/> class
        /// </summary>
        /// <param name="connection">Instance of <see cref="SqlConnection"/> class</param>
        public ExtendedPropertyRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Gets extended properties
        /// </summary>
        /// <param name="extendedProperty">Search parameter</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public async Task<List<ExtendedProperty>> GetAsync(ExtendedProperty extendedProperty)
        {
            using var command = _connection.CreateCommand();

            command.Connection = _connection;
            command.CommandType = CommandType.Text;
            command.CommandText = @"
                SELECT
                    [objtype], [objname], [name], [value]
                FROM
                    [fn_listextendedproperty]
                    (
                        @name, @level0type, @level0name, @level1type, @level1name, @level2type, @level2name
                    )
                ";

            command.Parameters.Add(new SqlParameter("@name", extendedProperty.Name));
            command.Parameters.Add(GetParameter(LEVEL_0_TYPE, SqlDbType.VarChar, extendedProperty.Level0Type));
            command.Parameters.Add(GetParameter(LEVEL_0_NAME, SqlDbType.VarChar, extendedProperty.Level0Name));
            command.Parameters.Add(GetParameter(LEVEL_1_TYPE, SqlDbType.VarChar, extendedProperty.Level1Type));
            command.Parameters.Add(GetParameter(LEVEL_1_NAME, SqlDbType.VarChar, extendedProperty.Level1Name));
            command.Parameters.Add(GetParameter(LEVEL_2_TYPE, SqlDbType.VarChar, extendedProperty.Level2Type));
            command.Parameters.Add(GetParameter(LEVEL_2_NAME, SqlDbType.VarChar, extendedProperty.Level2Name));

            using var dataReader = await command.ExecuteReaderAsync();

            var list = new List<ExtendedProperty>();

            while (dataReader.Read())
            {
                list.Add(new ExtendedProperty(dataReader.GetString(2), dataReader.GetString(3)));
            }

            return list;
        }

        /// <summary>
        /// Adds an extended property
        /// </summary>
        /// <param name="extendedProperty">Instance of <see cref="ExtendedProperty"/> class to add</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public async Task AddAsync(ExtendedProperty extendedProperty)
        {
            using var command = _connection.CreateCommand();

            command.Connection = _connection;
            command.CommandType = CommandType.Text;
            command.CommandText = " EXEC [sys].[sp_addextendedproperty] @name, @value, @level0type, @level0name, @level1type, @level1name, @level2type, @level2name ";

            command.Parameters.Add(new SqlParameter("@name", extendedProperty.Name));
            command.Parameters.Add(new SqlParameter("@value", extendedProperty.Value));
            command.Parameters.Add(GetParameter(LEVEL_0_TYPE, SqlDbType.VarChar, extendedProperty.Level0Type));
            command.Parameters.Add(GetParameter(LEVEL_0_NAME, SqlDbType.VarChar, extendedProperty.Level0Name));
            command.Parameters.Add(GetParameter(LEVEL_1_TYPE, SqlDbType.VarChar, extendedProperty.Level1Type));
            command.Parameters.Add(GetParameter(LEVEL_1_NAME, SqlDbType.VarChar, extendedProperty.Level1Name));
            command.Parameters.Add(GetParameter(LEVEL_2_TYPE, SqlDbType.VarChar, extendedProperty.Level2Type));
            command.Parameters.Add(GetParameter(LEVEL_2_NAME, SqlDbType.VarChar, extendedProperty.Level2Name));

            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Updates an extended property
        /// </summary>
        /// <param name="extendedProperty">Instance of <see cref="ExtendedProperty"/> class to update</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public async Task UpdateAsync(ExtendedProperty extendedProperty)
        {
            using var command = _connection.CreateCommand();

            command.Connection = _connection;
            command.CommandType = CommandType.Text;
            command.CommandText = " EXEC [sys].[sp_updateextendedproperty] @name, @value, @level0type, @level0name, @level1type, @level1name, @level2type, @level2name ";

            command.Parameters.Add(new SqlParameter("@name", extendedProperty.Name));
            command.Parameters.Add(new SqlParameter("@value", extendedProperty.Value));
            command.Parameters.Add(GetParameter(LEVEL_0_TYPE, SqlDbType.VarChar, extendedProperty.Level0Type));
            command.Parameters.Add(GetParameter(LEVEL_0_NAME, SqlDbType.VarChar, extendedProperty.Level0Name));
            command.Parameters.Add(GetParameter(LEVEL_1_TYPE, SqlDbType.VarChar, extendedProperty.Level1Type));
            command.Parameters.Add(GetParameter(LEVEL_1_NAME, SqlDbType.VarChar, extendedProperty.Level1Name));
            command.Parameters.Add(GetParameter(LEVEL_2_TYPE, SqlDbType.VarChar, extendedProperty.Level2Type));
            command.Parameters.Add(GetParameter(LEVEL_2_NAME, SqlDbType.VarChar, extendedProperty.Level2Name));

            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Drops an extended property
        /// </summary>
        /// <param name="extendedProperty">Instance of <see cref="ExtendedProperty"/> class to drop</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public async Task DropAsync(ExtendedProperty extendedProperty)
        {
            using var command = _connection.CreateCommand();

            command.Connection = _connection;
            command.CommandType = CommandType.Text;
            command.CommandText = " EXEC [sys].[sp_dropextendedproperty] @name, @level0type, @level0name, @level1type, @level1name, @level2type, @level2name ";

            command.Parameters.Add(new SqlParameter("@name", extendedProperty.Name));
            command.Parameters.Add(GetParameter(LEVEL_0_TYPE, SqlDbType.VarChar, extendedProperty.Level0Type));
            command.Parameters.Add(GetParameter(LEVEL_0_NAME, SqlDbType.VarChar, extendedProperty.Level0Name));
            command.Parameters.Add(GetParameter(LEVEL_1_TYPE, SqlDbType.VarChar, extendedProperty.Level1Type));
            command.Parameters.Add(GetParameter(LEVEL_1_NAME, SqlDbType.VarChar, extendedProperty.Level1Name));
            command.Parameters.Add(GetParameter(LEVEL_2_TYPE, SqlDbType.VarChar, extendedProperty.Level2Type));
            command.Parameters.Add(GetParameter(LEVEL_2_NAME, SqlDbType.VarChar, extendedProperty.Level2Name));

            await command.ExecuteNonQueryAsync();
        }
    }
}

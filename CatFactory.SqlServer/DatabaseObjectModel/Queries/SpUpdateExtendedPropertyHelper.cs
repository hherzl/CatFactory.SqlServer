﻿using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CatFactory.ObjectRelationalMapping;
using tokens = CatFactory.SqlServer.SqlServerToken;

namespace CatFactory.SqlServer.DatabaseObjectModel.Queries
{
    /// <summary>
    /// Provides extension methods to execute 'sp_updateextendedproperty' stored procedure
    /// </summary>
    public static class SpUpdateExtendedPropertyHelper
    {
        /// <summary>
        /// Executes 'sp_updateextendedproperty' stored procedure
        /// </summary>
        /// <param name="connection">Instance of <see cref="SqlConnection"/> class</param>
        /// <param name="extendedProperty">Instance of <see cref="ExtendedProperty"/>class</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task SpUpdateExtendedPropertyAsync(this SqlConnection connection, ExtendedProperty extendedProperty)
        {
            if (connection.State == ConnectionState.Closed)
                await connection.OpenAsync();

            using var command = connection.CreateCommand();

            command.Connection = connection;
            command.CommandType = CommandType.Text;
            command.CommandText = " EXEC [sys].[sp_updateextendedproperty] @name, @value, @level0type, @level0name, @level1type, @level1name, @level2type, @level2name ";

            command.Parameters.Add(new SqlParameter("@name", extendedProperty.Name));
            command.Parameters.Add(new SqlParameter("@value", extendedProperty.Value));
            command.Parameters.Add(connection.GetParameter(tokens.LEVEL_0_TYPE, SqlDbType.VarChar, extendedProperty.Level0Type));
            command.Parameters.Add(connection.GetParameter(tokens.LEVEL_0_NAME, SqlDbType.VarChar, extendedProperty.Level0Name));
            command.Parameters.Add(connection.GetParameter(tokens.LEVEL_1_TYPE, SqlDbType.VarChar, extendedProperty.Level1Type));
            command.Parameters.Add(connection.GetParameter(tokens.LEVEL_1_NAME, SqlDbType.VarChar, extendedProperty.Level1Name));
            command.Parameters.Add(connection.GetParameter(tokens.LEVEL_2_TYPE, SqlDbType.VarChar, extendedProperty.Level2Type));
            command.Parameters.Add(connection.GetParameter(tokens.LEVEL_2_NAME, SqlDbType.VarChar, extendedProperty.Level2Name));

            await command.ExecuteNonQueryAsync();
        }
#pragma warning disable CS1591

        public static async Task UpdateExtendedPropertyAsync(this SqlConnection connection, string name, string value)
            => await connection.SpUpdateExtendedPropertyAsync(new ExtendedProperty(name, value));

        public static async Task UpdateExtendedPropertyAsync(this SqlConnection connection, ITable table, string name, string value)
            => await connection.SpUpdateExtendedPropertyAsync(ExtendedProperty.CreateLevel1(tokens.SCHEMA, table.Schema, tokens.TABLE, table.Name, name, value));

        public static async Task UpdateExtendedPropertyAsync(this SqlConnection connection, ITable table, Column column, string name, string value)
            => await connection.SpUpdateExtendedPropertyAsync(ExtendedProperty.CreateLevel2(tokens.SCHEMA, table.Schema, tokens.TABLE, table.Name, tokens.COLUMN, column.Name, name, value));

        public static async Task UpdateExtendedPropertyAsync(this SqlConnection connection, IView view, string name, string value)
            => await connection.SpUpdateExtendedPropertyAsync(ExtendedProperty.CreateLevel1(tokens.SCHEMA, view.Schema, tokens.VIEW, view.Name, name, value));

        public static async Task UpdateExtendedPropertyAsync(this SqlConnection connection, IView view, Column column, string name, string value)
            => await connection.SpUpdateExtendedPropertyAsync(ExtendedProperty.CreateLevel2(tokens.SCHEMA, view.Schema, tokens.VIEW, view.Name, tokens.COLUMN, column.Name, name, value));
    }
}

using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CatFactory.ObjectRelationalMapping;
using CatFactory.SqlServer.Features;
using tokens = CatFactory.SqlServer.SqlServerToken;

namespace CatFactory.SqlServer.DatabaseObjectModel.Queries
{
    /// <summary>
    /// Provides extension methods to execute 'sp_dropextendedproperty' stored procedure
    /// </summary>
    public static class SpDropExtendedPropertyHelper
    {
        /// <summary>
        /// Executes 'sp_dropextendedproperty' stored procedure
        /// </summary>
        /// <param name="connection">Instance of <see cref="SqlConnection"/> class</param>
        /// <param name="extendedProperty">Instance of <see cref="ExtendedProperty"/>class</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task SpDropExtendedPropertyAsync(this SqlConnection connection, ExtendedProperty extendedProperty)
        {
            if (connection.State == ConnectionState.Closed)
                await connection.OpenAsync();

            using var command = connection.CreateCommand();

            command.Connection = connection;
            command.CommandType = CommandType.Text;
            command.CommandText = " EXEC [sys].[sp_dropextendedproperty] @name, @level0type, @level0name, @level1type, @level1name, @level2type, @level2name ";

            command.Parameters.Add(new SqlParameter("@name", extendedProperty.Name));
            command.Parameters.Add(connection.GetParameter(tokens.LEVEL_0_TYPE, SqlDbType.VarChar, extendedProperty.Level0Type));
            command.Parameters.Add(connection.GetParameter(tokens.LEVEL_0_NAME, SqlDbType.VarChar, extendedProperty.Level0Name));
            command.Parameters.Add(connection.GetParameter(tokens.LEVEL_1_TYPE, SqlDbType.VarChar, extendedProperty.Level1Type));
            command.Parameters.Add(connection.GetParameter(tokens.LEVEL_1_NAME, SqlDbType.VarChar, extendedProperty.Level1Name));
            command.Parameters.Add(connection.GetParameter(tokens.LEVEL_2_TYPE, SqlDbType.VarChar, extendedProperty.Level2Type));
            command.Parameters.Add(connection.GetParameter(tokens.LEVEL_2_NAME, SqlDbType.VarChar, extendedProperty.Level2Name));

            await command.ExecuteNonQueryAsync();
        }
#pragma warning disable CS1591

        public static async Task DropExtendedPropertyAsync(this SqlConnection connection, string name)
            => await connection.SpDropExtendedPropertyAsync(new ExtendedProperty(name));

        public static async Task DropExtendedPropertyAsync(this SqlConnection connection, ITable table, string name)
            => await connection.SpDropExtendedPropertyAsync(ExtendedProperty.CreateLevel1(tokens.SCHEMA, table.Schema, tokens.TABLE, table.Name, name));

        public static async Task DropExtendedPropertyAsync(this SqlConnection connection, ITable table, Column column, string name)
            => await connection.SpDropExtendedPropertyAsync(ExtendedProperty.CreateLevel2(tokens.SCHEMA, table.Schema, tokens.TABLE, table.Name, tokens.COLUMN, column.Name, name));

        public static async Task DropExtendedPropertyAsync(this SqlConnection connection, IView view, string name)
            => await connection.SpDropExtendedPropertyAsync(ExtendedProperty.CreateLevel1(tokens.SCHEMA, view.Schema, tokens.VIEW, view.Name, name));

        public static async Task DropExtendedPropertyAsync(this SqlConnection connection, IView view, Column column, string name)
            => await connection.SpDropExtendedPropertyAsync(ExtendedProperty.CreateLevel2(tokens.SCHEMA, view.Schema, tokens.VIEW, view.Name, tokens.COLUMN, column.Name, name));

        public static async Task DropExtendedPropertyIfExistsAsync(this SqlConnection connection, string name)
        {
            var extendedProperty = (await connection.FnListExtendedPropertyAsync(name)).FirstOrDefault();

            if (extendedProperty != null)
                await connection.SpDropExtendedPropertyAsync(new ExtendedProperty(name));
        }

        public static async Task DropExtendedPropertyIfExistsAsync(this SqlConnection connection, ITable table, string name)
        {
            var extendedProperty = (await connection.FnListExtendedPropertyAsync(table, name)).FirstOrDefault();

            if (extendedProperty != null)
                await connection.DropExtendedPropertyAsync(table, name);
        }

        public static async Task DropExtendedPropertyIfExistsAsync(this SqlConnection connection, ITable table, Column column, string name)
        {
            var extendedProperty = (await connection.GetExtendedProperties(table, column, name)).FirstOrDefault();

            if (extendedProperty != null)
                await connection.DropExtendedPropertyAsync(table, column, name);
        }

        public static async Task DropExtendedPropertyIfExistsAsync(this SqlConnection connection, IView view, string name)
        {
            var extendedProperty = (await connection.FnListExtendedPropertyAsync(view, name)).FirstOrDefault();

            if (extendedProperty != null)
                await connection.DropExtendedPropertyAsync(view, name);
        }

        public static async Task DropExtendedPropertyIfExistsAsync(this SqlConnection connection, IView view, Column column, string name)
        {
            var extendedProperty = (await connection.GetExtendedProperties(view, column, name)).FirstOrDefault();

            if (extendedProperty != null)
                await connection.DropExtendedPropertyAsync(view, column, name);
        }
    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CatFactory.ObjectRelationalMapping;
using tokens = CatFactory.SqlServer.SqlServerToken;

namespace CatFactory.SqlServer.DatabaseObjectModel.Queries
{
    /// <summary>
    /// Provides extension methods to read results from 'fn_listextendedproperty' table function
    /// </summary>
    public static class FnListExtendedPropertyHelper
    {
        /// <summary>
        /// Gets an enumerator for 'fn_listextendedproperty' table function result
        /// </summary>
        /// <param name="connection">Instance of <see cref="SqlConnection"/> class</param>
        /// <param name="extendedProperty">Instance of <see cref="ExtendedProperty"/>class</param>
        /// <returns>An enumerator of <see cref="ExtendedProperty"/></returns>
        public static async Task<ICollection<ExtendedProperty>> FnListExtendedPropertyAsync(this SqlConnection connection, ExtendedProperty extendedProperty)
        {
            if (connection.State == ConnectionState.Closed)
                await connection.OpenAsync();

            using var command = connection.CreateCommand();

            command.Connection = connection;
            command.CommandType = CommandType.Text;
            command.CommandText =
                $" SELECT [objtype], [objname], [name], [value] FROM [fn_listextendedproperty](@name, @level0type, @level0name, @level1type, @level1name, @level2type, @level2name) ";

            command.Parameters.Add(new SqlParameter("@name", extendedProperty.Name));
            command.Parameters.Add(connection.GetParameter(tokens.LEVEL_0_TYPE, SqlDbType.VarChar, extendedProperty.Level0Type));
            command.Parameters.Add(connection.GetParameter(tokens.LEVEL_0_NAME, SqlDbType.VarChar, extendedProperty.Level0Name));
            command.Parameters.Add(connection.GetParameter(tokens.LEVEL_1_TYPE, SqlDbType.VarChar, extendedProperty.Level1Type));
            command.Parameters.Add(connection.GetParameter(tokens.LEVEL_1_NAME, SqlDbType.VarChar, extendedProperty.Level1Name));
            command.Parameters.Add(connection.GetParameter(tokens.LEVEL_2_TYPE, SqlDbType.VarChar, extendedProperty.Level2Type));
            command.Parameters.Add(connection.GetParameter(tokens.LEVEL_2_NAME, SqlDbType.VarChar, extendedProperty.Level2Name));

            using var reader = await command.ExecuteReaderAsync();

            var collection = new Collection<ExtendedProperty>();

            while (await reader.ReadAsync())
            {
                collection.Add(new ExtendedProperty(reader.GetString(2), reader.GetString(3)));
            }

            return collection;
        }
#pragma warning disable CS1591

        public static async Task<ICollection<ExtendedProperty>> FnListExtendedPropertyAsync(this SqlConnection connection, string name)
            => await connection.FnListExtendedPropertyAsync(new ExtendedProperty(name));

        public static async Task<ICollection<ExtendedProperty>> FnListExtendedPropertyAsync(this SqlConnection connection, ITable table, string name)
            => await connection.FnListExtendedPropertyAsync(ExtendedProperty.CreateLevel1(tokens.SCHEMA, table.Schema, tokens.TABLE, table.Name, name));

        public static async Task<ICollection<ExtendedProperty>> FnListExtendedPropertyAsync(this SqlConnection connection, IView view, string name)
            => await connection.FnListExtendedPropertyAsync(ExtendedProperty.CreateLevel1(tokens.SCHEMA, view.Schema, tokens.VIEW, view.Name, name));
    }
}

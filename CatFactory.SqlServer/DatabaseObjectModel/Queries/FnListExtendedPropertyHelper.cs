using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CatFactory.SqlServer.DatabaseObjectModel.Queries
{
    /// <summary>
    /// Provides extension methods to read results from 'fn_listextendedproperty' table function
    /// </summary>
    public static class FnListExtendedPropertyHelper
    {
        private static SqlParameter GetParameter(string name, SqlDbType sqlDbType, string value)
        {
            var parameter = new SqlParameter(name, sqlDbType);

            if (string.IsNullOrEmpty(value))
                parameter.Value = DBNull.Value;
            else
                parameter.Value = value;

            return parameter;
        }

        /// <summary>
        /// Gets an enumerator for 'fn_listextendedproperty' table function result
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="extendedProperty">Instance of <see cref="ExtendedProperty"/>class</param>
        /// <returns>An enumerator of <see cref="SysSchema"/> that contains all schemas in database</returns>
        public static async Task<ICollection<ExtendedProperty>> FnListExtendedPropertyAsync(this DbConnection connection, ExtendedProperty extendedProperty)
        {
            var collection = new Collection<ExtendedProperty>();

            using var command = connection.CreateCommand();

            command.Connection = connection;
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
            command.Parameters.Add(GetParameter("@level0type", SqlDbType.VarChar, extendedProperty.Level0Type));
            command.Parameters.Add(GetParameter("@level0name", SqlDbType.VarChar, extendedProperty.Level0Name));
            command.Parameters.Add(GetParameter("@level1type", SqlDbType.VarChar, extendedProperty.Level1Type));
            command.Parameters.Add(GetParameter("@level1name", SqlDbType.VarChar, extendedProperty.Level1Name));
            command.Parameters.Add(GetParameter("@level2type", SqlDbType.VarChar, extendedProperty.Level2Type));
            command.Parameters.Add(GetParameter("@level2name", SqlDbType.VarChar, extendedProperty.Level2Name));

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                collection.Add(new ExtendedProperty
                {
                    Name = reader.GetString(2),
                    Value = reader.GetString(3)
                });
            }

            return collection;
        }
    }
}

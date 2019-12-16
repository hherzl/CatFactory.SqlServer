using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace CatFactory.SqlServer.DatabaseObjectModel.Queries
{
    /// <summary>
    /// Provides extension methods for execution of sp_help stored procedure
    /// </summary>
    public static class SpHelpHelper
    {
        /// <summary>
        /// Gets the result of sp_help stored procedure execution for specified object
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="id">Object ID</param>
        /// <returns>A <see cref="SpHelpResult"/> result that contains the definition of database object</returns>
        public static async Task<SpHelpResult> SpHelpAsync(this DbConnection connection, string id)
        {
            using (var command = connection.CreateCommand())
            {
                command.Connection = connection;
                command.CommandText = string.Format("sp_help '{0}'", id);

                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    var queryResult = new SpHelpResult();

                    while (await dataReader.NextResultAsync())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            var names = SqlServerDatabaseFactoryHelper.GetNames(dataReader).ToList();

                            var row = new Dictionary<string, object>();

                            for (var i = 0; i < names.Count; i++)
                            {
                                row.Add(names[i], dataReader.GetValue(i));
                            }

                            queryResult.Items.Add(row);
                        }
                    }

                    return queryResult;
                }
            }
        }
    }
}

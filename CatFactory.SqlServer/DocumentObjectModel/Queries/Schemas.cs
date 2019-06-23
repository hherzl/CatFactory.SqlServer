using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace CatFactory.SqlServer.DocumentObjectModel.Queries
{
    /// <summary>
    /// 
    /// </summary>
    public static class Schemas
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
    }
}

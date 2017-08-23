using System;
using CatFactory.Mapping;
using Microsoft.Extensions.Logging;

namespace CatFactory.SqlServer
{
    public partial class SqlServerDatabaseFactory
    {
        public static Database Import(ILogger<SqlServerDatabaseFactory> logger, String connectionString, params String[] exclusions)
        {
            var dbFactory = new SqlServerDatabaseFactory(logger)
            {
                ConnectionString = connectionString
            };

            dbFactory.Exclusions.AddRange(exclusions);

            return dbFactory.Import();
        }

        public static Database Import(String connectionString, params String[] exclusions)
        {
            return Import(null, connectionString, exclusions);
        }
    }
}

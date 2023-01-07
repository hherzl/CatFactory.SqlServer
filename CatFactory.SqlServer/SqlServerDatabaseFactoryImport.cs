using System;
using System.Linq;
using System.Threading.Tasks;
using CatFactory.ObjectRelationalMapping;
using Microsoft.Extensions.Logging;

namespace CatFactory.SqlServer
{
    public partial class SqlServerDatabaseFactory
    {
        /// <summary>
        /// Imports an existing database from SQL Server instance
        /// </summary>
        /// <param name="logger">Instance for <see cref="Logger"/> class</param>
        /// <param name="connectionString">Connection string</param>
        /// <param name="exclusions">Database object names to exclude from import action</param>
        /// <returns>An instance of <see cref="Database"/> class that represents an existing database in SQL Server instance</returns>
        public static async Task<Database> ImportAsync(ILogger<SqlServerDatabaseFactory> logger, string connectionString, params string[] exclusions)
        {
            var databaseFactory = new SqlServerDatabaseFactory(logger);

            databaseFactory.DatabaseImportSettings.ConnectionString = connectionString;
            databaseFactory.DatabaseImportSettings.Exclusions.AddRange(exclusions);

            return await databaseFactory.ImportAsync();
        }

        /// <summary>
        /// Imports an existing database from SQL Server instance
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="exclusions">Database object names to exclude from import action</param>
        /// <returns>An instance of <see cref="Database"/> class that represents an existing database in SQL Server instance</returns>
        public static async Task<Database> ImportAsync(string connectionString, params string[] exclusions)
            => await ImportAsync(null, connectionString, exclusions);

        /// <summary>
        /// Imports an existing database from SQL Server instance
        /// </summary>
        /// <param name="logger">Instance for <see cref="Logger"/> class</param>
        /// <param name="connectionString">Connection string</param>
        /// <param name="tables">Table names to include in import action</param>
        /// <returns>An instance of <see cref="Database"/> class that represents an existing database in SQL Server instance</returns>
        public static async Task<Database> ImportTablesAsync(ILogger<SqlServerDatabaseFactory> logger, string connectionString, params string[] tables)
        {
            var databaseFactory = new SqlServerDatabaseFactory(logger)
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = connectionString,
                    ImportViews = false
                }
            };

            using var connection = databaseFactory.GetConnection();

            await connection.OpenAsync();

            var database = SqlServerDatabase.CreateWithDefaults(connection.Database);

            database.ServerName = connection.DataSource;

            if (tables.Length == 0)
                database.DbObjects.AddRange((await databaseFactory.GetDbObjectsAsync(connection)));
            else
                database.DbObjects.AddRange((await databaseFactory.GetDbObjectsAsync(connection)).Where(item => tables.Contains(item.FullName)));

            database.Tables.AddRange(await databaseFactory.GetTablesAsync(connection, database.GetTables()));

            return database;
        }

        /// <summary>
        /// Imports an existing database from SQL Server instance
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="tables">Table names to include in import action</param>
        /// <returns>An instance of <see cref="Database"/> class that represents an existing database in SQL Server instance</returns>
        public static async Task<Database> ImportTablesAsync(string connectionString, params string[] tables)
            => await ImportTablesAsync(null, connectionString, tables);

        /// <summary>
        /// Imports an existing database from SQL Server instance
        /// </summary>
        /// <param name="logger">Instance for <see cref="Logger"/> class</param>
        /// <param name="connectionString">Connection string</param>
        /// <param name="views">View names to include in import action</param>
        /// <returns>An instance of <see cref="Database"/> class that represents an existing database in SQL Server instance</returns>
        public static async Task<Database> ImportViewsAsync(ILogger<SqlServerDatabaseFactory> logger, string connectionString, params string[] views)
        {
            var databaseFactory = new SqlServerDatabaseFactory(logger)
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = connectionString,
                    ImportTables = false
                }
            };

            using var connection = databaseFactory.GetConnection();

            await connection.OpenAsync();

            var database = SqlServerDatabase.CreateWithDefaults(connection.Database);

            database.ServerName = connection.DataSource;

            if (views.Length == 0)
                database.DbObjects.AddRange(await databaseFactory.GetDbObjectsAsync(connection));
            else
                database.DbObjects.AddRange((await databaseFactory.GetDbObjectsAsync(connection)).Where(item => views.Contains(item.FullName)).ToList());

            database.Views.AddRange((await databaseFactory.GetViewsAsync(connection, database.GetViews())).ToList());

            return database;
        }

        /// <summary>
        /// Imports an existing database from SQL Server instance
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="views">View names to include in import action</param>
        /// <returns>An instance of <see cref="Database"/> class that represents an existing database in SQL Server instance</returns>
        public static async Task<Database> ImportViewsAsync(string connectionString, params string[] views)
            => await ImportViewsAsync(null, connectionString, views);

        /// <summary>
        /// Imports an existing database from SQL Server instance
        /// </summary>
        /// <param name="logger">Instance for <see cref="Logger"/> class</param>
        /// <param name="connectionString">Connection string</param>
        /// <param name="names">Table or view names to include in import action</param>
        /// <returns>An instance of <see cref="Database"/> class that represents an existing database in SQL Server instance</returns>
        public static async Task<Database> ImportTablesAndViewsAsync(ILogger<SqlServerDatabaseFactory> logger, string connectionString, params string[] names)
        {
            var databaseFactory = new SqlServerDatabaseFactory(logger)
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = connectionString
                }
            };

            using var connection = databaseFactory.GetConnection();

            await connection.OpenAsync();

            var database = SqlServerDatabase.CreateWithDefaults(connection.Database);

            database.ServerName = connection.DataSource;

            if (names.Length == 0)
                database.DbObjects.AddRange(await databaseFactory.GetDbObjectsAsync(connection));
            else
                database.DbObjects.AddRange((await databaseFactory.GetDbObjectsAsync(connection)).Where(item => names.Contains(item.FullName)));

            var tables = await databaseFactory
                .GetTablesAsync(connection, database.GetTables());

            database.Tables.AddRange(tables);

            var views = await databaseFactory
                .GetViewsAsync(connection, database.GetViews());

            database.Views.AddRange(views);

            return database;
        }

        /// <summary>
        /// Imports an existing database from SQL Server instance
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="names">Table or view names to include in import action</param>
        /// <returns>An instance of <see cref="Database"/> class that represents an existing database in SQL Server instance</returns>
        public static async Task<Database> ImportTablesAndViewsAsync(string connectionString, params string[] names)
            => await ImportTablesAndViewsAsync(null, connectionString, names);
    }
}

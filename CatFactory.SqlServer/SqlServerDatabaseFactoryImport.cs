using System;
using System.Linq;
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
        public static Database Import(ILogger<SqlServerDatabaseFactory> logger, string connectionString, params string[] exclusions)
        {
            var databaseFactory = new SqlServerDatabaseFactory(logger);

            databaseFactory.DatabaseImportSettings.ConnectionString = connectionString;
            databaseFactory.DatabaseImportSettings.Exclusions.AddRange(exclusions);

            return databaseFactory.Import();
        }

        /// <summary>
        /// Imports an existing database from SQL Server instance
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="exclusions">Database object names to exclude from import action</param>
        /// <returns>An instance of <see cref="Database"/> class that represents an existing database in SQL Server instance</returns>
        public static Database Import(string connectionString, params string[] exclusions)
            => Import(null, connectionString, exclusions);

        /// <summary>
        /// Imports an existing database from SQL Server instance
        /// </summary>
        /// <param name="logger">Instance for <see cref="Logger"/> class</param>
        /// <param name="connectionString">Connection string</param>
        /// <param name="tables">Table names to include in import action</param>
        /// <returns>An instance of <see cref="Database"/> class that represents an existing database in SQL Server instance</returns>
        public static Database ImportTables(ILogger<SqlServerDatabaseFactory> logger, string connectionString, params string[] tables)
        {
            var databaseFactory = new SqlServerDatabaseFactory(logger)
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = connectionString,
                    ImportViews = false
                }
            };

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                var database = new Database
                {
                    Name = connection.Database
                };

                if (tables.Length == 0)
                    database.DbObjects.AddRange(databaseFactory.GetDbObjects(connection).ToList());
                else
                    database.DbObjects.AddRange(databaseFactory.GetDbObjects(connection).Where(item => tables.Contains(item.FullName)).ToList());

                database.Tables.AddRange(databaseFactory.GetTables(connection, database.GetTables()).ToList());

                return database;
            }
        }

        /// <summary>
        /// Imports an existing database from SQL Server instance
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="tables">Table names to include in import action</param>
        /// <returns>An instance of <see cref="Database"/> class that represents an existing database in SQL Server instance</returns>
        public static Database ImportTables(string connectionString, params string[] tables)
            => ImportTables(null, connectionString, tables);

        /// <summary>
        /// Imports an existing database from SQL Server instance
        /// </summary>
        /// <param name="logger">Instance for <see cref="Logger"/> class</param>
        /// <param name="connectionString">Connection string</param>
        /// <param name="views">View names to include in import action</param>
        /// <returns>An instance of <see cref="Database"/> class that represents an existing database in SQL Server instance</returns>
        public static Database ImportViews(ILogger<SqlServerDatabaseFactory> logger, string connectionString, params string[] views)
        {
            var databaseFactory = new SqlServerDatabaseFactory(logger)
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = connectionString,
                    ImportTables = false
                }
            };

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                var database = new Database
                {
                    Name = connection.Database
                };

                if (views.Length == 0)
                    database.DbObjects.AddRange(databaseFactory.GetDbObjects(connection).ToList());
                else
                    database.DbObjects.AddRange(databaseFactory.GetDbObjects(connection).Where(item => views.Contains(item.FullName)).ToList());

                database.Views.AddRange(databaseFactory.GetViews(connection, database.GetViews()).ToList());

                return database;
            }
        }

        /// <summary>
        /// Imports an existing database from SQL Server instance
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="views">View names to include in import action</param>
        /// <returns>An instance of <see cref="Database"/> class that represents an existing database in SQL Server instance</returns>
        public static Database ImportViews(string connectionString, params string[] views)
            => ImportViews(null, connectionString, views);

        /// <summary>
        /// Imports an existing database from SQL Server instance
        /// </summary>
        /// <param name="logger">Instance for <see cref="Logger"/> class</param>
        /// <param name="connectionString">Connection string</param>
        /// <param name="names">Table or view names to include in import action</param>
        /// <returns>An instance of <see cref="Database"/> class that represents an existing database in SQL Server instance</returns>
        public static Database ImportTablesAndViews(ILogger<SqlServerDatabaseFactory> logger, string connectionString, params string[] names)
        {
            var databaseFactory = new SqlServerDatabaseFactory(logger)
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = connectionString
                }
            };

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                var database = new Database
                {
                    Name = connection.Database
                };

                if (names.Length == 0)
                    database.DbObjects.AddRange(databaseFactory.GetDbObjects(connection).ToList());
                else
                    database.DbObjects.AddRange(databaseFactory.GetDbObjects(connection).Where(item => names.Contains(item.FullName)).ToList());

                database.Tables.AddRange(databaseFactory.GetTables(connection, database.GetTables()).ToList());

                database.Views.AddRange(databaseFactory.GetViews(connection, database.GetViews()).ToList());

                return database;
            }
        }

        /// <summary>
        /// Imports an existing database from SQL Server instance
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="names">Table or view names to include in import action</param>
        /// <returns>An instance of <see cref="Database"/> class that represents an existing database in SQL Server instance</returns>
        public static Database ImportTablesAndViews(string connectionString, params string[] names)
            => ImportTablesAndViews(null, connectionString, names);
    }
}

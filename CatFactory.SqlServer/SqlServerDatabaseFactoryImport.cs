﻿using System;
using System.Linq;
using CatFactory.Mapping;
using Microsoft.Extensions.Logging;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SqlServerDatabaseFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="connectionString"></param>
        /// <param name="exclusions"></param>
        /// <returns></returns>
        public static Database Import(ILogger<SqlServerDatabaseFactory> logger, string connectionString, params string[] exclusions)
        {
            var databaseFactory = new SqlServerDatabaseFactory(logger);

            databaseFactory.DatabaseImportSettings.ConnectionString = connectionString;
            databaseFactory.DatabaseImportSettings.Exclusions.AddRange(exclusions);

            return databaseFactory.Import();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="exclusions"></param>
        /// <returns></returns>
        public static Database Import(string connectionString, params string[] exclusions)
            => Import(null, connectionString, exclusions);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="connectionString"></param>
        /// <param name="tables"></param>
        /// <returns></returns>
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

            var database = new Database();

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                database.Name = connection.Database;

                if (tables.Length == 0)
                    database.DbObjects.AddRange(databaseFactory.GetDbObjects(connection).ToList());
                else
                    database.DbObjects.AddRange(databaseFactory.GetDbObjects(connection).Where(item => tables.Contains(item.FullName)).ToList());

                database.Tables.AddRange(databaseFactory.GetTables(connection, database.GetTables()).ToList());
            }

            return database;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="tables"></param>
        /// <returns></returns>
        public static Database ImportTables(string connectionString, params string[] tables)
            => ImportTables(null, connectionString, tables);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="connectionString"></param>
        /// <param name="views"></param>
        /// <returns></returns>
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

            var database = new Database();

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                database.Name = connection.Database;

                if (views.Length == 0)
                    database.DbObjects.AddRange(databaseFactory.GetDbObjects(connection).ToList());
                else
                    database.DbObjects.AddRange(databaseFactory.GetDbObjects(connection).Where(item => views.Contains(item.FullName)).ToList());

                database.Views.AddRange(databaseFactory.GetViews(connection, database.GetViews()).ToList());
            }

            return database;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="views"></param>
        /// <returns></returns>
        public static Database ImportViews(string connectionString, params string[] views)
            => ImportViews(null, connectionString, views);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="connectionString"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        public static Database ImportTablesAndViews(ILogger<SqlServerDatabaseFactory> logger, string connectionString, params string[] names)
        {
            var databaseFactory = new SqlServerDatabaseFactory(logger)
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = connectionString
                }
            };

            var database = new Database();

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                database.Name = connection.Database;

                if (names.Length == 0)
                    database.DbObjects.AddRange(databaseFactory.GetDbObjects(connection).ToList());
                else
                    database.DbObjects.AddRange(databaseFactory.GetDbObjects(connection).Where(item => names.Contains(item.FullName)).ToList());

                database.Tables.AddRange(databaseFactory.GetTables(connection, database.GetTables()).ToList());

                database.Views.AddRange(databaseFactory.GetViews(connection, database.GetViews()).ToList());
            }

            return database;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        public static Database ImportTablesAndViews(string connectionString, params string[] names)
            => ImportTablesAndViews(null, connectionString, names);
    }
}

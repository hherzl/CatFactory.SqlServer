using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using CatFactory.Mapping;
using Microsoft.Extensions.Logging;

namespace CatFactory.SqlServer
{
    public partial class SqlServerDatabaseFactory : IDatabaseFactory
    {
        public static Database Import(ILogger<SqlServerDatabaseFactory> logger, string connectionString, params string[] exclusions)
        {
            var databaseFactory = new SqlServerDatabaseFactory(logger)
            {
                ConnectionString = connectionString
            };

            databaseFactory.ImportSettings.Exclusions.AddRange(exclusions);

            return databaseFactory.Import();
        }

        public static Database Import(string connectionString, params string[] exclusions)
            => Import(null, connectionString, exclusions);

        public static Database ImportTables(ILogger<SqlServerDatabaseFactory> logger, string connectionString, params string[] tables)
        {
            var databaseFactory = new SqlServerDatabaseFactory(logger)
            {
                ConnectionString = connectionString,
                ImportSettings = new DatabaseImportSettings
                {
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

        public static Database ImportTables(string connectionString, params string[] tables)
            => ImportTables(null, connectionString, tables);

        public static Database ImportViews(ILogger<SqlServerDatabaseFactory> logger, string connectionString, params string[] views)
        {
            var databaseFactory = new SqlServerDatabaseFactory(logger)
            {
                ConnectionString = connectionString,
                ImportSettings = new DatabaseImportSettings
                {
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

        public static Database ImportViews(string connectionString, params string[] views)
            => ImportViews(null, connectionString, views);

        public static Database ImportTablesAndViews(ILogger<SqlServerDatabaseFactory> logger, string connectionString, params string[] names)
        {
            var databaseFactory = new SqlServerDatabaseFactory(logger)
            {
                ConnectionString = connectionString
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

        public static Database ImportTablesAndViews(string connectionString, params string[] names)
            => ImportTablesAndViews(null, connectionString, names);

        protected ILogger Logger;

        public SqlServerDatabaseFactory()
        {
        }

        public SqlServerDatabaseFactory(ILogger<SqlServerDatabaseFactory> logger)
        {
            Logger = logger;
        }

        public string ConnectionString { get; set; }

        public DbConnection GetConnection()
            => new SqlConnection(ConnectionString);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DatabaseImportSettings m_importSettings;

        public DatabaseImportSettings ImportSettings
        {
            get
            {
                return m_importSettings ?? (m_importSettings = new DatabaseImportSettings());
            }
            set
            {
                m_importSettings = value;
            }
        }

        public virtual Database Import()
        {
            var database = new Database
            {
                SupportTransactions = true,
                Mappings = DatabaseTypeMapList.Definition
            };

            using (var connection = GetConnection())
            {
                connection.Open();

                database.Name = connection.Database;

                var dbObjects = GetDbObjects(connection).ToList();

                foreach (var dbObject in dbObjects)
                {
                    if (ImportSettings.Exclusions.Contains(dbObject.FullName))
                        continue;

                    database.DbObjects.Add(dbObject);
                }

                if (ImportSettings.ImportTables)
                {
                    Logger?.LogInformation("Importing tables for '{0}'...", database.Name);

                    foreach (var table in GetTables(connection, database.GetTables()))
                    {
                        if (ImportSettings.Exclusions.Contains(table.FullName))
                            continue;

                        ImportDescription(connection, table);

                        database.Tables.Add(table);
                    }
                }

                if (ImportSettings.ImportViews)
                {
                    Logger?.LogInformation("Importing views for '{0}'...", database.Name);

                    foreach (var view in GetViews(connection, database.GetViews()))
                    {
                        if (ImportSettings.Exclusions.Contains(view.FullName))
                            continue;

                        ImportDescription(connection, view);

                        database.Views.Add(view);
                    }
                }

                if (ImportSettings.ImportStoredProcedures)
                {
                    Logger?.LogInformation("Importing stored procedures for '{0}'...", database.Name);

                    foreach (var storedProcedure in GetStoredProcedures(connection, database.GetStoredProcedures()))
                    {
                        if (ImportSettings.Exclusions.Contains(storedProcedure.FullName))
                            continue;

                        ImportDescription(connection, storedProcedure);

                        database.StoredProcedures.Add(storedProcedure);
                    }
                }

                if (ImportSettings.ImportTableFunctions)
                {
                    Logger?.LogInformation("Importing table functions for '{0}'...", database.Name);

                    foreach (var tableFunction in GetTableFunctions(connection, database.GetTableFunctions()))
                    {
                        if (ImportSettings.Exclusions.Contains(tableFunction.FullName))
                            continue;

                        ImportDescription(connection, tableFunction);

                        database.TableFunctions.Add(tableFunction);
                    }
                }

                if (ImportSettings.ImportScalarFunctions)
                {
                    Logger?.LogInformation("Importing scalar functions for '{0}'...", database.Name);

                    foreach (var scalarFunction in GetScalarFunctions(connection, database.GetScalarFunctions()))
                    {
                        if (ImportSettings.Exclusions.Contains(scalarFunction.FullName))
                            continue;

                        ImportDescription(connection, scalarFunction);

                        database.ScalarFunctions.Add(scalarFunction);
                    }
                }
            }

            return database;
        }

        protected virtual IEnumerable<DbObject> GetDbObjects(DbConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.Connection = connection;
                command.CommandText = ImportSettings.ImportCommandText;

                using (var dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        yield return new DbObject
                        {
                            Schema = dataReader.GetString(0),
                            Name = dataReader.GetString(1),
                            Type = dataReader.GetString(2)
                        };
                    }
                }
            }
        }

        protected virtual IEnumerable<Table> GetTables(DbConnection connection, IEnumerable<DbObject> tables)
        {
            foreach (var dbObject in tables)
            {
                using (var command = connection.CreateCommand())
                {
                    var table = new Table
                    {
                        Schema = dbObject.Schema,
                        Name = dbObject.Name
                    };

                    command.Connection = connection;
                    command.CommandText = string.Format("sp_help '{0}'", dbObject.FullName);

                    var queryResults = new List<QueryResult>();

                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.NextResult())
                        {
                            var queryResult = new QueryResult();

                            while (dataReader.Read())
                            {
                                var names = SqlServerDatabaseFactoryHelper.GetNames(dataReader).ToList();

                                var row = new Dictionary<string, object>();

                                for (var i = 0; i < names.Count; i++)
                                    row.Add(names[i], dataReader.GetValue(i));

                                queryResult.Items.Add(row);
                            }

                            queryResults.Add(queryResult);
                        }

                        foreach (var result in queryResults)
                        {
                            foreach (var item in result.Items)
                            {
                                if (item.ContainsKey("Column_name"))
                                    AddColumn(table, item);
                                else if (item.ContainsKey("Identity"))
                                    SetIdentity(table, item);
                                else if (item.ContainsKey("RowGuidCol"))
                                    SetRowGuidCol(table, item);
                                else if (item.ContainsKey("index_name"))
                                    AddIndexToTable(table, item);
                                else if (item.ContainsKey("constraint_type"))
                                    AddContraintToTable(table, item);
                            }
                        }

                        SetConstraintsFromConstraintDetails(table);

                        yield return table;
                    }
                }
            }
        }

        protected virtual void AddColumn(Table table, IDictionary<string, object> dictionary)
        {
            var column = SqlServerDatabaseFactoryHelper.GetColumn(dictionary);

            if (!ImportSettings.ExclusionTypes.Contains(column.Type))
                table.Columns.Add(column);
        }

        protected virtual void AddColumn(View view, IDictionary<string, object> dictionary)
        {
            var column = SqlServerDatabaseFactoryHelper.GetColumn(dictionary);

            if (!ImportSettings.ExclusionTypes.Contains(column.Type))
                view.Columns.Add(column);
        }

        protected virtual void AddColumn(TableFunction tableFunction, IDictionary<string, object> dictionary)
        {
            var column = SqlServerDatabaseFactoryHelper.GetColumn(dictionary);

            if (!ImportSettings.ExclusionTypes.Contains(column.Type))
                tableFunction.Columns.Add(column);
        }

        protected virtual void AddParameter(StoredProcedure storedProcedure, IDictionary<string, object> dictionary)
        {
            storedProcedure.Parameters.Add(SqlServerDatabaseFactoryHelper.GetParameter(dictionary));
        }

        protected virtual void AddParameter(ScalarFunction scalarFunction, IDictionary<string, object> dictionary)
        {
            scalarFunction.Parameters.Add(SqlServerDatabaseFactoryHelper.GetParameter(dictionary));
        }

        protected virtual void AddParameter(TableFunction tableFunction, IDictionary<string, object> dictionary)
        {
            tableFunction.Parameters.Add(SqlServerDatabaseFactoryHelper.GetParameter(dictionary));
        }

        protected virtual void SetIdentity(Table table, IDictionary<string, object> dictionary)
        {
            var identity = string.Concat(dictionary["Identity"]);

            if (string.Compare(identity, "No identity column defined.", true) != 0)
                table.Identity = new Identity(identity, Convert.ToInt32(dictionary["Seed"]), Convert.ToInt32(dictionary["Increment"]));
        }

        protected virtual void SetIdentity(View view, IDictionary<string, object> dictionary)
        {
            var identity = string.Concat(dictionary["Identity"]);

            if (string.Compare(identity, "No identity column defined.", true) != 0)
                view.Identity = new Identity(identity, Convert.ToInt32(dictionary["Seed"]), Convert.ToInt32(dictionary["Increment"]));
        }

        protected virtual void SetIdentity(TableFunction tableFunction, IDictionary<string, object> dictionary)
        {
            var identity = string.Concat(dictionary["Identity"]);

            if (string.Compare(identity, "No identity column defined.", true) != 0)
                tableFunction.Identity = new Identity(identity, Convert.ToInt32(dictionary["Seed"]), Convert.ToInt32(dictionary["Increment"]));
        }

        protected virtual void SetRowGuidCol(Table table, IDictionary<string, object> dictionary)
        {
            table.RowGuidCol = new RowGuidCol
            {
                Name = string.Concat(dictionary["RowGuidCol"])
            };
        }

        protected virtual void SetRowGuidCol(View view, IDictionary<string, object> dictionary)
        {
            view.RowGuidCol = new RowGuidCol
            {
                Name = string.Concat(dictionary["RowGuidCol"])
            };
        }

        protected virtual void AddIndexToTable(Table table, IDictionary<string, object> dictionary)
        {
            table.Indexes.Add(new Index
            {
                IndexName = string.Concat(dictionary["index_name"]),
                IndexDescription = string.Concat(dictionary["index_description"]),
                IndexKeys = string.Concat(dictionary["index_keys"])
            });
        }

        protected virtual void AddContraintToTable(Table table, IDictionary<string, object> dictionary)
        {
            table.ConstraintDetails.Add(new ConstraintDetail
            {
                ConstraintType = string.Concat(dictionary["constraint_type"]),
                ConstraintName = string.Concat(dictionary["constraint_name"]),
                DeleteAction = string.Concat(dictionary["delete_action"]),
                UpdateAction = string.Concat(dictionary["update_action"]),
                StatusEnabled = string.Concat(dictionary["status_enabled"]),
                StatusForReplication = string.Concat(dictionary["status_for_replication"]),
                ConstraintKeys = string.Concat(dictionary["constraint_keys"])
            });
        }

        protected virtual void SetConstraintsFromConstraintDetails(Table table)
        {
            foreach (var constraintDetail in table.ConstraintDetails)
            {
                if (constraintDetail.ConstraintType.Contains("PRIMARY KEY"))
                {
                    var key = string.Concat(constraintDetail.ConstraintKeys).Split(',').Select(item => item.Trim()).ToArray();

                    table.PrimaryKey = new PrimaryKey(key)
                    {
                        ConstraintName = constraintDetail.ConstraintName
                    };
                }
                else if (constraintDetail.ConstraintType.Contains("FOREIGN KEY"))
                {
                    var key = constraintDetail.ConstraintKeys.ToString().Split(',').Select(item => item.Trim()).ToArray();

                    table.ForeignKeys.Add(new ForeignKey(key)
                    {
                        ConstraintName = constraintDetail.ConstraintName
                    });
                }
                else if (constraintDetail.ConstraintKeys.Contains("REFERENCES"))
                {
                    var value = constraintDetail.ConstraintKeys.Replace("REFERENCES", string.Empty);

                    table.ForeignKeys.Last().References = value.Substring(0, value.IndexOf("(")).Trim();
                }
                else if (constraintDetail.ConstraintType.Contains("UNIQUE"))
                {
                    var key = constraintDetail.ConstraintKeys.ToString().Split(',').Select(item => item.Trim()).ToArray();

                    table.Uniques.Add(new Unique(key)
                    {
                        ConstraintName = constraintDetail.ConstraintName
                    });
                }
                else if (constraintDetail.ConstraintType.Contains("CHECK"))
                {
                    var key = constraintDetail.ConstraintKeys.ToString();

                    table.Checks.Add(new Check(key)
                    {
                        ConstraintName = constraintDetail.ConstraintName
                    });
                }
            }
        }

        private void ImportDescription(DbConnection connection, ITable table)
        {
            if (ImportSettings.ImportMSDescription)
            {
                table.Type = "table";

                foreach (var extendProperty in connection.GetMsDescriptionForDbObject(table))
                    table.Description = string.Concat(extendProperty.Value);

                foreach (var column in table.Columns)
                {
                    foreach (var extendProperty in connection.GetMsDescriptionForColumn(table, column))
                        column.Description = string.Concat(extendProperty.Value);
                }
            }
        }

        protected virtual IEnumerable<View> GetViews(DbConnection connection, IEnumerable<DbObject> views)
        {
            foreach (var dbObject in views)
            {
                using (var command = connection.CreateCommand())
                {
                    var view = new View
                    {
                        Schema = dbObject.Schema,
                        Name = dbObject.Name
                    };

                    command.Connection = connection;
                    command.CommandText = string.Format("sp_help '{0}'", dbObject.FullName);

                    var queryResults = new List<QueryResult>();

                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.NextResult())
                        {
                            var queryResult = new QueryResult();

                            while (dataReader.Read())
                            {
                                var names = SqlServerDatabaseFactoryHelper.GetNames(dataReader).ToList();

                                var row = new Dictionary<string, object>();

                                for (var i = 0; i < names.Count; i++)
                                    row.Add(names[i], dataReader.GetValue(i));

                                queryResult.Items.Add(row);
                            }

                            queryResults.Add(queryResult);
                        }

                        foreach (var result in queryResults)
                        {
                            foreach (var item in result.Items)
                            {
                                if (item.ContainsKey("Column_name"))
                                    AddColumn(view, item);
                                else if (item.ContainsKey("Identity"))
                                    SetIdentity(view, item);
                                else if (item.ContainsKey("RowGuidCol"))
                                    SetRowGuidCol(view, item);
                            }
                        }

                        yield return view;
                    }
                }
            }
        }

        private void ImportDescription(DbConnection connection, IView view)
        {
            if (ImportSettings.ImportMSDescription)
            {
                view.Type = "view";

                foreach (var extendProperty in connection.GetMsDescriptionForDbObject(view))
                    view.Description = string.Concat(extendProperty.Value);

                foreach (var column in view.Columns)
                {
                    foreach (var extendProperty in connection.GetMsDescriptionForColumn(view, column))
                        column.Description = string.Concat(extendProperty.Value);
                }
            }
        }

        protected virtual IEnumerable<StoredProcedure> GetStoredProcedures(DbConnection connection, IEnumerable<DbObject> storedProcedures)
        {
            foreach (var dbObject in storedProcedures)
            {
                using (var command = connection.CreateCommand())
                {
                    var storedProcedure = new StoredProcedure
                    {
                        Schema = dbObject.Schema,
                        Name = dbObject.Name
                    };

                    command.Connection = connection;
                    command.CommandText = string.Format("sp_help '{0}'", dbObject.FullName);

                    var queryResults = new List<QueryResult>();

                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.NextResult())
                        {
                            var queryResult = new QueryResult();

                            while (dataReader.Read())
                            {
                                var names = SqlServerDatabaseFactoryHelper.GetNames(dataReader).ToList();

                                var row = new Dictionary<string, object>();

                                for (var i = 0; i < names.Count; i++)
                                    row.Add(names[i], dataReader.GetValue(i));

                                queryResult.Items.Add(row);
                            }

                            queryResults.Add(queryResult);
                        }

                        foreach (var result in queryResults)
                        {
                            foreach (var item in result.Items)
                            {
                                if (item.ContainsKey("Parameter_name"))
                                    AddParameter(storedProcedure, item);
                            }
                        }

                        yield return storedProcedure;
                    }
                }
            }
        }

        private void ImportDescription(DbConnection connection, StoredProcedure storedProcedure)
        {
            if (ImportSettings.ImportMSDescription)
            {
                foreach (var extendProperty in connection.GetMsDescriptionForDbObject(storedProcedure))
                    storedProcedure.Description = string.Concat(extendProperty.Value);
            }
        }

        protected virtual IEnumerable<TableFunction> GetTableFunctions(DbConnection connection, IEnumerable<DbObject> tableFunctions)
        {
            foreach (var dbObject in tableFunctions)
            {
                using (var command = connection.CreateCommand())
                {
                    var tableFunction = new TableFunction
                    {
                        Schema = dbObject.Schema,
                        Name = dbObject.Name
                    };

                    command.Connection = connection;
                    command.CommandText = string.Format("sp_help '{0}'", dbObject.FullName);

                    var queryResults = new List<QueryResult>();

                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.NextResult())
                        {
                            var queryResult = new QueryResult();

                            while (dataReader.Read())
                            {
                                var names = SqlServerDatabaseFactoryHelper.GetNames(dataReader).ToList();

                                var row = new Dictionary<string, object>();

                                for (var i = 0; i < names.Count; i++)
                                    row.Add(names[i], dataReader.GetValue(i));

                                queryResult.Items.Add(row);
                            }

                            queryResults.Add(queryResult);
                        }

                        foreach (var result in queryResults)
                        {
                            foreach (var item in result.Items)
                            {
                                if (item.ContainsKey("Column_name"))
                                    AddColumn(tableFunction, item);
                                else if (item.ContainsKey("Identity"))
                                    SetIdentity(tableFunction, item);
                                else if (item.ContainsKey("Parameter_name"))
                                    AddParameter(tableFunction, item);
                            }
                        }

                        yield return tableFunction;
                    }
                }
            }
        }

        private void ImportDescription(DbConnection connection, TableFunction tableFunction)
        {
            if (ImportSettings.ImportMSDescription)
            {
                foreach (var extendProperty in connection.GetMsDescriptionForDbObject(tableFunction))
                    tableFunction.Description = string.Concat(extendProperty.Value);
            }
        }

        protected virtual IEnumerable<ScalarFunction> GetScalarFunctions(DbConnection connection, IEnumerable<DbObject> scalarFunctions)
        {
            foreach (var dbObject in scalarFunctions)
            {
                using (var command = connection.CreateCommand())
                {
                    var scalarFunction = new ScalarFunction
                    {
                        Schema = dbObject.Schema,
                        Name = dbObject.Name
                    };

                    command.Connection = connection;
                    command.CommandText = string.Format("sp_help '{0}'", dbObject.FullName);

                    var queryResults = new List<QueryResult>();

                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.NextResult())
                        {
                            var queryResult = new QueryResult();

                            while (dataReader.Read())
                            {
                                var names = SqlServerDatabaseFactoryHelper.GetNames(dataReader).ToList();

                                var row = new Dictionary<string, object>();

                                for (var i = 0; i < names.Count; i++)
                                    row.Add(names[i], dataReader.GetValue(i));

                                queryResult.Items.Add(row);
                            }

                            queryResults.Add(queryResult);
                        }

                        foreach (var result in queryResults)
                        {
                            foreach (var item in result.Items)
                            {
                                if (item.ContainsKey("Parameter_name"))
                                    AddParameter(scalarFunction, item);
                            }
                        }

                        yield return scalarFunction;
                    }
                }
            }
        }

        private void ImportDescription(DbConnection connection, ScalarFunction scalarFunction)
        {
            if (ImportSettings.ImportMSDescription)
            {
                foreach (var extendProperty in connection.GetMsDescriptionForDbObject(scalarFunction))
                    scalarFunction.Description = string.Concat(extendProperty.Value);
            }
        }
    }
}

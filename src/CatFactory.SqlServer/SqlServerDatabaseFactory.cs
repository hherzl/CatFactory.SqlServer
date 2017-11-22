using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using CatFactory.Mapping;
using Microsoft.Extensions.Logging;

namespace CatFactory.SqlServer
{
    public class SqlServerDatabaseFactory : IDatabaseFactory
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
            var databaseFactory = new SqlServerDatabaseFactory
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
                {
                    database.DbObjects.AddRange(databaseFactory.GetDbObjects(connection).ToList());
                }
                else
                {
                    database.DbObjects.AddRange(databaseFactory.GetDbObjects(connection).Where(item => tables.Contains(item.FullName)).ToList());
                }

                database.Tables.AddRange(databaseFactory.GetTables(connection, database.GetTables()).ToList());
            }

            return database;
        }

        public static Database ImportTables(string connectionString, params string[] tables)
            => ImportTables(null, connectionString, tables);

        public static Database ImportViews(ILogger<SqlServerDatabaseFactory> logger, string connectionString, params string[] views)
        {
            var databaseFactory = new SqlServerDatabaseFactory
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
                {
                    database.DbObjects.AddRange(databaseFactory.GetDbObjects(connection).ToList());
                }
                else
                {
                    database.DbObjects.AddRange(databaseFactory.GetDbObjects(connection).Where(item => views.Contains(item.FullName)).ToList());
                }

                database.Views.AddRange(databaseFactory.GetViews(connection, database.GetViews()).ToList());
            }

            return database;
        }

        public static Database ImportViews(string connectionString, params string[] views)
            => ImportViews(null, connectionString, views);

        public static Database ImportTablesAndViews(ILogger<SqlServerDatabaseFactory> logger, string connectionString, params string[] names)
        {
            var databaseFactory = new SqlServerDatabaseFactory
            {
                ConnectionString = connectionString
            };

            var database = new Database();

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                database.Name = connection.Database;

                if (names.Length == 0)
                {
                    database.DbObjects.AddRange(databaseFactory.GetDbObjects(connection).ToList());
                }
                else
                {
                    database.DbObjects.AddRange(databaseFactory.GetDbObjects(connection).Where(item => names.Contains(item.FullName)).ToList());
                }

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

        public DatabaseImportSettings ImportSettings { get; set; } = new DatabaseImportSettings();

        public virtual Database Import()
        {
            var database = new Database();

            var extendPropertyRepository = new ExtendPropertyRepository();

            using (var connection = GetConnection())
            {
                connection.Open();

                database.Name = connection.Database;

                var dbObjects = GetDbObjects(connection).ToList();

                foreach (var dbObject in dbObjects)
                {
                    if (ImportSettings.Exclusions.Contains(dbObject.FullName))
                    {
                        continue;
                    }

                    database.DbObjects.Add(dbObject);
                }

                if (ImportSettings.ImportTables)
                {
                    Logger?.LogInformation("Importing tables for '{0}'...", database.Name);

                    foreach (var table in GetTables(connection, database.GetTables()))
                    {
                        if (ImportSettings.Exclusions.Contains(table.FullName))
                        {
                            continue;
                        }

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
                        {
                            continue;
                        }

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
                        {
                            continue;
                        }

                        ImportDescription(connection, storedProcedure);

                        database.StoredProcedures.Add(storedProcedure);
                    }
                }

                if (ImportSettings.ImportScalarFunctions)
                {
                    Logger?.LogInformation("Importing scalar functions for '{0}'...", database.Name);

                    foreach (var scalarFunction in GetScalarFunctions(connection, database.GetScalarFunctions()))
                    {
                        if (ImportSettings.Exclusions.Contains(scalarFunction.FullName))
                        {
                            continue;
                        }

                        ImportDescription(connection, scalarFunction);

                        database.ScalarFunctions.Add(scalarFunction);
                    }
                }

                if (ImportSettings.ImportTableFunctions)
                {
                    Logger?.LogInformation("Importing table functions for '{0}'...", database.Name);

                    foreach (var tableFunction in GetTableFunctions(connection, database.GetTableFunctions()))
                    {
                        if (ImportSettings.Exclusions.Contains(tableFunction.FullName))
                        {
                            continue;
                        }

                        ImportDescription(connection, tableFunction);

                        database.TableFunctions.Add(tableFunction);
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
                        var dbObj = new DbObject
                        {
                            Schema = dataReader.GetString(0),
                            Name = dataReader.GetString(1),
                            Type = dataReader.GetString(2)
                        };

                        yield return dbObj;
                    }
                }
            }
        }

        protected virtual IEnumerable<Table> GetTables(DbConnection connection, IEnumerable<DbObject> tables)
        {
            foreach (var item in tables)
            {
                using (var command = connection.CreateCommand())
                {
                    var table = new Table
                    {
                        Schema = item.Schema,
                        Name = item.Name
                    };

                    command.Connection = connection;
                    command.CommandText = string.Format("sp_help '{0}'", item.FullName);

                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            dataReader.NextResult();

                            if (dataReader.HasRows && dataReader.GetName(0) == "Column_name")
                            {
                                AddColumnsToTable(table, dataReader);
                            }

                            dataReader.NextResult();

                            if (dataReader.HasRows && dataReader.GetName(0) == "Identity")
                            {
                                dataReader.Read();

                                SetIdentityToTable(table, dataReader);
                            }

                            dataReader.NextResult();

                            if (dataReader.HasRows && dataReader.GetName(0) == "constraint_type")
                            {
                                AddContraintsToTable(table, dataReader);
                            }

                        }

                        yield return table;
                    }
                }
            }
        }

        protected virtual void AddColumnsToTable(Table table, DbDataReader dataReader)
        {
            while (dataReader.Read())
            {
                var column = new Column();

                column.Name = string.Concat(dataReader["Column_name"]);
                column.Type = string.Concat(dataReader["Type"]);
                column.Length = int.Parse(string.Concat(dataReader["Length"]));
                column.Prec = string.Concat(dataReader["Prec"]).Trim().Length == 0 ? default(short) : short.Parse(string.Concat(dataReader["Prec"]));
                column.Scale = string.Concat(dataReader["Scale"]).Trim().Length == 0 ? default(short) : short.Parse(string.Concat(dataReader["Scale"]));
                column.Nullable = string.Compare(string.Concat(dataReader["Nullable"]), "yes", true) == 0 ? true : false;
                column.Collation = string.Concat(dataReader["Collation"]);

                if (ImportSettings.ExclusionTypes.Contains(column.Type))
                {
                    continue;
                }

                table.Columns.Add(column);
            }
        }

        protected virtual void SetIdentityToTable(Table table, DbDataReader dataReader)
        {
            var identity = string.Concat(dataReader["Identity"]);

            if (string.Compare(identity, "No identity column defined.", true) != 0)
            {
                table.Identity = new Identity(identity, Convert.ToInt32(dataReader["Seed"]), Convert.ToInt32(dataReader["Increment"]));
            }
        }

        protected virtual void AddContraintsToTable(Table table, DbDataReader dataReader)
        {
            while (dataReader.Read())
            {
                if (string.Concat(dataReader["constraint_type"]).Contains("PRIMARY KEY"))
                {
                    var key = string.Concat(dataReader["constraint_keys"]).Split(',').Select(item => item.Trim()).ToArray();

                    table.PrimaryKey = new PrimaryKey(key)
                    {
                        ConstraintName = string.Concat(dataReader["constraint_name"])
                    };
                }
                else if (string.Concat(dataReader["constraint_type"]).Contains("FOREIGN KEY"))
                {
                    var key = dataReader["constraint_keys"].ToString().Split(',').Select(item => item.Trim()).ToArray();

                    table.ForeignKeys.Add(new ForeignKey(key)
                    {
                        ConstraintName = string.Concat(dataReader["constraint_name"])
                    });
                }
                else if (string.Concat(dataReader["constraint_keys"]).Contains("REFERENCES"))
                {
                    var value = string.Concat(dataReader["constraint_keys"]).Replace("REFERENCES", string.Empty);

                    table.ForeignKeys[table.ForeignKeys.Count - 1].References = value.Substring(0, value.IndexOf("(")).Trim();
                }
                else if (string.Concat(dataReader["constraint_type"]).Contains("UNIQUE"))
                {
                    var key = dataReader["constraint_keys"].ToString().Split(',').Select(item => item.Trim()).ToArray();

                    table.Uniques.Add(new Unique(key)
                    {
                        ConstraintName = string.Concat(dataReader["constraint_name"])
                    });
                }
                else if (string.Concat(dataReader["constraint_type"]).Contains("CHECK"))
                {
                    var key = dataReader["constraint_keys"].ToString();

                    table.Checks.Add(new Check(key)
                    {
                        ConstraintName = string.Concat(dataReader["constraint_name"])
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
                {
                    table.Description = string.Concat(extendProperty.Value);
                }

                foreach (var column in table.Columns)
                {
                    foreach (var extendProperty in connection.GetMsDescriptionForColumn(table, column))
                    {
                        column.Description = string.Concat(extendProperty.Value);
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
                {
                    view.Description = string.Concat(extendProperty.Value);
                }

                foreach (var column in view.Columns)
                {
                    foreach (var extendProperty in connection.GetMsDescriptionForColumn(view, column))
                    {
                        column.Description = string.Concat(extendProperty.Value);
                    }
                }
            }
        }

        private void ImportDescription(DbConnection connection, StoredProcedure storedProcedure)
        {
            if (ImportSettings.ImportMSDescription)
            {
                // todo: add interface for stored procedure

                //var dbObject = dbObjects.First(item => item.FullName == storedProcedure.FullName);

                //foreach (var extendProperty in connection.GetMsDescriptionForDbObject(dbObject))
                //{
                //    storedProcedure.Description = string.Concat(extendProperty.Value);
                //}
            }
        }

        private void ImportDescription(DbConnection connection, ScalarFunction scalarFuntion)
        {
            if (ImportSettings.ImportMSDescription)
            {
                // todo: add interface for scalar function

                //var dbObject = dbObjects.First(item => item.FullName == scalarFunction.FullName);

                //foreach (var extendProperty in connection.GetMsDescriptionForDbObject(dbObject))
                //{
                //    scalarFunction.Description = string.Concat(extendProperty.Value);
                //}
            }
        }

        private void ImportDescription(DbConnection connection, TableFunction tableFunction)
        {
            if (ImportSettings.ImportMSDescription)
            {
                // todo: add interface for table function

                //var dbObject = dbObjects.First(item => item.FullName == tableFunction.FullName);

                //foreach (var extendProperty in connection.GetMsDescriptionForDbObject(dbObject))
                //{
                //    tableFunction.Description = string.Concat(extendProperty.Value);
                //}
            }
        }

        protected virtual IEnumerable<View> GetViews(DbConnection connection, IEnumerable<DbObject> views)
        {
            foreach (var item in views)
            {
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = string.Format("sp_help '{0}'", item.FullName);

                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var view = new View
                            {
                                Schema = item.Schema,
                                Name = item.Name
                            };

                            dataReader.NextResult();

                            while (dataReader.Read())
                            {
                                var column = new Column();

                                column.Name = string.Concat(dataReader["Column_name"]);
                                column.Type = string.Concat(dataReader["Type"]);
                                column.Length = int.Parse(string.Concat(dataReader["Length"]));
                                column.Prec = string.Concat(dataReader["Prec"]).Trim().Length == 0 ? default(short) : short.Parse(string.Concat(dataReader["Prec"]));
                                column.Scale = string.Concat(dataReader["Scale"]).Trim().Length == 0 ? default(short) : short.Parse(string.Concat(dataReader["Scale"]));
                                column.Nullable = string.Compare(string.Concat(dataReader["Nullable"]), "yes", true) == 0 ? true : false;
                                column.Collation = string.Concat(dataReader["Collation"]);

                                if (ImportSettings.ExclusionTypes.Contains(column.Type))
                                {
                                    continue;
                                }

                                view.Columns.Add(column);
                            }

                            yield return view;
                        }
                    }
                }
            }
        }

        protected virtual IEnumerable<StoredProcedure> GetStoredProcedures(DbConnection connection, IEnumerable<DbObject> storedProcedures)
        {
            foreach (var item in storedProcedures)
            {
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = string.Format("sp_help '{0}'", item.FullName);

                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var storedProcedure = new StoredProcedure
                            {
                                Schema = item.Schema,
                                Name = item.Name
                            };

                            dataReader.NextResult();

                            while (dataReader.Read())
                            {
                                var procedureParameter = new Parameter();

                                procedureParameter.Name = string.Concat(dataReader["Parameter_name"]);
                                procedureParameter.Type = string.Concat(dataReader["Type"]);
                                procedureParameter.Length = short.Parse(string.Concat(dataReader["Length"]));
                                procedureParameter.Prec = string.Concat(dataReader["Prec"]).Trim().Length == 0 ? default(int) : int.Parse(string.Concat(dataReader["Prec"]));
                                procedureParameter.ParamOrder = string.Concat(dataReader["Param_order"]).Trim().Length == 0 ? default(int) : int.Parse(string.Concat(dataReader["Param_order"]));
                                procedureParameter.Collation = string.Concat(dataReader["Collation"]);

                                storedProcedure.Parameters.Add(procedureParameter);
                            }

                            yield return storedProcedure;
                        }
                    }
                }
            }
        }

        protected virtual IEnumerable<ScalarFunction> GetScalarFunctions(DbConnection connection, IEnumerable<DbObject> scalarFunctions)
        {
            foreach (var item in scalarFunctions)
            {
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = string.Format("sp_help '{0}'", item.FullName);

                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var scalarFunction = new ScalarFunction
                            {
                                Schema = item.Schema,
                                Name = item.Name
                            };

                            dataReader.NextResult();

                            while (dataReader.Read())
                            {
                                var procedureParameter = new Parameter();

                                procedureParameter.Name = string.Concat(dataReader["Parameter_name"]);
                                procedureParameter.Type = string.Concat(dataReader["Type"]);
                                procedureParameter.Length = short.Parse(string.Concat(dataReader["Length"]));
                                procedureParameter.Prec = string.Concat(dataReader["Prec"]).Trim().Length == 0 ? default(int) : int.Parse(string.Concat(dataReader["Prec"]));
                                procedureParameter.ParamOrder = string.Concat(dataReader["Param_order"]).Trim().Length == 0 ? default(int) : int.Parse(string.Concat(dataReader["Param_order"]));
                                procedureParameter.Collation = string.Concat(dataReader["Collation"]);

                                scalarFunction.Parameters.Add(procedureParameter);
                            }

                            yield return scalarFunction;
                        }
                    }
                }
            }
        }

        protected virtual IEnumerable<TableFunction> GetTableFunctions(DbConnection connection, IEnumerable<DbObject> tableFunctions)
        {
            foreach (var item in tableFunctions)
            {
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = string.Format("sp_help '{0}'", item.FullName);

                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var tableFunction = new TableFunction
                            {
                                Schema = item.Schema,
                                Name = item.Name
                            };

                            dataReader.NextResult();

                            while (dataReader.Read())
                            {
                                var column = new Column();

                                column.Name = string.Concat(dataReader["Column_name"]);
                                column.Type = string.Concat(dataReader["Type"]);
                                column.Length = int.Parse(string.Concat(dataReader["Length"]));
                                column.Prec = string.Concat(dataReader["Prec"]).Trim().Length == 0 ? default(short) : short.Parse(string.Concat(dataReader["Prec"]));
                                column.Scale = string.Concat(dataReader["Scale"]).Trim().Length == 0 ? default(short) : short.Parse(string.Concat(dataReader["Scale"]));
                                column.Nullable = string.Compare(string.Concat(dataReader["Nullable"]), "yes", true) == 0 ? true : false;
                                column.Collation = string.Concat(dataReader["Collation"]);

                                if (ImportSettings.ExclusionTypes.Contains(column.Type))
                                {
                                    continue;
                                }

                                tableFunction.Columns.Add(column);
                            }

                            yield return tableFunction;
                        }
                    }
                }
            }
        }
    }
}

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
            var dbFactory = new SqlServerDatabaseFactory(logger)
            {
                ConnectionString = connectionString
            };

            dbFactory.ImportSettings.Exclusions.AddRange(exclusions);

            return dbFactory.Import();
        }

        public static Database Import(string connectionString, params string[] exclusions)
            => Import(null, connectionString, exclusions);

        protected ILogger Logger;

        public SqlServerDatabaseFactory()
        {
        }

        public SqlServerDatabaseFactory(ILogger<SqlServerDatabaseFactory> logger)
        {
            Logger = logger;
        }

        public string ConnectionString { get; set; }

        public DatabaseImportSettings ImportSettings { get; set; } = new DatabaseImportSettings();

        public virtual Database Import()
        {
            var database = new Database();

            var extendPropertyRepository = new ExtendPropertyRepository();

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                database.Name = connection.Database;

                var dbObjects = GetDbObjecs(connection).ToList();

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

                    foreach (var table in ImportTables(database))
                    {
                        if (ImportSettings.Exclusions.Contains(table.FullName))
                        {
                            continue;
                        }

                        var dbObject = dbObjects.First(item => item.FullName == table.FullName);

                        if (ImportSettings.ImportMSDescription)
                        {
                            dbObject.Type = "table";

                            foreach (var extendProperty in connection.GetMsDescriptionForDbObject(dbObject))
                            {
                                table.Description = string.Concat(extendProperty.Value);
                            }

                            foreach (var column in table.Columns)
                            {
                                foreach (var extendProperty in connection.GetMsDescriptionForColumn(dbObject, column))
                                {
                                    column.Description = string.Concat(extendProperty.Value);
                                }
                            }
                        }

                        database.Tables.Add(table);
                    }
                }

                if (ImportSettings.ImportViews)
                {
                    Logger?.LogInformation("Importing views for '{0}'...", database.Name);

                    foreach (var view in ImportViews(database))
                    {
                        if (ImportSettings.Exclusions.Contains(view.FullName))
                        {
                            continue;
                        }

                        var dbObject = dbObjects.First(item => item.FullName == view.FullName);

                        if (ImportSettings.ImportMSDescription)
                        {
                            foreach (var extendProperty in connection.GetMsDescriptionForDbObject(dbObject))
                            {
                                view.Description = string.Concat(extendProperty.Value);
                            }

                            foreach (var column in view.Columns)
                            {
                                foreach (var extendProperty in connection.GetMsDescriptionForColumn(dbObject, column))
                                {
                                    column.Description = string.Concat(extendProperty.Value);
                                }
                            }
                        }

                        database.Views.Add(view);
                    }
                }

                if (ImportSettings.ImportStoredProcedures)
                {
                    Logger?.LogInformation("Importing stored procedures for '{0}'...", database.Name);

                    foreach (var storedProcedure in ImportStoredProcedures(database))
                    {
                        if (ImportSettings.Exclusions.Contains(storedProcedure.FullName))
                        {
                            continue;
                        }

                        var dbObject = dbObjects.First(item => item.FullName == storedProcedure.FullName);

                        foreach (var extendProperty in connection.GetMsDescriptionForDbObject(dbObject))
                        {
                            storedProcedure.Description = string.Concat(extendProperty.Value);
                        }

                        database.StoredProcedures.Add(storedProcedure);
                    }
                }

                if (ImportSettings.ImportScalarFunctions)
                {
                    Logger?.LogInformation("Importing scalar functions for '{0}'...", database.Name);

                    foreach (var scalarFunction in ImportScalarFunctions(database))
                    {
                        if (ImportSettings.Exclusions.Contains(scalarFunction.FullName))
                        {
                            continue;
                        }

                        var dbObject = dbObjects.First(item => item.FullName == scalarFunction.FullName);

                        foreach (var extendProperty in connection.GetMsDescriptionForDbObject(dbObject))
                        {
                            scalarFunction.Description = string.Concat(extendProperty.Value);
                        }

                        database.ScalarFunctions.Add(scalarFunction);
                    }
                }

                if (ImportSettings.ImportTableFunctions)
                {
                    Logger?.LogInformation("Importing table functions for '{0}'...", database.Name);

                    foreach (var tableFunction in ImportTableFunctions(database))
                    {
                        if (ImportSettings.Exclusions.Contains(tableFunction.FullName))
                        {
                            continue;
                        }

                        var dbObject = dbObjects.First(item => item.FullName == tableFunction.FullName);

                        foreach (var extendProperty in connection.GetMsDescriptionForDbObject(dbObject))
                        {
                            tableFunction.Description = string.Concat(extendProperty.Value);
                        }

                        database.TableFunctions.Add(tableFunction);
                    }
                }
            }

            return database;
        }

        protected virtual IEnumerable<DbObject> GetDbObjecs(DbConnection connection)
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

        protected virtual IEnumerable<Table> ImportTables(Database database)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                foreach (var item in database.GetTables())
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

                connection.Close();
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

        protected virtual IEnumerable<View> ImportViews(Database database)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                foreach (var item in database.GetViews())
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

                                    view.Columns.Add(column);
                                }

                                yield return view;
                            }
                        }
                    }
                }

                connection.Close();
            }

        }

        protected virtual IEnumerable<StoredProcedure> ImportStoredProcedures(Database database)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                foreach (var item in database.GetProcedures())
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

                connection.Close();
            }
        }

        protected virtual IEnumerable<ScalarFunction> ImportScalarFunctions(Database database)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                foreach (var item in database.GetProcedures())
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

                connection.Close();
            }
        }

        protected virtual IEnumerable<TableFunction> ImportTableFunctions(Database database)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                foreach (var item in database.GetProcedures())
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
                                    var procedureParameter = new Parameter();

                                    procedureParameter.Name = string.Concat(dataReader["Parameter_name"]);
                                    procedureParameter.Type = string.Concat(dataReader["Type"]);
                                    procedureParameter.Length = short.Parse(string.Concat(dataReader["Length"]));
                                    procedureParameter.Prec = string.Concat(dataReader["Prec"]).Trim().Length == 0 ? default(int) : int.Parse(string.Concat(dataReader["Prec"]));
                                    procedureParameter.ParamOrder = string.Concat(dataReader["Param_order"]).Trim().Length == 0 ? default(int) : int.Parse(string.Concat(dataReader["Param_order"]));
                                    procedureParameter.Collation = string.Concat(dataReader["Collation"]);

                                    tableFunction.Parameters.Add(procedureParameter);
                                }

                                yield return tableFunction;
                            }
                        }
                    }
                }

                connection.Close();
            }
        }
    }
}

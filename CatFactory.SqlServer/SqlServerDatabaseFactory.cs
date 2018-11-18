using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using CatFactory.ObjectRelationalMapping;
using CatFactory.ObjectRelationalMapping.Programmability;
using CatFactory.SqlServer.Features;
using Microsoft.Extensions.Logging;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// Implements all operations related to import database feature
    /// </summary>
    public partial class SqlServerDatabaseFactory : IDatabaseFactory
    {
        /// <summary>
        /// Gets a instance for <see cref="Logger"/> class
        /// </summary>
        /// <returns>An instance for <see cref="SqlServerDatabaseFactory"/> class</returns>
        public static ILogger<SqlServerDatabaseFactory> GetLogger()
            => LoggingHelper.GetLogger<SqlServerDatabaseFactory>();

        /// <summary>
        /// Gets the <see cref="Logger"/> instance
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Initializes a new instance for <see cref="SqlServerDatabaseFactory"/> class
        /// </summary>
        public SqlServerDatabaseFactory()
        {
        }

        /// <summary>
        /// Initializes a new instance for <see cref="SqlServerDatabaseFactory"/> class
        /// </summary>
        /// <param name="logger"><see cref="Logger"/> class</param>
        public SqlServerDatabaseFactory(ILogger<SqlServerDatabaseFactory> logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// Gets an instance for <see cref="DbConnection"/> class
        /// </summary>
        /// <returns>An instance of <see cref="SqlConnection"/> class</returns>
        public DbConnection GetConnection()
            => new SqlConnection(DatabaseImportSettings.ConnectionString);

        /// <summary>
        /// Gets or sets the connnection string
        /// </summary>
        [Obsolete("Set connection string in ImportSettings")]
        public string ConnectionString
        {
            get
            {
                return DatabaseImportSettings.ConnectionString;
            }
            set
            {
                DatabaseImportSettings.ConnectionString = value;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DatabaseImportSettings m_databaseImportSettings;

        /// <summary>
        /// Gets or sets the database import settings
        /// </summary>
        public DatabaseImportSettings DatabaseImportSettings
        {
            get
            {
                return m_databaseImportSettings ?? (m_databaseImportSettings = new DatabaseImportSettings());
            }
            set
            {
                m_databaseImportSettings = value;
            }
        }

        /// <summary>
        /// Gets or sets the import settings
        /// </summary>
        [Obsolete("Use DatabaseImportSettings property")]
        public DatabaseImportSettings ImportSettings
        {
            get
            {
                return DatabaseImportSettings;
            }
            set
            {
                DatabaseImportSettings = value;
            }
        }

        /// <summary>
        /// Imports an existing database from SQL Server instance using database import settings
        /// </summary>
        /// <returns>An instance of <see cref="Database"/> class that represents a database from SQL Server instance</returns>
        public virtual Database Import()
        {
            var database = new Database
            {
                DefaultSchema = "dbo",
                SupportTransactions = true,
                DatabaseTypeMaps = DatabaseTypeMapList.Definition,
                NamingConvention = new SqlServerDatabaseNamingConvention()
            };

            using (var connection = GetConnection())
            {
                connection.Open();

                database.Name = connection.Database;

                SqlServerDatabaseFactoryHelper.AddUserDefinedDataTypes(database, connection);

                var dbObjects = GetDbObjects(connection).ToList();

                foreach (var dbObject in dbObjects)
                {
                    if (DatabaseImportSettings.Exclusions.Contains(dbObject.FullName))
                        continue;

                    database.DbObjects.Add(dbObject);
                }

                if (DatabaseImportSettings.ExtendedProperties.Count > 0)
                {
                    Logger?.LogInformation("Importing extended properties for database...");

                    ImportExtendedProperties(connection, database);
                }

                if (DatabaseImportSettings.ImportTables)
                {
                    Logger?.LogInformation("Importing tables for '{0}'...", database.Name);

                    foreach (var table in GetTables(connection, database.GetTables()))
                    {
                        if (DatabaseImportSettings.Exclusions.Contains(table.FullName))
                            continue;

                        database.Tables.Add(table);
                    }

                    if (DatabaseImportSettings.ExtendedProperties.Count > 0)
                    {
                        Logger?.LogInformation("Importing extended properties for tables...");

                        foreach (var table in database.Tables)
                            ImportExtendedProperties(connection, table);
                    }
                }

                if (DatabaseImportSettings.ImportViews)
                {
                    Logger?.LogInformation("Importing views for '{0}'...", database.Name);

                    foreach (var view in GetViews(connection, database.GetViews()))
                    {
                        if (DatabaseImportSettings.Exclusions.Contains(view.FullName))
                            continue;

                        database.Views.Add(view);
                    }

                    if (DatabaseImportSettings.ExtendedProperties.Count > 0)
                    {
                        Logger?.LogInformation("Importing extended properties for views...");

                        foreach (var view in database.Views)
                            ImportExtendedProperties(connection, view);
                    }
                }

                if (DatabaseImportSettings.ImportStoredProcedures)
                {
                    Logger?.LogInformation("Importing stored procedures for '{0}'...", database.Name);

                    foreach (var storedProcedure in GetStoredProcedures(connection, database.GetStoredProcedures()))
                    {
                        if (DatabaseImportSettings.Exclusions.Contains(storedProcedure.FullName))
                            continue;

                        database.StoredProcedures.Add(storedProcedure);
                    }

                    Logger?.LogInformation("Getting first result sets for stored procedures...");

                    foreach (var storedProcedure in database.StoredProcedures)
                    {
                        foreach (var firstResultSet in SqlServerDatabaseFactoryHelper.GetFirstResultSetForObject(storedProcedure, connection))
                        {
                            storedProcedure.FirstResultSetsForObject.Add(firstResultSet);
                        }
                    }

                    if (DatabaseImportSettings.ExtendedProperties.Count > 0)
                    {
                        Logger?.LogInformation("Importing extended properties for stored procedures...");

                        foreach (var storedProcedure in database.StoredProcedures)
                            ImportExtendedProperties(connection, storedProcedure);
                    }
                }

                if (DatabaseImportSettings.ImportTableFunctions)
                {
                    Logger?.LogInformation("Importing table functions for '{0}'...", database.Name);

                    foreach (var tableFunction in GetTableFunctions(connection, database.GetTableFunctions()))
                    {
                        if (DatabaseImportSettings.Exclusions.Contains(tableFunction.FullName))
                            continue;

                        database.TableFunctions.Add(tableFunction);
                    }

                    if (DatabaseImportSettings.ExtendedProperties.Count > 0)
                    {
                        Logger?.LogInformation("Importing extended properties for table functions...");

                        foreach (var tableFunction in database.TableFunctions)
                            ImportExtendedProperties(connection, tableFunction);
                    }
                }

                if (DatabaseImportSettings.ImportScalarFunctions)
                {
                    Logger?.LogInformation("Importing scalar functions for '{0}'...", database.Name);

                    foreach (var scalarFunction in GetScalarFunctions(connection, database.GetScalarFunctions()))
                    {
                        if (DatabaseImportSettings.Exclusions.Contains(scalarFunction.FullName))
                            continue;

                        database.ScalarFunctions.Add(scalarFunction);
                    }

                    if (DatabaseImportSettings.ExtendedProperties.Count > 0)
                    {
                        Logger?.LogInformation("Importing extended properties for scalar functions...");

                        foreach (var scalarFunction in database.ScalarFunctions)
                            ImportExtendedProperties(connection, scalarFunction);
                    }
                }
            }

            return database;
        }

        /// <summary>
        /// Gets database objects from connection
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <returns>A sequence of <see cref="DbObject"/> class that represents objects in database</returns>
        protected virtual IEnumerable<DbObject> GetDbObjects(DbConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.Connection = connection;
                command.CommandText = DatabaseImportSettings.ImportCommandText;

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

        /// <summary>
        /// Gets tables from database connection
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="tables">Sequence of <see cref="DbObject"/> that represents tables</param>
        /// <returns>A sequence of <see cref="Table"/></returns>
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

                    var queryResults = new List<DynamicQueryResult>();

                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.NextResult())
                        {
                            var queryResult = new DynamicQueryResult();

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
                                    AddConstraintToTable(table, item);
                                else if (item.ContainsKey("Table is referenced by foreign key"))
                                    AddTableReferenceToTable(table, item);
                            }
                        }

                        SetConstraintsFromConstraintDetails(table);

                        yield return table;
                    }
                }
            }
        }

        /// <summary>
        /// Adds a column in table from row dictionary
        /// </summary>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <param name="dictionary">Dictionary from data reader</param>
        protected virtual void AddColumn(Table table, IDictionary<string, object> dictionary)
        {
            var column = SqlServerDatabaseFactoryHelper.GetColumn(dictionary);

            if (!DatabaseImportSettings.ExclusionTypes.Contains(column.Type))
                table.Columns.Add(column);
        }

        /// <summary>
        /// Adds a column in table from row dictionary
        /// </summary>
        /// <param name="view">Instance of <see cref="View"/> class</param>
        /// <param name="dictionary">Dictionary from data reader</param>
        protected virtual void AddColumn(View view, IDictionary<string, object> dictionary)
        {
            var column = SqlServerDatabaseFactoryHelper.GetColumn(dictionary);

            if (!DatabaseImportSettings.ExclusionTypes.Contains(column.Type))
                view.Columns.Add(column);
        }

        /// <summary>
        /// Adds a column in table function from row dictionary
        /// </summary>
        /// <param name="tableFunction">Instance of <see cref="TableFunction"/> class</param>
        /// <param name="dictionary">Dictionary from data reader</param>
        protected virtual void AddColumn(TableFunction tableFunction, IDictionary<string, object> dictionary)
        {
            var column = SqlServerDatabaseFactoryHelper.GetColumn(dictionary);

            if (!DatabaseImportSettings.ExclusionTypes.Contains(column.Type))
                tableFunction.Columns.Add(column);
        }

        /// <summary>
        /// Adds a parameter in stored procedure from row dictionary
        /// </summary>
        /// <param name="storedProcedure">Instance of <see cref="StoredProcedure"/> class</param>
        /// <param name="dictionary">Dictionary from data reader</param>
        protected virtual void AddParameter(StoredProcedure storedProcedure, IDictionary<string, object> dictionary)
        {
            storedProcedure.Parameters.Add(SqlServerDatabaseFactoryHelper.GetParameter(dictionary));
        }

        /// <summary>
        /// Adds a parameter in scalar function from row dictionary
        /// </summary>
        /// <param name="scalarFunction">Instance of <see cref="ScalarFunction"/> class</param>
        /// <param name="dictionary">Dictionary from data reader</param>
        protected virtual void AddParameter(ScalarFunction scalarFunction, IDictionary<string, object> dictionary)
        {
            scalarFunction.Parameters.Add(SqlServerDatabaseFactoryHelper.GetParameter(dictionary));
        }

        /// <summary>
        /// Adds a parameter in table function from row dictionary
        /// </summary>
        /// <param name="tableFunction">Instance of <see cref="TableFunction"/> class</param>
        /// <param name="dictionary">Dictionary from data reader</param>
        protected virtual void AddParameter(TableFunction tableFunction, IDictionary<string, object> dictionary)
        {
            tableFunction.Parameters.Add(SqlServerDatabaseFactoryHelper.GetParameter(dictionary));
        }

        /// <summary>
        /// Sets identity for table
        /// </summary>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <param name="dictionary">Dictionary from data reader</param>
        protected virtual void SetIdentity(Table table, IDictionary<string, object> dictionary)
        {
            var identity = string.Concat(dictionary["Identity"]);

            if (string.Compare(identity, "No identity column defined.", true) != 0)
                table.Identity = new Identity(identity, Convert.ToInt32(dictionary["Seed"]), Convert.ToInt32(dictionary["Increment"]));
        }

        /// <summary>
        /// Sets identity for view
        /// </summary>
        /// <param name="view">Instance of <see cref="View"/> class</param>
        /// <param name="dictionary">Dictionary from data reader</param>
        protected virtual void SetIdentity(View view, IDictionary<string, object> dictionary)
        {
            var identity = string.Concat(dictionary["Identity"]);

            if (string.Compare(identity, "No identity column defined.", true) != 0)
                view.Identity = new Identity(identity, Convert.ToInt32(dictionary["Seed"]), Convert.ToInt32(dictionary["Increment"]));
        }

        /// <summary>
        /// Sets identity for table function
        /// </summary>
        /// <param name="tableFunction"></param>
        /// <param name="dictionary">Dictionary from data reader</param>
        protected virtual void SetIdentity(TableFunction tableFunction, IDictionary<string, object> dictionary)
        {
            var identity = string.Concat(dictionary["Identity"]);

            if (string.Compare(identity, "No identity column defined.", true) != 0)
                tableFunction.Identity = new Identity(identity, Convert.ToInt32(dictionary["Seed"]), Convert.ToInt32(dictionary["Increment"]));
        }

        /// <summary>
        /// Sets row guid column for table
        /// </summary>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <param name="dictionary">Dictionary from data reader</param>
        protected virtual void SetRowGuidCol(Table table, IDictionary<string, object> dictionary)
        {
            table.RowGuidCol = new RowGuidCol
            {
                Name = string.Concat(dictionary["RowGuidCol"])
            };
        }

        /// <summary>
        /// Sets row guid column for view
        /// </summary>
        /// <param name="view">Instance of <see cref="View"/> class</param>
        /// <param name="dictionary">Dictionary from data reader</param>
        protected virtual void SetRowGuidCol(View view, IDictionary<string, object> dictionary)
        {
            view.RowGuidCol = new RowGuidCol
            {
                Name = string.Concat(dictionary["RowGuidCol"])
            };
        }

        /// <summary>
        /// Adds index to table
        /// </summary>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <param name="dictionary">Dictionary from data reader</param>
        protected virtual void AddIndexToTable(Table table, IDictionary<string, object> dictionary)
        {
            table.Indexes.Add(SqlServerDatabaseFactoryHelper.GetIndex(dictionary));
        }

        /// <summary>
        /// Add index to view
        /// </summary>
        /// <param name="view">Instance of <see cref="View"/> class</param>
        /// <param name="dictionary">Dictionary from data reader</param>
        protected virtual void AddIndexToView(View view, IDictionary<string, object> dictionary)
        {
            view.Indexes.Add(SqlServerDatabaseFactoryHelper.GetIndex(dictionary));
        }

        /// <summary>
        /// Adds constraint to table
        /// </summary>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <param name="dictionary">Dictionary from data reader</param>
        protected virtual void AddConstraintToTable(Table table, IDictionary<string, object> dictionary)
        {
            table.ConstraintDetails.Add(SqlServerDatabaseFactoryHelper.GetConstraintDetail(dictionary));
        }

        /// <summary>
        /// Sets constraints for table from contraint details
        /// </summary>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        protected virtual void SetConstraintsFromConstraintDetails(Table table)
        {
            foreach (var constraintDetail in table.ConstraintDetails)
            {
                if (constraintDetail.ConstraintType.Contains("CHECK"))
                {
                    var key = constraintDetail.ConstraintKeys.ToString();

                    table.Checks.Add(new Check(key)
                    {
                        ConstraintName = constraintDetail.ConstraintName
                    });
                }
                else if (constraintDetail.ConstraintType.Contains("DEFAULT"))
                {
                    var key = constraintDetail.ConstraintKeys.ToString();

                    table.Defaults.Add(new Default(key)
                    {
                        ConstraintName = constraintDetail.ConstraintName
                    });
                }
                else if (constraintDetail.ConstraintType.Contains("FOREIGN KEY"))
                {
                    var key = constraintDetail.ConstraintKeys.ToString().Split(',').Select(item => item.Trim()).ToArray();

                    table.ForeignKeys.Add(new ForeignKey(key)
                    {
                        ConstraintName = constraintDetail.ConstraintName
                    });
                }
                else if (constraintDetail.ConstraintType.Contains("PRIMARY KEY"))
                {
                    var key = string.Concat(constraintDetail.ConstraintKeys).Split(',').Select(item => item.Trim()).ToArray();

                    table.PrimaryKey = new PrimaryKey(key)
                    {
                        ConstraintName = constraintDetail.ConstraintName
                    };
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
            }
        }

        /// <summary>
        /// Adds table reference for table
        /// </summary>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <param name="dictionary">Dictionary from data reader</param>
        protected virtual void AddTableReferenceToTable(Table table, IDictionary<string, object> dictionary)
        {
            table.TableReferences.Add(new TableReference
            {
                ReferenceDescription = string.Concat(dictionary["Table is referenced by foreign key"]),
            });
        }

        private void ImportExtendedProperties(DbConnection connection, Database database)
        {
            foreach (var name in DatabaseImportSettings.ExtendedProperties)
            {
                foreach (var extendedProperty in connection.GetExtendedProperties(name))
                {
                    database.ExtendedProperties.Add(new ExtendedProperty { Name = extendedProperty.Name, Value = extendedProperty.Value });
                }
            }
        }

        private void ImportExtendedProperties(DbConnection connection, Table table)
        {
            table.Type = "table";

            foreach (var name in DatabaseImportSettings.ExtendedProperties)
            {
                foreach (var extendedProperty in connection.GetExtendedProperties(table, name))
                {
                    table.ExtendedProperties.Add(new ExtendedProperty { Name = extendedProperty.Name, Value = extendedProperty.Value });

                    // todo: Remove this token
                    if (name == "MS_Description")
                        table.Description = extendedProperty.Value;
                }

                foreach (var column in table.Columns)
                {
                    foreach (var extendedProperty in connection.GetExtendedProperties(table, column, name))
                    {
                        column.ExtendedProperties.Add(new ExtendedProperty { Name = extendedProperty.Name, Value = extendedProperty.Value });

                        // todo: Remove this token
                        if (name == "MS_Description")
                            column.Description = extendedProperty.Value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets views from database connection
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="views">Sequence of views</param>
        /// <returns>A sequence of <see cref="View"/> that represents existing views in database</returns>
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

                    var queryResults = new List<DynamicQueryResult>();

                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.NextResult())
                        {
                            var queryResult = new DynamicQueryResult();

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
                                else if (item.ContainsKey("index_name"))
                                    AddIndexToView(view, item);
                            }
                        }

                        yield return view;
                    }
                }
            }
        }

        private void ImportExtendedProperties(DbConnection connection, View view)
        {
            view.Type = "view";

            foreach (var name in DatabaseImportSettings.ExtendedProperties)
            {
                foreach (var extendProperty in connection.GetExtendedProperties(view, name))
                {
                    view.ExtendedProperties.Add(new ExtendedProperty { Name = extendProperty.Name, Value = extendProperty.Value });

                    // todo: Remove this token
                    if (name == "MS_Description")
                        view.Description = extendProperty.Value;
                }

                foreach (var column in view.Columns)
                {
                    foreach (var extendedProperty in connection.GetExtendedProperties(view, column, name))
                    {
                        column.ExtendedProperties.Add(new ExtendedProperty { Name = extendedProperty.Name, Value = extendedProperty.Value });

                        // todo: Remove this token
                        if (name == "MS_Description")
                            column.Description = extendedProperty.Value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets scalar functions from database connection
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="scalarFunctions">Sequence of scalar functions</param>
        /// <returns>A sequence of <see cref="ScalarFunction"/> that represents existing views in database</returns>
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

                    var queryResults = new List<DynamicQueryResult>();

                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.NextResult())
                        {
                            var queryResult = new DynamicQueryResult();

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

        private void ImportExtendedProperties(DbConnection connection, ScalarFunction scalarFunction)
        {
            foreach (var name in DatabaseImportSettings.ExtendedProperties)
            {
                foreach (var extendedProperty in connection.GetExtendedProperties(scalarFunction, name))
                {
                    // todo: Remove this token
                    if (name == "MS_Description")
                        scalarFunction.Description = extendedProperty.Value;
                }
            }
        }

        /// <summary>
        /// Gets table functions from database connection
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="tableFunctions">Sequence of table functions</param>
        /// <returns>A sequence of <see cref="TableFunction"/> that represents existing views in database</returns>
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

                    var queryResults = new List<DynamicQueryResult>();

                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.NextResult())
                        {
                            var queryResult = new DynamicQueryResult();

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

        private void ImportExtendedProperties(DbConnection connection, TableFunction tableFunction)
        {
            foreach (var name in DatabaseImportSettings.ExtendedProperties)
            {
                foreach (var extendedProperty in connection.GetExtendedProperties(tableFunction, name))
                {
                    // todo: Remove this token
                    if (name == "MS_Description")
                        tableFunction.Description = extendedProperty.Value;
                }
            }
        }

        /// <summary>
        /// Gets stored procedures from database connection
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="storedProcedures">Sequence of stored procedures</param>
        /// <returns>A sequence of <see cref="StoredProcedure"/> that represents existing views in database</returns>
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

                    var queryResults = new List<DynamicQueryResult>();

                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.NextResult())
                        {
                            var queryResult = new DynamicQueryResult();

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

        private void ImportExtendedProperties(DbConnection connection, StoredProcedure storedProcedure)
        {
            foreach (var name in DatabaseImportSettings.ExtendedProperties)
            {
                foreach (var extendedProperty in connection.GetExtendedProperties(storedProcedure, name))
                {
                    // todo: Remove this token
                    if (name == "MS_Description")
                        storedProcedure.Description = extendedProperty.Value;
                }
            }
        }
    }
}

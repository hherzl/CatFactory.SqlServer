using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CatFactory.ObjectRelationalMapping;
using CatFactory.ObjectRelationalMapping.Programmability;
using CatFactory.SqlServer.DatabaseObjectModel;
using CatFactory.SqlServer.DatabaseObjectModel.Queries;
using CatFactory.SqlServer.Features;
using Microsoft.Extensions.Logging;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// Implements all operations related to import database feature
    /// </summary>
    public partial class SqlServerDatabaseFactory : IDatabaseFactory
    {
        private const string CHECK = "CHECK";
        private const string COLUMN_NAME = "Column_name";
        private const string CONSTRAINT_TYPE = "constraint_type";
        private const string DEFAULT = "DEFAULT";
        private const string DEFAULT_ON_COLUMN_ = "DEFAULT on column ";
        private const string FOREIGN_KEY = "FOREIGN KEY";
        private const string IDENTITY = "Identity";
        private const string INCREMENT = "Increment";
        private const string INDEX_NAME = "index_name";
        private const string NO_IDENTITY_COLUMN_DEFINED = "No identity column defined.";
        private const string PARAMETER_NAME = "Parameter_name";
        private const string PRIMARY_KEY = "PRIMARY KEY";
        private const string REFERENCES = "REFERENCES";
        private const string ROW_GUID_COL = "RowGuidCol";
        private const string SEED = "Seed";
        private const string TABLE_IS_REFERENCED_BY_FOREIGN_KEY = "Table is referenced by foreign key";
        private const string UNIQUE = "UNIQUE";

        /// <summary>
        /// Gets a instance for <see cref="Logger"/> class
        /// </summary>
        /// <returns>An instance for <see cref="SqlServerDatabaseFactory"/> class</returns>
        public static ILogger<SqlServerDatabaseFactory> GetLogger()
            => LoggingHelper.GetLogger<SqlServerDatabaseFactory>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DatabaseImportSettings m_databaseImportSettings;

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
        /// Initializes a new instance for <see cref="SqlServerDatabaseFactory"/> class
        /// </summary>
        /// <param name="databaseImportSettings">Instance of <see cref="DatabaseImportSettings"/> class</param>
        public SqlServerDatabaseFactory(DatabaseImportSettings databaseImportSettings)
        {
            m_databaseImportSettings = databaseImportSettings;
        }

        /// <summary>
        /// Initializes a new instance for <see cref="SqlServerDatabaseFactory"/> class
        /// </summary>
        /// <param name="logger"><see cref="Logger"/> class</param>
        /// <param name="databaseImportSettings">Instance of <see cref="DatabaseImportSettings"/> class</param>
        public SqlServerDatabaseFactory(ILogger<SqlServerDatabaseFactory> logger, DatabaseImportSettings databaseImportSettings)
        {
            Logger = logger;
            m_databaseImportSettings = databaseImportSettings;
        }

        /// <summary>
        /// Gets the <see cref="Logger"/> instance
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Gets an instance for <see cref="DbConnection"/> class
        /// </summary>
        /// <returns>An instance of <see cref="SqlConnection"/> class</returns>
        public SqlConnection GetConnection()
            => new(DatabaseImportSettings.ConnectionString);

        /// <summary>
        /// Gets or sets the database import settings
        /// </summary>
        public DatabaseImportSettings DatabaseImportSettings
        {
            get => m_databaseImportSettings ??= new DatabaseImportSettings();
            set => m_databaseImportSettings = value;
        }

        /// <summary>
        /// Gets a sequence of<see cref= "DatabaseTypeMap" /> class that represents data types for SQL Server database
        /// </summary>
        public IEnumerable<DatabaseTypeMap> DatabaseTypeMaps
            => SqlServerDatabaseTypeMaps.DatabaseTypeMaps;

        /// <summary>
        /// Imports an existing database from SQL Server instance using database import settings
        /// </summary>
        /// <returns>An instance of <see cref="Database"/> class that represents a database from SQL Server instance</returns>
        public virtual async Task<Database> ImportAsync()
        {
            using var connection = GetConnection();

            var database = SqlServerDatabase.CreateWithDefaults(connection.Database);

            database.ServerName = connection.DataSource;
            database.Dbms = "SQL Server";

            await connection.OpenAsync();

            await SqlServerDatabaseFactoryHelper.AddUserDefinedDataTypesAsync(database, connection);

            foreach (var dbObject in await GetDbObjectsAsync(connection))
            {
                if (DatabaseImportSettings.Exclusions.Contains(dbObject.FullName))
                    continue;

                database.DbObjects.Add(dbObject);
            }

            if (DatabaseImportSettings.ExtendedProperties.Count > 0)
            {
                Logger?.LogInformation("Importing extended properties for database...");

                await ImportExtendedPropertiesAsync(connection, database);
            }

            if (DatabaseImportSettings.ImportTables)
            {
                Logger?.LogInformation("Importing tables for '{0}' database...", database.Name);

                foreach (var table in await GetTablesAsync(connection, database.GetTables()))
                {
                    if (DatabaseImportSettings.Exclusions.Contains(table.FullName))
                        continue;

                    database.Tables.Add(table);
                }

                if (DatabaseImportSettings.ExtendedProperties.Count > 0)
                {
                    Logger?.LogInformation("Importing extended properties for tables...");

                    foreach (var table in database.Tables)
                    {
                        ImportExtendedProperties(connection, table);
                    }
                }
            }

            if (DatabaseImportSettings.ImportViews)
            {
                Logger?.LogInformation("Importing views for '{0}' database...", database.Name);

                foreach (var view in await GetViewsAsync(connection, database.GetViews()))
                {
                    if (DatabaseImportSettings.Exclusions.Contains(view.FullName))
                        continue;

                    database.Views.Add(view);
                }

                if (DatabaseImportSettings.ExtendedProperties.Count > 0)
                {
                    Logger?.LogInformation("Importing extended properties for views...");

                    foreach (var view in database.Views)
                    {
                        ImportExtendedProperties(connection, view);
                    }
                }
            }

            if (DatabaseImportSettings.ImportScalarFunctions)
            {
                Logger?.LogInformation("Importing scalar functions for '{0}' database...", database.Name);

                foreach (var scalarFunction in await GetScalarFunctionsAsync(connection, database.GetScalarFunctions()))
                {
                    if (DatabaseImportSettings.Exclusions.Contains(scalarFunction.FullName))
                        continue;

                    database.ScalarFunctions.Add(scalarFunction);
                }

                if (DatabaseImportSettings.ExtendedProperties.Count > 0)
                {
                    Logger?.LogInformation("Importing extended properties for scalar functions...");

                    foreach (var scalarFunction in database.ScalarFunctions)
                    {
                        ImportExtendedProperties(connection, scalarFunction);
                    }
                }

                database.ImportBag.ScalarFunctions = database.ScalarFunctions;
            }

            if (DatabaseImportSettings.ImportTableFunctions)
            {
                Logger?.LogInformation("Importing table functions for '{0}' database...", database.Name);

                foreach (var tableFunction in await GetTableFunctionsAsync(connection, database.GetTableFunctions()))
                {
                    if (DatabaseImportSettings.Exclusions.Contains(tableFunction.FullName))
                        continue;

                    database.TableFunctions.Add(tableFunction);
                }

                if (DatabaseImportSettings.ExtendedProperties.Count > 0)
                {
                    Logger?.LogInformation("Importing extended properties for table functions...");

                    foreach (var tableFunction in database.TableFunctions)
                    {
                        ImportExtendedProperties(connection, tableFunction);
                    }
                }

                database.ImportBag.TableFunctions = database.TableFunctions;
            }

            if (DatabaseImportSettings.ImportStoredProcedures)
            {
                Logger?.LogInformation("Importing stored procedures for '{0}' database...", database.Name);

                foreach (var storedProcedure in await GetStoredProceduresAsync(connection, database.GetStoredProcedures()))
                {
                    if (DatabaseImportSettings.Exclusions.Contains(storedProcedure.FullName))
                        continue;

                    database.StoredProcedures.Add(storedProcedure);
                }

                Logger?.LogInformation("Getting result sets for stored procedures...");

                foreach (var storedProcedure in database.StoredProcedures)
                {
                    foreach (var firstResultSet in await SqlServerDatabaseFactoryHelper.GetFirstResultSetForObjectAsync(storedProcedure, connection))
                    {
                        storedProcedure.FirstResultSetsForObject.Add(firstResultSet);
                    }

                    foreach (var resultSet in await SqlServerDatabaseFactoryHelper.GetResultSetsAsync(storedProcedure, connection))
                    {
                        storedProcedure.ResultSets.Add(resultSet);
                    }
                }

                if (DatabaseImportSettings.ExtendedProperties.Count > 0)
                {
                    Logger?.LogInformation("Importing extended properties for stored procedures...");

                    foreach (var storedProcedure in database.StoredProcedures)
                    {
                        ImportExtendedProperties(connection, storedProcedure);
                    }
                }

                database.ImportBag.StoredProcedures = database.StoredProcedures;
            }

            if (DatabaseImportSettings.ImportSequences)
            {
                Logger?.LogInformation("Importing sequences for '{0}' database...", database.Name);

                foreach (var sequence in await GetSequencesAsync(connection, database.GetSequences()))
                {
                    if (DatabaseImportSettings.Exclusions.Contains(sequence.FullName))
                        continue;

                    database.Sequences.Add(sequence);
                }

                database.ImportBag.Sequences = database.Sequences;
            }

            connection.Close();

            return database;
        }

        /// <summary>
        /// Imports an existing database from SQL Server instance using database import settings
        /// </summary>
        /// <returns>An instance of <see cref="Database"/> class that represents a database from SQL Server instance</returns>
        public virtual Database Import()
            => ImportAsync().GetAwaiter().GetResult();

        /// <summary>
        /// Gets database objects from connection
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <returns>A sequence of <see cref="DbObject"/> class that represents objects in database</returns>
        protected virtual async Task<ICollection<DbObject>> GetDbObjectsAsync(DbConnection connection)
        {
            using var command = connection.CreateCommand();

            command.Connection = connection;
            command.CommandText = DatabaseImportSettings.ImportCommandText;

            using var dataReader = await command.ExecuteReaderAsync();

            var collection = new Collection<DbObject>();

            while (await dataReader.ReadAsync())
            {
                collection.Add(new DbObject
                {
                    DataSource = connection.DataSource,
                    DatabaseName = connection.Database,
                    Schema = dataReader.GetString(0),
                    Name = dataReader.GetString(1),
                    Type = dataReader.GetString(2)
                });
            }

            return collection;
        }

        /// <summary>
        /// Gets tables from database connection
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="tables">Sequence of <see cref="DbObject"/> that represents tables</param>
        /// <returns>A sequence of <see cref="Table"/></returns>
        protected virtual async Task<ICollection<Table>> GetTablesAsync(DbConnection connection, IEnumerable<DbObject> tables)
        {
            var collection = new Collection<Table>();

            foreach (var dbObject in tables)
            {
                using var command = connection.CreateCommand();

                var table = new Table
                {
                    DataSource = connection.DataSource,
                    DatabaseName = connection.Database,
                    Schema = dbObject.Schema,
                    Name = dbObject.Name
                };

                command.Connection = connection;
                command.CommandText = string.Format("SP_HELP '{0}'", dbObject.FullName);

                var queryResults = new List<DynamicQueryResult>();

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.NextResultAsync())
                {
                    var queryResult = new DynamicQueryResult();

                    while (await reader.ReadAsync())
                    {
                        var names = SqlServerDatabaseFactoryHelper.GetNames(reader).ToList();

                        var row = new Dictionary<string, object>();

                        for (var i = 0; i < names.Count; i++)
                            row.Add(names[i], reader.GetValue(i));

                        queryResult.Items.Add(row);
                    }

                    queryResults.Add(queryResult);
                }

                table.ImportBag.ConstraintDetails = new Collection<ConstraintDetail>();
                table.ImportBag.TableReferences = new Collection<TableReference>();

                foreach (var result in queryResults)
                {
                    foreach (var item in result.Items)
                    {
                        if (item.ContainsKey(COLUMN_NAME))
                            AddColumn(table, item);
                        else if (item.ContainsKey(IDENTITY))
                            SetIdentity(table, item);
                        else if (item.ContainsKey(ROW_GUID_COL))
                            SetRowGuidCol(table, item);
                        else if (item.ContainsKey(INDEX_NAME))
                            AddIndexToTable(table, item);
                        else if (item.ContainsKey(CONSTRAINT_TYPE))
                            AddConstraintToTable(table, item);
                        else if (item.ContainsKey(TABLE_IS_REFERENCED_BY_FOREIGN_KEY))
                            AddTableReferenceToTable(table, item);
                    }
                }

                SetConstraintsFromConstraintDetails(table);

                collection.Add(table);
            }

            return collection;
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
        /// Sets identity for table
        /// </summary>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <param name="dictionary">Dictionary from data reader</param>
        protected virtual void SetIdentity(Table table, IDictionary<string, object> dictionary)
        {
            var identity = string.Concat(dictionary[IDENTITY]);

            if (string.Compare(identity, NO_IDENTITY_COLUMN_DEFINED, true) != 0)
                table.Identity = new Identity(identity, Convert.ToInt32(dictionary[SEED]), Convert.ToInt32(dictionary[INCREMENT]));
        }

        /// <summary>
        /// Sets identity for view
        /// </summary>
        /// <param name="view">Instance of <see cref="View"/> class</param>
        /// <param name="dictionary">Dictionary from data reader</param>
        protected virtual void SetIdentity(View view, IDictionary<string, object> dictionary)
        {
            var identity = string.Concat(dictionary[IDENTITY]);

            if (string.Compare(identity, NO_IDENTITY_COLUMN_DEFINED, true) != 0)
                view.Identity = new Identity(identity, Convert.ToInt32(dictionary[SEED]), Convert.ToInt32(dictionary[INCREMENT]));
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
        /// Adds a parameter in stored procedure from row dictionary
        /// </summary>
        /// <param name="storedProcedure">Instance of <see cref="StoredProcedure"/> class</param>
        /// <param name="dictionary">Dictionary from data reader</param>
        protected virtual void AddParameter(StoredProcedure storedProcedure, IDictionary<string, object> dictionary)
        {
            storedProcedure.Parameters.Add(SqlServerDatabaseFactoryHelper.GetParameter(dictionary));
        }

        /// <summary>
        /// Sets identity for table function
        /// </summary>
        /// <param name="tableFunction"></param>
        /// <param name="dictionary">Dictionary from data reader</param>
        protected virtual void SetIdentity(TableFunction tableFunction, IDictionary<string, object> dictionary)
        {
            var identity = string.Concat(dictionary[IDENTITY]);

            if (string.Compare(identity, NO_IDENTITY_COLUMN_DEFINED, true) != 0)
                tableFunction.Identity = new Identity(identity, Convert.ToInt32(dictionary[SEED]), Convert.ToInt32(dictionary[INCREMENT]));
        }

        /// <summary>
        /// Sets row guid column for table
        /// </summary>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <param name="dictionary">Dictionary from data reader</param>
        protected virtual void SetRowGuidCol(Table table, IDictionary<string, object> dictionary)
        {
            table.ImportBag.RowGuidCol = new RowGuidCol
            {
                Name = string.Concat(dictionary[ROW_GUID_COL])
            };
        }

        /// <summary>
        /// Sets row guid column for view
        /// </summary>
        /// <param name="view">Instance of <see cref="View"/> class</param>
        /// <param name="dictionary">Dictionary from data reader</param>
        protected virtual void SetRowGuidCol(View view, IDictionary<string, object> dictionary)
        {
            view.ImportBag.RowGuidCol = new RowGuidCol
            {
                Name = string.Concat(dictionary[ROW_GUID_COL])
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
            var item = SqlServerDatabaseFactoryHelper.GetConstraintDetail(dictionary);

            if (item.ConstraintType.Contains(DEFAULT_ON_COLUMN_))
            {
                var columnName = item.ConstraintType.Replace(DEFAULT_ON_COLUMN_, "").Trim();

                var column = table[columnName];

                if (column != null)
                    column.ImportBag.ComputedExpression = item.ConstraintKeys.Trim();
            }

            table.ImportBag.ConstraintDetails.Add(item);
        }

        /// <summary>
        /// Sets constraints for table from contraint details
        /// </summary>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        protected virtual void SetConstraintsFromConstraintDetails(Table table)
        {
            foreach (ConstraintDetail constraintDetail in table.ImportBag.ConstraintDetails)
            {
                if (constraintDetail.ConstraintType.Contains(CHECK))
                {
                    table.Checks.Add(new Check(constraintDetail.ConstraintName, new string[] { constraintDetail.ConstraintKeys }));
                }
                else if (constraintDetail.ConstraintType.Contains(DEFAULT))
                {
                    var column = constraintDetail.ConstraintType.Replace("DEFAULT on column ", string.Empty).Trim();

                    table.Defaults.Add(new Default(constraintDetail.ConstraintName, column)
                    {
                        Value = constraintDetail.ConstraintKeys
                    });

                    table[column].DefaultValue = constraintDetail.ConstraintKeys;
                }
                else if (constraintDetail.ConstraintType.Contains(FOREIGN_KEY))
                {
                    var key = constraintDetail.ConstraintKeys.ToString().Split(',').Select(item => item.Trim()).ToArray();

                    table.ForeignKeys.Add(new ForeignKey(constraintDetail.ConstraintName, key));
                }
                else if (constraintDetail.ConstraintType.Contains(PRIMARY_KEY))
                {
                    var key = string.Concat(constraintDetail.ConstraintKeys).Split(',').Select(item => item.Trim()).ToArray();

                    table.PrimaryKey = new PrimaryKey(constraintDetail.ConstraintName, key);
                }
                else if (constraintDetail.ConstraintKeys.Contains(REFERENCES))
                {
                    var value = constraintDetail.ConstraintKeys.Replace("REFERENCES", string.Empty);

                    table.ForeignKeys.Last().References = value.Substring(0, value.IndexOf("(")).Trim();
                }
                else if (constraintDetail.ConstraintType.Contains(UNIQUE))
                {
                    var key = constraintDetail.ConstraintKeys.ToString().Split(',').Select(item => item.Trim()).ToArray();

                    table.Uniques.Add(new Unique(constraintDetail.ConstraintName, key));
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
            table.ImportBag.TableReferences.Add(new TableReference
            {
                ReferenceDescription = string.Concat(dictionary[TABLE_IS_REFERENCED_BY_FOREIGN_KEY]),
            });
        }

        private async Task ImportExtendedPropertiesAsync(DbConnection connection, SqlServerDatabase database)
        {
            foreach (var name in DatabaseImportSettings.ExtendedProperties)
            {
                foreach (var exProperty in await connection.FnListExtendedPropertyAsync(new ExtendedProperty(name)))
                {
                    database.ExtendedProperties.Add(new ExtendedProperty(exProperty.Name, exProperty.Value));
                }
            }
        }

        private void ImportExtendedProperties(DbConnection connection, Table table)
        {
            table.Type = "table";
            table.ImportBag.ExtendedProperties = new Collection<ExtendedProperty>();

            foreach (var name in DatabaseImportSettings.ExtendedProperties)
            {
                foreach (var exProperty in connection.GetExtendedProperties(table, name))
                {
                    table.ImportBag.ExtendedProperties.Add(new ExtendedProperty(exProperty.Name, exProperty.Value));

                    // todo: Remove this token
                    if (name == SqlServerToken.MS_DESCRIPTION)
                        table.Description = exProperty.Value;
                }

                foreach (var column in table.Columns)
                {
                    column.ImportBag.ExtendedProperties = new Collection<ExtendedProperty>();

                    foreach (var exProperty in connection.GetExtendedProperties(table, column, name))
                    {
                        column.ImportBag.ExtendedProperties.Add(new ExtendedProperty(exProperty.Name, exProperty.Value));

                        // todo: Remove this token
                        if (name == SqlServerToken.MS_DESCRIPTION)
                            column.Description = exProperty.Value;
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
        protected virtual async Task<ICollection<View>> GetViewsAsync(DbConnection connection, IEnumerable<DbObject> views)
        {
            var collection = new Collection<View>();

            foreach (var dbObject in views)
            {
                using var command = connection.CreateCommand();

                command.Connection = connection;
                command.CommandText = string.Format("SP_HELP '{0}'", dbObject.FullName);

                var queryResults = new List<DynamicQueryResult>();

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.NextResultAsync())
                {
                    var queryResult = new DynamicQueryResult();

                    while (await reader.ReadAsync())
                    {
                        var names = SqlServerDatabaseFactoryHelper.GetNames(reader).ToList();

                        var row = new Dictionary<string, object>();

                        for (var i = 0; i < names.Count; i++)
                            row.Add(names[i], reader.GetValue(i));

                        queryResult.Items.Add(row);
                    }

                    queryResults.Add(queryResult);
                }

                var view = new View
                {
                    DataSource = connection.DataSource,
                    DatabaseName = connection.Database,
                    Schema = dbObject.Schema,
                    Name = dbObject.Name
                };

                foreach (var result in queryResults)
                {
                    foreach (var item in result.Items)
                    {
                        if (item.ContainsKey(COLUMN_NAME))
                            AddColumn(view, item);
                        else if (item.ContainsKey(IDENTITY))
                            SetIdentity(view, item);
                        else if (item.ContainsKey(ROW_GUID_COL))
                            SetRowGuidCol(view, item);
                        else if (item.ContainsKey(INDEX_NAME))
                            AddIndexToView(view, item);
                    }
                }

                collection.Add(view);
            }

            return collection;
        }

        private void ImportExtendedProperties(DbConnection connection, View view)
        {
            view.Type = "view";
            view.ImportBag.ExtendedProperties = new Collection<ExtendedProperty>();

            foreach (var name in DatabaseImportSettings.ExtendedProperties)
            {
                foreach (var exProperty in connection.GetExtendedProperties(view, name))
                {
                    view.ImportBag.ExtendedProperties.Add(new ExtendedProperty(exProperty.Name, exProperty.Value));

                    // todo: Remove this token
                    if (name == SqlServerToken.MS_DESCRIPTION)
                        view.ImportBag.Description = exProperty.Value;
                }

                foreach (var column in view.Columns)
                {
                    column.ImportBag.ExtendedProperties = new Collection<ExtendedProperty>();

                    foreach (var exProperty in connection.GetExtendedProperties(view, column, name))
                    {
                        column.ImportBag.ExtendedProperties.Add(new ExtendedProperty(exProperty.Name, exProperty.Value));

                        // todo: Remove this token
                        if (name == SqlServerToken.MS_DESCRIPTION)
                            column.Description = exProperty.Value;
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
        protected virtual async Task<ICollection<ScalarFunction>> GetScalarFunctionsAsync(DbConnection connection, IEnumerable<DbObject> scalarFunctions)
        {
            var collection = new Collection<ScalarFunction>();

            foreach (var dbObject in scalarFunctions)
            {
                using var command = connection.CreateCommand();

                var scalarFunction = new ScalarFunction
                {
                    DataSource = connection.DataSource,
                    DatabaseName = connection.Database,
                    Schema = dbObject.Schema,
                    Name = dbObject.Name
                };

                command.Connection = connection;
                command.CommandText = string.Format("SP_HELP '{0}'", dbObject.FullName);

                var queryResults = new List<DynamicQueryResult>();

                using var dataReader = await command.ExecuteReaderAsync();

                while (await dataReader.NextResultAsync())
                {
                    var queryResult = new DynamicQueryResult();

                    while (await dataReader.ReadAsync())
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
                        if (item.ContainsKey(PARAMETER_NAME))
                            AddParameter(scalarFunction, item);
                    }
                }

                collection.Add(scalarFunction);
            }

            return collection;
        }

        private void ImportExtendedProperties(DbConnection connection, ScalarFunction scalarFunction)
        {
            foreach (var name in DatabaseImportSettings.ExtendedProperties)
            {
                foreach (var exProperty in connection.GetExtendedProperties(scalarFunction, name))
                {
                    // todo: Remove this token
                    if (name == SqlServerToken.MS_DESCRIPTION)
                        scalarFunction.Description = exProperty.Value;
                }
            }
        }

        /// <summary>
        /// Gets table functions from database connection
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="tableFunctions">Sequence of table functions</param>
        /// <returns>A sequence of <see cref="TableFunction"/> that represents existing views in database</returns>
        protected virtual async Task<ICollection<TableFunction>> GetTableFunctionsAsync(DbConnection connection, IEnumerable<DbObject> tableFunctions)
        {
            var collection = new Collection<TableFunction>();

            foreach (var dbObject in tableFunctions)
            {
                using var command = connection.CreateCommand();

                command.Connection = connection;
                command.CommandText = string.Format("SP_HELP '{0}'", dbObject.FullName);

                var queryResults = new List<DynamicQueryResult>();

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.NextResultAsync())
                {
                    var queryResult = new DynamicQueryResult();

                    while (await reader.ReadAsync())
                    {
                        var names = SqlServerDatabaseFactoryHelper.GetNames(reader).ToList();

                        var row = new Dictionary<string, object>();

                        for (var i = 0; i < names.Count; i++)
                            row.Add(names[i], reader.GetValue(i));

                        queryResult.Items.Add(row);
                    }

                    queryResults.Add(queryResult);
                }

                var tableFunction = new TableFunction
                {
                    DataSource = connection.DataSource,
                    DatabaseName = connection.Database,
                    Schema = dbObject.Schema,
                    Name = dbObject.Name
                };

                foreach (var result in queryResults)
                {
                    foreach (var item in result.Items)
                    {
                        if (item.ContainsKey(COLUMN_NAME))
                            AddColumn(tableFunction, item);
                        else if (item.ContainsKey(IDENTITY))
                            SetIdentity(tableFunction, item);
                        else if (item.ContainsKey(PARAMETER_NAME))
                            AddParameter(tableFunction, item);
                    }
                }

                collection.Add(tableFunction);
            }

            return collection;
        }

        private void ImportExtendedProperties(DbConnection connection, TableFunction tableFunction)
        {
            foreach (var name in DatabaseImportSettings.ExtendedProperties)
            {
                foreach (var exProperty in connection.GetExtendedProperties(tableFunction, name))
                {
                    // todo: Remove this token
                    if (name == SqlServerToken.MS_DESCRIPTION)
                        tableFunction.Description = exProperty.Value;
                }
            }
        }

        /// <summary>
        /// Gets stored procedures from database connection
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="storedProcedures">Sequence of stored procedures</param>
        /// <returns>A sequence of <see cref="StoredProcedure"/> that represents existing views in database</returns>
        protected virtual async Task<ICollection<StoredProcedure>> GetStoredProceduresAsync(DbConnection connection, IEnumerable<DbObject> storedProcedures)
        {
            var collection = new Collection<StoredProcedure>();

            foreach (var dbObject in storedProcedures)
            {
                using var command = connection.CreateCommand();

                command.Connection = connection;
                command.CommandText = string.Format("SP_HELP '{0}'", dbObject.FullName);

                var queryResults = new List<DynamicQueryResult>();

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.NextResultAsync())
                {
                    var queryResult = new DynamicQueryResult();

                    while (await reader.ReadAsync())
                    {
                        var names = SqlServerDatabaseFactoryHelper.GetNames(reader).ToList();

                        var row = new Dictionary<string, object>();

                        for (var i = 0; i < names.Count; i++)
                            row.Add(names[i], reader.GetValue(i));

                        queryResult.Items.Add(row);
                    }

                    queryResults.Add(queryResult);
                }

                var storedProcedure = new StoredProcedure
                {
                    DataSource = connection.DataSource,
                    DatabaseName = connection.Database,
                    Schema = dbObject.Schema,
                    Name = dbObject.Name
                };

                foreach (var result in queryResults)
                {
                    foreach (var item in result.Items)
                    {
                        if (item.ContainsKey(PARAMETER_NAME))
                            AddParameter(storedProcedure, item);
                    }
                }

                collection.Add(storedProcedure);
            }

            return collection;
        }

        private void ImportExtendedProperties(DbConnection connection, StoredProcedure storedProcedure)
        {
            foreach (var name in DatabaseImportSettings.ExtendedProperties)
            {
                foreach (var exProperty in connection.GetExtendedProperties(storedProcedure, name))
                {
                    // todo: Remove this token
                    if (name == SqlServerToken.MS_DESCRIPTION)
                        storedProcedure.Description = exProperty.Value;
                }
            }
        }

        /// <summary>
        /// Gets sequences from database connection
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="sequences">Enumerator of sequences</param>
        /// <returns>A sequence of <see cref="ScalarFunction"/> that represents existing views in database</returns>
        protected virtual async Task<ICollection<Sequence>> GetSequencesAsync(DbConnection connection, IEnumerable<DbObject> sequences)
        {
            var sysSequences = await connection.GetSysSequencesAsync();

            var sysSchemas = await connection.GetSysSchemasAsync();

            var sysTypes = await connection.GetSysTypesAsync();

            var summary =
                from sysSequence in sysSequences
                join sysSchema in sysSchemas on sysSequence.SchemaId equals sysSchema.SchemaId
                join sysType in sysTypes on sysSequence.SystemTypeId equals sysType.SystemTypeId
                select new
                {
                    Schema = sysSchema.Name,
                    sysSequence.Name,
                    Type = sysType.Name,
                    sysSequence.StartValue,
                    sysSequence.MinimumValue,
                    sysSequence.MaximumValue,
                    sysSequence.CurrentValue,
                    sysSequence.Increment,
                    sysSequence.IsCached,
                    sysSequence.IsCycling
                };

            var databaseTypeMaps = SqlServerDatabaseTypeMaps.DatabaseTypeMaps;

            var collection = new Collection<Sequence>();

            foreach (var dbObject in sequences)
            {
                var record = summary
                    .FirstOrDefault(item => item.Schema == dbObject.Schema && item.Name == dbObject.Name);

                if (record == null)
                    continue;

                var sequenceDatabaseTypeMap = databaseTypeMaps
                    .First(item => item.DatabaseType == record.Type);

                if (sequenceDatabaseTypeMap.GetClrType() == typeof(byte))
                {
                    collection.Add(new ByteSequence
                    {
                        Name = record.Name,
                        Schema = record.Schema,
                        StartValue = (byte)record.StartValue,
                        MinimumValue = (byte)record.MinimumValue,
                        MaximumValue = (byte)record.MaximumValue,
                        CurrentValue = (byte)record.CurrentValue,
                        Increment = (byte)record.Increment,
                        IsCached = (bool)record.IsCached,
                        IsCycling = (bool)record.IsCycling
                    });
                }
                else if (sequenceDatabaseTypeMap.GetClrType() == typeof(short))
                {
                    collection.Add(new Int16Sequence
                    {
                        Name = record.Name,
                        Schema = record.Schema,
                        StartValue = (short)record.StartValue,
                        MinimumValue = (short)record.MinimumValue,
                        MaximumValue = (short)record.MaximumValue,
                        CurrentValue = (short)record.CurrentValue,
                        Increment = (short)record.Increment,
                        IsCached = (bool)record.IsCached,
                        IsCycling = (bool)record.IsCycling
                    });
                }
                else if (sequenceDatabaseTypeMap.GetClrType() == typeof(int))
                {
                    collection.Add(new Int32Sequence
                    {
                        Name = record.Name,
                        Schema = record.Schema,
                        StartValue = (int)record.StartValue,
                        MinimumValue = (int)record.MinimumValue,
                        MaximumValue = (int)record.MaximumValue,
                        CurrentValue = (int)record.CurrentValue,
                        Increment = (int)record.Increment,
                        IsCached = (bool)record.IsCached,
                        IsCycling = (bool)record.IsCycling
                    });
                }
                else if (sequenceDatabaseTypeMap.GetClrType() == typeof(long))
                {
                    collection.Add(new Int64Sequence
                    {
                        Name = record.Name,
                        Schema = record.Schema,
                        StartValue = (long)record.StartValue,
                        MinimumValue = (long)record.MinimumValue,
                        MaximumValue = (long)record.MaximumValue,
                        CurrentValue = (long)record.CurrentValue,
                        Increment = (long)record.Increment,
                        IsCached = (bool)record.IsCached,
                        IsCycling = (bool)record.IsCycling
                    });
                }
                else if (sequenceDatabaseTypeMap.GetClrType() == typeof(decimal))
                {
                    collection.Add(new DecimalSequence
                    {
                        Name = record.Name,
                        Schema = record.Schema,
                        StartValue = (decimal)record.StartValue,
                        MinimumValue = (decimal)record.MinimumValue,
                        MaximumValue = (decimal)record.MaximumValue,
                        CurrentValue = (decimal)record.CurrentValue,
                        Increment = (decimal)record.Increment,
                        IsCached = (bool)record.IsCached,
                        IsCycling = (bool)record.IsCycling
                    });
                }
            }

            return collection;
        }
    }
}

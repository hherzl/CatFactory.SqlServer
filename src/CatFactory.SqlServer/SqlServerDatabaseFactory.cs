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
        protected ILogger Logger;

        public SqlServerDatabaseFactory()
        {
            Init();
        }

        public SqlServerDatabaseFactory(ILogger<SqlServerDatabaseFactory> logger)
        {
            Logger = logger;

            Init();
        }

        public String ConnectionString { get; set; }

        public Boolean ImportMSDescription { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<String> m_exclusions;

        public List<String> Exclusions
        {
            get
            {
                return m_exclusions ?? (m_exclusions = new List<String>());
            }
            set
            {
                m_exclusions = value;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<String> m_exclusionTypes;

        public List<String> ExclusionTypes
        {
            get
            {
                return m_exclusionTypes ?? (m_exclusionTypes = new List<String>());
            }
            set
            {
                m_exclusionTypes = value;
            }
        }

        public String ImportCommandText { get; set; }

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
                    if (Exclusions.Contains(dbObject.FullName))
                    {
                        continue;
                    }

                    database.DbObjects.Add(dbObject);
                }

                Logger?.LogInformation("Importing tables for '{0}'...", database.Name);

                foreach (var table in ImportTables(database))
                {
                    if (Exclusions.Contains(table.FullName))
                    {
                        continue;
                    }

                    var dbObject = dbObjects.First(item => item.FullName == table.FullName);

                    if (ImportMSDescription)
                    {
                        dbObject.Type = "table";

                        foreach (var extendProperty in connection.GetMsDescriptionForDbObject(dbObject))
                        {
                            table.Description = String.Concat(extendProperty.Value);
                        }

                        foreach (var column in table.Columns)
                        {
                            foreach (var extendProperty in connection.GetMsDescriptionForColumn(dbObject, column))
                            {
                                column.Description = String.Concat(extendProperty.Value);
                            }
                        }
                    }

                    database.Tables.Add(table);
                }

                Logger?.LogInformation("Importing views for '{0}'...", database.Name);

                foreach (var view in ImportViews(database))
                {
                    if (Exclusions.Contains(view.FullName))
                    {
                        continue;
                    }

                    var dbObject = dbObjects.First(item => item.FullName == view.FullName);

                    if (ImportMSDescription)
                    {
                        foreach (var extendProperty in connection.GetMsDescriptionForDbObject(dbObject))
                        {
                            view.Description = String.Concat(extendProperty.Value);
                        }

                        foreach (var column in view.Columns)
                        {
                            foreach (var extendProperty in connection.GetMsDescriptionForColumn(dbObject, column))
                            {
                                column.Description = String.Concat(extendProperty.Value);
                            }
                        }
                    }

                    database.Views.Add(view);
                }

                Logger?.LogInformation("Importing stored procedures for '{0}'...", database.Name);

                foreach (var storedProcedure in ImportStoredProcedures(database))
                {
                    if (Exclusions.Contains(storedProcedure.FullName))
                    {
                        continue;
                    }

                    var dbObject = dbObjects.First(item => item.FullName == storedProcedure.FullName);

                    foreach (var extendProperty in connection.GetMsDescriptionForDbObject(dbObject))
                    {
                        storedProcedure.Description = String.Concat(extendProperty.Value);
                    }

                    database.StoredProcedures.Add(storedProcedure);
                }

                Logger?.LogInformation("Importing scalar functions for '{0}'...", database.Name);

                foreach (var scalarFunction in ImportScalarFunctions(database))
                {
                    if (Exclusions.Contains(scalarFunction.FullName))
                    {
                        continue;
                    }

                    var dbObject = dbObjects.First(item => item.FullName == scalarFunction.FullName);

                    foreach (var extendProperty in connection.GetMsDescriptionForDbObject(dbObject))
                    {
                        scalarFunction.Description = String.Concat(extendProperty.Value);
                    }

                    database.ScalarFunctions.Add(scalarFunction);
                }

                Logger?.LogInformation("Importing table functions for '{0}'...", database.Name);

                foreach (var tableFunction in ImportTableFunctions(database))
                {
                    if (Exclusions.Contains(tableFunction.FullName))
                    {
                        continue;
                    }

                    var dbObject = dbObjects.First(item => item.FullName == tableFunction.FullName);

                    foreach (var extendProperty in connection.GetMsDescriptionForDbObject(dbObject))
                    {
                        tableFunction.Description = String.Concat(extendProperty.Value);
                    }

                    database.TableFunctions.Add(tableFunction);
                }
            }

            return database;
        }

        protected virtual IEnumerable<DbObject> GetDbObjecs(DbConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.Connection = connection;
                command.CommandText = ImportCommandText;

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

        protected virtual IEnumerable<Table> ImportTables(Database db)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                foreach (var item in db.GetTables())
                {
                    using (var command = connection.CreateCommand())
                    {
                        var table = new Table
                        {
                            Schema = item.Schema,
                            Name = item.Name
                        };

                        command.Connection = connection;
                        command.CommandText = String.Format("sp_help '{0}'", item.FullName);

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

                column.Name = String.Concat(dataReader["Column_name"]);
                column.Type = String.Concat(dataReader["Type"]);
                column.Length = Int32.Parse(String.Concat(dataReader["Length"]));
                column.Prec = String.Concat(dataReader["Prec"]).Trim().Length == 0 ? (Int16)0 : Int16.Parse(String.Concat(dataReader["Prec"]));
                column.Scale = String.Concat(dataReader["Scale"]).Trim().Length == 0 ? (Int16)0 : Int16.Parse(String.Concat(dataReader["Scale"]));
                column.Nullable = String.Compare(String.Concat(dataReader["Nullable"]), "yes", true) == 0 ? true : false;
                column.Collation = String.Concat(dataReader["Collation"]);

                if (ExclusionTypes.Contains(column.Type))
                {
                    continue;
                }

                table.Columns.Add(column);
            }
        }

        protected virtual void SetIdentityToTable(Table table, DbDataReader dataReader)
        {
            var identity = String.Concat(dataReader["Identity"]);

            if (String.Compare(identity, "No identity column defined.", true) != 0)
            {
                table.Identity = new Identity(identity, Convert.ToInt32(dataReader["Seed"]), Convert.ToInt32(dataReader["Increment"]));
            }
        }

        protected virtual void AddContraintsToTable(Table table, DbDataReader dataReader)
        {
            while (dataReader.Read())
            {
                if (String.Concat(dataReader["constraint_type"]).Contains("PRIMARY KEY"))
                {
                    var key = String.Concat(dataReader["constraint_keys"]).Split(',').Select(item => item.Trim()).ToArray();

                    table.PrimaryKey = new PrimaryKey(key)
                    {
                        ConstraintName = String.Concat(dataReader["constraint_name"])
                    };
                }
                else if (String.Concat(dataReader["constraint_type"]).Contains("FOREIGN KEY"))
                {
                    var key = dataReader["constraint_keys"].ToString().Split(',').Select(item => item.Trim()).ToArray();

                    table.ForeignKeys.Add(new ForeignKey(key)
                    {
                        ConstraintName = String.Concat(dataReader["constraint_name"])
                    });
                }
                else if (String.Concat(dataReader["constraint_keys"]).Contains("REFERENCES"))
                {
                    var value = String.Concat(dataReader["constraint_keys"]).Replace("REFERENCES", String.Empty);

                    table.ForeignKeys[table.ForeignKeys.Count - 1].References = value.Substring(0, value.IndexOf("(")).Trim();
                }
                else if (String.Concat(dataReader["constraint_type"]).Contains("UNIQUE"))
                {
                    var key = dataReader["constraint_keys"].ToString().Split(',').Select(item => item.Trim()).ToArray();

                    table.Uniques.Add(new Unique(key)
                    {
                        ConstraintName = String.Concat(dataReader["constraint_name"])
                    });
                }
                else if (String.Concat(dataReader["constraint_type"]).Contains("CHECK"))
                {
                    var key = dataReader["constraint_keys"].ToString();

                    table.Checks.Add(new Check(key)
                    {
                        ConstraintName = String.Concat(dataReader["constraint_name"])
                    });
                }
            }
        }

        protected virtual IEnumerable<View> ImportViews(Database db)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                foreach (var item in db.GetViews())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = String.Format("sp_help '{0}'", item.FullName);

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

                                    column.Name = String.Concat(dataReader["Column_name"]);
                                    column.Type = String.Concat(dataReader["Type"]);
                                    column.Length = Int32.Parse(String.Concat(dataReader["Length"]));
                                    column.Prec = String.Concat(dataReader["Prec"]).Trim().Length == 0 ? (Int16)0 : Int16.Parse(String.Concat(dataReader["Prec"]));
                                    column.Scale = String.Concat(dataReader["Scale"]).Trim().Length == 0 ? (Int16)0 : Int16.Parse(String.Concat(dataReader["Scale"]));
                                    column.Nullable = String.Compare(String.Concat(dataReader["Nullable"]), "yes", true) == 0 ? true : false;
                                    column.Collation = String.Concat(dataReader["Collation"]);

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

        protected virtual IEnumerable<StoredProcedure> ImportStoredProcedures(Database db)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                foreach (var item in db.GetProcedures())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = String.Format("sp_help '{0}'", item.FullName);

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

                                    procedureParameter.Name = String.Concat(dataReader["Parameter_name"]);
                                    procedureParameter.Type = String.Concat(dataReader["Type"]);
                                    procedureParameter.Length = Int32.Parse(String.Concat(dataReader["Length"]));
                                    procedureParameter.Prec = String.Concat(dataReader["Prec"]).Trim().Length == 0 ? (Int16)0 : Int16.Parse(String.Concat(dataReader["Prec"]));
                                    procedureParameter.ParamOrder = String.Concat(dataReader["Param_order"]).Trim().Length == 0 ? (Int16)0 : Int16.Parse(String.Concat(dataReader["Param_order"]));
                                    procedureParameter.Collation = String.Concat(dataReader["Collation"]);

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

        protected virtual IEnumerable<ScalarFunction> ImportScalarFunctions(Database db)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                foreach (var item in db.GetProcedures())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = String.Format("sp_help '{0}'", item.FullName);

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

                                    procedureParameter.Name = String.Concat(dataReader["Parameter_name"]);
                                    procedureParameter.Type = String.Concat(dataReader["Type"]);
                                    procedureParameter.Length = Int32.Parse(String.Concat(dataReader["Length"]));
                                    procedureParameter.Prec = String.Concat(dataReader["Prec"]).Trim().Length == 0 ? (Int16)0 : Int16.Parse(String.Concat(dataReader["Prec"]));
                                    procedureParameter.ParamOrder = String.Concat(dataReader["Param_order"]).Trim().Length == 0 ? (Int16)0 : Int16.Parse(String.Concat(dataReader["Param_order"]));
                                    procedureParameter.Collation = String.Concat(dataReader["Collation"]);

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

        protected virtual IEnumerable<TableFunction> ImportTableFunctions(Database db)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                foreach (var item in db.GetProcedures())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = String.Format("sp_help '{0}'", item.FullName);

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

                                    procedureParameter.Name = String.Concat(dataReader["Parameter_name"]);
                                    procedureParameter.Type = String.Concat(dataReader["Type"]);
                                    procedureParameter.Length = Int32.Parse(String.Concat(dataReader["Length"]));
                                    procedureParameter.Prec = String.Concat(dataReader["Prec"]).Trim().Length == 0 ? (Int16)0 : Int16.Parse(String.Concat(dataReader["Prec"]));
                                    procedureParameter.ParamOrder = String.Concat(dataReader["Param_order"]).Trim().Length == 0 ? (Int16)0 : Int16.Parse(String.Concat(dataReader["Param_order"]));
                                    procedureParameter.Collation = String.Concat(dataReader["Collation"]);

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

        protected virtual IEnumerable<DbType> ImportDbTypes(Database db)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                foreach (var item in db.GetProcedures())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "select name, is_user_defined from sys.types";

                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                yield return new DbType
                                {
                                    Name = dataReader.GetString(0),
                                    IsUserDefined = dataReader.GetBoolean(1)
                                };
                            }
                        }
                    }
                }

                connection.Close();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using CatFactory.Diagnostics;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    public class SqlServerDatabaseFactory : IDatabaseFactory
    {
        public SqlServerDatabaseFactory()
        {
            ImportMSDescription = true;
        }

        public String ConnectionString { get; set; }

        public Boolean ImportMSDescription { get; set; }

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

        public Database Import()
        {
            Logger.Default.Log("Import database");

            var db = new Database();

            var repository = new ExtendPropertyRepository();

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                db.Name = connection.Database;

                var dbObjects = GetDbObjecs(connection).ToList();

                foreach (var dbObject in dbObjects)
                {
                    db.DbObjects.Add(dbObject);
                }

                foreach (var table in ImportTables(db))
                {
                    if (Exclusions.Contains(table.FullName))
                    {
                        continue;
                    }

                    var dbObject = dbObjects.First(item => item.FullName == table.FullName);

                    if (ImportMSDescription)
                    {
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

                    db.Tables.Add(table);
                }

                foreach (var view in ImportViews(db))
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

                    db.Views.Add(view);
                }

                foreach (var procedure in ImportProcedures(db))
                {
                    if (Exclusions.Contains(procedure.FullName))
                    {
                        continue;
                    }

                    var dbObject = dbObjects.First(item => item.FullName == procedure.FullName);

                    foreach (var extendProperty in connection.GetMsDescriptionForDbObject(dbObject))
                    {
                        procedure.Description = String.Concat(extendProperty.Value);
                    }

                    db.Procedures.Add(procedure);
                }

                connection.Close();
            }

            return db;
        }

        protected virtual IEnumerable<DbObject> GetDbObjecs(DbConnection connection)
        {
            Logger.Default.Log("Get DbObjecs");

            using (var command = connection.CreateCommand())
            {
                command.Connection = connection;
                command.CommandText = @"
                    select
                        schemas.name as schema_name,
                        tables.name as object_name,
                        'table' as object_type
                    from
                        sys.tables tables
                        inner join sys.schemas schemas on tables.schema_id = schemas.schema_id
                    union
                        select
                            schemas.name as schema_name,
                            views.name as object_name,
                            'view' as object_type
                        from
                            sys.views views
                            inner join sys.schemas schemas on views.schema_id = schemas.schema_id
                    union
                        select
                            sys.schemas.name as schema_name,
                            procedures.name as object_name,
                            'procedure' as object_type
                        from
                            sys.procedures procedures
                            inner join sys.schemas on procedures.schema_id = sys.schemas.schema_id
                ";

                using (var dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        yield return new DbObject()
                        {
                            Schema = dataReader.GetString(0),
                            Name = dataReader.GetString(1),
                            Type = dataReader.GetString(2)
                        };
                    }
                }
            }
        }

        protected IEnumerable<Table> ImportTables(Database db)
        {
            Logger.Default.Log("ImportTables");

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                foreach (var item in db.GetTables())
                {
                    using (var command = connection.CreateCommand())
                    {
                        Logger.Default.Log("Importing table: {0}", item.FullName);

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

        protected void AddColumnsToTable(Table table, DbDataReader dataReader)
        {
            Logger.Default.Log("AddColumnsToTable: {0}", table.FullName);

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

                table.Columns.Add(column);
            }
        }

        protected void SetIdentityToTable(Table table, DbDataReader dataReader)
        {
            Logger.Default.Log("SetIdentityToTable: {0}", table.FullName);

            var identity = String.Concat(dataReader["Identity"]);

            if (String.Compare(identity, "No identity column defined.", true) != 0)
            {
                table.Identity = new Identity(identity, Convert.ToInt32(dataReader["Seed"]), Convert.ToInt32(dataReader["Increment"]));
            }
        }

        protected void AddContraintsToTable(Table table, DbDataReader dataReader)
        {
            Logger.Default.Log("AddContraintsToTable: {0}", table.FullName);

            while (dataReader.Read())
            {
                if (String.Concat(dataReader["constraint_type"]).Contains("PRIMARY KEY"))
                {
                    table.PrimaryKey = new PrimaryKey
                    {
                        Key = new List<String>(dataReader["constraint_keys"].ToString().Split(',').Select(item => item.Trim()))
                    };
                }
                else if (String.Concat(dataReader["constraint_type"]).Contains("FOREIGN KEY"))
                {
                    table.ForeignKeys.Add(new ForeignKey(dataReader["constraint_keys"].ToString().Split(',').Select(item => item.Trim()).ToArray()));
                }
                else if (String.Concat(dataReader["constraint_keys"]).Contains("REFERENCES"))
                {
                    table.ForeignKeys[table.ForeignKeys.Count - 1].References = String.Concat(dataReader["constraint_keys"]).Replace("REFERENCES", String.Empty).Trim();
                }
                else if (String.Concat(dataReader["constraint_type"]).Contains("UNIQUE"))
                {
                    table.Uniques.Add(new Unique(dataReader["constraint_keys"].ToString().Split(',').Select(item => item.Trim()).ToArray()));
                }
            }
        }

        protected IEnumerable<View> ImportViews(Database db)
        {
            Logger.Default.Log("ImportViews");

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                foreach (var item in db.GetViews())
                {
                    using (var command = connection.CreateCommand())
                    {
                        Logger.Default.Log("Importing view: {0}", item.FullName);

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

        protected IEnumerable<Procedure> ImportProcedures(Database db)
        {
            Logger.Default.Log("ImportProcedures");

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                foreach (var item in db.GetProcedures())
                {
                    using (var command = connection.CreateCommand())
                    {
                        Logger.Default.Log("Importing procedure: {0}", item.FullName);

                        command.Connection = connection;
                        command.CommandText = String.Format("sp_help '{0}'", item.FullName);

                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var procedure = new Procedure
                                {
                                    Schema = item.Schema,
                                    Name = item.Name
                                };

                                dataReader.NextResult();

                                while (dataReader.Read())
                                {
                                    var procedureParameter = new ProcedureParameter();

                                    procedureParameter.Name = String.Concat(dataReader["Parameter_name"]);
                                    procedureParameter.Type = String.Concat(dataReader["Type"]);
                                    procedureParameter.Length = Int32.Parse(String.Concat(dataReader["Length"]));
                                    procedureParameter.Prec = String.Concat(dataReader["Prec"]).Trim().Length == 0 ? (Int16)0 : Int16.Parse(String.Concat(dataReader["Prec"]));
                                    procedureParameter.ParamOrder = String.Concat(dataReader["Param_order"]).Trim().Length == 0 ? (Int16)0 : Int16.Parse(String.Concat(dataReader["Param_order"]));
                                    procedureParameter.Collation = String.Concat(dataReader["Collation"]);

                                    procedure.Parameters.Add(procedureParameter);
                                }

                                yield return procedure;
                            }
                        }
                    }
                }

                connection.Close();
            }
        }
    }
}

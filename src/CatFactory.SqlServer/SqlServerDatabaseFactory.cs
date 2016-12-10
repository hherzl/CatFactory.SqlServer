using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using CatFactory.Diagnostics;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    public class SqlServerDatabaseFactory : IDatabaseFactory
    {
        public SqlServerDatabaseFactory()
        {
        }

        public String ConnectionString { get; set; }

        public Database Import()
        {
            Logger.Default.Log("Import database");

            var db = new Database();

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                db.Name = connection.Database;

                foreach (var item in GetDbObjecs(connection))
                {
                    db.DbObjects.Add(item);
                }

                foreach (var item in ImportTables(db))
                {
                    db.Tables.Add(item);
                }

                foreach (var item in ImportViews(db))
                {
                    db.Views.Add(item);
                }

                // todo: add procedures in import process

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
                    order by
                        schema_name,
                        object_type,
                        object_name
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
                column.Nullable = String.Compare(String.Concat(dataReader["Nullable"]), "yes", true) == 0 ? true : false;

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
            while (dataReader.Read())
            {
                if (String.Concat(dataReader["constraint_type"]) == "PRIMARY KEY (clustered)")
                {
                    table.PrimaryKey = new PrimaryKey
                    {
                        Key = new List<String>(dataReader["constraint_keys"].ToString().Split(','))
                    };
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
                                    column.Nullable = String.Compare(String.Concat(dataReader["Nullable"]), "yes", true) == 0 ? true : false;

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
    }
}

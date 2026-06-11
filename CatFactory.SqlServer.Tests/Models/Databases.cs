using System.Collections.ObjectModel;
using CatFactory.ObjectRelationalMapping;

namespace CatFactory.SqlServer.Tests.Models;

public static class Databases
{
    public static SqlServerDatabase Blogging
    {
        get
        {
            var db = new SqlServerDatabase
            {
                Name = "Blogging",
                DefaultSchema = "dbo",
                NamingConvention = new SqlServerDatabaseNamingConvention(),
                DatabaseTypeMaps = [.. new SqlServerDatabaseFactory().DatabaseTypeMaps],
                Tables =
                {
                    new Table
                    {
                        Schema = "dbo",
                        Name = "Blog",
                        Columns =
                        {
                            new Column("BlogId", "int"),
                            new Column("Name", "varchar", 128),
                            new Column("Url", "varchar", 255)
                        },
                        Identity = new("BlogId")
                    },
                    new Table
                    {
                        Schema = "dbo",
                        Name = "Post",
                        Columns =
                        {
                            new Column("PostId", "int"),
                            new Column("Title", "varchar", 128),
                            new Column("Content", "varchar"),
                            new Column("BlogId", "int")
                        },
                        Identity = new("PostId"),
                        Uniques =
                        {
                            new Unique
                            {
                                Key =
                                {
                                    "PostId",
                                    "BlogId"
                                }
                            }
                        }
                    }
                }
            };

            db.ImportBag.ExtendedProperties = new Collection<ExtendedProperty>();

            db
                .AddDbObjectsFromTables()
                .SetPrimaryKeyForTables()
                .LinkTables()
                .AddColumnsForTables(new Column[]
                {
                    new("CreationUser", "varchar", 50),
                    new("CreationDate", "datetime"),
                    new("LastUpdateUser", "varchar", 50, true),
                    new("LastUpdateDate", "datetime", true),
                    new("RowVersionId", "rowversion", true)
                });

            foreach (var table in db.Tables)
            {
                table.ImportBag.ExtendedProperties = new Collection<ExtendedProperty>();

                foreach (var column in table.Columns)
                {
                    column.ImportBag.ExtendedProperties = new Collection<ExtendedProperty>();
                }
            }

            return db;
        }
    }
}

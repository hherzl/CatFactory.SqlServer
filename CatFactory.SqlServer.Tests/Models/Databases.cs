﻿using System.Collections.ObjectModel;
using CatFactory.ObjectRelationalMapping;
using CatFactory.SqlServer.DatabaseObjectModel;

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
                DatabaseTypeMaps = new SqlServerDatabaseFactory().DatabaseTypeMaps.ToList(),
                Tables =
                {
                    new Table
                    {
                        Schema = "dbo",
                        Name = "Blog",
                        Columns =
                        {
                            new Column { Name = "BlogId", Type = "int" },
                            new Column { Name = "Name", Type = "varchar", Length = 128 },
                            new Column { Name = "Url", Type = "varchar", Length = 255 }
                        },
                        Identity = new Identity("BlogId")
                    },
                    new Table
                    {
                        Schema = "dbo",
                        Name = "Post",
                        Columns =
                        {
                            new Column { Name = "PostId", Type = "int" },
                            new Column { Name = "Title", Type = "varchar", Length = 128 },
                            new Column { Name = "Content", Type = "varchar" },
                            new Column { Name = "BlogId", Type = "int" }
                        },
                        Identity = new Identity("PostId"),
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
                    new Column { Name = "CreationUser", Type = "varchar", Length = 50 },
                    new Column { Name = "CreationDate", Type = "datetime" },
                    new Column { Name = "LastUpdateUser", Type = "varchar", Length = 50, Nullable = true },
                    new Column { Name = "LastUpdateDate", Type = "datetime", Nullable = true },
                    new Column { Name = "RowVersionID", Type = "rowversion", Nullable = true }
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

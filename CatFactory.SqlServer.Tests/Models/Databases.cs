using CatFactory.ObjectRelationalMapping;

namespace CatFactory.SqlServer.Tests.Models
{
    public static class Databases
    {
        public static Database Blogging
            => new Database
            {
                Name = "Blogging",
                DefaultSchema = "dbo",
                NamingConvention = new SqlServerDatabaseNamingConvention(),
                DatabaseTypeMaps = DatabaseTypeMapList.Definition,
                Tables =
                {
                    new Table
                    {
                        Schema = "dbo",
                        Name = "Blog",
                        Columns =
                        {
                            new Column { Name = "BlogId", Type = "int" },
                            new Column { Name = "Url", Type = "varchar", Length = 128 }
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
                            new Column { Name = "BlogId", Type = "int", Length = 25 }
                        },
                        Identity = new Identity("PostId")
                    }
                }
            }
            .AddDbObjectsFromTables()
            .SetPrimaryKeyForTables()
            .AddColumnsForTables(new Column[]
            {
                new Column { Name = "CreationUser", Type = "varchar", Length = 50 },
                new Column { Name = "CreationDate", Type = "datetime" },
                new Column { Name = "LastUpdateUser", Type = "varchar", Length = 50, Nullable = true },
                new Column { Name = "LastUpdateDate", Type = "datetime", Nullable = true },
                new Column { Name = "RowVersionID", Type = "rowversion", Nullable = true }
            })
            .LinkTables();
    }
}

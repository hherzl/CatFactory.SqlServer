using System;
using System.Collections.Generic;
using System.Linq;
using CatFactory.Mapping;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class Tests
    {
        [Fact]
        public void ImportStoreDatabaseTest()
        {
            var logger = LoggerMocker.GetLogger<SqlServerDatabaseFactory>();

            var db = SqlServerDatabaseFactory.Import(logger, "server=(local);database=Store;integrated security=yes;");
        }

        [Fact]
        public void ImportNorthwindDatabaseTest()
        {
            var logger = LoggerMocker.GetLogger<SqlServerDatabaseFactory>();

            var db = SqlServerDatabaseFactory.Import(logger, "server=(local);database=Northwind;integrated security=yes;", "dbo.ChangeLog");
        }

        [Fact]
        public void ImportAdventureWorksDatabase()
        {
            var logger = LoggerMocker.GetLogger<SqlServerDatabaseFactory>();

            // todo: add mapping for custom types
            var databaseFactory = new SqlServerDatabaseFactory()
            {
                ConnectionString = "server=(local);database=AdventureWorks2012;integrated security=yes;",
                ExclusionTypes = new List<String>()
                {
                    "geography"
                }
            };

            var database = databaseFactory.Import();

            var table = database.Tables.FirstOrDefault(item => item.FullName == "Person.Address");

            Assert.False(table == null);

            Assert.False(table.Columns.Contains(new Column { Name = "SpatialLocation" }));
        }
    }
}

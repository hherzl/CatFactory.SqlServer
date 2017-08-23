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
            var db = SqlServerDatabaseFactory
                .Import(LoggerMocker.GetLogger<SqlServerDatabaseFactory>(), "server=(local);database=Store;integrated security=yes;");
        }

        [Fact]
        public void ImportNorthwindDatabaseTest()
        {
            var db = SqlServerDatabaseFactory
                .Import(LoggerMocker.GetLogger<SqlServerDatabaseFactory>(), "server=(local);database=Northwind;integrated security=yes;", "dbo.ChangeLog");
        }

        [Fact]
        public void ImportAdventureWorksDatabase()
        {
            // todo: add mapping for custom types
            var factory = new SqlServerDatabaseFactory(LoggerMocker.GetLogger<SqlServerDatabaseFactory>())
            {
                ConnectionString = "server=(local);database=AdventureWorks2012;integrated security=yes;",
                ExclusionTypes = new List<String>()
                {
                    "geography"
                }
            };

            var db = factory.Import();

            var table = db.Tables.FirstOrDefault(item => item.FullName == "Person.Address");

            Assert.False(table == null);

            Assert.False(table.Columns.Contains(new Column { Name = "SpatialLocation" }));
        }
    }
}

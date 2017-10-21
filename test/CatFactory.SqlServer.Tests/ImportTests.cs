using System.Collections.Generic;
using CatFactory.Mapping;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class Tests
    {
        [Fact]
        public void ImportStoreDatabaseTest()
        {
            // Arrange
            var logger = LoggerMocker.GetLogger<SqlServerDatabaseFactory>();

            // Act
            var database = SqlServerDatabaseFactory.Import(logger, "server=(local);database=Store;integrated security=yes;");

            // Assert
            Assert.True(database.Tables.Count > 0);
        }

        [Fact]
        public void ImportNorthwindDatabaseTest()
        {
            // Arrange
            var logger = LoggerMocker.GetLogger<SqlServerDatabaseFactory>();

            // Act
            var database = SqlServerDatabaseFactory.Import(logger, "server=(local);database=Northwind;integrated security=yes;", "dbo.ChangeLog");

            // Assert
            Assert.True(database.Tables.Count > 0);
            Assert.True(database.FindTableBySchemaAndName("dbo.ChangeLog") == null);
        }

        [Fact]
        public void ImportAdventureWorksDatabase()
        {
            // Arrange
            var logger = LoggerMocker.GetLogger<SqlServerDatabaseFactory>();

            // todo: add mapping for custom types
            var databaseFactory = new SqlServerDatabaseFactory(logger)
            {
                ConnectionString = "server=(local);database=AdventureWorks2012;integrated security=yes;",
                ExclusionTypes = new List<string> { "geography" }
            };

            // Act
            var database = databaseFactory.Import();

            // Assert
            foreach (var table in database.Tables)
            {
                Assert.False(table.Columns.Contains(new Column { Name = "SpatialLocation" }));
            }
        }
    }
}

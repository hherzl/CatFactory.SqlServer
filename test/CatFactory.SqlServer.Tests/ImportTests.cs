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
            var database = SqlServerDatabaseFactory
                .Import(logger, "server=(local);database=Store;integrated security=yes;");

            // Assert
            Assert.True(database.Tables.Count > 0);
        }

        [Fact]
        public void ImportNorthwindDatabaseTest()
        {
            // Arrange
            var logger = LoggerMocker.GetLogger<SqlServerDatabaseFactory>();

            // Act
            var database = SqlServerDatabaseFactory
                .Import(logger, "server=(local);database=Northwind;integrated security=yes;", "dbo.ChangeLog");

            // Assert
            Assert.True(database.Tables.Count > 0);
            Assert.True(database.FindTableBySchemaAndName("dbo.ChangeLog") == null);
        }

        [Fact]
        public void FullImportNorthwindDatabaseTest()
        {
            // Arrange
            var logger = LoggerMocker.GetLogger<SqlServerDatabaseFactory>();
            var databaseFactory = new SqlServerDatabaseFactory(logger)
            {
                ConnectionString = "server=(local);database=Northwind;integrated security=yes;MultipleActiveResultSets=true;",
                ImportSettings = new DatabaseImportSettings
                {
                    ImportStoredProcedures = true,
                    ImportTableFunctions = true,
                    ImportScalarFunctions = true
                }
            };

            // Act
            var database = databaseFactory.Import();

            // Assert
            Assert.True(database.Tables.Count > 0);
            Assert.True(database.Views.Count > 0);
            Assert.True(database.StoredProcedures.Count > 0);
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
                ImportSettings = new DatabaseImportSettings
                {
                    ExclusionTypes = new List<string>
                    {
                        "geography"
                    }
                }
            };

            // Act
            var database = databaseFactory.Import();

            // Assert
            foreach (var table in database.Tables)
            {
                Assert.False(table.Columns.Contains(new Column { Name = "SpatialLocation" }));
            }
        }

        [Fact]
        public void FullImportAdventureWorksDatabase()
        {
            // Arrange
            var logger = LoggerMocker.GetLogger<SqlServerDatabaseFactory>();

            // todo: add mapping for custom types
            var databaseFactory = new SqlServerDatabaseFactory(logger)
            {
                ConnectionString = "server=(local);database=AdventureWorks2012;integrated security=yes;MultipleActiveResultSets=true;",
                ImportSettings = new DatabaseImportSettings
                {
                    ImportStoredProcedures = true,
                    ImportTableFunctions = true,
                    ImportScalarFunctions = true,
                    ExclusionTypes = new List<string>
                    {
                        "geography"
                    }
                }
            };

            // Act
            var database = databaseFactory.Import();

            // Assert
            foreach (var table in database.Tables)
            {
                Assert.False(table.Columns.Contains(new Column { Name = "SpatialLocation" }));
            }
        }

        [Fact]
        public void ImportWithLoggerStoreTablesTest()
        {
            // Arrange
            var logger = LoggerMocker.GetLogger<SqlServerDatabaseFactory>();

            // Act
            var database = SqlServerDatabaseFactory
                .ImportTables(
                logger,
                "server=(local);database=Store;integrated security=yes;",
                "Sales.Order",
                "Sales.OrderDetail");

            // Assert
            Assert.True(database.Tables.Count == 2);
            Assert.True(database.Views.Count == 0);
        }

        [Fact]
        public void ImportWithoutLoggerStoreTablesTest()
        {
            // Arrange
            // Act
            var database = SqlServerDatabaseFactory
                .ImportTables("server=(local);database=Store;integrated security=yes;",
                "Sales.Order",
                "Sales.OrderDetail");

            // Assert
            Assert.True(database.Tables.Count == 2);
            Assert.True(database.Views.Count == 0);
        }

        [Fact]
        public void ImportNorthwindTables()
        {
            // Arrange

            // Act
            var database = SqlServerDatabaseFactory
                .ImportTables("server=(local);database=Northwind;integrated security=yes;");

            // Assert
            Assert.True(database.Tables.Count > 0);
            Assert.True(database.Views.Count == 0);
        }

        [Fact]
        public void ImportNorthwindViews()
        {
            // Arrange

            // Act
            var database = SqlServerDatabaseFactory
                .ImportViews("server=(local);database=Northwind;integrated security=yes;");

            // Assert
            Assert.True(database.Tables.Count == 0);
            Assert.True(database.Views.Count > 0);
        }

        [Fact]
        public void ImportNorthwindTablesAndViews()
        {
            // Arrange

            // Act
            var database = SqlServerDatabaseFactory
                .ImportTablesAndViews("server=(local);database=Northwind;integrated security=yes;",
                "dbo.Orders",
                "dbo.Order Details",
                "dbo.Category Sales for 1997",
                "dbo.Product Sales for 1997");

            // Assert
            Assert.True(database.Tables.Count == 2);
            Assert.True(database.Views.Count == 2);
        }
    }
}

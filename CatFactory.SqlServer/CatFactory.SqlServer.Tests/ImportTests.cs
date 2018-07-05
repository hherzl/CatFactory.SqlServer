using System.Linq;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class Tests
    {
        [Fact]
        public void ImportStoreDatabaseTest()
        {
            // Arrange
            var logger = LoggerHelper.GetLogger<SqlServerDatabaseFactory>();

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
            var logger = LoggerHelper.GetLogger<SqlServerDatabaseFactory>();

            // Act
            var database = SqlServerDatabaseFactory
                .Import(logger, "server=(local);database=Northwind;integrated security=yes;", "dbo.ChangeLog");

            // Assert
            Assert.True(database.Tables.Count > 0);
            Assert.True(database.FindTable("dbo.ChangeLog") == null);
            Assert.True(database.FindTable("dbo.Products").Columns.Count > 0);
            Assert.True(database.FindTable("dbo.Products").PrimaryKey != null);
            Assert.True(database.FindTable("dbo.Products").ForeignKeys.Count > 0);
            Assert.True(database.Views.Count > 0);
            Assert.True(database.FindView("dbo.Invoices").Columns.Count > 0);
        }

        [Fact]
        public void FullImportNorthwindDatabaseTest()
        {
            // Arrange
            var logger = LoggerHelper.GetLogger<SqlServerDatabaseFactory>();
            var databaseFactory = new SqlServerDatabaseFactory(logger)
            {
                ImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=Northwind;integrated security=yes;MultipleActiveResultSets=true;",
                    ImportStoredProcedures = true,
                    ImportTableFunctions = true,
                    ImportScalarFunctions = true
                }
            };

            // Act
            var database = databaseFactory.Import();

            // Assert
            Assert.True(database.Tables.Count > 0);
            Assert.True(database.FindTable("dbo.Orders").Columns.Count > 0);
            Assert.True(database.FindTable("dbo.Orders").PrimaryKey != null);
            Assert.True(database.FindTable("dbo.Orders").ForeignKeys.Count > 0);
            Assert.True(database.Views.Count > 0);
            Assert.True(database.FindView("dbo.Invoices").Columns.Count > 0);
            Assert.True(database.StoredProcedures.Count > 0);
        }

        [Fact]
        public void ImportAdventureWorksDatabase()
        {
            // Arrange
            var logger = LoggerHelper.GetLogger<SqlServerDatabaseFactory>();

            // todo: add mapping for custom types
            var databaseFactory = new SqlServerDatabaseFactory(logger)
            {
                ImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=AdventureWorks2017;integrated security=yes;",
                    ExclusionTypes = { "geography" }
                }
            };

            // Act
            var database = databaseFactory.Import();

            // Assert
            foreach (var table in database.Tables)
            {
                var flag = table.Columns.FirstOrDefault(item => item.Name == "SpatialLocation") == null ? false : true;

                Assert.False(flag);
            }
        }

        [Fact]
        public void FullImportAdventureWorksDatabase()
        {
            // Arrange
            var logger = LoggerHelper.GetLogger<SqlServerDatabaseFactory>();

            // todo: add mapping for custom types
            var databaseFactory = new SqlServerDatabaseFactory(logger)
            {
                ImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=AdventureWorks2017;integrated security=yes;MultipleActiveResultSets=true;",
                    ImportStoredProcedures = true,
                    ImportTableFunctions = true,
                    ImportScalarFunctions = true,
                    ExtendedProperties = { "MS_Description" },
                    ExclusionTypes = { "geography" }
                }
            };

            // Act
            var database = databaseFactory.Import();

            // Assert
            foreach (var table in database.Tables)
            {
                var flag = table.Columns.FirstOrDefault(item => item.Name == "SpatialLocation") == null ? false : true;

                Assert.False(flag);
            }

            Assert.True(database.FindTable("Sales.SalesOrderHeader").Columns.Count > 0);
            Assert.False(string.IsNullOrEmpty(database.FindTable("Sales.SalesOrderHeader").Columns.First().Description));
            Assert.True(database.FindTable("Sales.SalesOrderHeader").PrimaryKey != null);
            Assert.True(database.FindTable("Sales.SalesOrderHeader").ForeignKeys.Count > 0);

            Assert.True(database.TableFunctions.FirstOrDefault(item => item.FullName == "dbo.ufnGetContactInformation").Parameters.Count == 1);
        }

        [Fact]
        public void ImportWithLoggerStoreTablesTest()
        {
            // Arrange
            var logger = LoggerHelper.GetLogger<SqlServerDatabaseFactory>();

            // Act
            var database = SqlServerDatabaseFactory
                .ImportTables(logger, "server=(local);database=Store;integrated security=yes;", "Sales.Order", "Sales.OrderDetail");

            // Assert
            Assert.True(database.Tables.Count == 2);
            Assert.True(database.FindTable("Sales.Order").Columns.Count > 0);
            Assert.True(database.FindTable("Sales.Order").PrimaryKey != null);
            Assert.True(database.FindTable("Sales.Order").ForeignKeys.Count > 0);
            Assert.True(database.Views.Count == 0);
        }

        [Fact]
        public void ImportWithoutLoggerStoreTablesTest()
        {
            // Arrange and Act
            var database = SqlServerDatabaseFactory
                .ImportTables("server=(local);database=Store;integrated security=yes;", "Sales.Order", "Sales.OrderDetail");

            // Assert
            Assert.True(database.Tables.Count == 2);
            Assert.True(database.Views.Count == 0);
        }

        [Fact]
        public void ImportNorthwindTables()
        {
            // Arrange and Act
            var database = SqlServerDatabaseFactory
                .ImportTables("server=(local);database=Northwind;integrated security=yes;");

            // Assert
            Assert.True(database.Tables.Count > 0);
            Assert.True(database.Views.Count == 0);
        }

        [Fact]
        public void ImportNorthwindViews()
        {
            // Arrange and Act
            var database = SqlServerDatabaseFactory
                .ImportViews("server=(local);database=Northwind;integrated security=yes;");

            // Assert
            Assert.True(database.Tables.Count == 0);
            Assert.True(database.Views.Count > 0);
        }

        [Fact]
        public void ImportNorthwindTablesAndViews()
        {
            // Arrange and Act
            var database = SqlServerDatabaseFactory
                .ImportTablesAndViews("server=(local);database=Northwind;integrated security=yes;",
                "dbo.Orders",
                "dbo.Order Details",
                "dbo.Category Sales for 1997",
                "dbo.Product Sales for 1997");

            // Assert
            Assert.True(database.Tables.Count == 2);
            Assert.True(database.FindTable("dbo.Orders").Columns.Count > 0);
            Assert.True(database.FindTable("dbo.Orders").PrimaryKey != null);
            Assert.True(database.Views.Count == 2);
        }
    }
}

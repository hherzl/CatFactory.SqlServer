using System.Linq;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class ImportTests
    {
        [Fact]
        public void ImportOnlineStoreDatabaseTest()
        {
            // Arrange and Act
            var database = SqlServerDatabaseFactory
                .Import("server=(local);database=OnlineStore;integrated security=yes;");

            // Assert
            Assert.True(database.Tables.Count > 0);

            Assert.True(database.FindTable("Warehouse.Product").Columns.Count > 0);
            Assert.True(database.FindTable("Warehouse.Product").PrimaryKey != null);
            Assert.True(database.FindTable("Warehouse.Product").ForeignKeys.Count > 0);

            Assert.True(database.Views.Count > 0);

            Assert.True(database.FindView("Sales.OrderSummary").Columns.Count > 0);
        }

        [Fact]
        public void ImportTablesFromOnlineStoreDatabaseTest()
        {
            // Arrange and Act
            var database = SqlServerDatabaseFactory
                .ImportTables("server=(local);database=OnlineStore;integrated security=yes;", "Sales.OrderHeader", "Sales.OrderDetail");

            // Assert
            Assert.True(database.Tables.Count == 2);

            Assert.True(database.FindTable("Sales.OrderHeader").Columns.Count > 0);
            Assert.True(database.FindTable("Sales.OrderHeader").PrimaryKey != null);
            Assert.True(database.FindTable("Sales.OrderHeader").ForeignKeys.Count > 0);
            Assert.True(database.FindTable("Sales.OrderDetail").ForeignKeys.Count > 0);

            Assert.True(database.Views.Count == 0);
        }

        [Fact]
        public void ImportNorthwindDatabaseTest()
        {
            // Arrange and Act
            var database = SqlServerDatabaseFactory
                .Import("server=(local);database=Northwind;integrated security=yes;", "dbo.ChangeLog");

            // Assert
            Assert.True(database.Tables.Count > 0);

            Assert.True(database.FindTable("dbo.ChangeLog") == null);
            Assert.True(database.FindTable("dbo.Products").Columns.Count > 0);
            Assert.True(database.FindTable("dbo.Products").PrimaryKey != null);
            Assert.True(database.FindTable("dbo.Products").ForeignKeys.Count > 0);
            Assert.True(database.FindTable("dbo.Products").Defaults.Count > 0);
            Assert.True(database.FindTable("dbo.Products").Checks.Count > 0);

            Assert.True(database.Views.Count > 0);

            Assert.True(database.FindView("dbo.Invoices").Columns.Count > 0);
        }

        [Fact]
        public void FullImportNorthwindDatabaseTest()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=Northwind;integrated security=yes;",
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

            Assert.True(database.StoredProcedures.First(item => item.FullName == "dbo.CustOrderHist").FirstResultSetsForObject.Count > 0);
        }

        [Fact]
        public void ImportTablesFromNorthwindDatabaseTest()
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
        public void ImportTablesAndViewsFromNorthwindTest()
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

        [Fact]
        public void ImportAdventureWorksDatabase()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory(SqlServerDatabaseFactory.GetLogger())
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=AdventureWorks2017;integrated security=yes;",
                    ImportStoredProcedures = true,
                    ImportScalarFunctions = true,
                    ImportTableFunctions = true,
                    ExclusionTypes =
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
                var flag = table.Columns.FirstOrDefault(item => item.Name == "SpatialLocation") == null ? false : true;

                Assert.False(flag);
            }

            Assert.True(database.FindTable("Sales.SalesOrderHeader").Columns.Count > 0);
            Assert.True(database.FindTable("Sales.SalesOrderHeader").Indexes.Count > 0);
            Assert.True(database.FindTable("Sales.SalesOrderHeader").PrimaryKey != null);
            Assert.True(database.FindTable("Sales.SalesOrderHeader").ForeignKeys.Count > 0);

            Assert.True(database.FindView("Production.vProductAndDescription").Columns.Count > 0);
            Assert.True(database.FindView("Production.vProductAndDescription").Indexes.Count > 0);

            Assert.True(database.TableFunctions.FirstOrDefault(item => item.FullName == "dbo.ufnGetContactInformation").Parameters.Count == 1);
            Assert.True(database.StoredProcedures.FirstOrDefault(item => item.FullName == "HumanResources.uspUpdateEmployeeHireInfo").FirstResultSetsForObject.Count == 0);
        }

        [Fact]
        public void ImportWideWorldImportersDatabase()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=WideWorldImporters;integrated security=yes;"
                }
            };

            // Act
            var database = databaseFactory.Import();

            // Assert
            Assert.True(database.FindTable("Warehouse.StockItems").Columns.Count > 0);
            Assert.True(database.FindTable("Warehouse.StockItems")["Tags"].Computed == "yes");
            Assert.True(database.FindTable("Warehouse.StockItems").Defaults.Count > 0);
            Assert.False(database.FindTable("Warehouse.StockItems")["StockItemID"].ComputedExpression == null);
        }
    }
}

using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class ImportTests
    {
        [Fact]
        public async Task ImportOnlineStoreDatabaseAsync()
        {
            // Arrange and Act
            var database = await SqlServerDatabaseFactory
                .ImportAsync("server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;");

            // Assert
            Assert.True(database.Tables.Count > 0);

            Assert.True(database.FindTable("Warehouse.Product").Columns.Count > 0);
            Assert.True(database.FindTable("Warehouse.Product").PrimaryKey != null);
            Assert.True(database.FindTable("Warehouse.Product").ForeignKeys.Count > 0);

            Assert.True(database.Views.Count > 0);

            Assert.True(database.FindView("Sales.OrderSummary").Columns.Count > 0);
        }

        [Fact]
        public async Task ImportTablesFromOnlineStoreDatabaseAsync()
        {
            // Arrange and Act
            var database = await SqlServerDatabaseFactory
                .ImportTablesAsync("server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;", "Sales.OrderHeader", "Sales.OrderDetail");

            // Assert
            Assert.True(database.Tables.Count == 2);

            Assert.True(database.FindTable("Sales.OrderHeader").Columns.Count > 0);
            Assert.True(database.FindTable("Sales.OrderHeader").PrimaryKey != null);
            Assert.True(database.FindTable("Sales.OrderHeader").ForeignKeys.Count > 0);
            Assert.True(database.FindTable("Sales.OrderDetail").ForeignKeys.Count > 0);

            Assert.True(database.Views.Count == 0);
        }

        [Fact]
        public async Task ImportNorthwindDatabaseAsync()
        {
            // Arrange and Act
            var database = await SqlServerDatabaseFactory
                .ImportAsync("server=(local); database=Northwind; integrated security=yes; TrustServerCertificate=True;", "dbo.ChangeLog");

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
        public async Task FullImportNorthwindDatabaseAsync()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local); database=Northwind; integrated security=yes; TrustServerCertificate=True;",
                    ImportStoredProcedures = true,
                    ImportTableFunctions = true,
                    ImportScalarFunctions = true
                }
            };

            // Act
            var database = (SqlServerDatabase)await databaseFactory.ImportAsync();

            // Assert
            Assert.True(database.Tables.Count > 0);

            Assert.True(database.FindTable("dbo.Orders").Columns.Count > 0);
            Assert.True(database.FindTable("dbo.Orders").PrimaryKey != null);
            Assert.True(database.FindTable("dbo.Orders").ForeignKeys.Count > 0);

            Assert.True(database.Views.Count > 0);

            Assert.True(database.FindView("dbo.Invoices").Columns.Count > 0);

            Assert.True(database.StoredProcedures.Count > 0);

            Assert.True(database.StoredProcedures.First(item => item.FullName == "dbo.CustOrderHist").ResultSets.Count > 0);
        }

        [Fact]
        public async Task ImportTablesFromNorthwindDatabaseAsync()
        {
            // Arrange and Act
            var database = await SqlServerDatabaseFactory
                .ImportTablesAsync("server=(local); database=Northwind; integrated security=yes; TrustServerCertificate=True;");

            // Assert
            Assert.True(database.Tables.Count > 0);
            Assert.True(database.Views.Count == 0);
        }

        [Fact]
        public async Task ImportNorthwindViewsAsync()
        {
            // Arrange and Act
            var database = await SqlServerDatabaseFactory
                .ImportViewsAsync("server=(local); database=Northwind; integrated security=yes; TrustServerCertificate=True;");

            // Assert
            Assert.True(database.Tables.Count == 0);
            Assert.True(database.Views.Count > 0);
        }

        [Fact]
        public async Task ImportTablesAndViewsFromNorthwindAsync()
        {
            // Arrange and Act
            var database = await SqlServerDatabaseFactory
                .ImportTablesAndViewsAsync("server=(local); database=Northwind; integrated security=yes; TrustServerCertificate=True;", "dbo.Orders", "dbo.Order Details", "dbo.Category Sales for 1997", "dbo.Product Sales for 1997");

            // Assert
            Assert.True(database.Tables.Count == 2);
            Assert.True(database.FindTable("dbo.Orders").Columns.Count > 0);
            Assert.True(database.FindTable("dbo.Orders").PrimaryKey != null);

            Assert.True(database.Views.Count == 2);
        }

        [Fact]
        public async Task ImportAdventureWorksDatabaseAsync()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory(SqlServerDatabaseFactory.GetLogger())
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local); database=AdventureWorks2017; integrated security=yes; TrustServerCertificate=True;",
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
            var database = (SqlServerDatabase)await databaseFactory.ImportAsync();

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
            Assert.True(database.StoredProcedures.FirstOrDefault(item => item.FullName == "HumanResources.uspUpdateEmployeeHireInfo").ResultSets.Count == 0);
        }

        [Fact]
        public async Task ImportWideWorldImportersDatabaseAsync()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local); database=WideWorldImporters; integrated security=yes; TrustServerCertificate=True;",
                    ImportSequences = true
                }
            };

            // Act
            var database = (SqlServerDatabase)await databaseFactory.ImportAsync();

            // Assert
            Assert.True(database.FindTable("Warehouse.StockItems").Columns.Count > 0);
            Assert.True(database.FindTable("Warehouse.StockItems").Defaults.Count > 0);
            Assert.True(database.FindTable("Warehouse.StockItems")["Tags"].Computed == "yes");
            Assert.False(database.FindTable("Warehouse.StockItems")["StockItemID"].ImportBag.ComputedExpression == null);

            Assert.True(database.Sequences.Count > 0);
        }
    }
}

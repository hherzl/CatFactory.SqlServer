using System.Threading.Tasks;
using CatFactory.SqlServer.Features;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class DocumentationTests
    {
        private const string OnlineStoreConnectionString = "server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;";
        private const string AdventureWorks2017ConnectionString = "server=(local); database=AdventureWorks2017; integrated security=yes; TrustServerCertificate=True;";
        private const string WideWorldImportersConnectionString = "server=(local); database=WideWorldImporters; integrated security=yes; TrustServerCertificate=True;";
        private const string NorthwindConnectionString = "server=(local); database=Northwind; integrated security=yes; TrustServerCertificate=True;";

        private const string MsDescription = "MS_Description";

        [Fact]
        public async Task GetExtendedProperties()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("AdventureWorks2017", AdventureWorks2017ConnectionString, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = (SqlServerDatabase)await dbFactory.ImportAsync();
            var table = db.FindTable("Production.Product");
            var view = db.FindView("HumanResources.vEmployee");

            // Assert
            Assert.True(db.ExtendedProperties.Count > 0);
            Assert.True(table.ImportBag.ExtendedProperties.Count > 0);
            Assert.True(view.ImportBag.ExtendedProperties.Count > 0);
        }

        [Fact]
        public async Task AddExtendedPropertiesForDatabase()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", OnlineStoreConnectionString, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();

            dbFactory.DropExtendedPropertyIfExists(MsDescription);
            dbFactory.AddExtendedProperty(db, SqlServerToken.MS_DESCRIPTION, "Online store");

            // Assert
        }

        [Fact]
        public async Task AddExtendedPropertiesForTable()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", OnlineStoreConnectionString, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();
            var table = db.FindTable("Warehouse.Product");

            dbFactory.DropExtendedPropertyIfExists(table, SqlServerToken.MS_DESCRIPTION);
            dbFactory.AddExtendedProperty(table, SqlServerToken.MS_DESCRIPTION, "Products catalog");

            // Assert
        }

        [Fact]
        public async Task AddExtendedPropertiesForColumnFromTable()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", OnlineStoreConnectionString, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();
            var table = db.FindTable("Warehouse.Product");

            dbFactory.DropExtendedPropertyIfExists(table, table["ID"], SqlServerToken.MS_DESCRIPTION);
            dbFactory.AddExtendedProperty(table, table["ID"], SqlServerToken.MS_DESCRIPTION, "ID for product");

            // Assert
        }

        [Fact]
        public async Task AddExtendedPropertiesForView()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", OnlineStoreConnectionString, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();
            var view = db.FindView("Sales.OrderSummary");

            dbFactory.DropExtendedPropertyIfExists(view, SqlServerToken.MS_DESCRIPTION);
            dbFactory.AddExtendedProperty(view, SqlServerToken.MS_DESCRIPTION, "Summary for orders");

            // Assert
        }

        [Fact]
        public async Task AddExtendedPropertiesForColumnFromView()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", OnlineStoreConnectionString, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();
            var view = db.FindView("Sales.OrderSummary");

            dbFactory.DropExtendedPropertyIfExists(view, view["CustomerName"], SqlServerToken.MS_DESCRIPTION);
            dbFactory.AddExtendedProperty(view, view["CustomerName"], SqlServerToken.MS_DESCRIPTION, "Name for customer (CompanyName)");
        }

        [Fact]
        public async Task UpdateExtendedPropertiesForDatabase()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", OnlineStoreConnectionString, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();

            dbFactory.DropExtendedPropertyIfExists(MsDescription);
            dbFactory.AddExtendedProperty(db, SqlServerToken.MS_DESCRIPTION, "Online store");
            dbFactory.UpdateExtendedProperty(db, SqlServerToken.MS_DESCRIPTION, "Online store (Update)");

            // Assert
        }

        [Fact]
        public async Task UpdateExtendedPropertiesForTable()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", OnlineStoreConnectionString, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();
            var table = db.FindTable("Warehouse.Product");

            dbFactory.DropExtendedPropertyIfExists(table, SqlServerToken.MS_DESCRIPTION);
            dbFactory.AddExtendedProperty(table, SqlServerToken.MS_DESCRIPTION, "Products catalog");
            dbFactory.UpdateExtendedProperty(table, SqlServerToken.MS_DESCRIPTION, "Products catalog (Update)");

            // Assert
        }

        [Fact]
        public async Task UpdateExtendedPropertiesForColumnFromTable()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", OnlineStoreConnectionString, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();
            var table = db.FindTable("Warehouse.Product");

            dbFactory.DropExtendedPropertyIfExists(table, table["ID"], SqlServerToken.MS_DESCRIPTION);
            dbFactory.AddExtendedProperty(table, table["ID"], SqlServerToken.MS_DESCRIPTION, "ID for product");
            dbFactory.UpdateExtendedProperty(table, table["ID"], SqlServerToken.MS_DESCRIPTION, "ID for product (Update)");

            // Assert
        }

        [Fact]
        public async Task UpdateExtendedPropertiesForView()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", OnlineStoreConnectionString, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();
            var view = db.FindView("Sales.OrderSummary");

            dbFactory.DropExtendedPropertyIfExists(view, SqlServerToken.MS_DESCRIPTION);
            dbFactory.AddExtendedProperty(view, SqlServerToken.MS_DESCRIPTION, "Summary for orders");
            dbFactory.UpdateExtendedProperty(view, SqlServerToken.MS_DESCRIPTION, "Summary for orders (Update)");

            // Assert
        }

        [Fact]
        public async Task UpdateExtendedPropertiesForColumnFromView()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", OnlineStoreConnectionString, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();
            var view = db.FindView("Sales.OrderSummary");

            dbFactory.DropExtendedPropertyIfExists(view, view["CustomerName"], SqlServerToken.MS_DESCRIPTION);
            dbFactory.AddExtendedProperty(view, view["CustomerName"], SqlServerToken.MS_DESCRIPTION, "Name for customer (CompanyName)");
            dbFactory.UpdateExtendedProperty(view, view["CustomerName"], SqlServerToken.MS_DESCRIPTION, "Name for customer (CompanyName)");
        }

        [Fact]
        public async Task DropExtendedPropertiesForDatabase()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", OnlineStoreConnectionString, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();

            dbFactory.DropExtendedPropertyIfExists(MsDescription);

            // Assert
        }

        [Fact]
        public async Task DropExtendedPropertiesForTable()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", OnlineStoreConnectionString, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();
            var table = db.FindTable("Warehouse.Product");

            dbFactory.DropExtendedPropertyIfExists(table, SqlServerToken.MS_DESCRIPTION);

            // Assert
        }

        [Fact]
        public async Task DropExtendedPropertiesForColumnFromTable()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", OnlineStoreConnectionString, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();
            var table = db.FindTable("Warehouse.Product");

            dbFactory.DropExtendedPropertyIfExists(table, table["ID"], SqlServerToken.MS_DESCRIPTION);

            // Assert
        }

        [Fact]
        public async Task DropExtendedPropertiesForView()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", OnlineStoreConnectionString, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();
            var view = db.FindView("HumanResources.EmployeeInfo");

            dbFactory.DropExtendedPropertyIfExists(view, SqlServerToken.MS_DESCRIPTION);

            // Assert
        }

        [Fact]
        public async Task DropExtendedPropertiesForColumnFromView()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", OnlineStoreConnectionString, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();
            var view = db.FindView("HumanResources.EmployeeInfo");

            dbFactory.DropExtendedPropertyIfExists(view, view["EmployeeName"], SqlServerToken.MS_DESCRIPTION);
        }
    }
}

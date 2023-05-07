using System.Threading.Tasks;
using CatFactory.SqlServer.DatabaseObjectModel.Queries;
using CatFactory.SqlServer.Tests.Settings;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class DocumentationTests
    {
        [Fact]
        public async Task GetExtendedPropertiesForAdventureWorks2017()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("AdventureWorks2017", ConnectionStrings.AdventureWorks2017, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = (SqlServerDatabase)await dbFactory.ImportAsync();
            var table = db.FindTable("Production.Product");
            var view = db.FindView("HumanResources.vEmployee");

            db.SyncMsDescription();

            // Assert
            Assert.True(db.ImportBag.ExtendedProperties.Count > 0);
            Assert.False(string.IsNullOrEmpty(db.Description));

            Assert.True(table.ImportBag.ExtendedProperties.Count > 0);

            Assert.True(view.ImportBag.ExtendedProperties.Count > 0);
        }

        [Fact]
        public async Task AddExtendedPropertiesForOnlineStore()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", ConnectionStrings.OnlineStore, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();

            using var connection = dbFactory.GetConnection();

            await connection.DropExtendedPropertyIfExistsAsync(SqlServerToken.MS_DESCRIPTION);

            await connection.AddExtendedPropertyAsync(SqlServerToken.MS_DESCRIPTION, "Online Store Database (Sample Database for Entity Framework Core for the The Enterprise)");

            // Assert
        }

        [Fact]
        public async Task AddExtendedPropertiesForTable()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", ConnectionStrings.OnlineStore, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();
            var table = db.FindTable("Warehouse.Product");

            using var connection = dbFactory.GetConnection();

            await connection.DropExtendedPropertyIfExistsAsync(table, SqlServerToken.MS_DESCRIPTION);

            await connection.AddExtendedPropertyAsync(table, SqlServerToken.MS_DESCRIPTION, "Products catalog");

            // Assert
        }

        [Fact]
        public async Task AddExtendedPropertiesForColumnFromTable()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", ConnectionStrings.OnlineStore, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();
            var table = db.FindTable("Warehouse.Product");

            using var connection = dbFactory.GetConnection();

            await connection.DropExtendedPropertyIfExistsAsync(table, table["ID"], SqlServerToken.MS_DESCRIPTION);

            await connection.AddExtendedPropertyAsync(table, table["ID"], SqlServerToken.MS_DESCRIPTION, "ID for product");

            // Assert
        }

        [Fact]
        public async Task AddExtendedPropertiesForView()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", ConnectionStrings.OnlineStore, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();
            var view = db.FindView("Sales.OrderSummary");

            using var connection = dbFactory.GetConnection();

            await connection.DropExtendedPropertyIfExistsAsync(view, SqlServerToken.MS_DESCRIPTION);

            await connection.AddExtendedPropertyAsync(view, SqlServerToken.MS_DESCRIPTION, "Summary for orders");

            // Assert
        }

        [Fact]
        public async Task AddExtendedPropertiesForColumnFromView()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", ConnectionStrings.OnlineStore, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();
            var view = db.FindView("Sales.OrderSummary");

            using var connection = dbFactory.GetConnection();

            await connection.DropExtendedPropertyIfExistsAsync(view, view["EmployeeName"], SqlServerToken.MS_DESCRIPTION);

            await connection.AddExtendedPropertyAsync(view, view["EmployeeName"], SqlServerToken.MS_DESCRIPTION, "Name for employee (Full name)");
        }

        [Fact]
        public async Task UpdateExtendedPropertiesForDatabase()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", ConnectionStrings.OnlineStore, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();

            using var connection = dbFactory.GetConnection();

            await connection.DropExtendedPropertyIfExistsAsync(SqlServerToken.MS_DESCRIPTION);

            await connection.AddExtendedPropertyAsync(SqlServerToken.MS_DESCRIPTION, "Online store");

            await connection.UpdateExtendedPropertyAsync(SqlServerToken.MS_DESCRIPTION, "Online store (Update)");

            // Assert
        }

        [Fact]
        public async Task UpdateExtendedPropertiesForTable()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", ConnectionStrings.OnlineStore, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();
            var table = db.FindTable("Warehouse.Product");

            using var connection = dbFactory.GetConnection();

            await connection.DropExtendedPropertyIfExistsAsync(table, SqlServerToken.MS_DESCRIPTION);

            await connection.AddExtendedPropertyAsync(table, SqlServerToken.MS_DESCRIPTION, "Products catalog");

            await connection.UpdateExtendedPropertyAsync(table, SqlServerToken.MS_DESCRIPTION, "Products catalog (Update)");

            // Assert
        }

        [Fact]
        public async Task UpdateExtendedPropertiesForColumnFromTable()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", ConnectionStrings.OnlineStore, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();
            var table = db.FindTable("Warehouse.Product");

            using var connection = dbFactory.GetConnection();

            await connection.DropExtendedPropertyIfExistsAsync(table, table["ID"], SqlServerToken.MS_DESCRIPTION);

            await connection.AddExtendedPropertyAsync(table, table["ID"], SqlServerToken.MS_DESCRIPTION, "Id for product");

            await connection.UpdateExtendedPropertyAsync(table, table["ID"], SqlServerToken.MS_DESCRIPTION, "Id for product (Update)");

            // Assert
        }

        [Fact]
        public async Task UpdateExtendedPropertiesForView()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", ConnectionStrings.OnlineStore, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();
            var view = db.FindView("Sales.OrderSummary");

            using var connection = dbFactory.GetConnection();

            await connection.DropExtendedPropertyIfExistsAsync(view, SqlServerToken.MS_DESCRIPTION);

            await connection.AddExtendedPropertyAsync(view, SqlServerToken.MS_DESCRIPTION, "Summary for orders");

            await connection.UpdateExtendedPropertyAsync(view, SqlServerToken.MS_DESCRIPTION, "Summary for orders (Update)");

            // Assert
        }

        [Fact]
        public async Task UpdateExtendedPropertiesForColumnFromView()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", ConnectionStrings.OnlineStore, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();
            var view = db.FindView("Sales.OrderSummary");

            using var connection = dbFactory.GetConnection();

            await connection.DropExtendedPropertyIfExistsAsync(view, view["CustomerName"], SqlServerToken.MS_DESCRIPTION);

            await connection.AddExtendedPropertyAsync(view, view["CustomerName"], SqlServerToken.MS_DESCRIPTION, "Name for customer (CompanyName)");

            await connection.UpdateExtendedPropertyAsync(view, view["CustomerName"], SqlServerToken.MS_DESCRIPTION, "Name for customer (CompanyName)");

            // Assert
        }

        [Fact]
        public async Task DropExtendedPropertiesForDatabase()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", ConnectionStrings.OnlineStore, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();

            using var connection = dbFactory.GetConnection();

            await connection.DropExtendedPropertyIfExistsAsync(SqlServerToken.MS_DESCRIPTION);

            // Assert
        }

        [Fact]
        public async Task DropExtendedPropertiesForTable()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", ConnectionStrings.OnlineStore, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();
            var table = db.FindTable("Warehouse.Product");

            using var connection = dbFactory.GetConnection();

            await connection.DropExtendedPropertyIfExistsAsync(table, SqlServerToken.MS_DESCRIPTION);

            // Assert
        }

        [Fact]
        public async Task DropExtendedPropertiesForColumnFromTable()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", ConnectionStrings.OnlineStore, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();
            var table = db.FindTable("Warehouse.Product");

            using var connection = dbFactory.GetConnection();

            await connection.DropExtendedPropertyIfExistsAsync(table, table["ID"], SqlServerToken.MS_DESCRIPTION);

            // Assert
        }

        [Fact]
        public async Task DropExtendedPropertiesForView()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", ConnectionStrings.OnlineStore, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();
            var view = db.FindView("HumanResources.EmployeeInfo");

            using var connection = dbFactory.GetConnection();

            await connection.DropExtendedPropertyIfExistsAsync(view, SqlServerToken.MS_DESCRIPTION);

            // Assert
        }

        [Fact]
        public async Task DropExtendedPropertiesForColumnFromView()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", ConnectionStrings.OnlineStore, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();
            var view = db.FindView("HumanResources.EmployeeInfo");

            using var connection = dbFactory.GetConnection();

            await connection.DropExtendedPropertyIfExistsAsync(view, view["EmployeeName"], SqlServerToken.MS_DESCRIPTION);
        }
    }
}

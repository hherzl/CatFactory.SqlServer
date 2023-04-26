using System.Threading.Tasks;
using CatFactory.SqlServer.Features;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class DocumentationTests
    {
        private const string MsDescription = "MS_Description";

        [Fact]
        public async Task GetExtendedProperties()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = DatabaseImportSettings.Create("server=(local); database=AdventureWorks2017; integrated security=yes; TrustServerCertificate=True;", MsDescription)
            };

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
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = DatabaseImportSettings.Create("server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;", MsDescription)
            };

            // Act
            var db = await dbFactory.ImportAsync();

            dbFactory.DropExtendedPropertyIfExists(MsDescription);
            dbFactory.AddExtendedProperty(db, MsDescription, "Online store");

            // Assert
        }

        [Fact]
        public async Task AddExtendedPropertiesForTable()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = DatabaseImportSettings.Create("server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;", MsDescription)
            };

            // Act
            var db = await dbFactory.ImportAsync();
            var table = db.FindTable("Warehouse.Product");

            dbFactory.DropExtendedPropertyIfExists(table, MsDescription);
            dbFactory.AddExtendedProperty(table, MsDescription, "Products catalog");

            // Assert
        }

        [Fact]
        public async Task AddExtendedPropertiesForColumnFromTable()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = DatabaseImportSettings.Create("server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;", MsDescription)
            };

            // Act
            var db = await dbFactory.ImportAsync();
            var table = db.FindTable("Warehouse.Product");

            dbFactory.DropExtendedPropertyIfExists(table, table["ID"], MsDescription);
            dbFactory.AddExtendedProperty(table, table["ID"], MsDescription, "ID for product");

            // Assert
        }

        [Fact]
        public async Task AddExtendedPropertiesForView()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = DatabaseImportSettings.Create("server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;", MsDescription)
            };

            // Act
            var db = await dbFactory.ImportAsync();
            var view = db.FindView("Sales.OrderSummary");

            dbFactory.DropExtendedPropertyIfExists(view, MsDescription);
            dbFactory.AddExtendedProperty(view, MsDescription, "Summary for orders");

            // Assert
        }

        [Fact]
        public async Task AddExtendedPropertiesForColumnFromView()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = DatabaseImportSettings.Create("server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;", MsDescription)
            };

            // Act
            var db = await dbFactory.ImportAsync();
            var view = db.FindView("Sales.OrderSummary");

            dbFactory.DropExtendedPropertyIfExists(view, view["CustomerName"], MsDescription);
            dbFactory.AddExtendedProperty(view, view["CustomerName"], MsDescription, "Name for customer (CompanyName)");
        }

        [Fact]
        public async Task UpdateExtendedPropertiesForDatabase()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = DatabaseImportSettings.Create("server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;", MsDescription)
            };

            // Act
            var db = await dbFactory.ImportAsync();

            dbFactory.DropExtendedPropertyIfExists(MsDescription);
            dbFactory.AddExtendedProperty(db, MsDescription, "Online store");
            dbFactory.UpdateExtendedProperty(db, MsDescription, "Online store (Update)");

            // Assert
        }

        [Fact]
        public async Task UpdateExtendedPropertiesForTable()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = DatabaseImportSettings.Create("server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;", MsDescription)
            };

            // Act
            var db = await dbFactory.ImportAsync();
            var table = db.FindTable("Warehouse.Product");

            dbFactory.DropExtendedPropertyIfExists(table, MsDescription);
            dbFactory.AddExtendedProperty(table, MsDescription, "Products catalog");
            dbFactory.UpdateExtendedProperty(table, MsDescription, "Products catalog (Update)");

            // Assert
        }

        [Fact]
        public async Task UpdateExtendedPropertiesForColumnFromTable()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = DatabaseImportSettings.Create("server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;", MsDescription)
            };

            // Act
            var db = await dbFactory.ImportAsync();
            var table = db.FindTable("Warehouse.Product");

            dbFactory.DropExtendedPropertyIfExists(table, table["ID"], MsDescription);
            dbFactory.AddExtendedProperty(table, table["ID"], MsDescription, "ID for product");
            dbFactory.UpdateExtendedProperty(table, table["ID"], MsDescription, "ID for product (Update)");

            // Assert
        }

        [Fact]
        public async Task UpdateExtendedPropertiesForView()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = DatabaseImportSettings.Create("server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;", MsDescription)
            };

            // Act
            var db = await dbFactory.ImportAsync();
            var view = db.FindView("Sales.OrderSummary");

            dbFactory.DropExtendedPropertyIfExists(view, MsDescription);
            dbFactory.AddExtendedProperty(view, MsDescription, "Summary for orders");
            dbFactory.UpdateExtendedProperty(view, MsDescription, "Summary for orders (Update)");

            // Assert
        }

        [Fact]
        public async Task UpdateExtendedPropertiesForColumnFromView()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = DatabaseImportSettings.Create("server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;", MsDescription)
            };

            // Act
            var db = await dbFactory.ImportAsync();
            var view = db.FindView("Sales.OrderSummary");

            dbFactory.DropExtendedPropertyIfExists(view, view["CustomerName"], MsDescription);
            dbFactory.AddExtendedProperty(view, view["CustomerName"], MsDescription, "Name for customer (CompanyName)");
            dbFactory.UpdateExtendedProperty(view, view["CustomerName"], MsDescription, "Name for customer (CompanyName)");
        }

        [Fact]
        public async Task DropExtendedPropertiesForDatabase()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = DatabaseImportSettings.Create("server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;", MsDescription)
            };

            // Act
            var db = await dbFactory.ImportAsync();

            dbFactory.DropExtendedPropertyIfExists(MsDescription);

            // Assert
        }

        [Fact]
        public async Task DropExtendedPropertiesForTable()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = DatabaseImportSettings.Create("server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;", MsDescription)
            };

            // Act
            var db = await dbFactory.ImportAsync();
            var table = db.FindTable("Warehouse.Product");

            dbFactory.DropExtendedPropertyIfExists(table, MsDescription);

            // Assert
        }

        [Fact]
        public async Task DropExtendedPropertiesForColumnFromTable()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = DatabaseImportSettings.Create("server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;", MsDescription)
            };

            // Act
            var db = await dbFactory.ImportAsync();
            var table = db.FindTable("Warehouse.Product");

            dbFactory.DropExtendedPropertyIfExists(table, table["ID"], MsDescription);

            // Assert
        }

        [Fact]
        public async Task DropExtendedPropertiesForView()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = DatabaseImportSettings.Create("server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;", MsDescription)
            };

            // Act
            var db = await dbFactory.ImportAsync();
            var view = db.FindView("HumanResources.EmployeeInfo");

            dbFactory.DropExtendedPropertyIfExists(view, MsDescription);

            // Assert
        }

        [Fact]
        public async Task DropExtendedPropertiesForColumnFromView()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = DatabaseImportSettings.Create("server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;", MsDescription)
            };

            // Act
            var db = await dbFactory.ImportAsync();
            var view = db.FindView("HumanResources.EmployeeInfo");

            dbFactory.DropExtendedPropertyIfExists(view, view["EmployeeName"], MsDescription);
        }
    }
}

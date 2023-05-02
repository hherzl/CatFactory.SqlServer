﻿using System.Threading.Tasks;
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

            await dbFactory.DropExtendedPropertyIfExists(MsDescription);
            
            await dbFactory.AddExtendedProperty(db, SqlServerToken.MS_DESCRIPTION, "Online Store Database (Sample Database for Entity Framework Core for the The Enterprise)");

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

            await dbFactory.DropExtendedPropertyIfExists(table, SqlServerToken.MS_DESCRIPTION);

            await dbFactory.AddExtendedProperty(table, SqlServerToken.MS_DESCRIPTION, "Products catalog");

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

            await dbFactory.DropExtendedPropertyIfExists(table, table["ID"], SqlServerToken.MS_DESCRIPTION);

            await dbFactory.AddExtendedProperty(table, table["ID"], SqlServerToken.MS_DESCRIPTION, "ID for product");

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

            await dbFactory.DropExtendedPropertyIfExists(view, SqlServerToken.MS_DESCRIPTION);

            await dbFactory.AddExtendedProperty(view, SqlServerToken.MS_DESCRIPTION, "Summary for orders");

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

            await dbFactory.DropExtendedPropertyIfExists(view, view["EmployeeName"], SqlServerToken.MS_DESCRIPTION);

            await dbFactory.AddExtendedProperty(view, view["EmployeeName"], SqlServerToken.MS_DESCRIPTION, "Name for employee (Full name)");
        }

        [Fact]
        public async Task UpdateExtendedPropertiesForDatabase()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", OnlineStoreConnectionString, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();

            await dbFactory.DropExtendedPropertyIfExists(MsDescription);

            await dbFactory.AddExtendedProperty(db, SqlServerToken.MS_DESCRIPTION, "Online store");

            await dbFactory.UpdateExtendedProperty(db, SqlServerToken.MS_DESCRIPTION, "Online store (Update)");

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

            await dbFactory.DropExtendedPropertyIfExists(table, SqlServerToken.MS_DESCRIPTION);

            await dbFactory.AddExtendedProperty(table, SqlServerToken.MS_DESCRIPTION, "Products catalog");
            
            await dbFactory.UpdateExtendedProperty(table, SqlServerToken.MS_DESCRIPTION, "Products catalog (Update)");

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

            await dbFactory.DropExtendedPropertyIfExists(table, table["ID"], SqlServerToken.MS_DESCRIPTION);

            await dbFactory.AddExtendedProperty(table, table["ID"], SqlServerToken.MS_DESCRIPTION, "ID for product");
            
            await dbFactory.UpdateExtendedProperty(table, table["ID"], SqlServerToken.MS_DESCRIPTION, "ID for product (Update)");

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

            await dbFactory.DropExtendedPropertyIfExists(view, SqlServerToken.MS_DESCRIPTION);

            await dbFactory.AddExtendedProperty(view, SqlServerToken.MS_DESCRIPTION, "Summary for orders");
            
            await dbFactory.UpdateExtendedProperty(view, SqlServerToken.MS_DESCRIPTION, "Summary for orders (Update)");

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

            await dbFactory.DropExtendedPropertyIfExists(view, view["CustomerName"], SqlServerToken.MS_DESCRIPTION);

            await dbFactory.AddExtendedProperty(view, view["CustomerName"], SqlServerToken.MS_DESCRIPTION, "Name for customer (CompanyName)");
            
            await dbFactory.UpdateExtendedProperty(view, view["CustomerName"], SqlServerToken.MS_DESCRIPTION, "Name for customer (CompanyName)");
        }

        [Fact]
        public async Task DropExtendedPropertiesForDatabase()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory(DatabaseImportSettings.Create("OnlineStore", OnlineStoreConnectionString, SqlServerToken.MS_DESCRIPTION));

            // Act
            var db = await dbFactory.ImportAsync();

            await dbFactory.DropExtendedPropertyIfExists(MsDescription);

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

            await dbFactory.DropExtendedPropertyIfExists(table, SqlServerToken.MS_DESCRIPTION);

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

            await dbFactory.DropExtendedPropertyIfExists(table, table["ID"], SqlServerToken.MS_DESCRIPTION);

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

            await dbFactory.DropExtendedPropertyIfExists(view, SqlServerToken.MS_DESCRIPTION);

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

            await dbFactory.DropExtendedPropertyIfExists(view, view["EmployeeName"], SqlServerToken.MS_DESCRIPTION);
        }
    }
}

using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class DocumentationTests
    {
        [Fact]
        public void TestGetExtendedProperties()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=AdventureWorks2017;integrated security=yes;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var database = databaseFactory.Import();
            var table = database.FindTable("Production.Product");
            var view = database.FindView("HumanResources.vEmployee");

            // Assert
            Assert.True(database.ExtendedProperties.Count > 0);
            Assert.True(table.ExtendedProperties.Count > 0);
            Assert.True(view.ExtendedProperties.Count > 0);
        }

        [Fact]
        public void TestAddExtendedPropertiesForDatabase()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=Store;integrated security=yes;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var database = databaseFactory.Import();

            databaseFactory.DropExtendedProperty(database, "MS_Description");
            databaseFactory.AddExtendedProperty(database, "MS_Description", "Online store");

            // Assert
        }

        [Fact]
        public void TestAddExtendedPropertiesForTable()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=Store;integrated security=yes;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var database = databaseFactory.Import();
            var table = database.FindTable("Production.Product");

            databaseFactory.DropExtendedProperty(table, "MS_Description");
            databaseFactory.AddExtendedProperty(table, "MS_Description", "Products catalog");

            // Assert
        }

        [Fact]
        public void TestAddExtendedPropertiesForColumnFromTable()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=Store;integrated security=yes;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var database = databaseFactory.Import();
            var table = database.FindTable("Production.Product");

            databaseFactory.DropExtendedProperty(table, table.GetColumn("ProductID"), "MS_Description");
            databaseFactory.AddExtendedProperty(table, table.GetColumn("ProductID"), "MS_Description", "ID for product");

            // Assert
        }

        [Fact]
        public void TestAddExtendedPropertiesForView()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=Store;integrated security=yes;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var database = databaseFactory.Import();
            var view = database.FindView("Sales.OrderSummary");

            databaseFactory.DropExtendedProperty(view, "MS_Description");
            databaseFactory.AddExtendedProperty(view, "MS_Description", "Summary for orders");

            // Assert
        }

        [Fact]
        public void TestAddExtendedPropertiesForColumnFromView()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=Store;integrated security=yes;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var database = databaseFactory.Import();
            var view = database.FindView("Sales.OrderSummary");

            databaseFactory.DropExtendedProperty(view, view.GetColumn("CustomerName"), "MS_Description");
            databaseFactory.AddExtendedProperty(view, view.GetColumn("CustomerName"), "MS_Description", "Name for customer (CompanyName)");
        }
    }
}

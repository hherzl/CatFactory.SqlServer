using CatFactory.SqlServer.Features;
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
                    ConnectionString = "server=(local);database=OnlineStore;integrated security=yes;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var database = databaseFactory.Import();

            databaseFactory.DropExtendedPropertyIfExists("MS_Description");
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
                    ConnectionString = "server=(local);database=OnlineStore;integrated security=yes;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var database = databaseFactory.Import();
            var table = database.FindTable("Warehouse.Product");

            databaseFactory.DropExtendedPropertyIfExists(table, "MS_Description");
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
                    ConnectionString = "server=(local);database=OnlineStore;integrated security=yes;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var database = databaseFactory.Import();
            var table = database.FindTable("Warehouse.Product");

            databaseFactory.DropExtendedPropertyIfExists(table, table["ID"], "MS_Description");
            databaseFactory.AddExtendedProperty(table, table["ID"], "MS_Description", "ID for product");

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
                    ConnectionString = "server=(local);database=OnlineStore;integrated security=yes;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var database = databaseFactory.Import();
            var view = database.FindView("Sales.OrderSummary");

            databaseFactory.DropExtendedPropertyIfExists(view, "MS_Description");
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
                    ConnectionString = "server=(local);database=OnlineStore;integrated security=yes;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var database = databaseFactory.Import();
            var view = database.FindView("Sales.OrderSummary");

            databaseFactory.DropExtendedPropertyIfExists(view, view["CustomerName"], "MS_Description");
            databaseFactory.AddExtendedProperty(view, view["CustomerName"], "MS_Description", "Name for customer (CompanyName)");
        }

        [Fact]
        public void TestUpdateExtendedPropertiesForDatabase()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=OnlineStore;integrated security=yes;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var database = databaseFactory.Import();

            databaseFactory.DropExtendedPropertyIfExists("MS_Description");
            databaseFactory.AddExtendedProperty(database, "MS_Description", "Online store");
            databaseFactory.UpdateExtendedProperty(database, "MS_Description", "Online store (Update)");

            // Assert
        }

        [Fact]
        public void TestUpdateExtendedPropertiesForTable()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=OnlineStore;integrated security=yes;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var database = databaseFactory.Import();
            var table = database.FindTable("Warehouse.Product");

            databaseFactory.DropExtendedPropertyIfExists(table, "MS_Description");
            databaseFactory.AddExtendedProperty(table, "MS_Description", "Products catalog");
            databaseFactory.UpdateExtendedProperty(table, "MS_Description", "Products catalog (Update)");

            // Assert
        }

        [Fact]
        public void TestUpdateExtendedPropertiesForColumnFromTable()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=OnlineStore;integrated security=yes;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var database = databaseFactory.Import();
            var table = database.FindTable("Warehouse.Product");

            databaseFactory.DropExtendedPropertyIfExists(table, table["ID"], "MS_Description");
            databaseFactory.AddExtendedProperty(table, table["ID"], "MS_Description", "ID for product");
            databaseFactory.UpdateExtendedProperty(table, table["ID"], "MS_Description", "ID for product (Update)");

            // Assert
        }

        [Fact]
        public void TestUpdateExtendedPropertiesForView()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=OnlineStore;integrated security=yes;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var database = databaseFactory.Import();
            var view = database.FindView("Sales.OrderSummary");

            databaseFactory.DropExtendedPropertyIfExists(view, "MS_Description");
            databaseFactory.AddExtendedProperty(view, "MS_Description", "Summary for orders");
            databaseFactory.UpdateExtendedProperty(view, "MS_Description", "Summary for orders (Update)");

            // Assert
        }

        [Fact]
        public void TestUpdateExtendedPropertiesForColumnFromView()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=OnlineStore;integrated security=yes;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var database = databaseFactory.Import();
            var view = database.FindView("Sales.OrderSummary");

            databaseFactory.DropExtendedPropertyIfExists(view, view["CustomerName"], "MS_Description");
            databaseFactory.AddExtendedProperty(view, view["CustomerName"], "MS_Description", "Name for customer (CompanyName)");
            databaseFactory.UpdateExtendedProperty(view, view["CustomerName"], "MS_Description", "Name for customer (CompanyName)");
        }

        [Fact]
        public void TestDropExtendedPropertiesForDatabase()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=OnlineStore;integrated security=yes;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var database = databaseFactory.Import();

            databaseFactory.DropExtendedPropertyIfExists("MS_Description");

            // Assert
        }

        [Fact]
        public void TestDropExtendedPropertiesForTable()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=OnlineStore;integrated security=yes;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var database = databaseFactory.Import();
            var table = database.FindTable("Warehouse.Product");

            databaseFactory.DropExtendedPropertyIfExists(table, "MS_Description");

            // Assert
        }

        [Fact]
        public void TestDropExtendedPropertiesForColumnFromTable()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=OnlineStore;integrated security=yes;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var database = databaseFactory.Import();
            var table = database.FindTable("Warehouse.Product");

            databaseFactory.DropExtendedPropertyIfExists(table, table["ID"], "MS_Description");

            // Assert
        }

        [Fact]
        public void TestDropExtendedPropertiesForView()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=OnlineStore;integrated security=yes;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var database = databaseFactory.Import();
            var view = database.FindView("HumanResources.EmployeeInfo");

            databaseFactory.DropExtendedPropertyIfExists(view, "MS_Description");

            // Assert
        }

        [Fact]
        public void TestDropExtendedPropertiesForColumnFromView()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=OnlineStore;integrated security=yes;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var database = databaseFactory.Import();
            var view = database.FindView("HumanResources.EmployeeInfo");

            databaseFactory.DropExtendedPropertyIfExists(view, view["EmployeeName"], "MS_Description");
        }
    }
}

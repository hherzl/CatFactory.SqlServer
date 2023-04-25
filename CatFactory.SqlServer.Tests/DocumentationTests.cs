using System.Threading.Tasks;
using CatFactory.SqlServer.Features;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class DocumentationTests
    {
        [Fact]
        public void GetExtendedProperties()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local); database=AdventureWorks2017; integrated security=yes; TrustServerCertificate=True;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var db = (SqlServerDatabase)dbFactory.Import();
            var table = db.FindTable("Production.Product");
            var view = db.FindView("HumanResources.vEmployee");

            // Assert
            Assert.True(db.ExtendedProperties.Count > 0);
            Assert.True(table.ImportBag.ExtendedProperties.Count > 0);
            Assert.True(view.ImportBag.ExtendedProperties.Count > 0);
        }

        [Fact]
        public void AddExtendedPropertiesForDatabase()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var db = dbFactory.Import();

            dbFactory.DropExtendedPropertyIfExists("MS_Description");
            dbFactory.AddExtendedProperty(db, "MS_Description", "Online store");

            // Assert
        }

        [Fact]
        public void AddExtendedPropertiesForTable()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var db = dbFactory.Import();
            var table = db.FindTable("Warehouse.Product");

            dbFactory.DropExtendedPropertyIfExists(table, "MS_Description");
            dbFactory.AddExtendedProperty(table, "MS_Description", "Products catalog");

            // Assert
        }

        [Fact]
        public void AddExtendedPropertiesForColumnFromTable()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var db = dbFactory.Import();
            var table = db.FindTable("Warehouse.Product");

            dbFactory.DropExtendedPropertyIfExists(table, table["ID"], "MS_Description");
            dbFactory.AddExtendedProperty(table, table["ID"], "MS_Description", "ID for product");

            // Assert
        }

        [Fact]
        public void AddExtendedPropertiesForView()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var db = dbFactory.Import();
            var view = db.FindView("Sales.OrderSummary");

            dbFactory.DropExtendedPropertyIfExists(view, "MS_Description");
            dbFactory.AddExtendedProperty(view, "MS_Description", "Summary for orders");

            // Assert
        }

        [Fact]
        public async Task AddExtendedPropertiesForColumnFromView()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var db = await dbFactory.ImportAsync();
            var view = db.FindView("Sales.OrderSummary");

            dbFactory.DropExtendedPropertyIfExists(view, view["CustomerName"], "MS_Description");
            dbFactory.AddExtendedProperty(view, view["CustomerName"], "MS_Description", "Name for customer (CompanyName)");
        }

        [Fact]
        public void UpdateExtendedPropertiesForDatabase()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var db = dbFactory.Import();

            dbFactory.DropExtendedPropertyIfExists("MS_Description");
            dbFactory.AddExtendedProperty(db, "MS_Description", "Online store");
            dbFactory.UpdateExtendedProperty(db, "MS_Description", "Online store (Update)");

            // Assert
        }

        [Fact]
        public void UpdateExtendedPropertiesForTable()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var db = dbFactory.Import();
            var table = db.FindTable("Warehouse.Product");

            dbFactory.DropExtendedPropertyIfExists(table, "MS_Description");
            dbFactory.AddExtendedProperty(table, "MS_Description", "Products catalog");
            dbFactory.UpdateExtendedProperty(table, "MS_Description", "Products catalog (Update)");

            // Assert
        }

        [Fact]
        public void UpdateExtendedPropertiesForColumnFromTable()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var db = dbFactory.Import();
            var table = db.FindTable("Warehouse.Product");

            dbFactory.DropExtendedPropertyIfExists(table, table["ID"], "MS_Description");
            dbFactory.AddExtendedProperty(table, table["ID"], "MS_Description", "ID for product");
            dbFactory.UpdateExtendedProperty(table, table["ID"], "MS_Description", "ID for product (Update)");

            // Assert
        }

        [Fact]
        public void UpdateExtendedPropertiesForView()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var db = dbFactory.Import();
            var view = db.FindView("Sales.OrderSummary");

            dbFactory.DropExtendedPropertyIfExists(view, "MS_Description");
            dbFactory.AddExtendedProperty(view, "MS_Description", "Summary for orders");
            dbFactory.UpdateExtendedProperty(view, "MS_Description", "Summary for orders (Update)");

            // Assert
        }

        [Fact]
        public void UpdateExtendedPropertiesForColumnFromView()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var db = dbFactory.Import();
            var view = db.FindView("Sales.OrderSummary");

            dbFactory.DropExtendedPropertyIfExists(view, view["CustomerName"], "MS_Description");
            dbFactory.AddExtendedProperty(view, view["CustomerName"], "MS_Description", "Name for customer (CompanyName)");
            dbFactory.UpdateExtendedProperty(view, view["CustomerName"], "MS_Description", "Name for customer (CompanyName)");
        }

        [Fact]
        public void DropExtendedPropertiesForDatabase()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var db = dbFactory.Import();

            dbFactory.DropExtendedPropertyIfExists("MS_Description");

            // Assert
        }

        [Fact]
        public void DropExtendedPropertiesForTable()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var db = dbFactory.Import();
            var table = db.FindTable("Warehouse.Product");

            dbFactory.DropExtendedPropertyIfExists(table, "MS_Description");

            // Assert
        }

        [Fact]
        public void DropExtendedPropertiesForColumnFromTable()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var db = dbFactory.Import();
            var table = db.FindTable("Warehouse.Product");

            dbFactory.DropExtendedPropertyIfExists(table, table["ID"], "MS_Description");

            // Assert
        }

        [Fact]
        public void DropExtendedPropertiesForView()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var db = dbFactory.Import();
            var view = db.FindView("HumanResources.EmployeeInfo");

            dbFactory.DropExtendedPropertyIfExists(view, "MS_Description");

            // Assert
        }

        [Fact]
        public void DropExtendedPropertiesForColumnFromView()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    }
                }
            };

            // Act
            var db = dbFactory.Import();
            var view = db.FindView("HumanResources.EmployeeInfo");

            dbFactory.DropExtendedPropertyIfExists(view, view["EmployeeName"], "MS_Description");
        }
    }
}

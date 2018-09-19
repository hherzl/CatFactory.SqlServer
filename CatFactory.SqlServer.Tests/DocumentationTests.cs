using System.Linq;
using CatFactory.Mapping;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class DocumentationTests
    {
        [Fact]
        public void TestGetTableAndViewMsDescriptionTest()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=AdventureWorks2017;integrated security=yes;MultipleActiveResultSets=true;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    },
                    ExclusionTypes =
                    {
                        "geography"
                    }
                }
            };

            // Act
            var database = databaseFactory.Import();
            var table = database.FindTable("Production.Product") as Table;
            var view = database.Views.First(item => item.FullName == "HumanResources.vEmployee");

            // Assert
            Assert.True(database.ExtendedProperties.Count > 0);

            Assert.True(table.ExtendedProperties.Count > 0);
            Assert.True(table.Columns.First().ExtendedProperties.Count > 0);
            Assert.True(view.ExtendedProperties.Count > 0);

            Assert.True(table.Description != null);
            Assert.True(table.Columns.First().Description != null);
            Assert.True(view.Description != null);
        }

        [Fact]
        public void TestAddTableMsDescriptionTest()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=Northwind;integrated security=yes;MultipleActiveResultSets=true;"
                }
            };

            // Act
            var database = databaseFactory.Import();

            var table = database.FindTable("dbo.Products");

            databaseFactory.AddOrUpdateExtendedProperty(table, "MS_Description", "Test description");

            var column = table.Columns.FirstOrDefault();

            databaseFactory.AddOrUpdateExtendedProperty(table, column, "MS_Description", "Primary key");

            databaseFactory.DropExtendedProperty(table, "MS_Description");
            databaseFactory.DropExtendedProperty(table, column, "MS_Description");

            // Assert
            Assert.True(table.Description == "Test description");
            Assert.True(column.Description == "Primary key");
        }

        [Fact]
        public void TestUpdateTableMsDescriptionTest()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=Northwind;integrated security=yes;MultipleActiveResultSets=true;"
                }
            };

            // Act
            var database = databaseFactory.Import();

            var table = database.FindTable("dbo.Products");

            databaseFactory.AddOrUpdateExtendedProperty(table, "MS_Description", "Test update description");

            var column = table.Columns.FirstOrDefault();

            databaseFactory.AddOrUpdateExtendedProperty(table, column, "MS_Description", "PK (updated)");

            // Assert
            Assert.True(table.Description == "Test update description");
            Assert.True(column.Description == "PK (updated)");
        }
    }
}

using System.Collections.Generic;
using System.Linq;
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
                ConnectionString = "server=(local);database=AdventureWorks2017;integrated security=yes;MultipleActiveResultSets=true;",
                ImportSettings = new DatabaseImportSettings
                {
                    ExtendedProperties = { "MS_Description" },
                    ExclusionTypes = new List<string> { "geography" }
                }
            };

            // Act
            var database = databaseFactory.Import();
            var table = database.FindTable("Production.Product");
            var view = database.Views.First(item => item.FullName == "HumanResources.vEmployee");

            // Assert
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
                ConnectionString = "server=(local);database=Northwind;integrated security=yes;MultipleActiveResultSets=true;"
            };

            // Act
            var database = databaseFactory.Import();

            var table = database.FindTable("dbo.Products");

            databaseFactory.DropExtendedProperty(table, "MS_Description");

            databaseFactory.AddExtendedProperty(table, "MS_Description", "Test description");

            var column = table.Columns.FirstOrDefault();

            databaseFactory.DropExtendedProperty(table, column, "MS_Description");

            databaseFactory.AddExtendedProperty(table, column, "MS_Description", "Primary key");

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
                ConnectionString = "server=(local);database=Northwind;integrated security=yes;MultipleActiveResultSets=true;"
            };

            // Act
            var database = databaseFactory.Import();

            var table = database.FindTable("dbo.Products");

            databaseFactory.UpdateExtendedProperty(table, "MS_Description", "Test update description");

            var column = table.Columns.FirstOrDefault();

            databaseFactory.UpdateExtendedProperty(table, column, "MS_Description", "PK (updated)");

            // Assert
            Assert.True(table.Description == "Test update description");
            Assert.True(column.Description == "PK (updated)");
        }
    }
}

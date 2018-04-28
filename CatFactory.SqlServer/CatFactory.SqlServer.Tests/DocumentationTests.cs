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
                    ImportMSDescription = true,
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

            // todo: Fix this assert
            // Assert.True(view.Columns.First().Description != null);
        }

        [Fact]
        public void TestAddTableMsDescriptionTest()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                ConnectionString = "server=(local);database=Northwind;integrated security=yes;MultipleActiveResultSets=true;"
            };

            databaseFactory.ImportSettings.ImportMSDescription = true;

            // Act
            var database = databaseFactory.Import();

            var table = database.FindTable("dbo.Products");

            databaseFactory.DropMsDescription(table);

            databaseFactory.AddMsDescription(table, "Test description");

            var column = table.Columns.FirstOrDefault();

            databaseFactory.DropMsDescription(table, column);

            databaseFactory.AddMsDescription(table, column, "Primary key");

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

            databaseFactory.ImportSettings.ImportMSDescription = true;

            // Act
            var database = databaseFactory.Import();

            var table = database.FindTable("dbo.Products");

            databaseFactory.UpdateMsDescription(table, "Test update description");

            var column = table.Columns.FirstOrDefault();

            databaseFactory.UpdateMsDescription(table, column, "PK (updated)");

            // Assert
            Assert.True(table.Description == "Test update description");
            Assert.True(column.Description == "PK (updated)");
        }
    }
}

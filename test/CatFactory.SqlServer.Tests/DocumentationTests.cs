using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class DocumentationTests
    {
        [Fact]
        public void TestGetMsDescriptionTest()
        {
            // Arrange
            var factory = new SqlServerDatabaseFactory
            {
                ConnectionString = "server=(local);database=AdventureWorks2012;integrated security=yes;",
                ImportMSDescription = true,
                ExclusionTypes = new List<String>()
                {
                    "geography"
                }
            };

            // Act
            var database = factory.Import();

            var table = database.FindTableBySchemaAndName("Production.Product");

            var view = database.Views.First(item => item.FullName == "HumanResources.vEmployee");

            // Assert
            Assert.True(table.Description != null);
            Assert.True(table.Columns.First().Description != null);

            Assert.True(view.Description != null);
            Assert.True(view.Columns.First().Description != null);
        }

        [Fact]
        public void TestAddMsDescriptionTest()
        {
            // Arrange
            var factory = new SqlServerDatabaseFactory
            {
                ConnectionString = "server=(local);database=Northwind;integrated security=yes;",
                ImportMSDescription = true
            };

            // Act
            var database = factory.Import();

            var table = database.FindTableBySchemaAndName("dbo.Products");

            factory.DropMsDescription(table);

            factory.AddMsDescription(table, "Test description");

            var column = table.Columns.FirstOrDefault();

            factory.DropMsDescription(table, column);

            factory.AddMsDescription(table, column, "Primary key");

            // Assert
            Assert.True(table.Description == "Test description");
            Assert.True(column.Description == "Primary key");
        }

        [Fact]
        public void TestUpdateMsDescriptionTest()
        {
            // Arrange
            var factory = new SqlServerDatabaseFactory
            {
                ConnectionString = "server=(local);database=Northwind;integrated security=yes;",
                ImportMSDescription = true
            };

            // Act
            var database = factory.Import();

            var table = database.FindTableBySchemaAndName("dbo.Products");

            factory.UpdateMsDescription(table, "Test update description");

            var column = table.Columns.FirstOrDefault();

            factory.UpdateMsDescription(table, column, "PK (updated)");

            // Assert
            Assert.True(table.Description == "Test update description");
            Assert.True(column.Description == "PK (updated)");
        }
    }
}

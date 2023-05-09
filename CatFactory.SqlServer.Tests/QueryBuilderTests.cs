using System;
using CatFactory.SqlServer.Features;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class QueryBuilderTests
    {
        [Fact]
        public void SelectAllTest()
        {
            // Arrange
            var model = new
            {
                Id = 0,
                Name = "",
                Email = "",
                Phone = ""
            };

            // Act
            var queryDefinition = model.SelectAll();

            // Assert
            Assert.True(queryDefinition.Columns.Count == 4);
        }

        [Fact]
        public void SelectByKeyTest()
        {
            // Arrange
            var model = new
            {
                Id = 0,
                Name = "",
                Email = "",
                Phone = ""
            };

            // Act
            var queryDefinition = model.SelectByKey(e => e.Id, model.Id);

            // Assert
            Assert.True(queryDefinition.Columns.Count == 4);
            Assert.True(queryDefinition.Conditions.Count == 1);
        }

        [Fact]
        public void InsertTest()
        {
            // Arrange
            var model = new
            {
                Id = Guid.Empty,
                Name = "",
                Email = "",
                Phone = ""
            };

            // Act
            var queryDefinition = model.Insert();

            // Assert
            Assert.True(queryDefinition.Columns.Count == 4);
            Assert.True(queryDefinition.Conditions.Count == 0);
        }

        [Fact]
        public void InsertWithIdentityTest()
        {
            // Arrange
            var model = new
            {
                Id = 0,
                Name = "",
                Email = "",
                Phone = ""
            };

            // Act
            var queryDefinition = model.Insert(e => e.Id);

            // Assert
            Assert.True(queryDefinition.Columns.Count == 4);
            Assert.True(queryDefinition.Conditions.Count == 0);
        }

        [Fact]
        public void UpdateByKeyTest()
        {
            // Arrange
            var model = new
            {
                Id = 0,
                Name = "",
                Email = "",
                Phone = ""
            };

            // Act
            var queryDefinition = model.Update(e => e.Id, model.Id);

            // Assert
            Assert.True(queryDefinition.Columns.Count == 4);
            Assert.True(queryDefinition.Conditions.Count == 1);
        }

        [Fact]
        public void DeleteByKeyTest()
        {
            // Arrange
            var model = new
            {
                Id = 0,
                Name = "",
                Email = "",
                Phone = ""
            };

            // Act
            var queryDefinition = model.Delete(e => e.Id, model.Id);

            // Assert
            Assert.True(queryDefinition.Columns.Count == 0);
            Assert.True(queryDefinition.Conditions.Count == 1);
        }
    }
}

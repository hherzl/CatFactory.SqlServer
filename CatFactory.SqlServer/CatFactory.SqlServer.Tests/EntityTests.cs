using CatFactory.Mapping;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class EntityTests
    {
        [Fact]
        public void DefineCategoryEntity()
        {
            // Arrange
            var database = new Database
            {
                Name = "Store"
            };

            // Act
            var entity = database
                .DefineEntity("dbo", "Category", new { ID = 0, Name = "" })
                .SetColumnForProperty(e => e.Name, new Column { Type = "nvarchar", Length = 50 })
                .SetIdentity(e => e.ID)
                .SetPrimaryKey(e => e.ID);

            // Assert
            Assert.True(entity.Table.Columns.Count == 2);
            Assert.False(entity.Table.PrimaryKey == null);
            Assert.False(entity.Table.Identity == null);
        }
    }
}

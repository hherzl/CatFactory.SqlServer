using CatFactory.Mapping;
using CatFactory.SqlServer.ObjectRelationalMapping;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class EntityTests
    {
        [Fact]
        public void TestDefinitionForBloggingEntities()
        {
            // Arrange
            var database = new Database
            {
                Name = "Blogging"
            };

            // Act
            var blog = database
                .DefineEntity("dbo", "Blog", new { BlogId = 0, Url = "" })
                .SetColumnForProperty(e => e.Url, new Column { Type = "nvarchar", Length = 128 })
                .SetIdentity(e => e.BlogId)
                .SetPrimaryKey(e => e.BlogId);

            var post = database
                .DefineEntity("dbo", "Post", new { PostId = 0, Title = "", Content = "", BlogId = 0 })
                .SetColumnForProperty(e => e.Title, new Column { Type = "nvarchar", Length = 128 })
                .SetColumnForProperty(e => e.Content, new Column { Type = "nvarchar" })
                .SetIdentity(e => e.PostId)
                .SetPrimaryKey(e => e.PostId);

            // Assert
            Assert.True(blog.Table.Columns.Count == 2);
            Assert.False(blog.Table.PrimaryKey == null);
            Assert.False(blog.Table.Identity == null);

            Assert.True(post.Table.Columns.Count == 4);
            Assert.False(post.Table.PrimaryKey == null);
            Assert.False(post.Table.Identity == null);

            Assert.True(database.Tables.Count == 2);

            Assert.True(database.FindTable("dbo.Blog").Columns.Count == 2);
            Assert.False(database.FindTable("dbo.Blog").Identity == null);
            Assert.False(database.FindTable("dbo.Blog").PrimaryKey == null);

            Assert.True(database.FindTable("dbo.Post").Columns.Count == 4);
            Assert.False(database.FindTable("dbo.Post").Identity == null);
            Assert.False(database.FindTable("dbo.Post").PrimaryKey == null);
        }
    }
}

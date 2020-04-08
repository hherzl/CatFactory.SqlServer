using CatFactory.SqlServer.ObjectRelationalMapping;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class EntityTests
    {
        [Fact]
        public void CreateDefinitionForCollegeEntities()
        {
            // Arrange
            var database = SqlServerDatabase.CreateWithDefaults("College");

            database.AddDefaultTypeMapFor(typeof(string), "nvarchar");

            // Act
            var student = database
                .DefineEntity(new { StudentId = 0, FirstName = "", MiddleName = "", LastName = "", Gender = "" })
                .SetNaming("Student")
                .SetColumnFor(e => e.FirstName, length: 10)
                .SetColumnFor(e => e.MiddleName, length: 10, nullable: true)
                .SetColumnFor(e => e.LastName, length: 10)
                .SetColumnFor(e => e.Gender, length: 1)
                .SetIdentity(e => e.StudentId)
                .SetPrimaryKey(e => e.StudentId)
                ;

            student
                .AddExtendedProperty(e => e.FirstName, "MS_Description", "First name")
                .AddExtendedProperty(e => e.MiddleName, "MS_Description", "Middle name")
                .AddExtendedProperty(e => e.LastName, "MS_Description", "Last name")
                ;

            var course = database
                .DefineEntity(new { CourseId = 0, Name = "" })
                .SetNaming("Course")
                .SetColumnFor(e => e.Name, type: "nvarchar", length: 255)
                .SetIdentity(e => e.CourseId)
                .SetPrimaryKey(e => e.CourseId)
                .AddUnique(e => e.Name)
                ;

            var courseStudent = database
                .DefineEntity(new { CourseStudentId = 0, CourseId = 0, StudentId = 0 })
                .SetNaming("CourseStudent")
                .SetIdentity(e => e.CourseStudentId)
                .SetPrimaryKey(e => e.CourseStudentId)
                .AddUnique(e => new { e.CourseId, e.StudentId })
                .AddForeignKey(e => e.CourseId, course.Table)
                .AddForeignKey(e => e.StudentId, student.Table)
                ;

            // Assert
            Assert.True(student.Table.Columns.Count == 5);
            Assert.False(student.Table.PrimaryKey == null);
            Assert.False(student.Table.Identity == null);

            Assert.True(course.Table.Columns.Count == 2);
            Assert.False(course.Table.PrimaryKey == null);
            Assert.False(course.Table.Identity == null);

            Assert.True(courseStudent.Table.Columns.Count == 3);
            Assert.False(courseStudent.Table.PrimaryKey == null);
            Assert.False(courseStudent.Table.PrimaryKey.Key.Count == 2);
            Assert.False(courseStudent.Table.Identity == null);

            Assert.True(database.Tables.Count == 3);

            Assert.True(database.FindTable("dbo.Student").Columns.Count == 5);
            Assert.False(database.FindTable("dbo.Student").Identity == null);
            Assert.False(database.FindTable("dbo.Student").PrimaryKey == null);
            Assert.True(database.FindTable("dbo.Student")["MiddleName"].ImportBag.ExtendedProperties.Count == 1);

            Assert.True(database.FindTable("dbo.Course").Columns.Count == 2);
            Assert.False(database.FindTable("dbo.Course").Identity == null);
            Assert.False(database.FindTable("dbo.Course").PrimaryKey == null);
            Assert.True(database.FindTable("dbo.Course").Uniques.Count == 1);

            Assert.True(database.FindTable("dbo.CourseStudent").Columns.Count == 3);
            Assert.False(database.FindTable("dbo.CourseStudent").Identity == null);
            Assert.False(database.FindTable("dbo.CourseStudent").PrimaryKey == null);
        }

        [Fact]
        public void CreateDefinitionForCMSEntities()
        {
            // Arrange
            var database = SqlServerDatabase.CreateWithDefaults("CMS");

            database.AddDefaultTypeMapFor(typeof(string), "nvarchar");

            // Act
            var blog = database
                .DefineEntity(new { BlogId = (short)0, Name = "" })
                .SetNaming("Blog", "WebSite")
                .SetColumnFor(e => e.Name, length: 100)
                .SetIdentity(e => e.BlogId)
                .SetPrimaryKey(e => e.BlogId)
                ;

            var post = database
                .DefineEntity(new { PostId = 0, BlogId = (short)0, Title = "", Content = "" })
                .SetNaming("Post", "WebSite")
                .SetColumnFor(e => e.Title, length: 100)
                .SetIdentity(e => e.BlogId)
                .SetPrimaryKey(e => e.BlogId)
                .AddForeignKey(e => e.BlogId, blog.Table)
                ;

            // Assert
            Assert.True(blog.Table.Columns.Count == 2);
            Assert.False(blog.Table.PrimaryKey == null);
            Assert.False(blog.Table.Identity == null);
            Assert.True(blog.Table["Name"].Length == 100);

            Assert.True(post.Table.Columns.Count == 4);
            Assert.False(post.Table.PrimaryKey == null);
            Assert.False(post.Table.Identity == null);
            Assert.True(post.Table["Title"].Length == 100);
            Assert.True(post.Table["Content"].Length == 0);
        }
    }
}

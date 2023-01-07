using CatFactory.ObjectRelationalMapping;
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
            var db = SqlServerDatabase.CreateWithDefaults("College");

            db.AddDefaultTypeMapFor(typeof(string), "nvarchar");

            // Act
            var student = db
                .DefineEntity(new
                {
                    StudentId = 0,
                    FirstName = "",
                    MiddleName = "",
                    LastName = "",
                    Gender = ""
                })
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

            var course = db
                .DefineEntity(new { CourseId = 0, Name = "" })
                .SetNaming("Course")
                .SetColumnFor(e => e.Name, type: "nvarchar", length: 255)
                .SetIdentity(e => e.CourseId)
                .SetPrimaryKey(e => e.CourseId)
                .AddUnique(e => e.Name)
                ;

            course
                .AddExtendedProperty(e => e.Name, "MS_Description", "Course name")
                ;

            var courseStudent = db
                .DefineEntity(new { CourseStudentId = 0, CourseId = 0, StudentId = 0 })
                .SetNaming("CourseStudent")
                .SetIdentity(e => e.CourseStudentId)
                .SetPrimaryKey(e => e.CourseStudentId)
                .AddUnique(e => new { e.CourseId, e.StudentId })
                .AddForeignKey(e => e.CourseId, course.Table)
                .AddForeignKey(e => e.StudentId, student.Table)
                ;

            courseStudent
                .AddExtendedProperty(e => e.CourseId, "MS_Description", "Course Id")
                .AddExtendedProperty(e => e.CourseId, "MS_Description", "Student Id")
                ;

            // Assert
            Assert.True(db.DbObjects.Count == 3);
            Assert.True(db.Tables.Count == 3);

            Assert.True(db.DbObjects[0].FullName == "dbo.Student");
            Assert.True(db.DbObjects[1].FullName == "dbo.Course");
            Assert.True(db.DbObjects[2].FullName == "dbo.CourseStudent");

            Assert.True(db.FindTable("dbo.Student").Columns.Count == 5);
            Assert.False(db.FindTable("dbo.Student").Identity == null);
            Assert.False(db.FindTable("dbo.Student").PrimaryKey == null);
            Assert.True(db.FindTable("dbo.Student")["MiddleName"].ImportBag.ExtendedProperties.Count == 1);

            Assert.True(db.FindTable("dbo.Course").Columns.Count == 2);
            Assert.False(db.FindTable("dbo.Course").Identity == null);
            Assert.False(db.FindTable("dbo.Course").PrimaryKey == null);
            Assert.True(db.FindTable("dbo.Course").Uniques.Count == 1);

            Assert.True(db.FindTable("dbo.CourseStudent").Columns.Count == 3);
            Assert.False(db.FindTable("dbo.CourseStudent").Identity == null);
            Assert.False(db.FindTable("dbo.CourseStudent").PrimaryKey == null);

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
        }

        [Fact]
        public void CreateDefinitionForCMSEntities()
        {
            // Arrange
            var db = SqlServerDatabase.CreateWithDefaults("CMS");

            db.AddDefaultTypeMapFor(typeof(string), "nvarchar");

            // Act
            var blog = db
                .DefineEntity(new
                {
                    BlogId = (short)0,
                    Name = ""
                })
                .SetNaming("Blog", "WebSite")
                .SetColumnFor(e => e.Name, length: 100)
                .SetIdentity(e => e.BlogId)
                .SetPrimaryKey(e => e.BlogId)
                ;

            var post = db
                .DefineEntity(new
                {
                    PostId = 0,
                    BlogId = (short)0,
                    Title = "",
                    Content = ""
                })
                .SetNaming("Post", "WebSite")
                .SetColumnFor(e => e.Title, length: 100)
                .SetIdentity(e => e.BlogId)
                .SetPrimaryKey(e => e.BlogId)
                .AddForeignKey(e => e.BlogId, blog.Table)
                ;

            // Assert
            Assert.True(db.DbObjects.Count == 2);
            Assert.True(db.Tables.Count == 2);

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

        [Fact]
        public void CreateDefinitionWithAuditPropertiesForLibraryEntities()
        {
            // Arrange
            var db = SqlServerDatabase.CreateWithDefaults("Library");

            db.AddDefaultTypeMapFor(typeof(string), "nvarchar");

            // Act
            var book = db
                .DefineEntity(new
                {
                    Id = 0,
                    Name = "",
                    Author = "",
                    Year = (short)0
                })
                .SetNaming("Book")
                .SetColumnFor(e => e.Name, length: 100)
                .SetColumnFor(e => e.Author, length: 25)
                .SetIdentity(e => e.Id)
                .SetPrimaryKey(e => e.Id)
                ;

            var stock = db
                .DefineEntity(new
                {
                    Id = 0,
                    BookId = 0,
                    Quantity = 0
                })
                .SetNaming("Stock")
                .SetIdentity(e => e.Id)
                .SetPrimaryKey(e => e.Id)
                .AddForeignKey(e => e.BookId, book.Table)
                ;

            db.AddColumnForTables(new Column { Name = "CreationUser", Type = "nvarchar", Length = 25 });
            db.AddColumnForTables(new Column { Name = "CreationDate", Type = "datetime" });

            // Assert
            Assert.True(db.FindTable("dbo.Book").Columns.Count == 6);
            Assert.True(db.FindTable("dbo.Stock").Columns.Count == 5);
        }
    }
}

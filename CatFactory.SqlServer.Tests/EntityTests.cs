using CatFactory.SqlServer.ObjectRelationalMapping;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class EntityTests
    {
        [Fact]
        public void TestDefinitionForCollegeEntities()
        {
            // Arrange
            var database = SqlServerDatabaseFactory.CreateWithDefaults("College");

            // Act
            var student = database
                .DefineEntity(new { StudentId = 0, FirstName = "", MiddleName = "", LastName = "" })
                .SetNaming("Student")
                .SetColumnFor(e => e.FirstName, type: "nvarchar", length: 10)
                    .AddExtendedProperty(e => e.FirstName, "MS_Description", "First name")
                .SetColumnFor(e => e.MiddleName, type: "nvarchar", length: 10, nullable: true)
                    .AddExtendedProperty(e => e.MiddleName, "MS_Description", "Middle name")
                .SetColumnFor(e => e.LastName, type: "nvarchar", length: 10)
                    .AddExtendedProperty(e => e.LastName, "MS_Description", "Last name")
                .SetIdentity(e => e.StudentId)
                .SetPrimaryKey(e => e.StudentId);

            var course = database
                .DefineEntity(new { CourseId = 0, Name = "" })
                .SetNaming("Course")
                .SetColumnFor(e => e.Name, type: "nvarchar", length: 255)
                .SetIdentity(e => e.CourseId)
                .SetPrimaryKey(e => e.CourseId)
                .AddUnique(e => e.Name);

            var courseStudent = database
                .DefineEntity(new { CourseStudentId = 0, CourseId = 0, StudentId = 0 })
                .SetNaming("CourseStudent")
                .SetIdentity(e => e.CourseStudentId)
                .SetPrimaryKey(e => e.CourseStudentId)
                .AddUnique(e => new { e.CourseId, e.StudentId })
                .AddForeignKey(e => e.CourseId, course.Table)
                .AddForeignKey(e => e.StudentId, student.Table);

            // Assert
            Assert.True(student.Table.Columns.Count == 4);
            Assert.False(student.Table.PrimaryKey == null);
            Assert.False(student.Table.Identity == null);

            Assert.True(course.Table.Columns.Count == 2);
            Assert.False(course.Table.PrimaryKey == null);
            Assert.False(course.Table.Identity == null);

            Assert.True(courseStudent.Table.Columns.Count == 3);
            Assert.False(courseStudent.Table.PrimaryKey == null);
            Assert.False(courseStudent.Table.Identity == null);

            Assert.True(database.Tables.Count == 3);

            Assert.True(database.FindTable("dbo.Student").Columns.Count == 4);
            Assert.False(database.FindTable("dbo.Student").Identity == null);
            Assert.False(database.FindTable("dbo.Student").PrimaryKey == null);
            Assert.True(database.FindTable("dbo.Student")["MiddleName"].ExtendedProperties.Count == 1);

            Assert.True(database.FindTable("dbo.Course").Columns.Count == 2);
            Assert.False(database.FindTable("dbo.Course").Identity == null);
            Assert.False(database.FindTable("dbo.Course").PrimaryKey == null);
            Assert.True(database.FindTable("dbo.Course").Uniques.Count == 1);

            Assert.True(database.FindTable("dbo.CourseStudent").Columns.Count == 3);
            Assert.False(database.FindTable("dbo.CourseStudent").Identity == null);
            Assert.False(database.FindTable("dbo.CourseStudent").PrimaryKey == null);
        }
    }
}

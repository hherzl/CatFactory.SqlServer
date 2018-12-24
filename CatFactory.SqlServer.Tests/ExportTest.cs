using CatFactory.SqlServer.CodeFactory;
using CatFactory.SqlServer.ObjectRelationalMapping;
using CatFactory.SqlServer.Tests.Models;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class ExportTest
    {
        [Fact]
        public void TestBloggingExportScript()
        {
            // Arrange
            var database = Databases.Blogging;

            // Act
            var codeBuilder = new SqlServerDatabaseScriptCodeBuilder
            {
                Database = database,
                OutputDirectory = "C:\\Temp\\CatFactory.SqlServer",
                ForceOverwrite = true
            };

            codeBuilder.CreateFile();

            // Assert
        }

        [Fact]
        public void TestCollegeExportScript()
        {
            // Arrange
            var database = SqlServerDatabaseFactory.CreateWithDefaults("College");

            // Act
            var student = database
                .DefineEntity(new { StudentId = 0, FirstName = "", MiddleName = "", LastName = "" })
                .SetName("Student")
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
                .SetName("Course")
                .SetColumnFor(e => e.Name, type: "nvarchar", length: 255)
                .SetIdentity(e => e.CourseId, seed: 1000, increment: 1000)
                .SetPrimaryKey(e => e.CourseId)
                .AddUnique(e => e.Name);

            var courseStudent = database
                .DefineEntity(new { CourseStudentId = 0, CourseId = 0, StudentId = 0 })
                .SetName("CourseStudent")
                .SetIdentity(e => e.CourseStudentId)
                .SetPrimaryKey(e => e.CourseStudentId)
                .AddUnique(e => new { e.CourseId, e.StudentId })
                .AddForeignKey(e => e.CourseId, course.Table)
                .AddForeignKey(e => e.StudentId, student.Table);

            var codeBuilder = new SqlServerDatabaseScriptCodeBuilder
            {
                Database = database,
                OutputDirectory = "C:\\Temp\\CatFactory.SqlServer",
                ForceOverwrite = true
            };

            codeBuilder.CreateFile();

            // Assert
        }
    }
}

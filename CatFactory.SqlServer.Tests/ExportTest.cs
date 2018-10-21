using CatFactory.SqlServer.CodeFactory;
using CatFactory.SqlServer.Tests.Models;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class ExportTest
    {
        [Fact]
        public void ExportScript()
        {
            // Arrange
            var database = Databases.Store;

            // Act
            var codeBuilder = new SqlCodeBuilder
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

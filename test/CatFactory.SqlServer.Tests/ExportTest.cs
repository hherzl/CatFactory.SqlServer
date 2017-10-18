using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class ExportTest
    {
        [Fact]
        public void ExportScript()
        {
            var codeBuilder = new SqlCodeBuilder
            {
                Database = StoreMockDatabase.Db,
                OutputDirectory = "C:\\Temp\\CatFactory.SqlServer",
                ForceOverwrite = true
            };

            codeBuilder.CreateFile();
        }
    }
}

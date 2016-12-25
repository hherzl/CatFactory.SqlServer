using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class ExportTest
    {
        [Fact]
        public void ExportScript()
        {
            var db = Mocks.StoreDatabase;

            var codeBuilder = new SqlCodeBuilder()
            {
                Database = db,
                OutputDirectory = "C:\\Temp"
            };

            codeBuilder.CreateFile();
        }
    }
}

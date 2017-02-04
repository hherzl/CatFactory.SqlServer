using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class ExportTest
    {
        [Fact]
        public void ExportScript()
        {
            var codeBuilder = new SqlCodeBuilder()
            {
                Database = StoreDatabase.Mock,
                OutputDirectory = "C:\\Temp\\CatFactory.SqlServer"
            };

            codeBuilder.CreateFile();
        }
    }
}

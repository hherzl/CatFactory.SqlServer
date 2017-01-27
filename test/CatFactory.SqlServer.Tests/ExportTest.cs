using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class ExportTest
    {
        [Fact]
        public void ExportScript()
        {
            var db = StoreDatabase.Mock;

            var codeBuilder = new SqlCodeBuilder()
            {
                Database = db,
                OutputDirectory = "C:\\Temp\\CatFactory.SqlServer"
            };

            codeBuilder.CreateFile();
        }
    }
}

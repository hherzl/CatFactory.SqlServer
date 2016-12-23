using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class StoredProcedureTests
    {
        [Fact]
        public void GenerateProcedureTest()
        {
            foreach (var table in Mocks.SalesDatabase.Tables)
            {
                var codeBuilder = new SqlStoredProcedureCodeBuilder
                {
                    Table = table,
                    OutputDirectory = "C:\\Temp\\StoredProcedures"
                };

                codeBuilder.CreateFile();
            }
        }
    }
}

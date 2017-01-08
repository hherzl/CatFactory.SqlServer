using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class StoredProcedureGenerationTests
    {
        [Fact]
        public void GenerateProcedureFromMockDatabaseTest()
        {
            foreach (var table in Mocks.StoreDatabase.Tables)
            {
                var codeBuilder = new SqlStoredProcedureCodeBuilder
                {
                    Table = table,
                    OutputDirectory = "C:\\Temp\\CatFactory.SqlServer\\StoredProceduresMockingDatabase"
                };

                codeBuilder.CreateFile();
            }
        }

        [Fact]
        public void GenerateProcedureFromExistingDatabaseTest()
        {
            var connectionString = "server=(local);database=Store;integrated security=yes;";

            var dbFactory = new SqlServerDatabaseFactory()
            {
                ConnectionString = connectionString
            };

            var db = dbFactory.Import();

            foreach (var table in db.Tables)
            {
                var codeBuilder = new SqlStoredProcedureCodeBuilder
                {
                    Table = table,
                    OutputDirectory = "C:\\Temp\\CatFactory.SqlServer\\StoredProceduresExistingDatabase"
                };

                codeBuilder.CreateFile();
            }
        }
    }
}

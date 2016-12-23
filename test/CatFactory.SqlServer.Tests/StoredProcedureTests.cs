using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class StoredProcedureTests
    {
        [Fact]
        public void GenerateProcedureFromMockDatabaseTest()
        {
            foreach (var table in Mocks.SalesDatabase.Tables)
            {
                var codeBuilder = new SqlStoredProcedureCodeBuilder
                {
                    Table = table,
                    OutputDirectory = "C:\\Temp\\StoredProceduresMockingDatabase"
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
                    OutputDirectory = "C:\\Temp\\StoredProceduresExistingDatabase"
                };

                codeBuilder.CreateFile();
            }
        }
    }
}

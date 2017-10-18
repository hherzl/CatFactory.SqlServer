using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class StoredProcedureGenerationTests
    {
        [Fact]
        public void GenerateProcedureFromMockDatabaseTest()
        {
            foreach (var table in StoreMockDatabase.Db.Tables)
            {
                var codeBuilder = new SqlStoredProcedureCodeBuilder
                {
                    Table = table,
                    OutputDirectory = "C:\\Temp\\CatFactory.SqlServer\\StoredProceduresMockingDatabase",
                    ForceOverwrite = true
                };

                codeBuilder.CreateFile();
            }
        }

        [Fact]
        public void GenerateProcedureFromExistingDatabaseTest()
        {
            var dbFactory = new SqlServerDatabaseFactory
            {
                ConnectionString = "server=(local);database=Store;integrated security=yes;"
            };

            var database = dbFactory.Import();

            foreach (var table in database.Tables)
            {
                var codeBuilder = new SqlStoredProcedureCodeBuilder
                {
                    Table = table,
                    OutputDirectory = "C:\\Temp\\CatFactory.SqlServer\\StoredProceduresExistingDatabase",
                    ForceOverwrite = true
                };

                codeBuilder.CreateFile();
            }
        }
    }
}

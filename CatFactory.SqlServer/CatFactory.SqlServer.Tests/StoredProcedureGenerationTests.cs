using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class StoredProcedureGenerationTests
    {
        [Fact]
        public void GenerateProceduresFromExistingDatabaseTest()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                ImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=Store;integrated security=yes;"
                }
            };

            // Act
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

            // Assert
        }

        [Fact]
        public void GenerateProceduresFromMockDatabaseTest()
        {
            // Arrange
            var database = Databases.Store;

            // Act
            foreach (var table in database.Tables)
            {
                var codeBuilder = new SqlStoredProcedureCodeBuilder
                {
                    Table = table,
                    OutputDirectory = "C:\\Temp\\CatFactory.SqlServer\\StoredProceduresMockingDatabase",
                    ForceOverwrite = true
                };

                codeBuilder.CreateFile();
            }

            // Assert
        }
    }
}

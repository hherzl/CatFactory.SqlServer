using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class StoredProcedureScaffoldingTests
    {
        [Fact]
        public void GenerateProceduresFromExistingDatabaseTest()
        {
            // Arrange
            var dbFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
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
                    Database = database,
                    Table = table,
                    OutputDirectory = "C:\\Temp\\CatFactory.SqlServer\\StoredProceduresFromExistingDatabase",
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
                    Database = database,
                    Table = table,
                    OutputDirectory = "C:\\Temp\\CatFactory.SqlServer\\StoredProceduresFromMockingDatabase",
                    ForceOverwrite = true
                };

                codeBuilder.CreateFile();
            }

            // Assert
        }
    }
}

using System.Collections.Generic;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class SerializationTests
    {
        [Fact]
        public void SerializeMockDatabaseTest()
        {
            // Arrange
            var database = Databases.Store;

            // Act
            var output = XmlSerializerHelper.Serialize(database);

            TextFileHelper.CreateFile("C:\\Temp\\CatFactory.SqlServer\\Store.xml", output);

            // Assert
        }

        [Fact]
        public void SerializeExistingDatabaseTest()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                ConnectionString = "server=(local);database=AdventureWorks2012;integrated security=yes;MultipleActiveResultSets=true;",
                ImportSettings = new DatabaseImportSettings
                {
                    ImportMSDescription = true,
                    Exclusions = new List<string> { "dbo.EventLog" }
                }
            };

            // Act
            var database = databaseFactory.Import();

            var output = XmlSerializerHelper.Serialize(database);

            TextFileHelper.CreateFile("C:\\Temp\\CatFactory.SqlServer\\AdventureWorks2012.xml", output);

            // Assert
        }
    }
}

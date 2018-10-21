using System.IO;
using CatFactory.SqlServer.Tests.Helpers;
using CatFactory.SqlServer.Tests.Models;
using Newtonsoft.Json;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class SerializationTests
    {
        [Fact]
        public void SerializeMockDatabaseToXmlTest()
        {
            // Arrange
            var database = Databases.Store;

            // Act
            var output = XmlSerializerHelper.Serialize(database);

            File.WriteAllText("C:\\Temp\\CatFactory.SqlServer\\Store.xml", output);

            // Assert
        }

        [Fact]
        public void SerializeMockDatabaseToJsonTest()
        {
            // Arrange
            var database = Databases.Store;

            // Act
            var output = JsonConvert.SerializeObject(database, Formatting.Indented);

            File.WriteAllText("C:\\Temp\\CatFactory.SqlServer\\Store.json", output);

            // Assert
        }

        [Fact]
        public void SerializeAdventureWorks2017DatabaseToXmlTest()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=AdventureWorks2017;integrated security=yes;MultipleActiveResultSets=true;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    },
                    Exclusions =
                    {
                        "dbo.EventLog"
                    }
                }
            };

            // Act
            var database = databaseFactory.Import();

            var output = XmlSerializerHelper.Serialize(database);

            File.WriteAllText("C:\\Temp\\CatFactory.SqlServer\\AdventureWorks2017.xml", output);

            // Assert
        }

        [Fact]
        public void SerializeAdventureWorks2017DatabaseToJsonTest()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                DatabaseImportSettings = new DatabaseImportSettings
                {
                    ConnectionString = "server=(local);database=AdventureWorks2017;integrated security=yes;MultipleActiveResultSets=true;",
                    ExtendedProperties =
                    {
                        "MS_Description"
                    },
                    Exclusions =
                    {
                        "dbo.EventLog"
                    }
                }
            };

            // Act
            var database = databaseFactory.Import();

            var output = JsonConvert.SerializeObject(database, Formatting.Indented);

            File.WriteAllText("C:\\Temp\\CatFactory.SqlServer\\AdventureWorks2017.json", output);

            // Assert
        }
    }
}

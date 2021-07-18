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
        public void SerializeMockDatabaseToXml()
        {
            // Arrange
            var database = Databases.Blogging;

            // Act
            var output = XmlSerializerHelper.Serialize(database);

            File.WriteAllText(@"C:\Temp\CatFactory.SqlServer\Blogging.xml", output);

            // Assert
        }

        [Fact]
        public void SerializeMockDatabaseToJson()
        {
            // Arrange
            var database = Databases.Blogging;

            // Act
            var output = JsonConvert.SerializeObject(database, Formatting.Indented);

            File.WriteAllText(@"C:\Temp\CatFactory.SqlServer\Blogging.json", output);

            // Assert
        }

        [Fact]
        public void SerializeAdventureWorks2017DatabaseToXml()
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

            File.WriteAllText(@"C:\Temp\CatFactory.SqlServer\AdventureWorks2017.xml", output);

            // Assert
        }

        [Fact]
        public void SerializeAdventureWorks2017DatabaseToJson()
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

            File.WriteAllText(@"C:\Temp\CatFactory.SqlServer\AdventureWorks2017.json", output);

            // Assert
        }
    }
}

using System.IO;
using CatFactory.SqlServer.Tests.Helpers;
using CatFactory.SqlServer.Tests.Models;
using CatFactory.SqlServer.Tests.Settings;
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
            var xml = XmlSerializerHelper.Serialize(database);

            File.WriteAllText(@"C:\Temp\CatFactory.SqlServer\Blogging.xml", xml);

            // Assert
        }

        [Fact]
        public void SerializeMockDatabaseToJson()
        {
            // Arrange
            var database = Databases.Blogging;

            // Act
            var json = JsonConvert.SerializeObject(database, Formatting.Indented);

            File.WriteAllText(@"C:\Temp\CatFactory.SqlServer\Blogging.json", json);

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
                    ConnectionString = ConnectionStrings.AdventureWorks2017,
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

            var xml = XmlSerializerHelper.Serialize(database);

            File.WriteAllText(@"C:\Temp\CatFactory.SqlServer\AdventureWorks2017.xml", xml);

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
                    ConnectionString = ConnectionStrings.AdventureWorks2017,
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

            var json = JsonConvert.SerializeObject(database, Formatting.Indented);

            File.WriteAllText(@"C:\Temp\CatFactory.SqlServer\AdventureWorks2017.json", json);

            // Assert
        }
    }
}

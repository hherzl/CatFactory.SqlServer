using System.Linq;
using System.Threading.Tasks;
using CatFactory.ObjectRelationalMapping;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class DatabaseTypeMapsTests
    {
        private const string OnlineStoreConnectionString = "server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;";
        private const string AdventureWorks2017ConnectionString = "server=(local); database=AdventureWorks2017; integrated security=yes; TrustServerCertificate=True;";

        [Fact]
        public async Task GetMapsForStringAsync()
        {
            // Arrange
            var database = await SqlServerDatabaseFactory.ImportAsync(OnlineStoreConnectionString);

            // Act
            var mapsForString = database.DatabaseTypeMaps.Where(item => item.GetClrType() == typeof(string)).ToList();

            // Assert
            Assert.True(mapsForString.Count == 6);
        }

        [Fact]
        public async Task GetMapsForDecimalAsync()
        {
            // Arrange
            var database = await SqlServerDatabaseFactory.ImportAsync(OnlineStoreConnectionString);

            // Act
            var mapsForString = database.DatabaseTypeMaps.Where(item => item.GetClrType() == typeof(decimal)).ToList();

            // Assert
            Assert.True(mapsForString.Count == 4);
        }

        [Fact]
        public async Task GetMapForVarcharAsync()
        {
            // Arrange
            var database = await SqlServerDatabaseFactory.ImportAsync(OnlineStoreConnectionString);

            // Act
            var mapForVarchar = database.DatabaseTypeMaps.FirstOrDefault(item => item.DatabaseType == "varchar");

            // Assert
            Assert.False(mapForVarchar == null);
        }

        [Fact]
        public async Task GetMapForTypeWithParentAsync()
        {
            // Arrange
            var database = await SqlServerDatabaseFactory.ImportAsync(AdventureWorks2017ConnectionString);

            // Act
            var mapForName = database.DatabaseTypeMaps.FirstOrDefault(item => item.DatabaseType == "Name");

            var parentType = mapForName.GetParentType(database.DatabaseTypeMaps);

            // Assert
            Assert.False(mapForName == null);
            Assert.False(parentType == null);
            Assert.True(parentType.GetClrType() == typeof(string));
        }
    }
}

using System.Linq;
using System.Threading.Tasks;
using CatFactory.ObjectRelationalMapping;
using CatFactory.SqlServer.ObjectRelationalMapping;
using CatFactory.SqlServer.Tests.Settings;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class DatabaseTypeMapsTests
    {
        [Fact]
        public async Task GetMapsForStringAsync()
        {
            // Arrange
            var database = await SqlServerDatabaseFactory.ImportAsync(ConnectionStrings.OnlineStore);

            // Act
            var mapsForString = database.DatabaseTypeMaps.GetByClrType(typeof(string)).ToList();

            // Assert
            Assert.True(mapsForString.Count == 6);
        }

        [Fact]
        public async Task GetMapsForDecimalAsync()
        {
            // Arrange
            var database = await SqlServerDatabaseFactory.ImportAsync(ConnectionStrings.OnlineStore);

            // Act
            var mapsForString = database.DatabaseTypeMaps.GetByClrType(typeof(decimal)).ToList();

            // Assert
            Assert.True(mapsForString.Count == 4);
        }

        [Fact]
        public async Task GetMapForVarcharAsync()
        {
            // Arrange
            var database = await SqlServerDatabaseFactory.ImportAsync(ConnectionStrings.OnlineStore);

            // Act
            var mapForVarchar = database.DatabaseTypeMaps.GetByDatabaseType("varchar");

            // Assert
            Assert.False(mapForVarchar == null);
        }

        [Fact]
        public async Task GetMapForTypeWithParentAsync()
        {
            // Arrange
            var database = await SqlServerDatabaseFactory.ImportAsync(ConnectionStrings.AdventureWorks2017);

            // Act
            var mapForName = database.DatabaseTypeMaps.GetByDatabaseType("Name");

            var parentType = mapForName.GetParentType(database.DatabaseTypeMaps);

            // Assert
            Assert.False(mapForName == null);
            Assert.False(parentType == null);
            Assert.True(parentType.GetClrType() == typeof(string));
        }
    }
}

using System.Linq;
using CatFactory.ObjectRelationalMapping;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class DatabaseTypeMapsTests
    {
        [Fact]
        public void TestGetMapsForString()
        {
            // Arrange
            var database = SqlServerDatabaseFactory
                .Import("server=(local);database=OnlineStore;integrated security=yes;");

            // Act
            var mapsForString = database.DatabaseTypeMaps.Where(item => item.GetClrType() == typeof(string)).ToList();

            // Assert
            Assert.True(mapsForString.Count == 6);
        }

        [Fact]
        public void TestGetMapsForDecimal()
        {
            // Arrange
            var database = SqlServerDatabaseFactory
                .Import("server=(local);database=OnlineStore;integrated security=yes;");

            // Act
            var mapsForString = database.DatabaseTypeMaps.Where(item => item.GetClrType() == typeof(decimal)).ToList();

            // Assert
            Assert.True(mapsForString.Count == 4);
        }

        [Fact]
        public void TestGetMapForVarchar()
        {
            // Arrange
            var database = SqlServerDatabaseFactory
                .Import("server=(local);database=OnlineStore;integrated security=yes;");

            // Act
            var mapForVarchar = database.DatabaseTypeMaps.FirstOrDefault(item => item.DatabaseType == "varchar");

            // Assert
            Assert.False(mapForVarchar == null);
        }

        [Fact]
        public void TestGetMapForTypeWithParent()
        {
            // Arrange
            var database = SqlServerDatabaseFactory
                .Import("server=(local);database=AdventureWorks2017;integrated security=yes;");

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

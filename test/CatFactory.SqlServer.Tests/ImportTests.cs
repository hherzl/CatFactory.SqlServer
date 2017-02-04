using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class Tests
    {
        [Fact]
        public void ImportNorthwindDatabaseTest()
        {
            var logger = LoggerMocker.GetLogger<SqlServerDatabaseFactory>();

            var db = SqlServerDatabaseFactory.Import(logger, "server=(local);database=Northwind;integrated security=yes;", "dbo.ChangeLog");
        }

        [Fact]
        public void ImportAdventureWorksDatabase()
        {
            var logger = LoggerMocker.GetLogger<SqlServerDatabaseFactory>();

            var db = SqlServerDatabaseFactory.Import(logger, "server=(local);database=AdventureWorks2012;integrated security=yes;");
        }
    }
}

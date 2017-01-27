using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class Tests
    {
        [Fact]
        public void ImportNorthwindDatabaseTest()
        {
            var connectionString = "server=(local);database=Northwind;integrated security=yes;";

            var logger = LoggerMocker.GetLogger<SqlServerDatabaseFactory>();

            var dbFactory = new SqlServerDatabaseFactory(logger)
            {
                ConnectionString = connectionString
            };

            dbFactory.Exclusions.Add("dbo.ChangeLog");

            var db = dbFactory.Import();
        }

        [Fact]
        public void ImportAdventureWorksDatabase()
        {
            var connectionString = "server=(local);database=AdventureWorks2012;integrated security=yes;";

            var logger = LoggerMocker.GetLogger<SqlServerDatabaseFactory>();

            var dbFactory = new SqlServerDatabaseFactory(logger)
            {
                ConnectionString = connectionString
            };

            var db = dbFactory.Import();
        }
    }
}

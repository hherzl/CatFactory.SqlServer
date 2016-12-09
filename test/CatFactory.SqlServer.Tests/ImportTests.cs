using CatFactory.SqlServer;
using Xunit;

namespace Tests
{
    public class Tests
    {
        [Fact]
        public void ImportNorthwindDatabaseTest()
        {
            var connectionString = "server=(local);database=Northwind;integrated security=yes;";

            var dbFactory = new SqlServerDatabaseFactory()
            {
                ConnectionString = connectionString
            };

            var db = dbFactory.Import();
        }

        [Fact]
        public void ImportAdventureWorksDatabase()
        {
            var connectionString = "server=(local);database=AdventureWorks2012;integrated security=yes;";

            var dbFactory = new SqlServerDatabaseFactory()
            {
                ConnectionString = connectionString
            };

            var db = dbFactory.Import();
        }
    }
}

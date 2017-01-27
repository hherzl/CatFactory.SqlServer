using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class SerializationTests
    {
        [Fact]
        public void SerializeMockDatabaseTest()
        {
            var db = StoreDatabase.Mock;

            var serializer = new Serializer() as ISerializer;

            var output = serializer.Serialize(db);

            TextFileHelper.CreateFile("C:\\Temp\\CatFactory.SqlServer\\Store.xml", output);
        }

        [Fact]
        public void SerializeExistingDatabaseTest()
        {
            var connectionString = "server=(local);database=AdventureWorks2012;integrated security=yes;";

            var dbFactory = new SqlServerDatabaseFactory()
            {
                ConnectionString = connectionString
            };

            dbFactory.Exclusions.Add("EventLog");

            var db = dbFactory.Import();

            var serializer = new Serializer() as ISerializer;

            var output = serializer.Serialize(db);

            TextFileHelper.CreateFile("C:\\Temp\\CatFactory.SqlServer\\AdventureWorks2012.xml", output);
        }
    }
}

using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class SerializationTests
    {
        [Fact]
        public void SerializeExistingDatabaseTest()
        {
            var connectionString = "server=(local);database=Northwind;integrated security=yes;";

            var dbFactory = new SqlServerDatabaseFactory()
            {
                ConnectionString = connectionString
            };

            var db = dbFactory.Import();

            var serializer = new Serializer() as ISerializer;

            var output = serializer.Serialize(db);

            TextFileHelper.CreateFile("C:\\Temp\\Northwind.xml", output);
        }
    }
}

using System;
using System.Collections.Generic;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class SerializationTests
    {
        [Fact]
        public void SerializeMockDatabaseTest()
        {
            var database = StoreMockDatabase.Db;

            var serializer = new Serializer();

            var output = serializer.Serialize(database);

            TextFileHelper.CreateFile("C:\\Temp\\CatFactory.SqlServer\\Store.xml", output);
        }

        [Fact]
        public void SerializeExistingDatabaseTest()
        {
            var databaseFactory = new SqlServerDatabaseFactory
            {
                ConnectionString = "server=(local);database=AdventureWorks2012;integrated security=yes;",
                Exclusions = new List<String>()
                {
                    "EventLog"
                }
            };

            var database = databaseFactory.Import();

            var serializer = new Serializer() as ISerializer;

            var output = serializer.Serialize(database);

            TextFileHelper.CreateFile("C:\\Temp\\CatFactory.SqlServer\\AdventureWorks2012.xml", output);
        }
    }
}

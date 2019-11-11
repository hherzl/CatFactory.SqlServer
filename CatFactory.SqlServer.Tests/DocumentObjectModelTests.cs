using System.Data.SqlClient;
using System.Linq;
using CatFactory.SqlServer.DocumentObjectModel.Queries;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class DocumentObjectModelTests
    {
        [Fact]
        public void TestGetSysSchemas()
        {
            // Arrange

            var connection = new SqlConnection("server=(local);database=WideWorldImporters;integrated security=yes;");

            // Act

            connection.Open();

            var types = connection.GetSysSchemas().ToList();

            connection.Dispose();

            // Assert

            Assert.True(types.Count > 0);
        }

        [Fact]
        public void TestGetSysTypes()
        {
            // Arrange

            var connection = new SqlConnection("server=(local);database=WideWorldImporters;integrated security=yes;");

            // Act

            connection.Open();

            var types = connection.GetSysTypes().ToList();

            connection.Dispose();

            // Assert

            Assert.True(types.Count > 0);
        }

        [Fact]
        public void TestGetSysTables()
        {
            // Arrange

            var connection = new SqlConnection("server=(local);database=WideWorldImporters;integrated security=yes;");

            // Act

            connection.Open();

            var tables = connection.GetSysTables().ToList();

            connection.Dispose();

            // Assert

            Assert.True(tables.Count > 0);
        }

        [Fact]
        public void TestGetSysViews()
        {
            // Arrange

            var connection = new SqlConnection("server=(local);database=WideWorldImporters;integrated security=yes;");

            // Act

            connection.Open();

            var views = connection.GetSysViews().ToList();

            connection.Dispose();

            // Assert

            Assert.True(views.Count > 0);
        }

        [Fact]
        public void TestGetSysColumns()
        {
            // Arrange

            var connection = new SqlConnection("server=(local);database=WideWorldImporters;integrated security=yes;");

            // Act

            connection.Open();

            var columns = connection.GetSysColumns().ToList();

            connection.Dispose();

            // Assert

            Assert.True(columns.Count > 0);
        }

        [Fact]
        public void TestGetSysSequences()
        {
            // Arrange

            var connection = new SqlConnection("server=(local);database=WideWorldImporters;integrated security=yes;");

            // Act

            connection.Open();

            var sequences = connection.GetSysSequences().ToList();

            connection.Dispose();

            // Assert

            Assert.True(sequences.Count > 0);
        }

        [Fact]
        public void TestGetFirstResultSetForCustOrdersOrdersStoredProcedure()
        {
            // Arrange

            var connection = new SqlConnection("server=(local);database=Northwind;integrated security=yes;");

            // Act

            connection.Open();

            var firstResultSet = connection.DmExecDescribeFirstResultSetForObject("CustOrdersOrders").ToList();

            connection.Dispose();

            // Assert

            Assert.True(firstResultSet.Count > 0);
        }
    }
}

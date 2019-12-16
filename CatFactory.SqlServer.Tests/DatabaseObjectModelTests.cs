using System.Data.SqlClient;
using System.Threading.Tasks;
using CatFactory.SqlServer.DatabaseObjectModel.Queries;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class DatabaseObjectModelTests
    {
        [Fact]
        public async Task TestGetSysSchemasAsync()
        {
            // Arrange

            var connection = new SqlConnection("server=(local);database=WideWorldImporters;integrated security=yes;");

            // Act

            await connection.OpenAsync();

            var types = await connection.GetSysSchemasAsync();

            connection.Dispose();

            // Assert

            Assert.True(types.Count > 0);
        }

        [Fact]
        public async Task TestGetSysTypesAsync()
        {
            // Arrange

            var connection = new SqlConnection("server=(local);database=WideWorldImporters;integrated security=yes;");

            // Act

            await connection.OpenAsync();

            var types = await connection.GetSysTypesAsync();

            connection.Dispose();

            // Assert

            Assert.True(types.Count > 0);
        }

        [Fact]
        public async Task TestGetSysTablesAsync()
        {
            // Arrange

            var connection = new SqlConnection("server=(local);database=WideWorldImporters;integrated security=yes;");

            // Act

            await connection.OpenAsync();

            var tables = await connection.GetSysTablesAsync();

            connection.Dispose();

            // Assert

            Assert.True(tables.Count > 0);
        }

        [Fact]
        public async Task TestGetSysViewsAsync()
        {
            // Arrange

            var connection = new SqlConnection("server=(local);database=WideWorldImporters;integrated security=yes;");

            // Act

            await connection.OpenAsync();

            var views = await connection.GetSysViewsAsync();

            connection.Dispose();

            // Assert

            Assert.True(views.Count > 0);
        }

        [Fact]
        public async Task TestGetSysColumnsAsync()
        {
            // Arrange

            var connection = new SqlConnection("server=(local);database=WideWorldImporters;integrated security=yes;");

            // Act

            await connection.OpenAsync();

            var columns = await connection.GetSysColumnsAsync();

            connection.Dispose();

            // Assert

            Assert.True(columns.Count > 0);
        }

        [Fact]
        public async Task TestGetSysSequencesAsync()
        {
            // Arrange

            var connection = new SqlConnection("server=(local);database=WideWorldImporters;integrated security=yes;");

            // Act

            await connection.OpenAsync();

            var sequences = await connection.GetSysSequencesAsync();

            connection.Dispose();

            // Assert

            Assert.True(sequences.Count > 0);
        }

        [Fact]
        public async Task TestGetFirstResultSetForCustOrdersOrdersStoredProcedureAsync()
        {
            // Arrange

            var connection = new SqlConnection("server=(local);database=Northwind;integrated security=yes;");

            // Act

            await connection.OpenAsync();

            var firstResultSet = await connection.DmExecDescribeFirstResultSetForObjectAsync("CustOrdersOrders");

            connection.Dispose();

            // Assert

            Assert.True(firstResultSet.Count > 0);
        }

        [Fact]
        public async Task TestSpHelpForTableAsync()
        {
            // Arrange

            var connection = new SqlConnection("server=(local);database=Northwind;integrated security=yes;");

            // Act

            await connection.OpenAsync();

            var spHelpResult = await connection.SpHelpAsync("Products");

            connection.Dispose();

            // Assert

            Assert.False(spHelpResult == null);
        }
    }
}

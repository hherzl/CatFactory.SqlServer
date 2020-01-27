using System.Data.SqlClient;
using System.Threading.Tasks;
using CatFactory.SqlServer.DatabaseObjectModel.Queries;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class DatabaseObjectModelTests
    {
        [Fact]
        public async Task GetSysSchemasAsync()
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
        public async Task GetSysTypesAsync()
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
        public async Task GetSysTypesBySchemaIdAsync()
        {
            // Arrange

            var connection = new SqlConnection("server=(local);database=WideWorldImporters;integrated security=yes;");

            // Act

            await connection.OpenAsync();

            var types = await connection.GetSysTypesAsync(schemaId: 14);

            connection.Dispose();

            // Assert

            Assert.True(types.Count > 0);
        }

        [Fact]
        public async Task GetSysTypesDefinedByUserAsync()
        {
            // Arrange

            var connection = new SqlConnection("server=(local);database=WideWorldImporters;integrated security=yes;");

            // Act

            await connection.OpenAsync();

            var types = await connection.GetSysTypesAsync(isUserDefined: true);

            connection.Dispose();

            // Assert

            Assert.True(types.Count == 4);
        }

        [Fact]
        public async Task GetSysTablesAsync()
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
        public async Task GetSysViewsAsync()
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
        public async Task GetSysColumnsAsync()
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
        public async Task GetSysSequencesAsync()
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
        public async Task GetFirstResultSetForCustOrdersOrdersStoredProcedureAsync()
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
        public async Task SpHelpForTableAsync()
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

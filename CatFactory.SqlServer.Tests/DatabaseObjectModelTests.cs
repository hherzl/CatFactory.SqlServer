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

            using var connection = new SqlConnection("server=(local);database=WideWorldImporters;integrated security=yes;");

            // Act

            await connection.OpenAsync();

            var schemas = await connection.GetSysSchemasAsync();

            // Assert

            Assert.True(schemas.Count > 0);
        }

        [Fact]
        public async Task GetSysTypesAsync()
        {
            // Arrange

            using var connection = new SqlConnection("server=(local);database=WideWorldImporters;integrated security=yes;");

            // Act

            await connection.OpenAsync();

            var types = await connection.GetSysTypesAsync();

            // Assert

            Assert.True(types.Count > 0);
        }

        [Fact]
        public async Task GetSysTypesBySchemaIdAsync()
        {
            // Arrange

            using var connection = new SqlConnection("server=(local);database=WideWorldImporters;integrated security=yes;");

            // Act

            await connection.OpenAsync();

            var types = await connection.GetSysTypesAsync(schemaId: 14);

            // Assert

            Assert.True(types.Count > 0);
        }

        [Fact]
        public async Task GetSysTypesDefinedByUserAsync()
        {
            // Arrange

            using var connection = new SqlConnection("server=(local);database=WideWorldImporters;integrated security=yes;");

            // Act

            await connection.OpenAsync();

            var types = await connection.GetSysTypesAsync(isUserDefined: true);

            // Assert

            Assert.True(types.Count == 4);
        }

        [Fact]
        public async Task GetSysTablesAsync()
        {
            // Arrange

            using var connection = new SqlConnection("server=(local);database=WideWorldImporters;integrated security=yes;");

            // Act

            await connection.OpenAsync();

            var tables = await connection.GetSysTablesAsync();

            // Assert

            Assert.True(tables.Count > 0);
        }

        [Fact]
        public async Task GetSysViewsAsync()
        {
            // Arrange

            using var connection = new SqlConnection("server=(local);database=WideWorldImporters;integrated security=yes;");

            // Act

            await connection.OpenAsync();

            var views = await connection.GetSysViewsAsync();

            // Assert

            Assert.True(views.Count > 0);
        }

        [Fact]
        public async Task GetSysColumnsAsync()
        {
            // Arrange

            using var connection = new SqlConnection("server=(local);database=WideWorldImporters;integrated security=yes;");

            // Act

            await connection.OpenAsync();

            var columns = await connection.GetSysColumnsAsync();

            // Assert

            Assert.True(columns.Count > 0);
        }

        [Fact]
        public async Task GetSysSequencesAsync()
        {
            // Arrange

            using var connection = new SqlConnection("server=(local);database=WideWorldImporters;integrated security=yes;");

            // Act

            await connection.OpenAsync();

            var sequences = await connection.GetSysSequencesAsync();

            // Assert

            Assert.True(sequences.Count > 0);
        }

        [Fact]
        public async Task GetFirstResultSetForCustOrdersOrdersStoredProcedureAsync()
        {
            // Arrange

            using var connection = new SqlConnection("server=(local);database=Northwind;integrated security=yes;");

            // Act

            await connection.OpenAsync();

            var firstResultSet = await connection.DmExecDescribeFirstResultSetForObjectAsync("CustOrdersOrders");

            // Assert

            Assert.True(firstResultSet.Count > 0);
        }

        [Fact]
        public async Task SpHelpForTableAsync()
        {
            // Arrange

            using var connection = new SqlConnection("server=(local);database=Northwind;integrated security=yes;");

            // Act

            await connection.OpenAsync();

            var spHelpResult = await connection.SpHelpAsync("Products");

            // Assert

            Assert.False(spHelpResult == null);
        }
    }
}

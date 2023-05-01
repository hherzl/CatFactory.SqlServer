﻿using System.Threading.Tasks;
using CatFactory.SqlServer.CodeFactory;
using CatFactory.SqlServer.Tests.Models;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class ScaffoldingStoredProceduresTests
    {
        private const string OnlineStoreConnectionString = "server=(local); database=OnlineStore; integrated security=yes; TrustServerCertificate=True;";

        [Fact]
        public async Task ScaffoldProceduresFromExistingDatabaseAsync()
        {
            // Arrange
            var database = await SqlServerDatabaseFactory.ImportAsync(OnlineStoreConnectionString);

            // Act
            foreach (var table in database.Tables)
            {
                var codeBuilder = new SqlStoredProcedureCodeBuilder
                {
                    Database = database,
                    Table = table,
                    OutputDirectory = @"C:\Temp\CatFactory.SqlServer\StoredProceduresFromExistingDatabase",
                    ForceOverwrite = true
                };

                codeBuilder.CreateFile();
            }

            // Assert
        }

        [Fact]
        public void ScaffoldProceduresFromMockDatabaseTest()
        {
            // Arrange
            var database = Databases.Blogging;

            // Act
            foreach (var table in database.Tables)
            {
                var codeBuilder = new SqlStoredProcedureCodeBuilder
                {
                    Database = database,
                    Table = table,
                    OutputDirectory = @"C:\Temp\CatFactory.SqlServer\StoredProceduresFromMockingDatabase",
                    ForceOverwrite = true
                };

                codeBuilder.CreateFile();
            }

            // Assert
        }
    }
}

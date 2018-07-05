using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    public static class ExtendPropertyExtensions
    {
        public static IEnumerable<ExtendProperty> GetExtendedPropertiesForDbObject(this DbConnection connection, IDbObject dbObject, string name)
        {
            var repository = new ExtendPropertyRepository();

            var level0type = "schema";
            var level0name = dbObject.Schema;
            var level1type = dbObject.Type;
            var level1name = dbObject.Name;
            var level2type = "default";
            var level2name = "default";

            return repository.GetExtendProperties(connection, name, level0type, level0name, level1type, level1name, level2type, level2name).ToList();
        }

        public static IEnumerable<ExtendProperty> GetExtendedPropertiesForColumn(this DbConnection connection, IDbObject dbObject, Column column, string name)
        {
            var repository = new ExtendPropertyRepository();

            var level0type = "schema";
            var level0name = dbObject.Schema;
            var level1type = dbObject.Type;
            var level1name = dbObject.Name;
            var level2type = "column";
            var level2name = column.Name;

            return repository.GetExtendProperties(connection, name, level0type, level0name, level1type, level1name, level2type, level2name).ToList();
        }

        public static void AddExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, string name, string value)
        {
            var repository = new ExtendPropertyRepository();

            var level0type = "schema";
            var level0name = table.Schema;
            var level1type = "table";
            var level1name = table.Name;
            var level2type = string.Empty;
            var level2name = string.Empty;

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                repository.AddExtendedProperty(connection, name, value, level0type, level0name, level1type, level1name, level2type, level2name);

                table.Description = value;
            }
        }

        public static void AddExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, Column column, string name, string value)
        {
            var repository = new ExtendPropertyRepository();

            var level0type = "schema";
            var level0name = table.Schema;
            var level1type = "table";
            var level1name = table.Name;
            var level2type = "column";
            var level2name = column.Name;

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                repository.AddExtendedProperty(connection, name, value, level0type, level0name, level1type, level1name, level2type, level2name);

                column.Description = value;
            }
        }

        public static void UpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, string name, string value)
        {
            var repository = new ExtendPropertyRepository();

            var level0type = "schema";
            var level0name = table.Schema;
            var level1type = "table";
            var level1name = table.Name;
            var level2type = string.Empty;
            var level2name = string.Empty;

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                repository.UpdateExtendedProperty(connection, name, value, level0type, level0name, level1type, level1name, level2type, level2name);

                table.Description = value;
            }
        }

        public static void UpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, Column column, string name, string value)
        {
            var repository = new ExtendPropertyRepository();

            var level0type = "schema";
            var level0name = table.Schema;
            var level1type = "table";
            var level1name = table.Name;
            var level2type = "column";
            var level2name = column.Name;

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                repository.UpdateExtendedProperty(connection, name, value, level0type, level0name, level1type, level1name, level2type, level2name);

                column.Description = value;
            }
        }

        public static void AddOrUpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, string name, string value)
        {
            var repository = new ExtendPropertyRepository();

            var level0type = "schema";
            var level0name = table.Schema;
            var level1type = "table";
            var level1name = table.Name;
            var level2type = string.Empty;
            var level2name = string.Empty;

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                var extendedProperty = repository.GetExtendProperties(connection, name, level0type, level0name, level1type, level1name, level2type, level2name).FirstOrDefault();

                if (extendedProperty == null)
                    repository.AddExtendedProperty(connection, name, value, level0type, level0name, level1type, level1name, level2type, level2name);
                else
                    repository.UpdateExtendedProperty(connection, name, value, level0type, level0name, level1type, level1name, level2type, level2name);

                table.Description = value;
            }
        }

        public static void AddOrUpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, Column column, string name, string value)
        {
            var repository = new ExtendPropertyRepository();

            var level0type = "schema";
            var level0name = table.Schema;
            var level1type = "table";
            var level1name = table.Name;
            var level2type = "column";
            var level2name = column.Name;

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                var extendedProperty = repository.GetExtendProperties(connection, name, level0type, level0name, level1type, level1name, level2type, level2name).FirstOrDefault();

                if (extendedProperty == null)
                    repository.AddExtendedProperty(connection, name, value, level0type, level0name, level1type, level1name, level2type, level2name);
                else
                    repository.UpdateExtendedProperty(connection, name, value, level0type, level0name, level1type, level1name, level2type, level2name);

                column.Description = value;
            }
        }

        public static void DropExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, string name)
        {
            var repository = new ExtendPropertyRepository();

            var level0type = "schema";
            var level0name = table.Schema;
            var level1type = "table";
            var level1name = table.Name;
            var level2type = string.Empty;
            var level2name = string.Empty;

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                repository.DropExtendedProperty(connection, name, level0type, level0name, level1type, level1name, level2type, level2name);
            }
        }

        public static void DropExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, Column column, string name)
        {
            var repository = new ExtendPropertyRepository();

            var level0type = "schema";
            var level0name = table.Schema;
            var level1type = "table";
            var level1name = table.Name;
            var level2type = "column";
            var level2name = column.Name;

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                repository.DropExtendedProperty(connection, name, level0type, level0name, level1type, level1name, level2type, level2name);
            }
        }
    }
}

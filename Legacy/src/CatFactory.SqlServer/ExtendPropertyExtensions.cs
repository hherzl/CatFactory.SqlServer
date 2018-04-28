using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    public static class ExtendPropertyExtensions
    {
        public static IEnumerable<ExtendProperty> GetMsDescriptionForDbObject(this DbConnection connection, IDbObject dbObject)
        {
            var repository = new ExtendPropertyRepository();

            var name = "'MS_Description'";
            var level0type = "'schema'";
            var level0name = string.Format("'{0}'", dbObject.Schema);
            var level1type = string.Format("'{0}'", dbObject.Type);
            var level1name = string.Format("'{0}'", dbObject.Name);
            var level2type = "default";
            var level2name = "default";

            return repository.GetExtendProperties(connection, name, level0type, level0name, level1type, level1name, level2type, level2name).ToList();
        }

        public static IEnumerable<ExtendProperty> GetMsDescriptionForColumn(this DbConnection connection, IDbObject dbObject, Column column)
        {
            var repository = new ExtendPropertyRepository();

            var name = "'MS_Description'";
            var level0type = "'schema'";
            var level0name = string.Format("'{0}'", dbObject.Schema);
            var level1type = string.Format("'{0}'", dbObject.Type);
            var level1name = string.Format("'{0}'", dbObject.Name);
            var level2type = "'column'";
            var level2name = string.Format("'{0}'", column.Name);

            return repository.GetExtendProperties(connection, name, level0type, level0name, level1type, level1name, level2type, level2name).ToList();
        }

        public static void AddMsDescription(this SqlServerDatabaseFactory databaseFactory, ITable table, string description)
        {
            var repository = new ExtendPropertyRepository();

            var name = "MS_Description";
            var level0type = "schema";
            var level0name = table.Schema;
            var level1type = "table";
            var level1name = table.Name;
            var level2type = string.Empty;
            var level2name = string.Empty;

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                repository.AddExtendedProperty(connection, name, description, level0type, level0name, level1type, level1name, level2type, level2name);

                table.Description = description;
            }
        }

        public static void AddMsDescription(this SqlServerDatabaseFactory databaseFactory, ITable table, Column column, string description)
        {
            var repository = new ExtendPropertyRepository();

            var name = "MS_Description";
            var level0type = "schema";
            var level0name = table.Schema;
            var level1type = "table";
            var level1name = table.Name;
            var level2type = "column";
            var level2name = column.Name;

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                repository.AddExtendedProperty(connection, name, description, level0type, level0name, level1type, level1name, level2type, level2name);

                column.Description = description;
            }
        }

        public static void UpdateMsDescription(this SqlServerDatabaseFactory databaseFactory, ITable table, string description)
        {
            var repository = new ExtendPropertyRepository();

            var name = "MS_Description";
            var level0type = "schema";
            var level0name = table.Schema;
            var level1type = "table";
            var level1name = table.Name;
            var level2type = string.Empty;
            var level2name = string.Empty;

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                repository.UpdateExtendedProperty(connection, name, description, level0type, level0name, level1type, level1name, level2type, level2name);

                table.Description = description;
            }
        }

        public static void UpdateMsDescription(this SqlServerDatabaseFactory databaseFactory, ITable table, Column column, string description)
        {
            var repository = new ExtendPropertyRepository();

            var name = "MS_Description";
            var level0type = "schema";
            var level0name = table.Schema;
            var level1type = "table";
            var level1name = table.Name;
            var level2type = "column";
            var level2name = column.Name;

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                repository.UpdateExtendedProperty(connection, name, description, level0type, level0name, level1type, level1name, level2type, level2name);

                column.Description = description;
            }
        }

        public static void DropMsDescription(this SqlServerDatabaseFactory databaseFactory, ITable table)
        {
            var repository = new ExtendPropertyRepository();

            var name = "MS_Description";
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

        public static void DropMsDescription(this SqlServerDatabaseFactory databaseFactory, ITable table, Column column)
        {
            var repository = new ExtendPropertyRepository();

            var name = "MS_Description";
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

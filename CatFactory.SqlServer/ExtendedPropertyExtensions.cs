using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    public static class ExtendedPropertyExtensions
    {
        public static IEnumerable<ExtendedProperty> GetExtendedPropertiesForDbObject(this DbConnection connection, IDbObject dbObject, string name)
        {
            var model = new ExtendedPropertyModel
            {
                Name = name,
                Level0type = "schema",
                Level0name = dbObject.Schema,
                Level1type = dbObject.Type,
                Level1name = dbObject.Name,
                Level2type = "default",
                Level2name = "default"
            };

            return new ExtendedPropertyRepository().GetExtendedProperties(connection, model).ToList();
        }

        public static IEnumerable<ExtendedProperty> GetExtendedPropertiesForColumn(this DbConnection connection, IDbObject dbObject, Column column, string name)
        {
            var model = new ExtendedPropertyModel
            {
                Name = name,
                Level0type = "schema",
                Level0name = dbObject.Schema,
                Level1type = dbObject.Type,
                Level1name = dbObject.Name,
                Level2type = "column",
                Level2name = column.Name
            };

            return new ExtendedPropertyRepository().GetExtendedProperties(connection, model).ToList();
        }

        public static void AddExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, string name, string value)
        {
            var model = new ExtendedPropertyModel
            {
                Name = name,
                Value = value,
                Level0type = "schema",
                Level0name = table.Schema,
                Level1type = "table",
                Level1name = table.Name,
                Level2type = string.Empty,
                Level2name = string.Empty
            };

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                new ExtendedPropertyRepository().AddExtendedProperty(connection, model);

                table.Description = value;
            }
        }

        public static void AddExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, Column column, string name, string value)
        {
            var model = new ExtendedPropertyModel
            {
                Name = name,
                Value = value,
                Level0type = "schema",
                Level0name = table.Schema,
                Level1type = "table",
                Level1name = table.Name,
                Level2type = "column",
                Level2name = column.Name
            };

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                new ExtendedPropertyRepository().AddExtendedProperty(connection, model);

                column.Description = value;
            }
        }

        public static void UpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, string name, string value)
        {
            var model = new ExtendedPropertyModel
            {
                Name = name,
                Value = value,
                Level0type = "schema",
                Level0name = table.Schema,
                Level1type = "table",
                Level1name = table.Name,
                Level2type = string.Empty,
                Level2name = string.Empty
            };

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                new ExtendedPropertyRepository().UpdateExtendedProperty(connection, model);

                table.Description = value;
            }
        }

        public static void UpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, Column column, string name, string value)
        {
            var model = new ExtendedPropertyModel
            {
                Name = name,
                Value = value,
                Level0type = "schema",
                Level0name = table.Schema,
                Level1type = "table",
                Level1name = table.Name,
                Level2type = "column",
                Level2name = column.Name
            };

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                new ExtendedPropertyRepository().UpdateExtendedProperty(connection, model);

                column.Description = value;
            }
        }

        public static void AddOrUpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, string name, string value)
        {
            var model = new ExtendedPropertyModel
            {
                Name = name,
                Value = value,
                Level0type = "schema",
                Level0name = table.Schema,
                Level1type = "table",
                Level1name = table.Name,
                Level2type = string.Empty,
                Level2name = string.Empty
            };

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                var repository = new ExtendedPropertyRepository();

                var extendedProperty = repository.GetExtendedProperties(connection, model).FirstOrDefault();

                if (extendedProperty == null)
                    repository.AddExtendedProperty(connection, model);
                else
                    repository.UpdateExtendedProperty(connection, model);

                table.Description = value;
            }
        }

        public static void AddOrUpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, Column column, string name, string value)
        {
            var model = new ExtendedPropertyModel
            {
                Name = name,
                Value = value,
                Level0type = "schema",
                Level0name = table.Schema,
                Level1type = "table",
                Level1name = table.Name,
                Level2type = "column",
                Level2name = column.Name
            };

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                var repository = new ExtendedPropertyRepository();

                var extendedProperty = repository.GetExtendedProperties(connection, model).FirstOrDefault();

                if (extendedProperty == null)
                    repository.AddExtendedProperty(connection, model);
                else
                    repository.UpdateExtendedProperty(connection, model);

                column.Description = value;
            }
        }

        public static void DropExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, string name)
        {
            var model = new ExtendedPropertyModel
            {
                Name = name,
                Level0type = "schema",
                Level0name = table.Schema,
                Level1type = "table",
                Level1name = table.Name,
                Level2type = string.Empty,
                Level2name = string.Empty
            };

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                new ExtendedPropertyRepository().DropExtendedProperty(connection, model);
            }
        }

        public static void DropExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, Column column, string name)
        {
            var model = new ExtendedPropertyModel
            {
                Name = name,
                Level0type = "schema",
                Level0name = table.Schema,
                Level1type = "table",
                Level1name = table.Name,
                Level2type = "column",
                Level2name = column.Name
            };

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                new ExtendedPropertyRepository().DropExtendedProperty(connection, model);
            }
        }

        public static void AddOrUpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, IView view, string name, string value)
        {
            var model = new ExtendedPropertyModel
            {
                Name = name,
                Value = value,
                Level0type = "schema",
                Level0name = view.Schema,
                Level1type = "view",
                Level1name = view.Name,
                Level2type = string.Empty,
                Level2name = string.Empty
            };

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                var repository = new ExtendedPropertyRepository();

                var extendedProperty = repository.GetExtendedProperties(connection, model).FirstOrDefault();

                if (extendedProperty == null)
                    repository.AddExtendedProperty(connection, model);
                else
                    repository.UpdateExtendedProperty(connection, model);

                view.Description = value;
            }
        }

        public static void AddOrUpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, IView view, Column column, string name, string value)
        {
            var model = new ExtendedPropertyModel
            {
                Name = name,
                Value = value,
                Level0type = "schema",
                Level0name = view.Schema,
                Level1type = "view",
                Level1name = view.Name,
                Level2type = "column",
                Level2name = column.Name
            };

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                var repository = new ExtendedPropertyRepository();

                var extendedProperty = repository.GetExtendedProperties(connection, model).FirstOrDefault();

                if (extendedProperty == null)
                    repository.AddExtendedProperty(connection, model);
                else
                    repository.UpdateExtendedProperty(connection, model);

                column.Description = value;
            }
        }
    }
}

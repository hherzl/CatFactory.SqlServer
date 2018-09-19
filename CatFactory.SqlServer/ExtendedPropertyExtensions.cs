using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExtendedPropertyExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IEnumerable<ExtendedProperty> GetExtendedPropertiesForDbObject(this DbConnection connection, string name)
        {
            var model = new ExtendedProperty
            {
                Name = name,
                Level0Type = "",
                Level0Name = "",
                Level1Type = "",
                Level1Name = "",
                Level2Type = "",
                Level2Name = ""
            };

            return new ExtendedPropertyRepository(connection).GetExtendedProperties(model).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="dbObject"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IEnumerable<ExtendedProperty> GetExtendedPropertiesForDbObject(this DbConnection connection, IDbObject dbObject, string name)
        {
            var model = new ExtendedProperty
            {
                Name = name,
                Level0Type = "schema",
                Level0Name = dbObject.Schema,
                Level1Type = dbObject.Type,
                Level1Name = dbObject.Name,
                Level2Type = "default",
                Level2Name = "default"
            };

            return new ExtendedPropertyRepository(connection).GetExtendedProperties(model).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="dbObject"></param>
        /// <param name="column"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IEnumerable<ExtendedProperty> GetExtendedPropertiesForColumn(this DbConnection connection, IDbObject dbObject, Column column, string name)
        {
            var model = new ExtendedProperty
            {
                Name = name,
                Level0Type = "schema",
                Level0Name = dbObject.Schema,
                Level1Type = dbObject.Type,
                Level1Name = dbObject.Name,
                Level2Type = "column",
                Level2Name = column.Name
            };

            return new ExtendedPropertyRepository(connection).GetExtendedProperties(model).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseFactory"></param>
        /// <param name="table"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void AddExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, string name, string value)
        {
            var model = new ExtendedProperty
            {
                Name = name,
                Value = value,
                Level0Type = "schema",
                Level0Name = table.Schema,
                Level1Type = "table",
                Level1Name = table.Name,
                Level2Type = "",
                Level2Name = ""
            };

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                new ExtendedPropertyRepository(connection).AddExtendedProperty(model);

                table.Description = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseFactory"></param>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void AddExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, Column column, string name, string value)
        {
            var model = new ExtendedProperty
            {
                Name = name,
                Value = value,
                Level0Type = "schema",
                Level0Name = table.Schema,
                Level1Type = "table",
                Level1Name = table.Name,
                Level2Type = "column",
                Level2Name = column.Name
            };

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                new ExtendedPropertyRepository(connection).AddExtendedProperty(model);

                column.Description = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseFactory"></param>
        /// <param name="table"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void UpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, string name, string value)
        {
            var model = new ExtendedProperty
            {
                Name = name,
                Value = value,
                Level0Type = "schema",
                Level0Name = table.Schema,
                Level1Type = "table",
                Level1Name = table.Name,
                Level2Type = "",
                Level2Name = ""
            };

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                new ExtendedPropertyRepository(connection).UpdateExtendedProperty(model);

                table.Description = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseFactory"></param>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void UpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, Column column, string name, string value)
        {
            var model = new ExtendedProperty
            {
                Name = name,
                Value = value,
                Level0Type = "schema",
                Level0Name = table.Schema,
                Level1Type = "table",
                Level1Name = table.Name,
                Level2Type = "column",
                Level2Name = column.Name
            };

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                new ExtendedPropertyRepository(connection).UpdateExtendedProperty(model);

                column.Description = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseFactory"></param>
        /// <param name="table"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void AddOrUpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, string name, string value)
        {
            var model = new ExtendedProperty
            {
                Name = name,
                Value = value,
                Level0Type = "schema",
                Level0Name = table.Schema,
                Level1Type = "table",
                Level1Name = table.Name,
                Level2Type = "",
                Level2Name = ""
            };

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                var repository = new ExtendedPropertyRepository(connection);

                var extendedProperty = repository.GetExtendedProperties(model).FirstOrDefault();

                if (extendedProperty == null)
                    repository.AddExtendedProperty(model);
                else
                    repository.UpdateExtendedProperty(model);

                table.Description = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseFactory"></param>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void AddOrUpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, Column column, string name, string value)
        {
            var model = new ExtendedProperty
            {
                Name = name,
                Value = value,
                Level0Type = "schema",
                Level0Name = table.Schema,
                Level1Type = "table",
                Level1Name = table.Name,
                Level2Type = "column",
                Level2Name = column.Name
            };

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                var repository = new ExtendedPropertyRepository(connection);

                var extendedProperty = repository.GetExtendedProperties(model).FirstOrDefault();

                if (extendedProperty == null)
                    repository.AddExtendedProperty(model);
                else
                    repository.UpdateExtendedProperty(model);

                column.Description = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseFactory"></param>
        /// <param name="table"></param>
        /// <param name="name"></param>
        public static void DropExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, string name)
        {
            var model = new ExtendedProperty
            {
                Name = name,
                Level0Type = "schema",
                Level0Name = table.Schema,
                Level1Type = "table",
                Level1Name = table.Name,
                Level2Type = "",
                Level2Name = ""
            };

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                new ExtendedPropertyRepository(connection).DropExtendedProperty(model);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseFactory"></param>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <param name="name"></param>
        public static void DropExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, Column column, string name)
        {
            var model = new ExtendedProperty
            {
                Name = name,
                Level0Type = "schema",
                Level0Name = table.Schema,
                Level1Type = "table",
                Level1Name = table.Name,
                Level2Type = "column",
                Level2Name = column.Name
            };

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                new ExtendedPropertyRepository(connection).DropExtendedProperty(model);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseFactory"></param>
        /// <param name="view"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void AddOrUpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, IView view, string name, string value)
        {
            var model = new ExtendedProperty
            {
                Name = name,
                Value = value,
                Level0Type = "schema",
                Level0Name = view.Schema,
                Level1Type = "view",
                Level1Name = view.Name,
                Level2Type = "",
                Level2Name = ""
            };

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                var repository = new ExtendedPropertyRepository(connection);

                var extendedProperty = repository.GetExtendedProperties(model).FirstOrDefault();

                if (extendedProperty == null)
                    repository.AddExtendedProperty(model);
                else
                    repository.UpdateExtendedProperty(model);

                view.Description = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseFactory"></param>
        /// <param name="view"></param>
        /// <param name="column"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void AddOrUpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, IView view, Column column, string name, string value)
        {
            var model = new ExtendedProperty
            {
                Name = name,
                Value = value,
                Level0Type = "schema",
                Level0Name = view.Schema,
                Level1Type = "view",
                Level1Name = view.Name,
                Level2Type = "column",
                Level2Name = column.Name
            };

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                var repository = new ExtendedPropertyRepository(connection);

                var extendedProperty = repository.GetExtendedProperties(model).FirstOrDefault();

                if (extendedProperty == null)
                    repository.AddExtendedProperty(model);
                else
                    repository.UpdateExtendedProperty(model);

                column.Description = value;
            }
        }
    }
}

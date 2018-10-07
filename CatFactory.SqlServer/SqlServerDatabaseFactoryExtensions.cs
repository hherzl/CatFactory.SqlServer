using System.Linq;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// 
    /// </summary>
    public static class SqlServerDatabaseFactoryExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseFactory"></param>
        /// <param name="database"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void AddExtendedProperty(this SqlServerDatabaseFactory databaseFactory, Database database, string name, string value)
        {
            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                new ExtendedPropertyRepository(connection).AddExtendedProperty(new ExtendedProperty { Name = name, Value = value });

                database.Description = value;
            }
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
                Level1Name = table.Name
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
        /// <param name="view"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void AddExtendedProperty(this SqlServerDatabaseFactory databaseFactory, IView view, string name, string value)
        {
            var model = new ExtendedProperty
            {
                Name = name,
                Value = value,
                Level0Type = "schema",
                Level0Name = view.Schema,
                Level1Type = "view",
                Level1Name = view.Name
            };

            using (var connection = databaseFactory.GetConnection())
            {
                connection.Open();

                new ExtendedPropertyRepository(connection).AddExtendedProperty(model);

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
        public static void AddExtendedProperty(this SqlServerDatabaseFactory databaseFactory, IView view, Column column, string name, string value)
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
                Level1Name = table.Name
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
                Level1Name = table.Name
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
                Level1Name = view.Name
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseFactory"></param>
        /// <param name="database"></param>
        /// <param name="name"></param>
        public static void DropExtendedProperty(this SqlServerDatabaseFactory databaseFactory, Database database, string name)
        {
            var model = new ExtendedProperty
            {
                Name = name
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
        /// <param name="name"></param>
        public static void DropExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, string name)
        {
            var model = new ExtendedProperty
            {
                Name = name,
                Level0Type = "schema",
                Level0Name = table.Schema,
                Level1Type = "table",
                Level1Name = table.Name
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
        public static void DropExtendedProperty(this SqlServerDatabaseFactory databaseFactory, IView view, string name)
        {
            var model = new ExtendedProperty
            {
                Name = name,
                Level0Type = "schema",
                Level0Name = view.Schema,
                Level1Type = "view",
                Level1Name = view.Name
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
        /// <param name="column"></param>
        /// <param name="name"></param>
        public static void DropExtendedProperty(this SqlServerDatabaseFactory databaseFactory, IView view, Column column, string name)
        {
            var model = new ExtendedProperty
            {
                Name = name,
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

                new ExtendedPropertyRepository(connection).DropExtendedProperty(model);
            }
        }
    }
}

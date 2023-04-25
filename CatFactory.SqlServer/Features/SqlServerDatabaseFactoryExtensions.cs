using System.Linq;
using CatFactory.ObjectRelationalMapping;
using CatFactory.SqlServer.DatabaseObjectModel;

namespace CatFactory.SqlServer.Features
{
    /// <summary>
    /// Contains extension methods for <see cref="SqlServerDatabaseFactory"/> class to allow perform CRUD operations for extended properties
    /// </summary>
    public static class SqlServerDatabaseFactoryExtensions
    {
#pragma warning disable CS1591
        public const string SCHEMA = "schema";
        public const string TABLE = "table";
        public const string VIEW = "view";
        public const string COLUMN = "column";
#pragma warning restore CS1591

        /// <summary>
        /// Adds an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="database">Instance of <see cref="Database"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <param name="value">Extended property value</param>
        public static void AddExtendedProperty(this SqlServerDatabaseFactory databaseFactory, Database database, string name, string value)
        {
            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var repository = new ExtendedPropertyRepository(connection);

            repository.Add(new ExtendedProperty(name, value));

            database.Description = value;
        }

        /// <summary>
        /// Adds an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <param name="value">Extended property value</param>
        public static void AddExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, string name, string value)
        {
            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var repository = new ExtendedPropertyRepository(connection);

            repository.Add(new ExtendedProperty(name, SCHEMA, table.Schema, TABLE, table.Name)
            {
                Value = value
            });

            table.Description = value;
        }

        /// <summary>
        /// Adds an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <param name="column">Instance of <see cref="Column"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <param name="value">Extended property value</param>
        public static void AddExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, Column column, string name, string value)
        {
            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var repository = new ExtendedPropertyRepository(connection);

            repository.Add(new ExtendedProperty(name, SCHEMA, table.Schema, TABLE, table.Name, COLUMN, column.Name)
            {
                Value = value
            });

            column.Description = value;
        }

        /// <summary>
        /// Adds an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="view">Instance of <see cref="View"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <param name="value">Extended property value</param>
        public static void AddExtendedProperty(this SqlServerDatabaseFactory databaseFactory, IView view, string name, string value)
        {
            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var repository = new ExtendedPropertyRepository(connection);

            repository.Add(new ExtendedProperty(name, SCHEMA, view.Schema, VIEW, view.Name)
            {
                Value = value
            });

            view.Description = value;
        }

        /// <summary>
        /// Adds an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="view">Instance of <see cref="View"/> class</param>
        /// <param name="column">Instance of <see cref="Column"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <param name="value">Extended property value</param>
        public static void AddExtendedProperty(this SqlServerDatabaseFactory databaseFactory, IView view, Column column, string name, string value)
        {
            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var repository = new ExtendedPropertyRepository(connection);

            repository.Add(new ExtendedProperty(name, SCHEMA, view.Schema, VIEW, view.Name, COLUMN, column.Name)
            {
                Value = value
            });

            column.Description = value;
        }

        /// <summary>
        /// Updates an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="database"></param>
        /// <param name="name">Extended property name</param>
        /// <param name="value">Extended property value</param>
        public static void UpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, Database database, string name, string value)
        {
            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var repository = new ExtendedPropertyRepository(connection);

            repository.Update(new ExtendedProperty(name, value));

            database.Description = value;
        }

        /// <summary>
        /// Updates an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <param name="value">Extended property value</param>
        public static void UpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, string name, string value)
        {
            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var repository = new ExtendedPropertyRepository(connection);

            repository.Update(new ExtendedProperty(name, SCHEMA, table.Schema, TABLE, table.Name)
            {
                Value = value
            });

            table.Description = value;
        }

        /// <summary>
        /// Updates an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <param name="column">Instance of <see cref="Column"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <param name="value">Extended property value</param>
        public static void UpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, Column column, string name, string value)
        {
            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var repository = new ExtendedPropertyRepository(connection);

            new ExtendedPropertyRepository(connection).Update(new ExtendedProperty(name, SCHEMA, table.Schema, TABLE, table.Name, COLUMN, column.Name)
            {
                Value = value
            });

            column.Description = value;
        }

        /// <summary>
        /// Updates an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="view">Instance of <see cref="View"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <param name="value">Extended property value</param>
        public static void UpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, IView view, string name, string value)
        {
            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var repository = new ExtendedPropertyRepository(connection);

            repository.Update(new ExtendedProperty(name, SCHEMA, view.Schema, VIEW, view.Name)
            {
                Value = value
            });

            view.Description = value;
        }

        /// <summary>
        /// Updates an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="view">Instance of <see cref="View"/> class</param>
        /// <param name="column">Instance of <see cref="Column"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <param name="value">Extended property value</param>
        public static void UpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, IView view, Column column, string name, string value)
        {
            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var repository = new ExtendedPropertyRepository(connection);

            repository.Update(new ExtendedProperty(name, SCHEMA, view.Schema, VIEW, view.Name, COLUMN, column.Name)
            {
                Value = value
            });

            column.Description = value;
        }

        /// <summary>
        /// Adds if not exists or updates if exists an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="database">Instance of <see cref="Database"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <param name="value">Extended property value</param>
        public static void AddOrUpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, Database database, string name, string value)
        {
            var model = new ExtendedProperty(name, value);

            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var repository = new ExtendedPropertyRepository(connection);

            var extendedProperty = repository.Get(model).FirstOrDefault();

            if (extendedProperty == null)
                repository.Add(model);
            else
                repository.Update(model);

            database.Description = value;
        }

        /// <summary>
        /// Adds if not exists or updates if exists an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <param name="value">Extended property value</param>
        public static void AddOrUpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, string name, string value)
        {
            var model = new ExtendedProperty(name, SCHEMA, table.Schema, TABLE, table.Name)
            {
                Value = value
            };

            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var repository = new ExtendedPropertyRepository(connection);

            var extendedProperty = repository.Get(model).FirstOrDefault();

            if (extendedProperty == null)
                repository.Add(model);
            else
                repository.Update(model);

            table.Description = value;
        }

        /// <summary>
        /// Adds if not exists or updates if exists an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <param name="column">Instance of <see cref="Column"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <param name="value">Extended property value</param>
        public static void AddOrUpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, Column column, string name, string value)
        {
            var model = new ExtendedProperty(name, SCHEMA, table.Schema, TABLE, table.Name, COLUMN, column.Name)
            {
                Value = value
            };

            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var repository = new ExtendedPropertyRepository(connection);

            var extendedProperty = repository.Get(model).FirstOrDefault();

            if (extendedProperty == null)
                repository.Add(model);
            else
                repository.Update(model);

            column.Description = value;
        }

        /// <summary>
        /// Adds if not exists or updates if exists an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="view">Instance of <see cref="View"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <param name="value">Extended property value</param>
        public static void AddOrUpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, IView view, string name, string value)
        {
            var model = new ExtendedProperty(name, SCHEMA, view.Schema, VIEW, view.Name)
            {
                Value = value
            };

            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var repository = new ExtendedPropertyRepository(connection);

            var extendedProperty = repository.Get(model).FirstOrDefault();

            if (extendedProperty == null)
                repository.Add(model);
            else
                repository.Update(model);

            view.Description = value;
        }

        /// <summary>
        /// Adds if not exists or updates if exists an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="view">Instance of <see cref="View"/> class</param>
        /// <param name="column">Instance of <see cref="Column"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <param name="value">Extended property value</param>
        public static void AddOrUpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, IView view, Column column, string name, string value)
        {
            var model = new ExtendedProperty(name, SCHEMA, view.Schema, VIEW, view.Name, COLUMN, column.Name)
            {
                Value = value
            };

            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var repository = new ExtendedPropertyRepository(connection);

            var extendedProperty = repository.Get(model).FirstOrDefault();

            if (extendedProperty == null)
                repository.Add(model);
            else
                repository.Update(model);

            column.Description = value;
        }

        /// <summary>
        /// Drops an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="database"></param>
        /// <param name="name">Extended property name</param>
        public static void DropExtendedProperty(this SqlServerDatabaseFactory databaseFactory, Database database, string name)
        {
            using var connection = databaseFactory.GetConnection();

            connection.Open();

            new ExtendedPropertyRepository(connection).Drop(new ExtendedProperty(name));
        }

        /// <summary>
        /// Drops an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <param name="name">Extended property name</param>
        public static void DropExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, string name)
        {
            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var repository = new ExtendedPropertyRepository(connection);

            repository.Drop(new ExtendedProperty(name, SCHEMA, table.Schema, TABLE, table.Name));
        }

        /// <summary>
        /// Drops an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <param name="column">Instance of <see cref="Column"/> class</param>
        /// <param name="name">Extended property name</param>
        public static void DropExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, Column column, string name)
        {
            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var repository = new ExtendedPropertyRepository(connection);

            repository.Drop(new ExtendedProperty(name, SCHEMA, table.Schema, TABLE, table.Name, COLUMN, column.Name));
        }

        /// <summary>
        /// Drops an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="view">Instance of <see cref="View"/> class</param>
        /// <param name="name">Extended property name</param>
        public static void DropExtendedProperty(this SqlServerDatabaseFactory databaseFactory, IView view, string name)
        {
            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var repository = new ExtendedPropertyRepository(connection);

            repository.Drop(new ExtendedProperty(name, SCHEMA, view.Schema, VIEW, view.Name));
        }

        /// <summary>
        /// Drops an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="view">Instance of <see cref="View"/> class</param>
        /// <param name="column">Instance of <see cref="Column"/> class</param>
        /// <param name="name">Extended property name</param>
        public static void DropExtendedProperty(this SqlServerDatabaseFactory databaseFactory, IView view, Column column, string name)
        {
            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var repository = new ExtendedPropertyRepository(connection);

            repository.Drop(new ExtendedProperty(name, SCHEMA, view.Schema, VIEW, view.Name, COLUMN, column.Name));
        }

        /// <summary>
        /// Drops an extended property for database object if exists
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="name">Extended property name</param>
        public static void DropExtendedPropertyIfExists(this SqlServerDatabaseFactory databaseFactory, string name)
        {
            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var extendedProperty = connection.GetExtendedProperties(name).FirstOrDefault();

            if (extendedProperty != null)
                new ExtendedPropertyRepository(connection).Drop(new ExtendedProperty(name));
        }

        /// <summary>
        /// Drops an extended property for database object if exists
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <param name="name">Extended property name</param>
        public static void DropExtendedPropertyIfExists(this SqlServerDatabaseFactory databaseFactory, ITable table, string name)
        {
            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var extendedProperty = connection.GetExtendedProperties(table, name).FirstOrDefault();

            if (extendedProperty != null)
                new ExtendedPropertyRepository(connection).Drop(new ExtendedProperty(name, SCHEMA, table.Schema, TABLE, table.Name));
        }

        /// <summary>
        /// Drops an extended property for database object if exists
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <param name="column">Instance of <see cref="Column"/> class</param>
        /// <param name="name">Extended property name</param>
        public static void DropExtendedPropertyIfExists(this SqlServerDatabaseFactory databaseFactory, ITable table, Column column, string name)
        {
            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var extendedProperty = connection.GetExtendedProperties(table, column, name).FirstOrDefault();

            if (extendedProperty != null)
                new ExtendedPropertyRepository(connection).Drop(new ExtendedProperty(name, SCHEMA, table.Schema, TABLE, table.Name, COLUMN, column.Name));
        }

        /// <summary>
        /// Drops an extended property for database object if exists
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="view">Instance of <see cref="View"/> class</param>
        /// <param name="name">Extended property name</param>
        public static void DropExtendedPropertyIfExists(this SqlServerDatabaseFactory databaseFactory, IView view, string name)
        {
            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var extendedProperty = connection.GetExtendedProperties(view, name).FirstOrDefault();

            if (extendedProperty != null)
                new ExtendedPropertyRepository(connection).Drop(new ExtendedProperty(name, SCHEMA, view.Schema, VIEW, view.Name));
        }

        /// <summary>
        /// Drops an extended property for database object if exists
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="view">Instance of <see cref="View"/> class</param>
        /// <param name="column">Instance of <see cref="Column"/> class</param>
        /// <param name="name">Extended property name</param>
        public static void DropExtendedPropertyIfExists(this SqlServerDatabaseFactory databaseFactory, IView view, Column column, string name)
        {
            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var extendedProperty = connection.GetExtendedProperties(view, name).FirstOrDefault();

            if (extendedProperty != null)
                new ExtendedPropertyRepository(connection).Drop(new ExtendedProperty(name, SCHEMA, view.Schema, VIEW, view.Name, COLUMN, column.Name));
        }
    }
}

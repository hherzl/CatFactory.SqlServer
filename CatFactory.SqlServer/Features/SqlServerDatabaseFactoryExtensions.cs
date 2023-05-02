using System.Linq;
using System.Threading.Tasks;
using CatFactory.ObjectRelationalMapping;
using CatFactory.SqlServer.DatabaseObjectModel;

using token = CatFactory.SqlServer.SqlServerToken;

namespace CatFactory.SqlServer.Features
{
    /// <summary>
    /// Contains extension methods for <see cref="SqlServerDatabaseFactory"/> class to allow perform CRUD operations for extended properties
    /// </summary>
    public static class SqlServerDatabaseFactoryExtensions
    {
        /// <summary>
        /// Adds an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="database">Instance of <see cref="Database"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <param name="value">Extended property value</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task AddExtendedProperty(this SqlServerDatabaseFactory databaseFactory, Database database, string name, string value)
        {
            using var connection = databaseFactory.GetConnection();

            await connection.OpenAsync();

            var repository = new ExtendedPropertyRepository(connection);

            await repository.AddAsync(new ExtendedProperty(name, value));

            database.Description = value;
        }

        /// <summary>
        /// Adds an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <param name="value">Extended property value</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task AddExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, string name, string value)
        {
            using var connection = databaseFactory.GetConnection();

            await connection.OpenAsync();

            var repository = new ExtendedPropertyRepository(connection);

            await repository.AddAsync(new ExtendedProperty(name, token.SCHEMA, table.Schema, token.TABLE, table.Name)
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
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task AddExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, Column column, string name, string value)
        {
            using var connection = databaseFactory.GetConnection();

            await connection.OpenAsync();

            var repository = new ExtendedPropertyRepository(connection);

            await repository.AddAsync(new ExtendedProperty(name, token.SCHEMA, table.Schema, token.TABLE, table.Name, token.COLUMN, column.Name)
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
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task AddExtendedProperty(this SqlServerDatabaseFactory databaseFactory, IView view, string name, string value)
        {
            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var repository = new ExtendedPropertyRepository(connection);

            await repository.AddAsync(new ExtendedProperty(name, token.SCHEMA, view.Schema, token.VIEW, view.Name)
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
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task AddExtendedProperty(this SqlServerDatabaseFactory databaseFactory, IView view, Column column, string name, string value)
        {
            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var repository = new ExtendedPropertyRepository(connection);

            await repository.AddAsync(new ExtendedProperty(name, token.SCHEMA, view.Schema, token.VIEW, view.Name, token.COLUMN, column.Name)
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
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task UpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, Database database, string name, string value)
        {
            using var connection = databaseFactory.GetConnection();

            await connection.OpenAsync();

            var repository = new ExtendedPropertyRepository(connection);

            await repository.UpdateAsync(new ExtendedProperty(name, value));

            database.Description = value;
        }

        /// <summary>
        /// Updates an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <param name="value">Extended property value</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task UpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, string name, string value)
        {
            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var repository = new ExtendedPropertyRepository(connection);

            await repository.UpdateAsync(new ExtendedProperty(name, token.SCHEMA, table.Schema, token.TABLE, table.Name)
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
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task UpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, Column column, string name, string value)
        {
            using var connection = databaseFactory.GetConnection();

            connection.Open();

            var repository = new ExtendedPropertyRepository(connection);

            await repository.UpdateAsync(new ExtendedProperty(name, token.SCHEMA, table.Schema, token.TABLE, table.Name, token.COLUMN, column.Name)
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
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task UpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, IView view, string name, string value)
        {
            using var connection = databaseFactory.GetConnection();

            await connection.OpenAsync();

            var repository = new ExtendedPropertyRepository(connection);

            await repository.UpdateAsync(new ExtendedProperty(name, token.SCHEMA, view.Schema, token.VIEW, view.Name)
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
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task UpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, IView view, Column column, string name, string value)
        {
            using var connection = databaseFactory.GetConnection();

            await connection.OpenAsync();

            var repository = new ExtendedPropertyRepository(connection);

            await repository.UpdateAsync(new ExtendedProperty(name, token.SCHEMA, view.Schema, token.VIEW, view.Name, token.COLUMN, column.Name)
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
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task AddOrUpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, Database database, string name, string value)
        {
            var model = new ExtendedProperty(name, value);

            using var connection = databaseFactory.GetConnection();

            await connection.OpenAsync();

            var repository = new ExtendedPropertyRepository(connection);

            var extendedProperty = (await repository.GetAsync(model)).FirstOrDefault();

            if (extendedProperty == null)
                await repository.AddAsync(model);
            else
                await repository.UpdateAsync(model);

            database.Description = value;
        }

        /// <summary>
        /// Adds if not exists or updates if exists an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <param name="value">Extended property value</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task AddOrUpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, string name, string value)
        {
            var model = new ExtendedProperty(name, token.SCHEMA, table.Schema, token.TABLE, table.Name)
            {
                Value = value
            };

            using var connection = databaseFactory.GetConnection();

            await connection.OpenAsync();

            var repository = new ExtendedPropertyRepository(connection);

            var extendedProperty = (await repository.GetAsync(model)).FirstOrDefault();

            if (extendedProperty == null)
                await repository.AddAsync(model);
            else
                await repository.UpdateAsync(model);

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
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task AddOrUpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, Column column, string name, string value)
        {
            var model = new ExtendedProperty(name, token.SCHEMA, table.Schema, token.TABLE, table.Name, token.COLUMN, column.Name)
            {
                Value = value
            };

            using var connection = databaseFactory.GetConnection();

            await connection.OpenAsync();

            var repository = new ExtendedPropertyRepository(connection);

            var extendedProperty = (await repository.GetAsync(model)).FirstOrDefault();

            if (extendedProperty == null)
                await repository.AddAsync(model);
            else
                await repository.UpdateAsync(model);

            column.Description = value;
        }

        /// <summary>
        /// Adds if not exists or updates if exists an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="view">Instance of <see cref="View"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <param name="value">Extended property value</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task AddOrUpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, IView view, string name, string value)
        {
            var model = new ExtendedProperty(name, token.SCHEMA, view.Schema, token.VIEW, view.Name)
            {
                Value = value
            };

            using var connection = databaseFactory.GetConnection();

            await connection.OpenAsync();

            var repository = new ExtendedPropertyRepository(connection);

            var extendedProperty = (await repository.GetAsync(model)).FirstOrDefault();

            if (extendedProperty == null)
                await repository.AddAsync(model);
            else
                await repository.UpdateAsync(model);

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
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task AddOrUpdateExtendedProperty(this SqlServerDatabaseFactory databaseFactory, IView view, Column column, string name, string value)
        {
            var model = new ExtendedProperty(name, token.SCHEMA, view.Schema, token.VIEW, view.Name, token.COLUMN, column.Name)
            {
                Value = value
            };

            using var connection = databaseFactory.GetConnection();

            await connection.OpenAsync();

            var repository = new ExtendedPropertyRepository(connection);

            var extendedProperty = (await repository.GetAsync(model)).FirstOrDefault();

            if (extendedProperty == null)
                await repository.AddAsync(model);
            else
                await repository.UpdateAsync(model);

            column.Description = value;
        }

        /// <summary>
        /// Drops an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="database">Instance of <see cref="Database"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task DropExtendedProperty(this SqlServerDatabaseFactory databaseFactory, Database database, string name)
        {
            using var connection = databaseFactory.GetConnection();

            await connection.OpenAsync();

            await new ExtendedPropertyRepository(connection).DropAsync(new ExtendedProperty(name));
        }

        /// <summary>
        /// Drops an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task DropExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, string name)
        {
            using var connection = databaseFactory.GetConnection();

            await connection.OpenAsync();

            var repository = new ExtendedPropertyRepository(connection);

            await repository.DropAsync(new ExtendedProperty(name, token.SCHEMA, table.Schema, token.TABLE, table.Name));
        }

        /// <summary>
        /// Drops an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <param name="column">Instance of <see cref="Column"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task DropExtendedProperty(this SqlServerDatabaseFactory databaseFactory, ITable table, Column column, string name)
        {
            using var connection = databaseFactory.GetConnection();

            await connection.OpenAsync();

            var repository = new ExtendedPropertyRepository(connection);

            await repository.DropAsync(new ExtendedProperty(name, token.SCHEMA, table.Schema, token.TABLE, table.Name, token.COLUMN, column.Name));
        }

        /// <summary>
        /// Drops an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="view">Instance of <see cref="View"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task DropExtendedProperty(this SqlServerDatabaseFactory databaseFactory, IView view, string name)
        {
            using var connection = databaseFactory.GetConnection();

            await connection.OpenAsync();

            var repository = new ExtendedPropertyRepository(connection);

            await repository.DropAsync(new ExtendedProperty(name, token.SCHEMA, view.Schema, token.VIEW, view.Name));
        }

        /// <summary>
        /// Drops an extended property for database object
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="view">Instance of <see cref="View"/> class</param>
        /// <param name="column">Instance of <see cref="Column"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task DropExtendedProperty(this SqlServerDatabaseFactory databaseFactory, IView view, Column column, string name)
        {
            using var connection = databaseFactory.GetConnection();

            await connection.OpenAsync();

            var repository = new ExtendedPropertyRepository(connection);

            await repository.DropAsync(new ExtendedProperty(name, token.SCHEMA, view.Schema, token.VIEW, view.Name, token.COLUMN, column.Name));
        }

        /// <summary>
        /// Drops an extended property for database object if exists
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task DropExtendedPropertyIfExists(this SqlServerDatabaseFactory databaseFactory, string name)
        {
            using var connection = databaseFactory.GetConnection();

            await connection.OpenAsync();

            var extendedProperty = (await connection.GetExtendedProperties(name)).FirstOrDefault();

            var repository = new ExtendedPropertyRepository(connection);

            if (extendedProperty != null)
                await repository.DropAsync(new ExtendedProperty(name));
        }

        /// <summary>
        /// Drops an extended property for database object if exists
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task DropExtendedPropertyIfExists(this SqlServerDatabaseFactory databaseFactory, ITable table, string name)
        {
            using var connection = databaseFactory.GetConnection();

            await connection.OpenAsync();

            var extendedProperty = (await connection.GetExtendedProperties(table, name)).FirstOrDefault();

            var repository = new ExtendedPropertyRepository(connection);

            if (extendedProperty != null)
                await repository.DropAsync(new ExtendedProperty(name, token.SCHEMA, table.Schema, token.TABLE, table.Name));
        }

        /// <summary>
        /// Drops an extended property for database object if exists
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <param name="column">Instance of <see cref="Column"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task DropExtendedPropertyIfExists(this SqlServerDatabaseFactory databaseFactory, ITable table, Column column, string name)
        {
            using var connection = databaseFactory.GetConnection();

            await connection.OpenAsync();

            var extendedProperty = (await connection.GetExtendedProperties(table, column, name)).FirstOrDefault();

            var repository = new ExtendedPropertyRepository(connection);

            if (extendedProperty != null)
                await repository.DropAsync(new ExtendedProperty(name, token.SCHEMA, table.Schema, token.TABLE, table.Name, token.COLUMN, column.Name));
        }

        /// <summary>
        /// Drops an extended property for database object if exists
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="view">Instance of <see cref="View"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task DropExtendedPropertyIfExists(this SqlServerDatabaseFactory databaseFactory, IView view, string name)
        {
            using var connection = databaseFactory.GetConnection();

            await connection.OpenAsync();

            var extendedProperty = (await connection.GetExtendedProperties(view, name)).FirstOrDefault();

            var repository = new ExtendedPropertyRepository(connection);

            if (extendedProperty != null)
                await repository.DropAsync(new ExtendedProperty(name, token.SCHEMA, view.Schema, token.VIEW, view.Name));
        }

        /// <summary>
        /// Drops an extended property for database object if exists
        /// </summary>
        /// <param name="databaseFactory">Instance of <see cref="SqlServerDatabaseFactory"/> class</param>
        /// <param name="view">Instance of <see cref="View"/> class</param>
        /// <param name="column">Instance of <see cref="Column"/> class</param>
        /// <param name="name">Extended property name</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task DropExtendedPropertyIfExists(this SqlServerDatabaseFactory databaseFactory, IView view, Column column, string name)
        {
            using var connection = databaseFactory.GetConnection();

            await connection.OpenAsync();

            var extendedProperty = (await connection.GetExtendedProperties(view, name)).FirstOrDefault();

            var repository = new ExtendedPropertyRepository(connection);

            if (extendedProperty != null)
                await repository.DropAsync(new ExtendedProperty(name, token.SCHEMA, view.Schema, token.VIEW, view.Name, token.COLUMN, column.Name));
        }
    }
}

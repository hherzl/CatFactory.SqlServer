using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CatFactory.ObjectRelationalMapping;
using CatFactory.ObjectRelationalMapping.Programmability;
using CatFactory.SqlServer.DatabaseObjectModel;

namespace CatFactory.SqlServer.Features
{
    /// <summary>
    /// Contains extension methods for <see cref="DbConnection"/> class
    /// </summary>
    public static class SqlConnectionExtensions
    {
        /// <summary>
        /// Gets a sequence of extended properties for database
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A sequence of <see cref="ExtendedProperty"/> class</returns>
        public static async Task<List<ExtendedProperty>> GetExtendedProperties(this SqlConnection connection, string name)
            => await new ExtendedPropertyRepository(connection).GetAsync(new ExtendedProperty(name));

        /// <summary>
        /// Gets a sequence of extended properties for table
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="table">Name for table</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A sequence of <see cref="ExtendedProperty"/> class</returns>
        public static async Task<List<ExtendedProperty>> GetExtendedProperties(this SqlConnection connection, ITable table, string name)
            => await new ExtendedPropertyRepository(connection).GetAsync(new ExtendedProperty(name, SqlServerToken.SCHEMA, table.Schema, table.Type, table.Name));

        /// <summary>
        /// Gets a sequence of extended properties for view
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="view">Name for view</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A sequence of <see cref="ExtendedProperty"/> class</returns>
        public static async Task<List<ExtendedProperty>> GetExtendedProperties(this SqlConnection connection, IView view, string name)
            => await new ExtendedPropertyRepository(connection).GetAsync(new ExtendedProperty(name, SqlServerToken.SCHEMA, view.Schema, view.Type, view.Name));

        /// <summary>
        /// Gets a sequence of extended properties for table function
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="tableFunction">Name for table function</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A sequence of <see cref="ExtendedProperty"/> class</returns>
        public static async Task<List<ExtendedProperty>> GetExtendedProperties(this SqlConnection connection, ITableFunction tableFunction, string name)
            => await new ExtendedPropertyRepository(connection).GetAsync(new ExtendedProperty(name, SqlServerToken.SCHEMA, tableFunction.Schema, tableFunction.Type, tableFunction.Name));

        /// <summary>
        /// Gets a sequence of extended properties for scalar function
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="scalarFunction">Name for scalar function</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A sequence of <see cref="ExtendedProperty"/> class</returns>
        public static async Task<List<ExtendedProperty>> GetExtendedProperties(this SqlConnection connection, ScalarFunction scalarFunction, string name)
            => await new ExtendedPropertyRepository(connection).GetAsync(new ExtendedProperty(name, SqlServerToken.SCHEMA, scalarFunction.Schema, scalarFunction.Type, scalarFunction.Name));

        /// <summary>
        /// Gets a sequence of extended properties for stored procedure
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="storedProcedure">Name for stored procedure</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A sequence of <see cref="ExtendedProperty"/> class</returns>
        public static async Task<List<ExtendedProperty>> GetExtendedProperties(this SqlConnection connection, StoredProcedure storedProcedure, string name)
            => await new ExtendedPropertyRepository(connection).GetAsync(new ExtendedProperty(name, SqlServerToken.SCHEMA, storedProcedure.Schema, storedProcedure.Type, storedProcedure.Name));

        /// <summary>
        /// Gets a sequence of extended properties for table's column
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="table">Name for table</param>
        /// <param name="column">Name for column</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A sequence of <see cref="ExtendedProperty"/> class</returns>
        public static async Task<List<ExtendedProperty>> GetExtendedProperties(this SqlConnection connection, ITable table, Column column, string name)
            => await new ExtendedPropertyRepository(connection).GetAsync(new ExtendedProperty(name, SqlServerToken.SCHEMA, table.Schema, table.Type, table.Name, "column", column.Name));

        /// <summary>
        /// Gets a sequence of extended properties for view's column
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="view">Name for view</param>
        /// <param name="column">Name for column</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A sequence of <see cref="ExtendedProperty"/> class</returns>
        public static async Task<IEnumerable<ExtendedProperty>> GetExtendedProperties(this SqlConnection connection, IView view, Column column, string name)
            => await new ExtendedPropertyRepository(connection).GetAsync(new ExtendedProperty(name, SqlServerToken.SCHEMA, view.Schema, view.Type, view.Name, "column", column.Name));
    }
}

using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using CatFactory.ObjectRelationalMapping;
using CatFactory.ObjectRelationalMapping.Programmability;
using CatFactory.SqlServer.DocumentObjectModel;

namespace CatFactory.SqlServer.Features
{
    /// <summary>
    /// Contains extension methods for <see cref="DbConnection"/> class
    /// </summary>
    public static class DbConnectionExtensions
    {
        /// <summary>
        /// Gets a sequence of extended properties for database
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A sequence of <see cref="ExtendedProperty"/> class</returns>
        public static IEnumerable<ExtendedProperty> GetExtendedProperties(this DbConnection connection, string name)
            => new ExtendedPropertyRepository(connection).GetExtendedProperties(new ExtendedProperty(name)).ToList();

        /// <summary>
        /// Gets a sequence of extended properties for table
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="table">Name for table</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A sequence of <see cref="ExtendedProperty"/> class</returns>
        public static IEnumerable<ExtendedProperty> GetExtendedProperties(this DbConnection connection, ITable table, string name)
            => new ExtendedPropertyRepository(connection).GetExtendedProperties(new ExtendedProperty(name, "schema", table.Schema, table.Type, table.Name)).ToList();

        /// <summary>
        /// Gets a sequence of extended properties for view
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="view">Name for view</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A sequence of <see cref="ExtendedProperty"/> class</returns>
        public static IEnumerable<ExtendedProperty> GetExtendedProperties(this DbConnection connection, IView view, string name)
            => new ExtendedPropertyRepository(connection).GetExtendedProperties(new ExtendedProperty(name, "schema", view.Schema, view.Type, view.Name)).ToList();

        /// <summary>
        /// Gets a sequence of extended properties for table function
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="tableFunction">Name for table function</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A sequence of <see cref="ExtendedProperty"/> class</returns>
        public static IEnumerable<ExtendedProperty> GetExtendedProperties(this DbConnection connection, ITableFunction tableFunction, string name)
            => new ExtendedPropertyRepository(connection).GetExtendedProperties(new ExtendedProperty(name, "schema", tableFunction.Schema, tableFunction.Type, tableFunction.Name)).ToList();

        /// <summary>
        /// Gets a sequence of extended properties for scalar function
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="scalarFunction">Name for scalar function</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A sequence of <see cref="ExtendedProperty"/> class</returns>
        public static IEnumerable<ExtendedProperty> GetExtendedProperties(this DbConnection connection, ScalarFunction scalarFunction, string name)
            => new ExtendedPropertyRepository(connection).GetExtendedProperties(new ExtendedProperty(name, "schema", scalarFunction.Schema, scalarFunction.Type, scalarFunction.Name)).ToList();

        /// <summary>
        /// Gets a sequence of extended properties for stored procedure
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="storedProcedure">Name for stored procedure</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A sequence of <see cref="ExtendedProperty"/> class</returns>
        public static IEnumerable<ExtendedProperty> GetExtendedProperties(this DbConnection connection, StoredProcedure storedProcedure, string name)
            => new ExtendedPropertyRepository(connection).GetExtendedProperties(new ExtendedProperty(name, "schema", storedProcedure.Schema, storedProcedure.Type, storedProcedure.Name)).ToList();

        /// <summary>
        /// Gets a sequence of extended properties for table's column
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="table">Name for table</param>
        /// <param name="column">Name for column</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A sequence of <see cref="ExtendedProperty"/> class</returns>
        public static IEnumerable<ExtendedProperty> GetExtendedProperties(this DbConnection connection, ITable table, Column column, string name)
            => new ExtendedPropertyRepository(connection).GetExtendedProperties(new ExtendedProperty(name, "schema", table.Schema, table.Type, table.Name, "column", column.Name)).ToList();

        /// <summary>
        /// Gets a sequence of extended properties for view's column
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="view">Name for view</param>
        /// <param name="column">Name for column</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A sequence of <see cref="ExtendedProperty"/> class</returns>
        public static IEnumerable<ExtendedProperty> GetExtendedProperties(this DbConnection connection, IView view, Column column, string name)
            => new ExtendedPropertyRepository(connection).GetExtendedProperties(new ExtendedProperty(name, "schema", view.Schema, view.Type, view.Name, "column", column.Name)).ToList();
    }
}

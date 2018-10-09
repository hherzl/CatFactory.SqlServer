using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
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
            => new ExtendedPropertyRepository(connection).GetExtendedProperties(new ExtendedProperty { Name = name }).ToList();

        /// <summary>
        /// Gets a sequence of extended properties for table
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="table">Name for table</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A sequence of <see cref="ExtendedProperty"/> class</returns>
        public static IEnumerable<ExtendedProperty> GetExtendedProperties(this DbConnection connection, ITable table, string name)
            => new ExtendedPropertyRepository(connection).GetExtendedProperties(new ExtendedProperty
            {
                Name = name,
                Level0Type = "schema",
                Level0Name = table.Schema,
                Level1Type = table.Type,
                Level1Name = table.Name
            }).ToList();

        /// <summary>
        /// Gets a sequence of extended properties for view
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="view">Name for view</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A sequence of <see cref="ExtendedProperty"/> class</returns>
        public static IEnumerable<ExtendedProperty> GetExtendedProperties(this DbConnection connection, IView view, string name)
            => new ExtendedPropertyRepository(connection).GetExtendedProperties(new ExtendedProperty
            {
                Name = name,
                Level0Type = "schema",
                Level0Name = view.Schema,
                Level1Type = view.Type,
                Level1Name = view.Name
            }).ToList();

        /// <summary>
        /// Gets a sequence of extended properties for table function
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="tableFunction">Name for table function</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A sequence of <see cref="ExtendedProperty"/> class</returns>
        public static IEnumerable<ExtendedProperty> GetExtendedProperties(this DbConnection connection, ITableFunction tableFunction, string name)
            => new ExtendedPropertyRepository(connection).GetExtendedProperties(new ExtendedProperty
            {
                Name = name,
                Level0Type = "schema",
                Level0Name = tableFunction.Schema,
                Level1Type = tableFunction.Type,
                Level1Name = tableFunction.Name
            }).ToList();

        /// <summary>
        /// Gets a sequence of extended properties for scalar function
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="scalarFunction">Name for scalar function</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A sequence of <see cref="ExtendedProperty"/> class</returns>
        public static IEnumerable<ExtendedProperty> GetExtendedProperties(this DbConnection connection, ScalarFunction scalarFunction, string name)
            => new ExtendedPropertyRepository(connection).GetExtendedProperties(new ExtendedProperty
            {
                Name = name,
                Level0Type = "schema",
                Level0Name = scalarFunction.Schema,
                Level1Type = scalarFunction.Type,
                Level1Name = scalarFunction.Name
            }).ToList();

        /// <summary>
        /// Gets a sequence of extended properties for stored procedure
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="storedProcedure">Name for stored procedure</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A sequence of <see cref="ExtendedProperty"/> class</returns>
        public static IEnumerable<ExtendedProperty> GetExtendedProperties(this DbConnection connection, StoredProcedure storedProcedure, string name)
            => new ExtendedPropertyRepository(connection).GetExtendedProperties(new ExtendedProperty
            {
                Name = name,
                Level0Type = "schema",
                Level0Name = storedProcedure.Schema,
                Level1Type = storedProcedure.Type,
                Level1Name = storedProcedure.Name
            }).ToList();

        /// <summary>
        /// Gets a sequence of extended properties for table's column
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="table">Name for table</param>
        /// <param name="column">Name for column</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A sequence of <see cref="ExtendedProperty"/> class</returns>
        public static IEnumerable<ExtendedProperty> GetExtendedProperties(this DbConnection connection, ITable table, Column column, string name)
            => new ExtendedPropertyRepository(connection).GetExtendedProperties(new ExtendedProperty
            {
                Name = name,
                Level0Type = "schema",
                Level0Name = table.Schema,
                Level1Type = table.Type,
                Level1Name = table.Name,
                Level2Type = "column",
                Level2Name = column.Name
            }).ToList();

        /// <summary>
        /// Gets a sequence of extended properties for view's column
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="view">Name for view</param>
        /// <param name="column">Name for column</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A sequence of <see cref="ExtendedProperty"/> class</returns>
        public static IEnumerable<ExtendedProperty> GetExtendedProperties(this DbConnection connection, IView view, Column column, string name)
            => new ExtendedPropertyRepository(connection).GetExtendedProperties(new ExtendedProperty
            {
                Name = name,
                Level0Type = "schema",
                Level0Name = view.Schema,
                Level1Type = view.Type,
                Level1Name = view.Name,
                Level2Type = "column",
                Level2Name = column.Name
            }).ToList();
    }
}

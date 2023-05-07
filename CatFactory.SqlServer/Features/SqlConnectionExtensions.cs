using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CatFactory.ObjectRelationalMapping;
using CatFactory.ObjectRelationalMapping.Programmability;
using CatFactory.SqlServer.DatabaseObjectModel;
using CatFactory.SqlServer.DatabaseObjectModel.Queries;

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
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task<ICollection<ExtendedProperty>> GetExtendedProperties(this SqlConnection connection, string name)
            => await connection.FnListExtendedPropertyAsync(new ExtendedProperty(name));

        /// <summary>
        /// Gets a sequence of extended properties for table
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="table">Name for table</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task<ICollection<ExtendedProperty>> GetExtendedProperties(this SqlConnection connection, ITable table, string name)
            => await connection.FnListExtendedPropertyAsync(ExtendedProperty.CreateLevel1(SqlServerToken.SCHEMA, table.Schema, SqlServerToken.TABLE, table.Name, name));

        /// <summary>
        /// Gets a sequence of extended properties for table's column
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="table">Name for table</param>
        /// <param name="column">Name for column</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task<ICollection<ExtendedProperty>> GetExtendedProperties(this SqlConnection connection, ITable table, Column column, string name)
            => await connection.FnListExtendedPropertyAsync(ExtendedProperty.CreateLevel2(SqlServerToken.SCHEMA, table.Schema, SqlServerToken.TABLE, table.Name, SqlServerToken.COLUMN, column.Name, name));

        /// <summary>
        /// Gets a sequence of extended properties for view
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="view">Name for view</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task<ICollection<ExtendedProperty>> GetExtendedProperties(this SqlConnection connection, IView view, string name)
            => await connection.FnListExtendedPropertyAsync(ExtendedProperty.CreateLevel1(SqlServerToken.SCHEMA, view.Schema, SqlServerToken.VIEW, view.Name, name));

        /// <summary>
        /// Gets a sequence of extended properties for view's column
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="view">Name for view</param>
        /// <param name="column">Name for column</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task<ICollection<ExtendedProperty>> GetExtendedProperties(this SqlConnection connection, IView view, Column column, string name)
            => await connection.FnListExtendedPropertyAsync(ExtendedProperty.CreateLevel2(SqlServerToken.SCHEMA, view.Schema, SqlServerToken.VIEW, view.Name, SqlServerToken.COLUMN, column.Name, name));

        /// <summary>
        /// Gets a sequence of extended properties for table function
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="tableFunction">Name for table function</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task<ICollection<ExtendedProperty>> GetExtendedProperties(this SqlConnection connection, ITableFunction tableFunction, string name)
            => await connection.FnListExtendedPropertyAsync(ExtendedProperty.CreateLevel1(SqlServerToken.SCHEMA, tableFunction.Schema, SqlServerToken.FUNCTION, tableFunction.Name, name));

        /// <summary>
        /// Gets a sequence of extended properties for scalar function
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="scalarFunction">Name for scalar function</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task<ICollection<ExtendedProperty>> GetExtendedProperties(this SqlConnection connection, ScalarFunction scalarFunction, string name)
            => await connection.FnListExtendedPropertyAsync(ExtendedProperty.CreateLevel1(SqlServerToken.SCHEMA, scalarFunction.Schema, SqlServerToken.FUNCTION, scalarFunction.Name, name));

        /// <summary>
        /// Gets a sequence of extended properties for stored procedure
        /// </summary>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <param name="storedProcedure">Name for stored procedure</param>
        /// <param name="name">Name for extended property</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task<ICollection<ExtendedProperty>> GetExtendedProperties(this SqlConnection connection, StoredProcedure storedProcedure, string name)
            => await connection.FnListExtendedPropertyAsync(ExtendedProperty.CreateLevel1(SqlServerToken.SCHEMA, storedProcedure.Schema, SqlServerToken.PROCEDURE, storedProcedure.Name, name));
    }
}

using System.Collections.Generic;
using System.Linq;
using CatFactory.ObjectRelationalMapping;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// Contains extension methods for <see cref="Database"/> class
    /// </summary>
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Gets all database objects that represent tables from database instance
        /// </summary>
        /// <param name="database">Instance of <see cref="Database"/> class</param>
        /// <returns>A sequence of <see cref="DbObject"/> that represents tables in database instance</returns>
        public static IEnumerable<DbObject> GetTables(this Database database)
            => database.DbObjects.Where(item => new string[] { "USER_TABLE" }.Contains(item.Type));

        /// <summary>
        /// Gets all database objects that represent views from database instance
        /// </summary>
        /// <param name="database">Instance of <see cref="Database"/> class</param>
        /// <returns>A sequence of <see cref="DbObject"/> that represents views in database instance</returns>
        public static IEnumerable<DbObject> GetViews(this Database database)
            => database.DbObjects.Where(item => new string[] { "VIEW" }.Contains(item.Type));

        /// <summary>
        /// Gets all database objects that represent scalar functions from database instance
        /// </summary>
        /// <param name="database">Instance of <see cref="Database"/> class</param>
        /// <returns>A sequence of <see cref="DbObject"/> that represents scalar functions in database instance</returns>
        public static IEnumerable<DbObject> GetScalarFunctions(this Database database)
            => database.DbObjects.Where(item => new string[] { "SQL_SCALAR_FUNCTION" }.Contains(item.Type));

        /// <summary>
        /// Gets all database objects that represent table functions from database instance
        /// </summary>
        /// <param name="database">Instance of <see cref="Database"/> class</param>
        /// <returns>A sequence of <see cref="DbObject"/> that represents table functions in database instance</returns>
        public static IEnumerable<DbObject> GetTableFunctions(this Database database)
            => database.DbObjects.Where(item => new string[] { "SQL_TABLE_VALUED_FUNCTION" }.Contains(item.Type));

        /// <summary>
        /// Gets all database objects that represent stored procedures from database instance
        /// </summary>
        /// <param name="database">Instance of <see cref="Database"/> class</param>
        /// <returns>A sequence of <see cref="DbObject"/> that represents stored procedures in database instance</returns>
        public static IEnumerable<DbObject> GetStoredProcedures(this Database database)
            => database.DbObjects.Where(item => new string[] { "SQL_STORED_PROCEDURE" }.Contains(item.Type));

        /// <summary>
        /// Gets all database objects that represent sequences from database instance
        /// </summary>
        /// <param name="database">Instance of <see cref="Database"/> class</param>
        /// <returns>A sequence of <see cref="DbObject"/> that represents sequences in database instance</returns>
        public static IEnumerable<DbObject> GetSequences(this Database database)
            => database.DbObjects.Where(item => new string[] { "SEQUENCE_OBJECT" }.Contains(item.Type));

        /// <summary>
        /// Get the name for table according to database naming convention
        /// </summary>
        /// <param name="database">Instance of <see cref="Database"/> class</param>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <returns>A <see cref="string"/> that represents the name for database object</returns>
        public static string GetObjectName(this Database database, ITable table)
            => database.NamingConvention.GetObjectName(table.Schema, table.Name);

        /// <summary>
        /// Gets the database object name according to database naming convention
        /// </summary>
        /// <param name="database">Instance of <see cref="Database"/> class</param>
        /// <param name="name">Name for database object</param>
        /// <returns>A <see cref="string"/> that represents the name for database object</returns>
        public static string GetObjectName(this Database database, string name)
            => database.NamingConvention.GetObjectName(name);

        /// <summary>
        /// Gets the database object name according to database naming convention
        /// </summary>
        /// <param name="database">Instance of <see cref="Database"/> class</param>
        /// <param name="column">Instance of <see cref="Column"/> class</param>
        /// <returns>A <see cref="string"/> that represents the name for database object</returns>
        public static string GetObjectName(this Database database, Column column)
            => database.NamingConvention.GetObjectName(column.Name);

        /// <summary>
        /// Gets the procedure name according to database naming convention
        /// </summary>
        /// <param name="database">Instance of <see cref="Database"/> class</param>
        /// <param name="table">Instance of <see cref="Table"/> class</param>
        /// <param name="action">CRUD action</param>
        /// <returns>A <see cref="string"/> that represents the name for procedure</returns>
        public static string GetProcedureName(this Database database, ITable table, string action)
            => database.NamingConvention.GetObjectName(table.Schema, string.Format("{0}{1}", table.Name, action));

        /// <summary>
        /// Gets the parameter name according to database naming convention
        /// </summary>
        /// <param name="database">Instance of <see cref="Database"/> class</param>
        /// <param name="column">Instance of <see cref="Column"/> class</param>
        /// <returns>A <see cref="string"/> that represents the name for parameter</returns>
        public static string GetParameterName(this Database database, Column column)
            => database.NamingConvention.GetParameterName(column.Name);

        /// <summary>
        /// Gets the database object name according to database naming convention
        /// </summary>
        /// <param name="database">Instance of <see cref="Database"/> class</param>
        /// <param name="name">Name for parameter</param>
        /// <returns>A <see cref="string"/> that represents the name for parameter</returns>
        public static string GetParameterName(this Database database, string name)
            => database.NamingConvention.GetParameterName(name);
    }
}

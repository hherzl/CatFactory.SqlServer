using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// 
    /// </summary>
    public static class DbObjectExtensions
    {
        static DbObjectExtensions()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="table"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string GetProcedureName(this Database database, ITable table, string action)
            => database.NamingConvention.GetObjectName(table.Schema, string.Format("{0}{1}", table.Name, action));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string GetObjectName(this Database database, ITable table)
            => database.NamingConvention.GetObjectName(table.Schema, table.Name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static string GetObjectName(this Database database, Column column)
            => database.NamingConvention.GetObjectName(column.Name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetObjectName(this Database database, string name)
            => database.NamingConvention.GetObjectName(name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static string GetParameterName(this Database database, Column column)
            => database.NamingConvention.GetParameterName(column.Name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetParameterName(this Database database, string name)
            => database.NamingConvention.GetParameterName(name);
    }
}

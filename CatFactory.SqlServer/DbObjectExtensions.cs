using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    public static class DbObjectExtensions
    {
        private static IDatabaseNamingConvention namingConvention;

        static DbObjectExtensions()
        {
            namingConvention = new DatabaseNamingConvention();
        }

        public static string GetObjectName(this ITable table)
            => string.IsNullOrEmpty(table.Schema) ? string.Format("[{0}]", table.Name) : string.Format("[{0}].[{1}]", table.Schema, table.Name);

        public static string GetObjectName(this IView view)
            => string.IsNullOrEmpty(view.Schema) ? string.Format("[{0}]", view.Name) : string.Format("[{0}].[{1}]", view.Schema, view.Name);

        public static string GetObjectName(this Column column)
            => string.Format("[{0}]", column.Name);

        public static string GetParameterName(this Column column)
            => string.Format("@{0}", NamingConvention.GetCamelCase(column.Name));

        public static string GetObjectName(this string value)
            => string.Format("[{0}]", value);

        public static string GetParameterName(this string name)
            => string.Format("@{0}", NamingConvention.GetCamelCase(name));

        public static string GetProcedureName(this ITable table, string action)
            => string.IsNullOrEmpty(table.Schema) ? string.Format("[{0}]", table.Name) : string.Format("[{0}].[{1}{2}]", table.Schema, table.Name, action);
    }
}

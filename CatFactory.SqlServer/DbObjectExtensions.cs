using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    public static class DbObjectExtensions
    {
        static DbObjectExtensions()
        {
        }

        public static string GetProcedureName(this Database database, ITable table, string action)
            => database.NamingConvention.GetObjectName(table.Schema, string.Format("{0}{1}", table.Name, action));

        public static string GetObjectName(this Database database, ITable table)
            => database.NamingConvention.GetObjectName(table.Schema, table.Name);

        public static string GetObjectName(this Database database, Column column)
            => database.NamingConvention.GetObjectName(column.Name);

        public static string GetObjectName(this Database database, string name)
            => database.NamingConvention.GetObjectName(name);

        public static string GetParameterName(this Database database, Column column)
            => database.NamingConvention.GetParameterName(column.Name);

        public static string GetParameterName(this Database database, string name)
            => database.NamingConvention.GetParameterName(name);
    }
}

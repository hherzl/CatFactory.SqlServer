using System;
using CatFactory.CodeFactory;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    public static class Extensions
    {
        public static String GetObjectName(this Table table)
            => String.IsNullOrEmpty(table.Schema) ? String.Format("[{0}]", table.Name) : String.Format("[{0}].[{1}]", table.Schema, table.Name);

        public static String GetObjectName(this Column column)
            => String.Format("[{0}]", column.Name);

        public static String GetObjectName(this String value)
            => String.Format("[{0}]", value);

        public static String GetParameterName(this Column column)
            => String.Format("@{0}", NamingConvention.GetCamelCase(column.Name));

        public static String GetParameterName(this String name)
            => String.Format("@{0}", NamingConvention.GetCamelCase(name));

        public static String GetProcedureName(this Table table, String action)
            => String.IsNullOrEmpty(table.Schema) ? String.Format("[{0}]", table.Name) : String.Format("[{0}].[{1}{2}]", table.Schema, table.Name, action);
    }
}

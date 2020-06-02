using System;

namespace CatFactory.SqlServer.ObjectRelationalMapping
{
#pragma warning disable CS1591
    public class DefaultTypeMap
    {
        public DefaultTypeMap()
        {
        }

        public DefaultTypeMap(Type type, string databaseType)
        {
            Type = type;
            DatabaseType = databaseType;
        }

        public Type Type { get; set; }

        public string DatabaseType { get; set; }
    }
#pragma warning restore CS1591
}

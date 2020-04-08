using System;
using CatFactory.SqlServer.ObjectRelationalMapping;

namespace CatFactory.SqlServer
{
#pragma warning disable CS1591
    public static class SqlServerDatabaseExtensions
    {
        public static SqlServerDatabase AddDefaultTypeMapFor(this SqlServerDatabase database, Type type, string databaseType)
        {
            database.DefaultTypeMaps.Add(new DefaultTypeMap { Type = type, DatabaseType = databaseType });

            return database;
        }
    }
#pragma warning restore CS1591
}

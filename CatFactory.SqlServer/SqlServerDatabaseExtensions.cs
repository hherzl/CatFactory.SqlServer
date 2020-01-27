using System;
using System.Collections.Generic;
using CatFactory.SqlServer.ObjectRelationalMapping;

namespace CatFactory.SqlServer
{
    public static class SqlServerDatabaseExtensions
    {
        public static SqlServerDatabase AddDefaultTypeMapFor(this SqlServerDatabase database, Type type, string databaseType)
        {
            if (database.DefaultTypeMaps == null)
                database.DefaultTypeMaps = new List<DefaultTypeMap>();

            database.DefaultTypeMaps.Add(new DefaultTypeMap { Type = type, DatabaseType = databaseType });

            return database;
        }
    }
}

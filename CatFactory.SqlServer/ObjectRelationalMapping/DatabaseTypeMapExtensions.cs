using System;
using System.Collections.Generic;
using System.Linq;
using CatFactory.ObjectRelationalMapping;

namespace CatFactory.SqlServer.ObjectRelationalMapping
{
#pragma warning disable CS1591
    public static class DatabaseTypeMapExtensions
    {
        public static DatabaseTypeMap GetByDatabaseType(this IEnumerable<DatabaseTypeMap> sequence, string databaseType)
            => sequence.FirstOrDefault(item => item.DatabaseType == databaseType);

        public static IEnumerable<DatabaseTypeMap> GetByClrType(this IEnumerable<DatabaseTypeMap> sequence, Type type)
            => sequence.Where(item => item.GetClrType() == type);
    }
}

using System.Collections.Generic;
using System.Linq;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    public static class DatabaseExtensions
    {
        public static IEnumerable<DbObject> GetTables(this Database database)
            => database.DbObjects.Where(item => new string[] { "USER_TABLE" }.Contains(item.Type));

        public static IEnumerable<DbObject> GetViews(this Database database)
            => database.DbObjects.Where(item => new string[] { "VIEW" }.Contains(item.Type));

        public static IEnumerable<DbObject> GetStoredProcedures(this Database database)
            => database.DbObjects.Where(item => new string[] { "SQL_STORED_PROCEDURE" }.Contains(item.Type));

        public static IEnumerable<DbObject> GetScalarFunctions(this Database database)
            => database.DbObjects.Where(item => new string[] { "SQL_SCALAR_FUNCTION" }.Contains(item.Type));

        public static IEnumerable<DbObject> GetTableFunctions(this Database database)
            => database.DbObjects.Where(item => new string[] { "SQL_TABLE_VALUED_FUNCTION" }.Contains(item.Type));
    }
}

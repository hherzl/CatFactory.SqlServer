using System.Collections.Generic;
using System.Linq;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    public static class DatabaseExtensions
    {
        public static IEnumerable<DbObject> GetTables(this Database database)
            => database.DbObjects.Where(item => item.Type == "USER_TABLE");

        public static IEnumerable<DbObject> GetViews(this Database database)
            => database.DbObjects.Where(item => item.Type == "VIEW");

        public static IEnumerable<DbObject> GetProcedures(this Database database)
            => database.DbObjects.Where(item => item.Type == "SQL_STORED_PROCEDURE");

        public static IEnumerable<DbObject> GetScalarFunctions(this Database database)
            => database.DbObjects.Where(item => item.Type == "SQL_SCALAR_FUNCTION");

        public static IEnumerable<DbObject> GetTableFunctions(this Database database)
            => database.DbObjects.Where(item => item.Type == "SQL_TABLE_VALUED_FUNCTION");
    }
}

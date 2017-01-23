using System.Collections.Generic;
using System.Linq;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    public static class DatabaseExtensions
    {
        public static IEnumerable<DbObject> GetTables(this Database db)
            => db.DbObjects.Where(item => item.Type == "table");

        public static IEnumerable<DbObject> GetViews(this Database db)
            => db.DbObjects.Where(item => item.Type == "view");

        public static IEnumerable<DbObject> GetProcedures(this Database db)
            => db.DbObjects.Where(item => item.Type == "procedure");
    }
}

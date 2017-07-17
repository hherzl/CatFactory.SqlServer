using System;
using System.Collections.Generic;
using System.Linq;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    public static class DatabaseExtensions
    {
        public static IEnumerable<DbObject> GetTables(this Database db)
            => db.DbObjects.Where(item => item.Type == "USER_TABLE");

        public static IEnumerable<DbObject> GetViews(this Database db)
            => db.DbObjects.Where(item => item.Type == "VIEW");

        public static IEnumerable<DbObject> GetProcedures(this Database db)
            => db.DbObjects.Where(item => item.Type == "SQL_STORED_PROCEDURE");

        public static IEnumerable<DbObject> GetScalarFunctions(this Database db)
            => db.DbObjects.Where(item => item.Type == "SQL_SCALAR_FUNCTION");

        public static IEnumerable<DbObject> GetTableFunctions(this Database db)
            => db.DbObjects.Where(item => item.Type == "SQL_TABLE_VALUED_FUNCTION");

        public static Boolean IsPrimaryKeyGuid(this Table table)
            => table.PrimaryKey != null && table.PrimaryKey.Key.Count == 1 && table.Columns[0].Type == "uniqueidentifier" ? true : false;
    }
}

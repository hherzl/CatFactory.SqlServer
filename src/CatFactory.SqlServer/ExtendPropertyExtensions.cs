using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    public static class ExtendPropertyExtensions
    {
        public static IEnumerable<ExtendProperty> GetMsDescriptionForDbObject(this SqlConnection connection, DbObject dbObject)
        {
            var repository = new ExtendPropertyRepository();

            var name = "'MS_Description'";
            var level0type = "'schema'";
            var level0name = String.Format("'{0}'", dbObject.Schema);
            var level1type = String.Format("'{0}'", dbObject.Type);
            var level1name = String.Format("'{0}'", dbObject.Name);
            var level2type = "default";
            var level2name = "default";

            return repository.GetExtendProperties(connection, name, level0type, level0name, level1type, level1name, level2type, level2name).ToList();
        }

        public static IEnumerable<ExtendProperty> GetMsDescriptionForColumn(this SqlConnection connection, DbObject dbObject, Column column)
        {
            var repository = new ExtendPropertyRepository();

            var name = "'MS_Description'";
            var level0type = "'schema'";
            var level0name = String.Format("'{0}'", dbObject.Schema);
            var level1type = String.Format("'{0}'", dbObject.Type);
            var level1name = String.Format("'{0}'", dbObject.Name);
            var level2type = "'column'";
            var level2name = String.Format("'{0}'", column.Name);

            return repository.GetExtendProperties(connection, name, level0type, level0name, level1type, level1name, level2type, level2name).ToList();
        }
    }
}

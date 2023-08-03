using System;
using System.Collections.ObjectModel;
using CatFactory.SqlServer.DatabaseObjectModel;
using CatFactory.SqlServer.Features;

namespace CatFactory.SqlServer
{
#pragma warning disable CS1591
    public static class SqlServerDatabaseExtensions
    {
        public static SqlServerDatabase AddDefaultTypeMapFor(this SqlServerDatabase database, Type type, string databaseType)
        {
            database.DefaultTypeMaps.Add(new(type, databaseType));

            return database;
        }

        public static void SyncMsDescription(this SqlServerDatabase database)
        {
            var databaseExtendedProperties = (Collection<ExtendedProperty>)database.ImportBag.ExtendedProperties;

            database.Description = databaseExtendedProperties.GetByName(SqlServerToken.MS_DESCRIPTION)?.Value;

            foreach (var table in database.Tables)
            {
                var tableExtendedProperties = (Collection<ExtendedProperty>)table.ImportBag.ExtendedProperties;

                table.Description = tableExtendedProperties.GetByName(SqlServerToken.MS_DESCRIPTION)?.Value;

                foreach (var column in table.Columns)
                {
                    var columnExtendedProperties = (Collection<ExtendedProperty>)column.ImportBag.ExtendedProperties;

                    column.Description = columnExtendedProperties.GetByName(SqlServerToken.MS_DESCRIPTION)?.Value;
                }
            }

            foreach (var view in database.Views)
            {
                var viewExtendedProperties = (Collection<ExtendedProperty>)view.ImportBag.ExtendedProperties;

                view.Description = viewExtendedProperties.GetByName(SqlServerToken.MS_DESCRIPTION)?.Value;

                foreach (var column in view.Columns)
                {
                    var columnExtendedProperties = (Collection<ExtendedProperty>)column.ImportBag.ExtendedProperties;

                    column.Description = columnExtendedProperties.GetByName(SqlServerToken.MS_DESCRIPTION)?.Value;
                }
            }
        }
    }
}

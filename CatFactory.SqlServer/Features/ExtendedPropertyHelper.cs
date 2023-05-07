using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CatFactory.ObjectRelationalMapping;
using CatFactory.ObjectRelationalMapping.Programmability;
using CatFactory.SqlServer.DatabaseObjectModel;
using CatFactory.SqlServer.DatabaseObjectModel.Queries;

namespace CatFactory.SqlServer.Features
{
#pragma warning disable CS1591
    public static class ExtendedPropertyHelper
    {
        public static async Task ImportExtendedPropertiesAsync(this SqlConnection connection, SqlServerDatabaseFactory databaseFactory, SqlServerDatabase database)
        {
            database.ImportBag.ExtendedProperties = new Collection<ExtendedProperty>();

            foreach (var name in databaseFactory.DatabaseImportSettings.ExtendedProperties)
            {
                foreach (var property in await connection.FnListExtendedPropertyAsync(new ExtendedProperty(name)))
                {
                    database.ImportBag.ExtendedProperties.Add(new ExtendedProperty(property.Name, property.Value));
                }
            }
        }

        public static async Task ImportExtendedPropertiesAsync(this SqlConnection connection, SqlServerDatabaseFactory databaseFactory, Table table)
        {
            table.Type = SqlServerToken.TABLE;
            table.ImportBag.ExtendedProperties = new Collection<ExtendedProperty>();

            foreach (var name in databaseFactory.DatabaseImportSettings.ExtendedProperties)
            {
                foreach (var property in await connection.GetExtendedProperties(table, name))
                {
                    table.ImportBag.ExtendedProperties.Add(new ExtendedProperty(property.Name, property.Value));
                }

                foreach (var column in table.Columns)
                {
                    column.ImportBag.ExtendedProperties = new Collection<ExtendedProperty>();

                    foreach (var property in await connection.GetExtendedProperties(table, column, name))
                    {
                        column.ImportBag.ExtendedProperties.Add(new ExtendedProperty(property.Name, property.Value));
                    }
                }
            }
        }

        public static async Task ImportExtendedPropertiesAsync(this SqlConnection connection, SqlServerDatabaseFactory databaseFactory, View view)
        {
            view.Type = SqlServerToken.VIEW;
            view.ImportBag.ExtendedProperties = new Collection<ExtendedProperty>();

            foreach (var name in databaseFactory.DatabaseImportSettings.ExtendedProperties)
            {
                foreach (var property in await connection.GetExtendedProperties(view, name))
                {
                    view.ImportBag.ExtendedProperties.Add(new ExtendedProperty(property.Name, property.Value));
                }

                foreach (var column in view.Columns)
                {
                    column.ImportBag.ExtendedProperties = new Collection<ExtendedProperty>();

                    foreach (var property in await connection.GetExtendedProperties(view, column, name))
                    {
                        column.ImportBag.ExtendedProperties.Add(new ExtendedProperty(property.Name, property.Value));
                    }
                }
            }
        }

        public static async Task ImportExtendedPropertiesAsync(this SqlConnection connection, SqlServerDatabaseFactory databaseFactory, ScalarFunction scalarFunction)
        {
            scalarFunction.Type = SqlServerToken.FUNCTION;
            scalarFunction.ImportBag.ExtendedProperties = new Collection<ExtendedProperty>();

            foreach (var name in databaseFactory.DatabaseImportSettings.ExtendedProperties)
            {
                foreach (var property in await connection.GetExtendedProperties(scalarFunction, name))
                {
                    scalarFunction.ImportBag.ExtendedProperties.Add(new ExtendedProperty(property.Name, property.Value));
                }
            }
        }

        public static async Task ImportExtendedPropertiesAsync(this SqlConnection connection, SqlServerDatabaseFactory databaseFactory, TableFunction tableFunction)
        {
            tableFunction.Type = SqlServerToken.FUNCTION;
            tableFunction.ImportBag.ExtendedProperties = new Collection<ExtendedProperty>();

            foreach (var name in databaseFactory.DatabaseImportSettings.ExtendedProperties)
            {
                foreach (var property in await connection.GetExtendedProperties(tableFunction, name))
                {
                    tableFunction.ImportBag.ExtendedProperties.Add(new ExtendedProperty(property.Name, property.Value));
                }
            }
        }

        public static async Task ImportExtendedPropertiesAsync(this SqlConnection connection, SqlServerDatabaseFactory databaseFactory, StoredProcedure storedProcedure)
        {
            storedProcedure.Type = SqlServerToken.PROCEDURE;
            storedProcedure.ImportBag.ExtendedProperties = new Collection<ExtendedProperty>();

            foreach (var name in databaseFactory.DatabaseImportSettings.ExtendedProperties)
            {
                foreach (var property in await connection.GetExtendedProperties(storedProcedure, name))
                {
                    storedProcedure.ImportBag.ExtendedProperties.Add(new ExtendedProperty(property.Name, property.Value));
                }
            }
        }
    }
}

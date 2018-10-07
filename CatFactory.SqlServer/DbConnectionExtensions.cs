using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// 
    /// </summary>
    public static class DbConnectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IEnumerable<ExtendedProperty> GetExtendedProperties(this DbConnection connection, string name)
        {
            var model = new ExtendedProperty
            {
                Name = name
            };

            return new ExtendedPropertyRepository(connection).GetExtendedProperties(model).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="table"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IEnumerable<ExtendedProperty> GetExtendedProperties(this DbConnection connection, ITable table, string name)
        {
            var model = new ExtendedProperty
            {
                Name = name,
                Level0Type = "schema",
                Level0Name = table.Schema,
                Level1Type = table.Type,
                Level1Name = table.Name
            };

            return new ExtendedPropertyRepository(connection).GetExtendedProperties(model).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="view"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IEnumerable<ExtendedProperty> GetExtendedProperties(this DbConnection connection, IView view, string name)
        {
            var model = new ExtendedProperty
            {
                Name = name,
                Level0Type = "schema",
                Level0Name = view.Schema,
                Level1Type = view.Type,
                Level1Name = view.Name
            };

            return new ExtendedPropertyRepository(connection).GetExtendedProperties(model).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableFunction"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IEnumerable<ExtendedProperty> GetExtendedProperties(this DbConnection connection, ITableFunction tableFunction, string name)
        {
            var model = new ExtendedProperty
            {
                Name = name,
                Level0Type = "schema",
                Level0Name = tableFunction.Schema,
                Level1Type = tableFunction.Type,
                Level1Name = tableFunction.Name
            };

            return new ExtendedPropertyRepository(connection).GetExtendedProperties(model).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="scalarFunction"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IEnumerable<ExtendedProperty> GetExtendedProperties(this DbConnection connection, ScalarFunction scalarFunction, string name)
        {
            var model = new ExtendedProperty
            {
                Name = name,
                Level0Type = "schema",
                Level0Name = scalarFunction.Schema,
                Level1Type = scalarFunction.Type,
                Level1Name = scalarFunction.Name
            };

            return new ExtendedPropertyRepository(connection).GetExtendedProperties(model).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="storedProcedure"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IEnumerable<ExtendedProperty> GetExtendedProperties(this DbConnection connection, StoredProcedure storedProcedure, string name)
        {
            var model = new ExtendedProperty
            {
                Name = name,
                Level0Type = "schema",
                Level0Name = storedProcedure.Schema,
                Level1Type = storedProcedure.Type,
                Level1Name = storedProcedure.Name
            };

            return new ExtendedPropertyRepository(connection).GetExtendedProperties(model).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IEnumerable<ExtendedProperty> GetExtendedProperties(this DbConnection connection, ITable table, Column column, string name)
        {
            var model = new ExtendedProperty
            {
                Name = name,
                Level0Type = "schema",
                Level0Name = table.Schema,
                Level1Type = table.Type,
                Level1Name = table.Name,
                Level2Type = "column",
                Level2Name = column.Name
            };

            return new ExtendedPropertyRepository(connection).GetExtendedProperties(model).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="view"></param>
        /// <param name="column"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IEnumerable<ExtendedProperty> GetExtendedProperties(this DbConnection connection, IView view, Column column, string name)
        {
            var model = new ExtendedProperty
            {
                Name = name,
                Level0Type = "schema",
                Level0Name = view.Schema,
                Level1Type = view.Type,
                Level1Name = view.Name,
                Level2Type = "column",
                Level2Name = column.Name
            };

            return new ExtendedPropertyRepository(connection).GetExtendedProperties(model).ToList();
        }
    }
}

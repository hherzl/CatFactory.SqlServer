using System;
using System.Linq.Expressions;
using CatFactory.SqlServer.DatabaseObjectModel;

namespace CatFactory.SqlServer.ObjectRelationalMapping
{
    public static partial class EntityHelper
    {
#pragma warning disable CS1591
        public static EntityResult<TModel> AddExtendedProp<TModel>(this EntityResult<TModel> result, string name, string value) where TModel : class
        {
            result.Table.ImportBag.ExtendedProperties.Add(new ExtendedProperty(name, value));

            return result;
        }

        public static EntityResult<TModel> AddExtendedProp<TModel, TProp>(this EntityResult<TModel> result, Expression<Func<TModel, TProp>> selector, string name, string value) where TModel : class
        {
            result.Table[GetPropertyName(selector)].ImportBag.ExtendedProperties.Add(new ExtendedProperty(name, value));

            return result;
        }
    }
}

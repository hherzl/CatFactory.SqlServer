using System;
using System.Linq.Expressions;
using CatFactory.ObjectRelationalMapping;

namespace CatFactory.SqlServer.ObjectRelationalMapping
{
#pragma warning disable CS1591
    public static partial class EntityHelper
    {
        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, string>> selector, int length = 0, bool nullable = false, string type = null, string collation = null) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                result.Table[column.Name].Length = length;
                result.Table[column.Name].Nullable = nullable;

                if (!string.IsNullOrEmpty(type))
                    result.Table[column.Name].Type = type;

                if (!string.IsNullOrEmpty(collation))
                    result.Table[column.Name].Collation = collation;
            }
            else
            {
                result.Table.Columns.Add(column);
            }

            return result;
        }
    }
}

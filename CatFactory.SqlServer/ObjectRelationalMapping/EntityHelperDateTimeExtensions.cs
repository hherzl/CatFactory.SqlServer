using System;
using System.Linq.Expressions;
using CatFactory.ObjectRelationalMapping;

namespace CatFactory.SqlServer.ObjectRelationalMapping
{
    public static partial class EntityHelper
    {
#pragma warning disable CS1591
        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, DateTime>> selector, bool nullable = false, string type = null) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                result.Table[column.Name].Nullable = nullable;

                if (!string.IsNullOrEmpty(type))
                    result.Table[column.Name].Type = type;
            }
            else
            {
                result.Table.Columns.Add(column);
            }

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, DateTime?>> selector, bool nullable = false, string type = null) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                result.Table[column.Name].Nullable = nullable;

                if (!string.IsNullOrEmpty(type))
                    result.Table[column.Name].Type = type;
            }
            else
            {
                result.Table.Columns.Add(column);
            }

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, DateTimeOffset>> selector, bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
                result.Table[column.Name].Nullable = nullable;
            else
                result.Table.Columns.Add(column);

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, DateTimeOffset?>> selector, bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
                result.Table[column.Name].Nullable = nullable;
            else
                result.Table.Columns.Add(column);

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, TimeSpan>> selector, bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
                result.Table[column.Name].Nullable = nullable;
            else
                result.Table.Columns.Add(column);

            return result;
        }

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, TimeSpan?>> selector, bool nullable = false) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
                result.Table[column.Name].Nullable = nullable;
            else
                result.Table.Columns.Add(column);

            return result;
        }
    }
}

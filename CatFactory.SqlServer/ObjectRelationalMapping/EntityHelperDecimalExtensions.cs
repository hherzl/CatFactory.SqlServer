using System;
using System.Linq.Expressions;
using CatFactory.ObjectRelationalMapping;

namespace CatFactory.SqlServer.ObjectRelationalMapping
{
    public static partial class EntityHelper
    {
#pragma warning disable CS1591
        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, decimal>> selector, short prec = 0, short scale = 0, bool nullable = false, string type = null) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                result.Table[column.Name].Prec = prec;
                result.Table[column.Name].Scale = scale;
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

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, decimal?>> selector, short prec = 0, short scale = 0, bool nullable = false, string type = null) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                result.Table[column.Name].Prec = prec;
                result.Table[column.Name].Scale = scale;
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

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, double>> selector, short prec = 0, short scale = 0, bool nullable = false, string type = null) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                result.Table[column.Name].Prec = prec;
                result.Table[column.Name].Scale = scale;
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

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, double?>> selector, short prec = 0, short scale = 0, bool nullable = false, string type = null) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                result.Table[column.Name].Prec = prec;
                result.Table[column.Name].Scale = scale;
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

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, float>> selector, short prec = 0, short scale = 0, bool nullable = false, string type = null) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                result.Table[column.Name].Prec = prec;
                result.Table[column.Name].Scale = scale;
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

        public static EntityResult<TModel> SetColumnFor<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, float?>> selector, short prec = 0, short scale = 0, bool nullable = false, string type = null) where TModel : class
        {
            var column = new Column
            {
                Name = GetPropertyName(selector)
            };

            if (result.Table.Columns.Contains(column))
            {
                result.Table[column.Name].Prec = prec;
                result.Table[column.Name].Scale = scale;
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
    }
}

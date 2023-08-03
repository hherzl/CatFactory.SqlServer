using System;
using System.Linq.Expressions;

namespace CatFactory.SqlServer.ObjectRelationalMapping
{
    public static partial class EntityHelper
    {
#pragma warning disable CS1591
        public static EntityResult<TModel> SetIdentity<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, decimal>> selector, int seed = 1, int increment = 1) where TModel : class
        {
            result.Table.Identity = new(GetPropertyName(selector), seed, increment);

            return result;
        }

        public static EntityResult<TModel> SetIdentity<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, decimal?>> selector, int seed = 1, int increment = 1) where TModel : class
        {
            result.Table.Identity = new(GetPropertyName(selector), seed, increment);

            return result;
        }

        public static EntityResult<TModel> SetIdentity<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, int>> selector, int seed = 1, int increment = 1) where TModel : class
        {
            result.Table.Identity = new(GetPropertyName(selector), seed, increment);

            return result;
        }

        public static EntityResult<TModel> SetIdentity<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, int?>> selector, int seed = 1, int increment = 1) where TModel : class
        {
            result.Table.Identity = new(GetPropertyName(selector), seed, increment);

            return result;
        }

        public static EntityResult<TModel> SetIdentity<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, long>> selector, int seed = 1, int increment = 1) where TModel : class
        {
            result.Table.Identity = new(GetPropertyName(selector), seed, increment);

            return result;
        }

        public static EntityResult<TModel> SetIdentity<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, long?>> selector, int seed = 1, int increment = 1) where TModel : class
        {
            result.Table.Identity = new(GetPropertyName(selector), seed, increment);

            return result;
        }

        public static EntityResult<TModel> SetIdentity<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, short>> selector, int seed = 1, int increment = 1) where TModel : class
        {
            result.Table.Identity = new(GetPropertyName(selector), seed, increment);

            return result;
        }

        public static EntityResult<TModel> SetIdentity<TModel>(this EntityResult<TModel> result, Expression<Func<TModel, short?>> selector, int seed = 1, int increment = 1) where TModel : class
        {
            result.Table.Identity = new(GetPropertyName(selector), seed, increment);

            return result;
        }
    }
}

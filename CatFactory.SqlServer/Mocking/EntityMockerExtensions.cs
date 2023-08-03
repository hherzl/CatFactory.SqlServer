using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CatFactory.SqlServer.Mocking
{
#pragma warning disable CS1591
    public static class EntityMockerExtensions
    {
        private static string GetPropertyName<TModel, TProperty>(Expression<Func<TModel, TProperty>> selector)
        {
            var memberExpression = selector.Body as MemberExpression;

            if (memberExpression == null)
            {
                if (selector.Body is UnaryExpression unaryExpression)
                    memberExpression = unaryExpression.Operand as MemberExpression;
            }

            return memberExpression.Member.Name;
        }

        public static EntityMocker<TModel> MockProperty<TModel>(this EntityMocker<TModel> source, Expression<Func<TModel, string>> selector, IEnumerable<string> values) where TModel : class
        {
            var name = GetPropertyName(selector);

            source.EntitySettings.Add(new EntitySetting
            {
                Name = name,
                Values = values
            });

            return source;
        }

        public static EntityMocker<TModel> MockProperty<TModel>(this EntityMocker<TModel> source, Expression<Func<TModel, DateTime?>> selector, Func<DateTime> propertyFunc) where TModel : class
        {
            var name = GetPropertyName(selector);

            source.EntitySettings.Add(new EntitySetting
            {
                Name = name,
                DateTimeFunc = propertyFunc
            });

            return source;
        }

        public static EntityMocker<TModel> MockProperty<TModel>(this EntityMocker<TModel> source, Expression<Func<TModel, decimal?>> selector, Func<decimal> propertyFunc) where TModel : class
        {
            var name = GetPropertyName(selector);

            source.EntitySettings.Add(new EntitySetting
            {
                Name = name,
                DecimalFunc = propertyFunc
            });

            return source;
        }

        public static EntityMocker<TModel> MockProperty<TModel>(this EntityMocker<TModel> source, Expression<Func<TModel, short?>> selector, Func<short> propertyFunc) where TModel : class
        {
            var name = GetPropertyName(selector);

            source.EntitySettings.Add(new EntitySetting
            {
                Name = name,
                Int16Func = propertyFunc
            });

            return source;
        }

        public static EntityMocker<TModel> MockProperty<TModel>(this EntityMocker<TModel> source, Expression<Func<TModel, int?>> selector, Func<int> propertyFunc) where TModel : class
        {
            var name = GetPropertyName(selector);

            source.EntitySettings.Add(new EntitySetting
            {
                Name = name,
                Int32Func = propertyFunc
            });

            return source;
        }

        public static EntityMocker<TModel> MockProperty<TModel>(this EntityMocker<TModel> source, Expression<Func<TModel, long?>> selector, Func<long> propertyFunc) where TModel : class
        {
            var name = GetPropertyName(selector);

            source.EntitySettings.Add(new EntitySetting
            {
                Name = name,
                Int64Func = propertyFunc
            });

            return source;
        }

        public static EntityMocker<TModel> MockProperty<TModel>(this EntityMocker<TModel> source, Expression<Func<TModel, string>> selector, Func<string> propertyFunc) where TModel : class
        {
            var name = GetPropertyName(selector);

            source.EntitySettings.Add(new EntitySetting
            {
                Name = name,
                StringFunc = propertyFunc
            });

            return source;
        }

        public static EntityMocker<TModel> When<TModel>(this EntityMocker<TModel> source, string when, Expression<Func<TModel, string>> selector, IEnumerable<string> values) where TModel : class
        {
            var name = GetPropertyName(selector);

            source.EntitySettings.Add(new EntitySetting
            {
                When = when,
                Name = name,
                Values = values
            });

            return source;
        }
    }
}

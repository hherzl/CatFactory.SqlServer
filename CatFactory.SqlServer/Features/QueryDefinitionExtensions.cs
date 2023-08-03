using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CatFactory.SqlServer.Features
{
#pragma warning disable CS1591
    public static class QueryDefinitionExtensions
    {
        private static string GetPropertyName<TModel, TProperty>(Expression<Func<TModel, TProperty>> selector)
        {
            var memberExpression = selector.Body as MemberExpression;

            if (memberExpression == null)
            {
                if (selector.Body is UnaryExpression unaryExpression)
                    memberExpression = (MemberExpression)unaryExpression.Operand;
            }

            return memberExpression.Member.Name;
        }

        private static IEnumerable<string> GetPropertyNames<TModel, TProperty>(Expression<Func<TModel, TProperty>> selector)
        {
            if (selector.Body is NewExpression newExpression)
            {
                foreach (Expression argument in newExpression.Arguments)
                {
                    var prop = (MemberExpression)argument;

                    yield return prop.Member.Name;
                }
            }
            else
            {
                var memberExpression = selector.Body as MemberExpression;

                if (memberExpression == null)
                {
                    if (selector.Body is UnaryExpression unaryExpression)
                        memberExpression = (MemberExpression)unaryExpression.Operand;
                }

                yield return memberExpression.Member.Name;
            }
        }

        public static QueryDefinition<TModel> SelectAll<TModel>(this TModel model, string source = null)
        {
            var queryDefinition = new QueryDefinition<TModel>(source ?? typeof(TModel).Name);

            foreach (var property in typeof(TModel).GetProperties())
            {
                queryDefinition.Columns.Add(property.Name);
            }

            return queryDefinition;
        }

        public static QueryDefinition<TModel> SelectByKey<TModel, TKey>(this TModel model, Expression<Func<TModel, TKey>> selector, object value, string source = null)
        {
            var queryDefinition = new QueryDefinition<TModel>(source ?? typeof(TModel).Name);

            foreach (var property in typeof(TModel).GetProperties())
            {
                queryDefinition.Columns.Add(property.Name);
            }

            var key = GetPropertyName(selector);

            queryDefinition.Conditions.Add(new(key, ComparisonOperator.Equals, value));

            return queryDefinition;
        }

        public static QueryDefinition<TModel> Insert<TModel>(this TModel model, string source = null)
        {
            var queryDefinition = new QueryDefinition<TModel>(source ?? typeof(TModel).Name);

            foreach (var property in typeof(TModel).GetProperties())
            {
                queryDefinition.Columns.Add(property.Name);
            }

            return queryDefinition;
        }

        public static QueryDefinition<TModel> Insert<TModel, TIdentity>(this TModel model, Expression<Func<TModel, TIdentity>> selector, string source = null)
        {
            var queryDefinition = new QueryDefinition<TModel>(source ?? typeof(TModel).Name);

            foreach (var property in typeof(TModel).GetProperties())
            {
                queryDefinition.Columns.Add(property.Name);
            }

            var identity = GetPropertyName(selector);

            return queryDefinition;
        }

        public static QueryDefinition<TModel> Update<TModel, TKey>(this TModel model, Expression<Func<TModel, TKey>> selector, object value, string source = null)
        {
            var queryDefinition = new QueryDefinition<TModel>(source ?? typeof(TModel).Name);

            foreach (var property in typeof(TModel).GetProperties())
            {
                queryDefinition.Columns.Add(property.Name);
            }

            var key = GetPropertyName(selector);

            queryDefinition.Conditions.Add(new(key, ComparisonOperator.Equals, value));

            return queryDefinition;
        }

        public static QueryDefinition<TModel> Delete<TModel, TKey>(this TModel model, Expression<Func<TModel, TKey>> selector, object value, string source = null)
        {
            var queryDefinition = new QueryDefinition<TModel>(source ?? typeof(TModel).Name);

            var key = GetPropertyName(selector);

            queryDefinition.Conditions.Add(new(key, ComparisonOperator.Equals, value));

            return queryDefinition;
        }
    }
}

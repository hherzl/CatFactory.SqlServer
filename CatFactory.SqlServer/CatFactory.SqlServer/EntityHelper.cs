using System;
using System.Linq.Expressions;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    public static class EntityHelper
    {
        public static EntityResult<TModel> DefineEntity<TModel>(this Database database, string schema, string name, TModel model)
        {
            var result = new EntityResult<TModel> { Model = model };

            result.Table = new Table
            {
                Schema = schema,
                Name = name
            };

            foreach (var property in model.GetType().GetProperties())
                result.Table.Columns.Add(new Column { Name = property.Name });

            return result;
        }

        private static string GetPropertyName<TModel, TProperty>(Expression<Func<TModel, TProperty>> selector)
        {
            var memberExpression = selector.Body as MemberExpression;

            if (memberExpression == null)
            {
                var unary = (UnaryExpression)selector.Body;
                memberExpression = unary.Operand as MemberExpression;
            }

            return memberExpression.Member.Name;
        }

        public static EntityResult<TModel> SetColumnForProperty<TModel, TProperty>(this EntityResult<TModel> result, Expression<Func<TModel, TProperty>> selector, Column column)
        {
            if (string.IsNullOrEmpty(column.Name))
                column.Name = GetPropertyName(selector);

            if (result.Table.Columns.Contains(column))
            {
                result.Table.GetColumn(column.Name).Type = column.Type;
                result.Table.GetColumn(column.Name).Length = column.Length;
                result.Table.GetColumn(column.Name).Prec = column.Prec;
                result.Table.GetColumn(column.Name).Scale = column.Scale;
                result.Table.GetColumn(column.Name).Nullable = column.Nullable;
                result.Table.GetColumn(column.Name).Collation = column.Collation;
                result.Table.GetColumn(column.Name).Description = column.Description;
            }
            else
            {
                result.Table.Columns.Add(column);
            }

            return result;
        }

        public static EntityResult<TModel> SetIdentity<TModel, TProperty>(this EntityResult<TModel> result, Expression<Func<TModel, TProperty>> selector, int seed = 1, int increment = 1)
        {
            var name = GetPropertyName(selector);

            result.Table.Identity = new Identity(name, seed, increment);

            return result;
        }

        public static EntityResult<TModel> SetPrimaryKey<TModel, TProperty>(this EntityResult<TModel> result, Expression<Func<TModel, TProperty>> selector, string constraintName = null)
        {
            var name = GetPropertyName(selector);

            result.Table.PrimaryKey = new PrimaryKey(name);

            return result;
        }

        public static void Translate(Database database)
        {
            // todo: Implement this feature
        }
    }
}

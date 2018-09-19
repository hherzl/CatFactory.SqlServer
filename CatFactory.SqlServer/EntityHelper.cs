using System;
using System.Linq.Expressions;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// 
    /// </summary>
    public static class EntityHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="database"></param>
        /// <param name="schema"></param>
        /// <param name="name"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static EntityResult<TModel> DefineEntity<TModel>(this Database database, string schema, string name, TModel model)
        {
            var result = new EntityResult<TModel>
            {
                Model = model
            };

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
            if (!(selector.Body is MemberExpression memberExpression))
            {
                var unary = (UnaryExpression)selector.Body;

                memberExpression = unary.Operand as MemberExpression;
            }

            return memberExpression.Member.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="result"></param>
        /// <param name="selector"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static EntityResult<TModel> SetColumnForProperty<TModel, TProperty>(this EntityResult<TModel> result, Expression<Func<TModel, TProperty>> selector, Column column)
        {
            if (string.IsNullOrEmpty(column.Name))
                column.Name = GetPropertyName(selector);

            if (result.Table.Columns.Contains(column))
            {
                result.Table[column.Name].Type = column.Type;
                result.Table[column.Name].Length = column.Length;
                result.Table[column.Name].Prec = column.Prec;
                result.Table[column.Name].Scale = column.Scale;
                result.Table[column.Name].Nullable = column.Nullable;
                result.Table[column.Name].Collation = column.Collation;
                result.Table[column.Name].Description = column.Description;
            }
            else
            {
                result.Table.Columns.Add(column);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="result"></param>
        /// <param name="selector"></param>
        /// <param name="seed"></param>
        /// <param name="increment"></param>
        /// <returns></returns>
        public static EntityResult<TModel> SetIdentity<TModel, TProperty>(this EntityResult<TModel> result, Expression<Func<TModel, TProperty>> selector, int seed = 1, int increment = 1)
        {
            var name = GetPropertyName(selector);

            result.Table.Identity = new Identity(name, seed, increment);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="result"></param>
        /// <param name="selector"></param>
        /// <param name="constraintName"></param>
        /// <returns></returns>
        public static EntityResult<TModel> SetPrimaryKey<TModel, TProperty>(this EntityResult<TModel> result, Expression<Func<TModel, TProperty>> selector, string constraintName = null)
        {
            var name = GetPropertyName(selector);

            result.Table.PrimaryKey = new PrimaryKey(name);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        public static void Translate(Database database)
        {
            // todo: Implement this feature
        }
    }
}

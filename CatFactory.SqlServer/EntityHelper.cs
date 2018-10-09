using System;
using System.Linq.Expressions;
using CatFactory.Mapping;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// Contains extension methods to define entities (Tables) using lambda expressions
    /// </summary>
    public static class EntityHelper
    {
        /// <summary>
        /// Defines an entity from anonymous type
        /// </summary>
        /// <typeparam name="TModel">Anonymous type</typeparam>
        /// <param name="database"><see cref="Database"/> instance</param>
        /// <param name="schema">Schema name</param>
        /// <param name="name">Table name</param>
        /// <param name="model">Anonymous type</param>
        /// <returns><see cref="Database"/> Instance</returns>
        public static EntityResult<TModel> DefineEntity<TModel>(this Database database, string schema, string name, TModel model) where TModel : class
        {
            var result = new EntityResult<TModel>
            {
                Model = model,
                Table = new Table
                {
                    Schema = schema,
                    Name = name
                }
            };

            foreach (var property in model.GetType().GetProperties())
                result.Table.Columns.Add(new Column { Name = property.Name });

            database.Tables.Add(result.Table);

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
        public static EntityResult<TModel> SetColumnForProperty<TModel, TProperty>(this EntityResult<TModel> result, Expression<Func<TModel, TProperty>> selector, Column column) where TModel : class
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
        public static EntityResult<TModel> SetIdentity<TModel, TProperty>(this EntityResult<TModel> result, Expression<Func<TModel, TProperty>> selector, int seed = 1, int increment = 1) where TModel : class
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
        public static EntityResult<TModel> SetPrimaryKey<TModel, TProperty>(this EntityResult<TModel> result, Expression<Func<TModel, TProperty>> selector, string constraintName = null) where TModel : class
        {
            var name = GetPropertyName(selector);

            result.Table.PrimaryKey = new PrimaryKey(name);

            return result;
        }
    }
}
